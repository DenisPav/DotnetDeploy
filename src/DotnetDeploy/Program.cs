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
            Container = ServiceCollectionBuilder.CreateWith(services => {
                services.AddSingleton<ConnectionInfo>(_ => new ConnectionInfo("", "", new PasswordAuthenticationMethod("", "")));
                services.AddTransient<SftpClient>();
                services.AddTransient<FileService>();
            }).BuildServiceProvider();

            var configBuilder = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();

            var path = Path.Combine(Directory.GetCurrentDirectory(), "deploy.json");

            if (File.Exists(path))
            {
                var config = JSON.Deserialize<object>(File.ReadAllText(path), new Options(serializationNameFormat: SerializationNameFormat.CamelCase));

            }
        }
    }
}
