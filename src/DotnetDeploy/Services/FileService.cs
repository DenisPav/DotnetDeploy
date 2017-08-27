using DotnetDeploy.Config;
using Renci.SshNet;
using System.IO;
using System.Linq;

namespace DotnetDeploy.Services
{
    public class FileService
    {
        private SftpClient Client { get; set; }
        private JsonConfig Config { get; set; }

        public FileService(SftpClient client, JsonConfig config)
        {
            this.Client = client;
            this.Config = config;
        }

        public void Upload()
        {
            Client.Connect();
            Client.CreateDirectory(Config.HostDirectory);
            Client.ChangeDirectory(Config.HostDirectory);

            var dir = new DirectoryInfo(Config.TargetDir);

            CreateDirs(dir);

            Client.Disconnect();
        }

        private void CreateDirs(DirectoryInfo root)
        {
            var subDirs = root.GetDirectories();

            UploadDirContents(root);

            if (subDirs.Length == 0)
                return;

            foreach (var subDir in subDirs)
            {
                Client.CreateDirectory(subDir.Name);
                Client.ChangeDirectory(subDir.Name);

                UploadDirContents(subDir);

                CreateDirs(subDir);
                Client.ChangeDirectory("../");
            }
        }

        private void UploadDirContents(DirectoryInfo dir)
            => dir.GetFiles("*", SearchOption.TopDirectoryOnly)
                .ToList()
                .ForEach(file =>
                {
                    using (var stream = File.OpenRead(file.FullName))
                        Client.UploadFile(stream, file.Name);
                });
    }
}
