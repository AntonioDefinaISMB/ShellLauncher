using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShellLauncher
{
    public class SettingsThread
    {
        public System.Threading.ManualResetEvent resetEvent;
        public String globalPath;
        public bool waitForExit;
        public bool leaveShellActiveWhenTaskExit;

        public SettingsThread(String globalPath, bool waitForEnd, bool leaveShellActiveWhenTaskExit)
        {
            this.globalPath = globalPath;
            this.waitForExit = waitForEnd;
            this.leaveShellActiveWhenTaskExit = leaveShellActiveWhenTaskExit;
        }
    }

    public class ApplicationSettings
    {
        public String   lineLaunchFirstTask         = "";
        public String   lineLaunchSecondTask        = "";
        public String   connectionAddress           = "";
        public int      TCPPort                     = 0;
        public int      intervalTasks               = 0;
        public bool     leaveShellActiveAfterExit   = false;

        public const String Separator                   = "=";
        public const String LineFileSeparator           = "\n";
        public const char charToRemove                  = '\r';
        public const String LabelLaunchFirstTask        = "-firstTask";
        public const String LabelLaunchSecondTask       = "-secondTask";
        public const String LabelconnectionAddress      = "-connectionAddress";
        public const String LabelTCPPort                = "-TCPPort";
        public const String LabelIntervalTasks          = "-intervalTasks";
        public const String LabelFileSettings           = "-settingsLauncher";
        public const String LabelLeaveShellExecute      = "-leaveShellAlwaysActive";
        public const String PathDefaultSettingsLauncher = "settingsLauncher.conf";

        public static Dictionary<string, TypeParameter> dictionarySettingsMandatory = new Dictionary<String, TypeParameter>()
        {
            { LabelLaunchFirstTask,TypeParameter.NAME_FIRST_TASK },
            { LabelLaunchSecondTask,TypeParameter.NAME_SECOND_TASK },
            { LabelconnectionAddress,TypeParameter.ADDRESS_CONNECTION },
            { LabelTCPPort,TypeParameter.TCPPORT_CONNECTION },
        };

        public static Dictionary<string, TypeParameter> optionalSettings = new Dictionary<String, TypeParameter>()
        {
            { LabelIntervalTasks,TypeParameter.INTERVAL_TASKS},
            { LabelLeaveShellExecute,TypeParameter.LEAVESHELLACTIVEAFTERTASKEXIT}
        };

        public bool ExtractSettingsFromFile(String pathFileSettings)
        {
            try
            {
                if (System.IO.File.Exists(pathFileSettings) == true)
                {
                    System.IO.StreamReader myFile = new System.IO.StreamReader(pathFileSettings);
                    string globalContent = myFile.ReadToEnd();

                    myFile.Close();

                    if (globalContent.Contains(LineFileSeparator))
                    {
                        String[] listSettings = globalContent.Split(LineFileSeparator.ToCharArray());

                        for (int indexString = 0; indexString < listSettings.Length; indexString++)
                        {
                            if (listSettings[indexString].Contains(charToRemove))
                            {
                                listSettings[indexString] = listSettings[indexString].Remove(listSettings[indexString].IndexOf(charToRemove), 1);
                            }
                        }
                        return ProcessSettings(listSettings);
                    }
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ProcessSettings(string[] args)
        {
            try
            {
                int counterElements = 0;
                int tempElement     = 0;
                foreach (string st in args)
                {
                    TypeParameter typeParam = TypeParameter.NONE;

                    if (st.Contains(Separator))
                    {
                        String[] listString = st.Split(Separator.ToCharArray());

                        for (int indexParam = 0; indexParam < listString.Length; indexParam++)
                        {
                            String param = listString[indexParam];

                            switch (indexParam)
                            {
                                case 0:
                                    typeParam = TypeParameter.NONE;

                                    if (dictionarySettingsMandatory.ContainsKey(param) == true)
                                    {
                                        typeParam = dictionarySettingsMandatory[param];
                                        Console.Write("Label: " + typeParam + ";");
                                    }
                                    else if (optionalSettings.ContainsKey(param) == true)
                                    {
                                        typeParam = optionalSettings[param];
                                        Console.Write("Label: " + typeParam + ";");
                                    }
                                    break;

                                case 1:
                                    switch (typeParam)
                                    {
                                        case TypeParameter.NAME_FIRST_TASK:
                                            lineLaunchFirstTask = param;
                                            Console.Write("lineLaunchFirstTask: " + lineLaunchFirstTask + "\r\n");
                                            break;
                                        case TypeParameter.NAME_SECOND_TASK:
                                            lineLaunchSecondTask = param;
                                            Console.Write("lineLaunchSecondTask: " + param + "\r\n");
                                            break;
                                        case TypeParameter.ADDRESS_CONNECTION:
                                            connectionAddress = param;
                                            Console.Write("connectionAddress: " + param + "\r\n");
                                            break;
                                        case TypeParameter.TCPPORT_CONNECTION:
                                            TCPPort = int.Parse(param);
                                            Console.Write("TCPPort: " + param + "\r\n");
                                            break;

                                        case TypeParameter.INTERVAL_TASKS:
                                            intervalTasks = int.Parse(param);
                                            Console.Write("intervalTasks: " + param + "\r\n");
                                            break;

                                        case TypeParameter.LEAVESHELLACTIVEAFTERTASKEXIT:
                                            tempElement = int.Parse(param);
                                            leaveShellActiveAfterExit = tempElement == 0 ? false : true;
                                            Console.Write("leaveShellActiveAfterExit: " + param + "\r\n");
                                            break;
                                    }
                                    if (typeParam != TypeParameter.NONE)
                                    {
                                        counterElements++;
                                    }
                                    break;
                            }
                        }
                    }
                }
                if (counterElements < dictionarySettingsMandatory.Count)
                {
                    return false;
                }
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    public enum TypeParameter
    {
        NONE,
        NAME_FIRST_TASK,
        NAME_SECOND_TASK,
        ADDRESS_CONNECTION,
        TCPPORT_CONNECTION,
        INTERVAL_TASKS,
        LEAVESHELLACTIVEAFTERTASKEXIT
    }
}
