using DotnetDeploy.Config;
using DotnetDeploy.Services;
using Jil;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Renci.SshNet;
using System;
using System.IO;
using System.Linq;

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
            
            var configFile = JSON.Deserialize<JsonConfig>(File.ReadAllText(path), new Options(serializationNameFormat: SerializationNameFormat.CamelCase));

            Container = ServiceCollectionBuilder.CreateWith(services => {
                services.AddSingleton<JsonConfig>(configFile);
                services.AddSingleton<ConnectionInfo>(_ => {
                    var config = _.GetRequiredService<JsonConfig>();

                    return new ConnectionInfo(config.Host, config.Username, new PasswordAuthenticationMethod(config.Username, config.Password));
                });
                services.AddTransient<SftpClient>();
                services.AddTransient<SshClient>();
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
