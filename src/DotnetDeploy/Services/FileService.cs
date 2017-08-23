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
            Client.CreateDirectory("./app");
            Client.ChangeDirectory("./app");

            var dir = new DirectoryInfo(Config.TargetDir)
                .GetFiles("*", SearchOption.AllDirectories)
                .Select(file => new { file.FullName, Name = $"{file.Name}.{file.Extension}" })
                .ToList();

            dir.ForEach(file =>
            {
                using (var stream = File.OpenRead(file.FullName))
                {
                    Client.UploadFile(stream, file.Name);
                }
            });

            Client.Disconnect();
        }
    }
}
