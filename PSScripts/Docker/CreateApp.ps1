param(
    [Parameter()]
    [string]$appName,
 
    [Parameter()]
    [string]$imageName,

    [Parameter()]
    [string]$port,
    
    [Parameter()]
    [string]$environment
)

$argPort = $port + ":" + "80"

Write-Host -NoNewline "`t====>creating App ..."
$releaseDate = Get-Date -Format "dd/MM/yyyy HH:mm K"
& docker run --detach --publish $argPort --env "ASPNETCORE_ENVIRONMENT=$environment" --env "OCTOPUS_RELEASEDATE=$releaseDate" --name $appName $imageName | Out-Null

if ( $LASTEXITCODE -eq "0") {
    Write-Host "`tApp is created"
}
else {
    Write-Host "`tApp create error" -ForegroundColor red
}