param(
    [Parameter(Mandatory = $true)]
    [string]$imageName,
 
    [Parameter()]
    [char][ValidateSet('s', 'n', 'S', 'N')]$output
)

Write-Host -NoNewline "`t====>building App ..."

Set-Location ..\..
$cmdBuild = "docker build --no-cache --build-arg BUILDVERSIONNUMBER=1.3.4.6 --build-arg NUGETSERVERS='http://nugetserver.gabetti.it/nuget https://www.nuget.org/api/v2/ https://api.nuget.org/v3/index.json' --tag $imageName -f Anagrafica.Api/Dockerfile . | " + $(($null -eq $output -or $output -eq 's' -or $output -eq 'S') ? 'Out-Default' : 'Out-Null');
Invoke-Expression $cmdBuild
Set-Location PSScripts\docker

if ( $LASTEXITCODE -eq "0") {
    Write-Host "`tApp is built"
}
else {
    Write-Host "`tApp build error" -ForegroundColor red
    exit $LASTEXITCODE
}