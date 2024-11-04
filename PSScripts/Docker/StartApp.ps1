param(
     [Parameter()]
     [string]$appName
)

Write-Host -NoNewline "`t====>starting App ..."
docker start $appName | Out-Null

if( $LASTEXITCODE -eq "0")
{
    Write-Host "`tApp is started"
}
else
{
    Write-Host "`tApp start error" -ForegroundColor red
}