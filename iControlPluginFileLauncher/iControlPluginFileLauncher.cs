using System;
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


        public void Handle(string[] commands, string ip) {
            foreach (string path in System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "\\exec")) {
                System.IO.FileInfo file = new System.IO.FileInfo(path);

                if (System.IO.Path.GetFileNameWithoutExtension(path).Equals(commands[0])) {
                    System.Diagnostics.Process.Start(path, String.Join(" ", commands, 1, commands.Length - 1));
                    pluginHost.Log("Launched " + path, this);
                }
            }
        }
    }
}
