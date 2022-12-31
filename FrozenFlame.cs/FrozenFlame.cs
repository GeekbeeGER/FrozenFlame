using System;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using WindowsGSM.Functions;
using WindowsGSM.GameServer.Engine;
using WindowsGSM.GameServer.Query;

namespace WindowsGSM.Plugins
{
    public class FrozenFlame : SteamCMDAgent // SteamCMDAgent is used because FrozenFlame relies on SteamCMD for installation and update process
    {
        // - Plugin Details
        public Plugin Plugin = new Plugin
        {
            name = "WindowsGSM.FrozenFlame", // WindowsGSM.XXXX
            author = "GeekbeeGER",
            description = "WindowsGSM plugin for supporting FrozenFlame Dedicated Server",
            version = "1.0",
            url = "https://github.com/GeekbeeGER/WindowsGSM.FrozenFlame", // Github repository link (Best practice)
            color = "#9eff99" // Color Hex
        };


        // - Standard Constructor and properties
        public FrozenFlame(ServerConfig serverData) : base(serverData) => base.serverData = _serverData = serverData;
        private readonly ServerConfig _serverData; // Store server start metadata, such as start ip, port, start param, etc

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using WindowsGSM.Functions;
using WindowsGSM.GameServer.Query;
using WindowsGSM.GameServer.Engine;
using System.IO;
using System.Linq;
using System.Net;



namespace WindowsGSM.Plugins
{
    public class FrozenFlame : SteamCMDAgent
    {
        // - Plugin Details
        public Plugin Plugin = new Plugin
        {
            name = "WindowsGSM.FrozenFlame", // WindowsGSM.XXXX
            author = "Geekbee",
            description = "WindowsGSM plugin for supporting FrozenFlame Dedicated Server",
            version = "1.0",
            url = "https://github.com/GeekbeeGER/WindowsGSM.FrozenFlame", // Github repository link (Best practice)
            color = "#34c9ec" // Color Hex
        };

        // - Settings properties for SteamCMD installer
        public override bool loginAnonymous => true;
        public override string AppId => "1348640"; // Game server appId

        // - Standard Constructor and properties
        public FrozenFlame(ServerConfig serverData) : base(serverData) => base.serverData = _serverData = serverData;
        private readonly ServerConfig _serverData;
        public string Error, Notice;


        // - Game server Fixed variables
        public override string StartPath => @"FrozenFlameServer.exe"; // Game server start path
        public string FullName = "FrozenFlame Dedicated Server"; // Game server FullName
        public bool AllowsEmbedConsole = true;  // Does this server support output redirect?
        public int PortIncrements = 10; // This tells WindowsGSM how many ports should skip after installation
        public object QueryMethod = new A2S(); // Query method should be use on current server type. Accepted value: null or new A2S() or new FIVEM() or new UT3()


        // - Game server default values
        public string Port = "25575"; // Default port
        public string QueryPort = "27015"; // Default query port
        public string Defaultmap = "MAP"; // Default map name
        public string Maxplayers = "8"; // Default maxplayers
        public string Additional = "-log -LOCALLOGTIMES -port=25575 -queryPort=27015 -MetaGameServerName=CoolServer -RconPassword=password -RconPort=7777""; // Additional server start parameter


        // - Create a default cfg for the game server after installation
        public async void CreateServerCFG()
        {
            //No config file seems
        }

        // - Start server function, return its Process to WindowsGSM
        public async Task<Process> Start()
        {
			
            string shipExePath = Functions.ServerPath.GetServersServerFiles(_serverData.ServerID, StartPath);
            if (!File.Exists(shipExePath))
            {
                Error = $"{Path.GetFileName(shipExePath)} not found ({shipExePath})";
                return null;
            }			
			
		

            // Prepare start parameter

			string param = $"-batchmode {_serverData.ServerParam}" + (!AllowsEmbedConsole ? " -log" : string.Empty);	
	


            // Prepare Process
            var p = new Process
            {
                StartInfo =
                {
                    WorkingDirectory = ServerPath.GetServersServerFiles(_serverData.ServerID),
                    FileName = shipExePath,
                    Arguments = param,
                    WindowStyle = ProcessWindowStyle.Minimized,
                    UseShellExecute = false
                },
                EnableRaisingEvents = true
            };

            // Set up Redirect Input and Output to WindowsGSM Console if EmbedConsole is on
            if (AllowsEmbedConsole)
            {
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                var serverConsole = new ServerConsole(_serverData.ServerID);
                p.OutputDataReceived += serverConsole.AddOutput;
                p.ErrorDataReceived += serverConsole.AddOutput;

                // Start Process
                try
                {
                    p.Start();
                }
                catch (Exception e)
                {
                    Error = e.Message;
                    return null; // return null if fail to start
                }

                p.BeginOutputReadLine();
                p.BeginErrorReadLine();
                return p;
            }

            // Start Process
            try
            {
                p.Start();
                return p;
            }
            catch (Exception e)
            {
                Error = e.Message;
                return null; // return null if fail to start
            }
        }
	
	   // - Stop server function
	   public async Task Stop(Process p) => await Task.Run(() => { p.Kill(); }); // I believe Core Keeper don't have a proper way to stop the server so just kill it
    }
}
        // - Settings properties for SteamCMD installer
        public override bool loginAnonymous => true; // FrozenFlame requires to login steam account to install the server, so loginAnonymous = false
        public override string AppId => "1348640"; // Game server appId, FrozenFlame is 233780


        // - Game server Fixed variables
        public override string StartPath => "FrozenFlameServer.exe"; // Game server start path, for FrozenFlame, it is FrozenFlameserver.exe
        public string FullName = "FrozenFlame Dedicated Server"; // Game server FullName
        public bool AllowsEmbedConsole = true;  // Does this server support output redirect?
        public int PortIncrements = 2; // This tells WindowsGSM how many ports should skip after installation
        public object QueryMethod = new A2S(); // Query method should be use on current server type. Accepted value: null or new A2S() or new FIVEM() or new UT3()


        // - Game server default values
        public string Port = "25575"; // Default port
        public string QueryPort = "27015"; // Default query port
        public string Defaultmap = "empty"; // Default map name
        public string Maxplayers = "64"; // Default maxplayers
        public string Additional = "-log -LOCALLOGTIMES -port=25575 -queryPort=27015 -MetaGameServerName=CoolServer -RconPassword=password -RconPort=7777"; // Additional server start parameter


        // - Create a default cfg for the game server after installation
        public async void CreateServerCFG() { }


        // - Start server function, return its Process to WindowsGSM
        public async Task<Process> Start()
        {
            // Prepare start parameter
            var param = new StringBuilder();
            param.Append(string.IsNullOrWhiteSpace(_serverData.ServerPort) ? string.Empty : $" -port={_serverData.ServerPort}");
            param.Append(string.IsNullOrWhiteSpace(_serverData.ServerName) ? string.Empty : $" -name=\"{_serverData.ServerName}\"");
            param.Append(string.IsNullOrWhiteSpace(_serverData.ServerParam) ? string.Empty : $" {_serverData.ServerParam}");
 
            // Prepare Process
            var p = new Process
            {
                StartInfo =
                {
                    WorkingDirectory = ServerPath.GetServersServerFiles(_serverData.ServerID),
                    FileName = ServerPath.GetServersServerFiles(_serverData.ServerID, StartPath),
                    Arguments = param,
                    WindowStyle = ProcessWindowStyle.Minimized,
                    UseShellExecute = false
                },
                EnableRaisingEvents = true
            };

            // Set up Redirect Input and Output to WindowsGSM Console if EmbedConsole is on
            if (AllowsEmbedConsole)
            {
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                var serverConsole = new ServerConsole(_serverData.ServerID);
                p.OutputDataReceived += serverConsole.AddOutput;
                p.ErrorDataReceived += serverConsole.AddOutput;

                // Start Process
                try
                {
                    p.Start();
                }
                catch (Exception e)
                {
                    Error = e.Message;
                    return null; // return null if fail to start
                }

                p.BeginOutputReadLine();
                p.BeginErrorReadLine();
                return p;
            }

            // Start Process
            try
            {
                p.Start();
                return p;
            }
            catch (Exception e)
            {
                Error = e.Message;
                return null; // return null if fail to start
            }
        }


		// - Stop server function
        public async Task Stop(Process p)
        {
            await Task.Run(() =>
            {
                if (p.StartInfo.CreateNoWindow)
                {
                    Functions.ServerConsole.SetMainWindow(p.MainWindowHandle);
                    Functions.ServerConsole.SendWaitToMainWindow("^c");
					
                }
                else
                {
                    Functions.ServerConsole.SetMainWindow(p.MainWindowHandle);
                    Functions.ServerConsole.SendWaitToMainWindow("^c");
                }
            });
			await Task.Delay(20000);
        }

// fixes WinGSM bug, https://github.com/WindowsGSM/WindowsGSM/issues/57#issuecomment-983924499
        public async Task<Process> Update(bool validate = false, string custom = null)
        {
            var (p, error) = await Installer.SteamCMD.UpdateEx(serverData.ServerID, AppId, validate, custom: custom, loginAnonymous: loginAnonymous);
            Error = error;
            await Task.Run(() => { p.WaitForExit(); });
            return p;
        }

    }
}
