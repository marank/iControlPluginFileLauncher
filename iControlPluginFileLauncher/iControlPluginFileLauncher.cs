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

        private string Path = AppDomain.CurrentDomain.BaseDirectory + "\\exec";

        public bool Init() {
            if (!System.IO.Directory.Exists(Path)) {
                System.IO.Directory.CreateDirectory(Path);
            }

            return true;
        }

        public void Handle(string[] commands, string ip) {
            foreach (string file in System.IO.Directory.GetFiles(Path)) {
                System.IO.FileInfo fi = new System.IO.FileInfo(file);

                if (System.IO.Path.GetFileNameWithoutExtension(file).Equals(commands[0])) {
                    System.Diagnostics.Process.Start(file, String.Join(" ", commands, 1, commands.Length - 1));
                    pluginHost.Log("Launched " + file, this);
                }
            }
        }
    }
}
