param(
    [Parameter()]
    [string]$servicename,
 
    [Parameter()]
    [string]$environment,
 
    [Parameter()]
    [string]$basepath
)

If (Get-Service $servicename -ErrorAction SilentlyContinue) {
    Write-Host "$servicename already exist!"
}
else {
    New-Service -Name $servicename -Description "Anagrafica Api Service" -DisplayName "_Anagrafica Api" -BinaryPathName "$basepath\AnagraficaAPI\Anagrafica.Api\bin\Release\netcoreapp3.1\AnagraficaAPI.exe $environment" | Out-Null
    Write-Host "`t====>$serviceName installed"
}