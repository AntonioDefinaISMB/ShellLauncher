using System;

namespace ShellLauncher
{
    class Program
    {
        /**/
        #region MainFunction
        static void Main(string[] args)
        {
            bool settingsAcquired = false;

            Version                 version                     = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
            string                  versionString               = version.ToString();
            ApplicationSettings     applicationSettings         = new ApplicationSettings();   
            
            Console.WriteLine("Shell Launcher version " + versionString);

            if (args!=null && args.Length>0)
            {
                if (args[0]!=null && args[0].Contains("?"))
                {
                    PrintMenu();
                    return;
                }

                if(args[0].Contains(ApplicationSettings.LabelFileSettings) && args[0].Contains(ApplicationSettings.Separator))
                {
                    string[]    listParams          = args[0].Split(ApplicationSettings.Separator.ToCharArray());
                    string      pathFileSettings    = "";

                    if (listParams!=null && listParams.Length>1)
                    {
                        pathFileSettings = listParams[1];

                        settingsAcquired = applicationSettings.ExtractSettingsFromFile(pathFileSettings);
                    }
                }
                else
                {
                    settingsAcquired = applicationSettings.ProcessSettings(args);
                }
            }

            if (settingsAcquired == false && applicationSettings.ExtractSettingsFromFile(ApplicationSettings.PathDefaultSettingsLauncher) ==false)
            {
                Console.WriteLine("SHELL LAUNCHER SETTINGS ARE MISSING!");
                PrintMenu();
                return;
            }
            ShellLauncher.ExecuteStandardShellScriptAsync(applicationSettings.lineLaunchFirstTask, false,false);

            if (AsynchronousClient.StartClient(applicationSettings.connectionAddress, applicationSettings.TCPPort) == true)
            {
                if (applicationSettings.intervalTasks > 0)
                {
                    Console.WriteLine("WAIT FOR TIMEOUT OF "+ applicationSettings.intervalTasks.ToString()+" MILLISECONDS...");
                    System.Threading.Thread.Sleep(applicationSettings.intervalTasks);
                }
                Console.WriteLine("START LAUNCHING NEW PROCESS");
                ShellLauncher.ExecuteStandardShellScriptAsync(applicationSettings.lineLaunchSecondTask, true,false);
            }
            else
            {
                Console.WriteLine("FAILED CONNECTION WITH SERVER");
            }

            ShellLauncher.WaitAllThread();
        }

        #endregion MainFunction

        #region SupportMethods
        static void PrintMenu()
        {
            Console.WriteLine("HELP ShellLauncher. 3 Different Options");

            Console.WriteLine("FIRST CHOICE: ");
            Console.Write("ShellLauncher ");

            foreach (string label in ApplicationSettings.dictionarySettingsMandatory.Keys)
            {
                Console.Write(label + ApplicationSettings.Separator + "<" + ApplicationSettings.dictionarySettingsMandatory[label] + ">");

                Console.Write(" ");
            }
            foreach (string label in ApplicationSettings.optionalSettings.Keys)
            {
                Console.Write("[" + label + ApplicationSettings.Separator + "<" + ApplicationSettings.optionalSettings[label] + ">" + "]");

                Console.Write(" ");
            }

            Console.WriteLine();
            Console.WriteLine("SECOND CHOICE: ");
            Console.WriteLine("ShellLauncher");
            Console.WriteLine("It tries to extract settings from file path" + ApplicationSettings.PathDefaultSettingsLauncher);
            Console.WriteLine();
            Console.WriteLine("THIRD CHOICE: ");
            Console.WriteLine("ShellLauncher " + ApplicationSettings.LabelFileSettings + "=<PathFileSettings>");
            Console.WriteLine("It extract settings (see the format of first choice set by line)");
            Console.WriteLine();
            Console.ReadKey();
        }
        #endregion SupportMethods
    }
}
