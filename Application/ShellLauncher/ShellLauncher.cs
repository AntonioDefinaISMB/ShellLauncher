using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShellLauncher
{
    class ShellLauncher
    {
        public static void ExecuteCommandSync(object command)
        {
            try
            {
                // create the ProcessStartInfo using "cmd" as the program to be run,
                // and "/c " as the parameters.
                // Incidentally, /c tells cmd that we want it to execute the command that follows,
                // and then exit.

                Console.WriteLine(command.ToString());

                System.Diagnostics.ProcessStartInfo procStartInfo =
                    new System.Diagnostics.ProcessStartInfo("cmd.exe", "/C " + command);

                // The following commands are needed to redirect the standard output.
                // This means that it will be redirected to the Process.StandardOutput StreamReader.
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                // Do not create the black window.
                procStartInfo.CreateNoWindow = false;
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                // Get the output into a string
                string result = proc.StandardOutput.ReadToEnd();
                // Display the command output.
                Console.WriteLine(result);
            }
            catch (Exception objException)
            {
                Console.WriteLine(objException.Message);
            }
        }

        const int MAX_NUMBER_TASK = 2;

        static ManualResetEvent[] syncEvents = new ManualResetEvent[MAX_NUMBER_TASK];
        static int indexCurrentTask = 0;

        public static void WaitAllThread()
        {
            for (int index = 0; index < syncEvents.Length; index++)
            {
                if (syncEvents[index] != null)
                {
                    syncEvents[index].WaitOne();
                }
            }
        }

        public static void ExecuteStandardShellScriptAsync(String globalPathScript, bool waitForExit,bool leaveShellOpenAfterTaskExit)
        {
            SettingsThread settings = new SettingsThread(globalPathScript, waitForExit, leaveShellOpenAfterTaskExit);

            settings.resetEvent = new ManualResetEvent(false);
            syncEvents[indexCurrentTask++] = settings.resetEvent;

            System.Threading.Thread threadLauncher = new System.Threading.Thread(new ParameterizedThreadStart(ThreadShellLauncher));
            threadLauncher.Start(settings);
        }

        public static string threadGlobalPath = "";
        public static bool threadWaitForExit = false;

        private static void ThreadShellLauncher(object pSettings)
        {
            try
            {
                SettingsThread settings = (SettingsThread)pSettings;

                ExecuteStandardShellScript(settings.globalPath, settings.waitForExit,settings.leaveShellActiveWhenTaskExit);

                if (settings.resetEvent != null)
                {
                    settings.resetEvent.Set();
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine("ERROR: " + exc.Message);
            }
        }

        public static void ExecuteStandardShellScript(String globalPathScript, bool waitForExit, bool leaveShellOpenAfterExit)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();

            String settingsLaunchShell = leaveShellOpenAfterExit == false ? "/C " : "/K ";

            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = settingsLaunchShell + globalPathScript;
            //startInfo.Verb = "runas"; //IN ORDER TO LAUNCH AS ADMINISTRATOR (User confirm is required!)
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            process.StartInfo = startInfo;

            process.Start();

            if (waitForExit)
            {
                process.WaitForExit();
            }
        }

    }
}
