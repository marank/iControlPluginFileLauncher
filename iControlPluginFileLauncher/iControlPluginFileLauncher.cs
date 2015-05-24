using System;
using System.Collections.Generic;
using iControlPluginInterface;

namespace iControlPluginFileLauncher {
    class iControlPlugin : IiControlPlugin {
        public string Name {
            get {
                return "FileLauncher";
            }
        }

        public string Author {
            get {
                return "Matthias Rank";
            }
        }

        private iControlPluginInterface.IiControlPluginHost pluginHost;
        public iControlPluginInterface.IiControlPluginHost Host {
            set {
                pluginHost = value;
            }
            get {
                return pluginHost;
            }
        }

        private string Path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "exec");

        public bool Init() {
            if (!System.IO.Directory.Exists(Path)) {
                System.IO.Directory.CreateDirectory(Path);
            }

            string configFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins", "iControlPluginFileLauncher.config");
            if (System.IO.File.Exists(configFile)) {
                Dictionary<string, string> settings = pluginHost.DeserializeJSON(configFile);
                bool value;
                if (settings.ContainsKey("enabled") && Boolean.TryParse(settings["enabled"], out value) && value == false) {
                    pluginHost.Log("Plugin disabled", this);
                    return false;
                }
            }

            return true;
        }

        public void Handle(string[] commands, string ip) {
            if (commands[0] == "launch") {
                foreach (string file in System.IO.Directory.GetFiles(Path)) {
                    System.IO.FileInfo fi = new System.IO.FileInfo(file);

                    if (System.IO.Path.GetFileNameWithoutExtension(file).Equals(commands[1]) || System.IO.Path.GetFileName(file).Equals(commands[1])) {
                        System.Diagnostics.Process.Start(file, String.Join(" ", commands, 2, commands.Length - 2));
                        pluginHost.Log("Launched " + file, this);
                    }
                }
        }
          }
    }
}
