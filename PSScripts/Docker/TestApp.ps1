Write-Host -NoNewline "`t====>anagraficaapi testing App - "
curl -s http://127.0.0.1:6002/api/Info/IsAlive | Out-Null

if( $LASTEXITCODE -eq "0")
{
    Write-Host "is tested successfully"
}
else
{
    Write-Host "test error" -ForegroundColor red
    exit $LASTEXITCODE
}