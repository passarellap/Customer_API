param(
    [Parameter(Mandatory = $true)]
    [string]$serviceName,

    [Parameter(Mandatory = $true)]
    [string]$environment,
 
    [Parameter(Mandatory = $true)]
    [string]$basepath
)

& .\StopService.ps1 -serviceName $serviceName
& .\DeinstallService.ps1 -serviceName $serviceName
& .\BuildService.ps1 -serviceName $serviceName
if ($LASTEXITCODE -eq "0") {
    & .\InstallService.ps1 -serviceName $serviceName -basepath $basepath -environment $environment 
    & .\StartService.ps1 -serviceName $serviceName
}
else {
    Write-Host "Build Error !!!"
}