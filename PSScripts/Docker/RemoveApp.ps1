param(
     [Parameter()]
     [string]$appName
)

Write-Host -NoNewline "`t====>removing App ..."

$jsonEntryApp = docker ps --all --filter "name=$appName" --format "{{json .}}"

<#
{
	"Command": "\"dotnet CensimentoAp…\"",
	"CreatedAt": "2020-12-10 17:12:11 +0100 CET",
	"ID": "90a4b1c03c8d",
	"Image": "censimentoapi-image",
	"Labels": "",
	"LocalVolumes": "0",
	"Mounts": "",
	"Names": "censimentoapi-app",
	"Networks": "bridge",
	"Ports": "0.0.0.0:6001->80/tcp",
	"RunningFor": "20 minutes ago",
	"Size": "0B",
	"Status": "Up 20 minutes"
}
#>

if($jsonEntryApp -ne $null)
{
	$entryApp = ConvertFrom-Json $jsonEntryApp
	docker rm -v $entryApp.id | Out-Null
	Write-Host "`tApp is removed"
}
else
{
	Write-Host -NoNewline "`tApp don't exist" -ForegroundColor red
}