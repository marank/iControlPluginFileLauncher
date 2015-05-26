using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using iControlInterfaces;

namespace iControlPluginFileLauncher {
    public class iControlPlugin : IiControlPlugin {
        public string Name {
            get {
                return "FileLauncher";
            }
        }
        public string Author {
            get {
                return "marank <development@m-rank.de>";
            }
        }
        public string Version {
            get {
                return "0.0.1";
            }
        }

        private IiControlPluginHost _pluginHost;
        public IiControlPluginHost Host {
            set {
                _pluginHost = value;
            }
            get {
                return _pluginHost;
            }
        }

        private string _execpath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "exec");
        private string _configpath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins", "iControlPluginFileLauncher.config");
        private Dictionary<string, object> _settings;

        public bool Init() {
            if (!System.IO.Directory.Exists(_execpath)) {
                System.IO.Directory.CreateDirectory(_execpath);
            }

            if (System.IO.File.Exists(_configpath)) {
                _settings = Host.DeserializeJSON(_configpath);
                if (_settings.ContainsKey("enabled") && Convert.ToBoolean(_settings["enabled"]) == false) {
                    _pluginHost.Log("Plugin disabled", this);
                    return false;
                }
            }

            return true;
        }

        public void Handle(string[] commands,  IiControlClient client) {
            if (commands[0] == "launch") {
                NetworkStream clientStream = client.TCP.GetStream();
                ASCIIEncoding encoder = new ASCIIEncoding();
                foreach (string file in System.IO.Directory.GetFiles(_execpath)) {
                    System.IO.FileInfo fi = new System.IO.FileInfo(file);

                    if (System.IO.Path.GetFileNameWithoutExtension(file).Equals(commands[1]) || System.IO.Path.GetFileName(file).Equals(commands[1])) {
                        System.Diagnostics.Process.Start(file, String.Join(" ", commands, 2, commands.Length - 2));
                        Host.Log("Launched " + file, this);
                        byte[] buffer = encoder.GetBytes("Launched " + file);
                        clientStream.Write(buffer, 0, buffer.Length);
                        clientStream.Flush();
                    }
                }
            }
        }
    }
}
