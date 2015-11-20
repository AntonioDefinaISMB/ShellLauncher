# ShellLauncher #

## Introduction ##
C# Application for launching two different process in two shell configurable. The first process is a TCP server application exploited by the second process.

-------------------------

## Executable Utilization ##

ShellLauncher can replace the Windows Standard Shell (explorer.exe or cmd.exe), set the key Shell under HKEY_Current_User\Software\Microsoft\Windows NT\CurrentVersion\Winlogon\Shell (See [MSDN Help](MSDN Help "https://msdn.microsoft.com/en-us/library/ms838576(v=winembedded.5).aspx") for details).

ShellLauncher.exe accepts different kind of command line arguments. In the following the type of launching.

### Request Help ###

*%PATHSHELL%>ShellLauncher.exe ?*

Provides the type of use.

### [Inline Parameters]  ###

*%PATHSHELL%>ShellLauncher.exe -firstTask=PATH_FIRST_TASK-secondTask=PATH_SECOND_TASK-connectionAddress=ADDRESS_CONNECTION-TCPPort=TCPPORT_CONNECTION[-intervalTasks=INTERVAL_TASKS] [-leaveShellAlwaysActive=LEAVESHELLACTIVEAFTERTASKEXIT]*

The list of parameters (Mandatory and optionals) are listed in the following: 

- **PATH_FIRST_TASK**: The global path of the first process (executable or batch file) to launch. It is intended to be a server exploited by the second process;
- **PATH_SECOND_TASK**: The global path of the second process that communicates with the first one by means of TCP Connection;
- **ADDRESS_CONNECTION**: The connection address of the first process (N.B. ShellLauncher tries to perform TCP connection to a specified IP address and TCP port before launching the second process);
- **TCPPORT_CONNECTION**: The TCP Port of the first process;
- **INTERVAL_TASKS** *[Optional]*: The interval (expressed in milliseconds) between the TCP connection attempt and the launch of the second process;
- **LEAVESHELLACTIVEAFTERTASKEXIT** *[Optional]*: Flag that indicates whether leaves both shells active (set to 1) or not (set to 0).


### Specified Configuration Files  ###

*%PATHSHELL%>ShellLauncher.exe -settingsLauncher=PATH_FILE_SETTINGS*

It extract settings from *PATH_FILE_SETTINGS*. The format of the configuration file reflects the one reported in the Paragraph `Inline Parameters` with parameters disposed per line. In the following the explicit format:

*-firstTask=PATH_FIRST_TASK*  
*-secondTask=PATH_SECOND_TASK*  
*-connectionAddress=ADDRESS_CONNECTION*   
*-TCPPort=TCPPORT_CONNECTION*  
*-intervalTasks=INTERVAL_TASKS*  
*-leaveShellAlwaysActive=LEAVESHELLACTIVEAFTERTASKEXIT*  

The meaning of the parameters is illustrated in Paragraph `Inline Parameters`.

### Default Configuration  ###

*%PATHSHELL%>*ShellLauncher.exe

In this case, ShellLauncher tries to extract settings from default file settings located in *%PATHSHELL%\settingsLauncher.conf* (if present).

