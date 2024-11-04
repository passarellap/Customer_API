param(
     [Parameter()]
     [string]$appName
)

Write-Host -NoNewline "`t====>stopping App ..."

$jsonEntryApp = docker ps -all --filter "name=$appName" --format "{{json .}}"

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
	if($entryApp.state -eq "running")
	{
		docker stop $entryApp.id | Out-Null
		Write-Host "`tApp is now stopped"
	}
	else
	{
		Write-Host "`tApp is already stopped"
	}
}
else
{
	Write-Host "`tApp don't exist" -ForegroundColor red
	exit 404
}