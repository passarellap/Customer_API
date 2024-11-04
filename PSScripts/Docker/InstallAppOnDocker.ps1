param(
    [Parameter(Mandatory = $true)]
    [string]$environment,
 
    [Parameter()]
    [char][ValidateSet('s', 'n', 'S', 'N')]$silent,
 
    [Parameter()]
    [char][ValidateSet('s', 'n', 'S', 'N')]$output
)
if ($silent -eq $null) {
    $silent = "N"
}

if ($output -eq $null) {
    $output = "S"
}

Write-Host "==================================================="
Write-Host "Build and install Anagrafica Api Service on Docker"
Write-Host "===================================================`r"

$appName = "anagraficaapi-app"
$imageName = "anagraficaapi-image:1.0"

& .\StopApp.ps1 -appName $appName
if ( $LASTEXITCODE -eq "0") {
    & .\RemoveApp.ps1 -appName $appName
}
& .\BuildApp.ps1 -imageName $imageName -output $output
if ( $LASTEXITCODE -eq "0") {
    & .\CreateApp.ps1 -port 6002 -appName $appName -imageName $imageName -environment $environment
    & .\StartApp.ps1 -appName $appName
}