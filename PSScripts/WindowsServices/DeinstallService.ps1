param(
    [Parameter()]
    [string]$serviceName
)

If (Get-Service $serviceName -ErrorAction SilentlyContinue) {
    $arrService = Get-Service -Name $serviceName
    if ($arrService.Status -eq 'Running') {
        Stop-Service -Name $serviceName
    }

    Remove-Service $serviceName
    Write-Host "`t====>$serviceName removed"
}