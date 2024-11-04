param(
     [Parameter()]
     [string]$serviceName
 )

If (Get-Service $serviceName -ErrorAction SilentlyContinue) {
    $ServiceInfo = Get-Service -Name $serviceName
    if ($ServiceInfo.Status -eq 'Stopped'){
        Start-Service -Name $serviceName
        Write-Host "`t====>$serviceName started"
    }
    else
    {
        Write-Host "$serviceName already started!"   
    }
}
else
{
    Write-Host "$serviceName not exist!"   
}