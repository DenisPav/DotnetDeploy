# DotnetDeploy 🚀

Command tool which deploys .NET Core apps via SSH (SFTP) and executes command after that (for example you can start your app after deployment 😊)

## Intro

Tool is located on a MyGet feed [here](https://www.myget.org/F/dotnetdeploy/api/v3/index.json). You'll probably need to create NuGet.Config and add this feed to your code in order to use it since it isn't yet published on NuGet.
In order to use this, you'll just need to reference nuget package as a cli tool via `<DotNetCliToolReference>` tag.
After that setup **deploy.json** in root of your project and run `dotnet deploy`.

## Configuration

All configuration is done via file named **deploy.json** which contains host configuration and command executed after the deployment on the specified host.

Sample config looks like this:

```javascript
{
    "host": "host ip",
    "hostDirectory": "directory to which to deploy (will be created)",
    "username": "username",
    "password": "pass",
    "targetDir": "local dir",
    "endCommand": "command to execute after deployment",
    "commandTimeout": 2
}
``` 