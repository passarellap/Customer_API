param(
     [Parameter()]
     [string]$serviceName
 )

If (Get-Service $serviceName -ErrorAction SilentlyContinue) {
    $ServiceInfo = Get-Service -Name $serviceName
    if ($ServiceInfo.Status -eq 'Running'){
        Stop-Service -Name $serviceName
        Write-Host "`t====>$serviceName stopped"
    }
}