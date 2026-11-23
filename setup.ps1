<#
    setup.ps1 - richtet das Repository einmalig ein.

    Aufruf, aus diesem Ordner heraus:

        .\setup.ps1 -GitHubName marcel123

    Was es macht:
      1. prueft, ob Git da ist
      2. ersetzt DEIN-GITHUB-NAME in repo.json durch deinen echten Namen
      3. legt das lokale Git-Repository an und schiebt alles zu GitHub
      4. nennt dir am Ende den Link fuer Dalamud

    Das GitHub-Repository musst du vorher im Browser angelegt haben:
    github.com -> + -> New repository -> Name "DalamudPlugins" -> Public ->
    KEINEN Haken bei "Add a README file" -> Create repository.
#>

param(
    [Parameter(Mandatory = $true)]
    [string] $GitHubName,

    [string] $RepoName = 'DalamudPlugins',

    [string] $RepoRoot = $PSScriptRoot,

    # Nur die Datei anpassen, nichts hochladen.
    [switch] $NurErsetzen
)

$ErrorActionPreference = 'Stop'

function Write-TextNoBom {
    param(
        [Parameter(Mandatory = $true)] [string] $Path,
        [Parameter(Mandatory = $true)] [string] $Text
    )

    $utf8NoBom = New-Object System.Text.UTF8Encoding $false
    [System.IO.File]::WriteAllText($Path, $Text, $utf8NoBom)
}

function Schritt($text) {
    Write-Host ''
    Write-Host "== $text" -ForegroundColor Cyan
}

# --- Name pruefen ---------------------------------------------------------------
$GitHubName = $GitHubName.Trim()

if ($GitHubName -notmatch '^[A-Za-z0-9](?:[A-Za-z0-9]|-(?=[A-Za-z0-9])){0,38}$') {
    throw "'$GitHubName' sieht nicht nach einem GitHub-Benutzernamen aus. Erlaubt sind Buchstaben, Ziffern und Bindestriche."
}

if ($GitHubName -eq 'DEIN-GITHUB-NAME') {
    throw 'Bitte deinen echten GitHub-Namen angeben, nicht den Platzhalter.'
}

# --- 1. Git vorhanden? ----------------------------------------------------------
Schritt 'Git suchen'

$git = Get-Command git -ErrorAction SilentlyContinue
if ($null -eq $git) {
    Write-Host 'Git ist nicht installiert.' -ForegroundColor Yellow
    Write-Host 'Installieren mit:  winget install --id Git.Git -e'
    Write-Host 'Danach PowerShell neu oeffnen und dieses Skript nochmal starten.'
    return
}

Write-Host "gefunden: $($git.Source)"

# --- 2. Platzhalter ersetzen ----------------------------------------------------
Schritt 'repo.json anpassen'

$repoPath = Join-Path $RepoRoot 'repo.json'
if (-not (Test-Path $repoPath)) {
    throw "repo.json liegt nicht in $RepoRoot"
}

$text = Get-Content $repoPath -Raw
$offen = ([regex]::Matches($text, 'DEIN-GITHUB-NAME')).Count

if ($offen -gt 0) {
    $text = $text.Replace('DEIN-GITHUB-NAME', $GitHubName)

    if ($RepoName -ne 'DalamudPlugins') {
        $text = $text.Replace('/DalamudPlugins/', "/$RepoName/")
        $text = $text.Replace("/$GitHubName/DalamudPlugins`"", "/$GitHubName/$RepoName`"")
    }

    Write-TextNoBom -Path $repoPath -Text $text
    Write-Host "$offen Stellen ersetzt durch '$GitHubName'."
}
else {
    Write-Host 'Kein Platzhalter mehr drin, bleibt wie es ist.'
}

# Gegenprobe: ist das noch gueltiges JSON?
try {
    $eintraege = Get-Content $repoPath -Raw | ConvertFrom-Json
    Write-Host "repo.json ist gueltig, $(@($eintraege).Count) Plugins drin."
}
catch {
    throw "repo.json laesst sich nicht mehr lesen: $($_.Exception.Message)"
}

# --- 2b. Warnen, wo noch keine Zip liegt ----------------------------------------
$fehlend = @()
foreach ($eintrag in @($eintraege)) {
    $zip = Join-Path (Join-Path (Join-Path $RepoRoot 'plugins') $eintrag.InternalName) 'latest.zip'
    if (-not (Test-Path $zip)) {
        $fehlend += $eintrag.InternalName
    }
}

if ($fehlend.Count -gt 0) {
    Write-Host ''
    Write-Host "Noch ohne latest.zip: $($fehlend -join ', ')" -ForegroundColor Yellow
    Write-Host 'Die stehen spaeter zwar in der Liste, lassen sich aber nicht installieren.'
    Write-Host 'Bauen kannst du sie mit:  .\build-all.ps1 -DevRoot C:\dev'
}

if ($NurErsetzen) {
    Write-Host ''
    Write-Host 'Fertig (nur ersetzt, nichts hochgeladen).' -ForegroundColor Green
    return
}

# --- 3. Hochladen ---------------------------------------------------------------
Schritt 'Zu GitHub schieben'

Push-Location $RepoRoot
try {
    $url = "https://github.com/$GitHubName/$RepoName.git"

    if (-not (Test-Path (Join-Path $RepoRoot '.git'))) {
        & git init | Out-Null
        Write-Host 'Lokales Repository angelegt.'
    }

    # Name und Mail, falls noch nie gesetzt. Reine Beschriftung neben den Aenderungen.
    if ([string]::IsNullOrWhiteSpace((& git config user.name))) {
        & git config user.name $GitHubName
        Write-Host "user.name auf '$GitHubName' gesetzt (gilt nur in diesem Ordner)."
    }

    if ([string]::IsNullOrWhiteSpace((& git config user.email))) {
        & git config user.email "$GitHubName@users.noreply.github.com"
        Write-Host 'user.email auf die noreply-Adresse von GitHub gesetzt.'
    }

    # Fernziel eintragen oder korrigieren
    $vorhanden = & git remote
    if ($vorhanden -contains 'origin') {
        & git remote set-url origin $url
    }
    else {
        & git remote add origin $url
    }

    Write-Host "origin zeigt auf $url"

    & git add -A
    $status = & git status --porcelain

    if ([string]::IsNullOrWhiteSpace($status)) {
        Write-Host 'Nichts geaendert, es gibt nichts zu committen.'
    }
    else {
        & git commit -m 'Plugin-Repository aktualisiert' | Out-Null
        Write-Host 'Commit angelegt.'
    }

    # Erst jetzt umbenennen: vor dem ersten Commit gibt es den Branch noch nicht,
    # und aeltere Git-Versionen steigen dabei aus.
    & git branch -M main 2>$null | Out-Null

    Write-Host ''
    Write-Host 'Jetzt kommt der Push. Beim ersten Mal oeffnet sich ein Browserfenster' -ForegroundColor Yellow
    Write-Host 'zum Anmelden bei GitHub. Ein normales Passwort nimmt GitHub nicht mehr an.' -ForegroundColor Yellow
    Write-Host ''

    & git push -u origin main
    if ($LASTEXITCODE -ne 0) {
        throw 'Der Push ist fehlgeschlagen. Haeufigste Ursache: das Repository gibt es auf GitHub noch nicht, oder es heisst anders.'
    }
}
finally {
    Pop-Location
}

# --- 4. Fertig ------------------------------------------------------------------
$link = "https://raw.githubusercontent.com/$GitHubName/$RepoName/main/repo.json"

Write-Host ''
Write-Host 'Fertig.' -ForegroundColor Green
Write-Host ''
Write-Host 'Dieser Link kommt im Spiel in /xlsettings -> Experimental -> Custom Plugin Repositories:'
Write-Host ''
Write-Host "    $link" -ForegroundColor Cyan
Write-Host ''
Write-Host 'Er liegt jetzt auch in der Zwischenablage.'

Set-Clipboard -Value $link
