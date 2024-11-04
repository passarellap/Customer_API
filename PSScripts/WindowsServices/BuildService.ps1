param(
    [Parameter()]
    [string]$servicename
)

dotnet clean ../../AnagraficaAPI.sln | Out-Null
if ( $LASTEXITCODE -eq "0") {
    dotnet build ../../AnagraficaAPI.sln --configuration Release | Out-Null
    if ( $LASTEXITCODE -eq "0") {
        Write-Host "`t====>$serviceName built"
    }
}