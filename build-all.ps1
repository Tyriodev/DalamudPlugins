<#
    build-all.ps1 - baut alle Plugins und legt sie in dieses Repository.

    Aufruf, aus diesem Ordner heraus:

        .\build-all.ps1 -DevRoot C:\dev

    Darunter wird nach den Projektdateien gesucht, also z.B.
    C:\dev\Glacier\Glacier\Glacier.csproj. Wie tief sie liegen, ist egal.

    Fuer jedes gefundene Plugin laeuft publish.ps1: Version aus der .csproj lesen,
    ins Manifest schreiben, bauen, latest.zip hierher kopieren, repo.json nachziehen.

    Ein einzelnes Plugin:

        .\build-all.ps1 -DevRoot C:\dev -Nur Glacier

    Danach hochladen mit:

        .\push.ps1
#>

param(
    [Parameter(Mandatory = $true)]
    [string] $DevRoot,

    [string] $RepoRoot = $PSScriptRoot,

    # Nur diese Plugins bauen. Ohne Angabe: alle aus repo.json.
    [string[]] $Nur
)

$ErrorActionPreference = 'Stop'

if (-not (Test-Path $DevRoot)) {
    throw "Den Ordner $DevRoot gibt es nicht."
}

$repoPath = Join-Path $RepoRoot 'repo.json'
if (-not (Test-Path $repoPath)) {
    throw "repo.json liegt nicht in $RepoRoot"
}

$publish = Join-Path $RepoRoot 'publish.ps1'
if (-not (Test-Path $publish)) {
    throw "publish.ps1 liegt nicht in $RepoRoot"
}

$namen = @(Get-Content $repoPath -Raw | ConvertFrom-Json | ForEach-Object { $_.InternalName })

if ($Nur) {
    $namen = @($namen | Where-Object { $Nur -contains $_ })
    if ($namen.Count -eq 0) {
        throw "In repo.json steht keines von: $($Nur -join ', ')"
    }
}

Write-Host "Suche $($namen.Count) Projekte unter $DevRoot" -ForegroundColor Cyan
Write-Host ''

$gebaut = @()
$fehlend = @()
$kaputt = @()

foreach ($name in $namen) {
    $projekt = Get-ChildItem -Path $DevRoot -Recurse -Filter "$name.csproj" -ErrorAction SilentlyContinue |
        Where-Object { $_.FullName -notmatch '\\(bin|obj)\\' } |
        Select-Object -First 1

    if ($null -eq $projekt) {
        Write-Host "[  ] $name - keine $name.csproj gefunden" -ForegroundColor DarkGray
        $fehlend += $name
        continue
    }

    Write-Host "[..] $name" -ForegroundColor Cyan

    try {
        & $publish -Project $projekt.FullName -RepoRoot $RepoRoot
        Write-Host "[ok] $name" -ForegroundColor Green
        $gebaut += $name
    }
    catch {
        Write-Host "[!!] $name - $($_.Exception.Message)" -ForegroundColor Red
        $kaputt += $name
    }

    Write-Host ''
}

Write-Host '--------------------------------------------------'
Write-Host "fertig:        $($gebaut.Count)  $($gebaut -join ', ')"

if ($fehlend.Count -gt 0) {
    Write-Host "nicht gefunden: $($fehlend.Count)  $($fehlend -join ', ')" -ForegroundColor DarkGray
}

if ($kaputt.Count -gt 0) {
    Write-Host "fehlgeschlagen: $($kaputt.Count)  $($kaputt -join ', ')" -ForegroundColor Red
    Write-Host ''
    Write-Host 'Die Fehlermeldungen stehen oben. Der Rest ist trotzdem fertig und kann hoch.'
}

Write-Host ''
Write-Host 'Weiter mit:  .\push.ps1' -ForegroundColor Cyan
