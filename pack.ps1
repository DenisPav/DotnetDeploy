Set-Location $PSScriptRoot

$revNumber = @{ $true = $env:APPVEYOR_BUILD_NUMBER; $false = 1 }[$env:APPVEYOR_BUILD_NUMBER -ne $NULL];
$revNumber = "dev-{0:D4}" -f [convert]::ToInt32($revNumber, 10)

dotnet pack ./src/DotnetDeploy/DotnetDeploy.csproj -o ../../artifacts --version-suffix=$revNumber

