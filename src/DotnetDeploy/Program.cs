using DotnetDeploy.Config;
using DotnetDeploy.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Renci.SshNet;
using System;
using System.IO;

namespace DotnetDeploy
{
    class Program
    {
        public static IServiceProvider Container { get; set; }

        static void Main(string[] args)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "deploy.json");
            if (!File.Exists(path))
                throw new Exception($"File: {path} doesn't exist!");

            Container = ServiceCollectionBuilder.CreateWith(services => {
                services.AddSingleton<JsonConfig>(_ => JsonConvert.DeserializeObject<JsonConfig>(File.ReadAllText(path)));
                services.AddSingleton<ConnectionInfo>(_ => {
                    var config = _.GetRequiredService<JsonConfig>();

                    return new ConnectionInfo(config.Host, config.Username, new PasswordAuthenticationMethod(config.Username, config.Password));
                });
                services.AddTransient<SftpClient>(_ => { var instance = new SftpClient(_.GetRequiredService<ConnectionInfo>()); instance.Connect(); return instance; });
                services.AddTransient<SshClient>(_ => { var instance = new SshClient(_.GetRequiredService<ConnectionInfo>()); instance.Connect(); return instance; });
                services.AddTransient<FileService>();
            }).BuildServiceProvider();

            var configBuilder = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();

            Container.GetRequiredService<FileService>()
                .Upload();
        }
    }
}
