curl -s http://127.0.0.1:6002/api/Info/IsAlive | Out-Null

if ( $LASTEXITCODE -eq "0") {
    Write-Host "AnagraficaApi test successfully"
}
else {
    Write-Host "AnagraficaApi test error" -ForegroundColor red
    exit $LASTEXITCODE
}