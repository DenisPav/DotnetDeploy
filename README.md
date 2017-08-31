# DotnetDeploy 🚀

Command tool which deploys .NET Core apps via SSH (SFTP) and executes command after that (for example you can start your app after deployment 😊)

## Intro

In order to use this, you'll just need to reference nuget package.
After that setup deploy.json in root of your project and run `dotnet deploy`.

## Config

All configuration is done via file named `deploy.json` which contains host configuration and command executed after the deployment on the specified host.