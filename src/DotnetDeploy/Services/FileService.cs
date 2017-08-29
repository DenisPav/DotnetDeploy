using DotnetDeploy.Config;
using DotnetDeploy.Extensions;
using Renci.SshNet;
using System;
using System.IO;
using System.Linq;

namespace DotnetDeploy.Services
{
    public class FileService
    {
        private SftpClient Client { get; set; }
        private JsonConfig Config { get; set; }
        private SshClient SshClient { get; set; }

        public FileService(SftpClient client, JsonConfig config, SshClient sshClient)
        {
            this.Client = client;
            this.Config = config;
            this.SshClient = sshClient;
        }

        public void Upload()
        {
            Client.CreateDirectory(Config.HostDirectory);
            Client.ChangeDirectory(Config.HostDirectory);

            CreateOutputDirs(new DirectoryInfo(Config.TargetDir));

            var command = SshClient.CreateCommand(Config.EndCommand);
            command.CommandTimeout = TimeSpan.FromSeconds(Config.CommandTimeout.HasValue ? Config.CommandTimeout.Value : 2);
            this.TryCatchExecute(() => command.Execute());
        }

        private void CreateOutputDirs(DirectoryInfo root)
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

                CreateOutputDirs(subDir);
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
