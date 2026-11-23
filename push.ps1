<#
    push.ps1 - schiebt den aktuellen Stand zu GitHub.

    Aufruf, aus diesem Ordner heraus:

        .\push.ps1
        .\push.ps1 -Text "Glacier 0.4.1"

    Zeigt vorher, was sich geaendert hat, und nennt hinterher den Link fuer Dalamud.
#>

param(
    [string] $Text,

    [string] $RepoRoot = $PSScriptRoot
)

$ErrorActionPreference = 'Stop'

if (-not (Test-Path (Join-Path $RepoRoot '.git'))) {
    throw 'Hier ist noch kein Git-Repository. Einmal .\setup.ps1 -GitHubName DEINNAME laufen lassen.'
}

Push-Location $RepoRoot
try {
    $status = & git status --porcelain

    if ([string]::IsNullOrWhiteSpace($status)) {
        Write-Host 'Nichts geaendert. GitHub hat bereits den aktuellen Stand.' -ForegroundColor Green
        return
    }

    Write-Host 'Geaendert:' -ForegroundColor Cyan
    $status | ForEach-Object { Write-Host "  $_" }
    Write-Host ''

    # Ohne eigenen Text: die Versionen aus repo.json als Beschriftung nehmen.
    if ([string]::IsNullOrWhiteSpace($Text)) {
        $eintraege = @(Get-Content (Join-Path $RepoRoot 'repo.json') -Raw | ConvertFrom-Json)
        $Text = 'Stand ' + (Get-Date -Format 'yyyy-MM-dd HH:mm') +
                ' (' + (($eintraege | ForEach-Object { "$($_.InternalName) $($_.AssemblyVersion)" }) -join ', ') + ')'
    }

    & git add -A
    & git commit -m $Text | Out-Null
    & git push

    if ($LASTEXITCODE -ne 0) {
        throw 'Der Push ist fehlgeschlagen. Steht die Anmeldung bei GitHub noch?'
    }

    $url = & git remote get-url origin
    # Das ':' mitteilen, damit auch eine SSH-Adresse (git@github.com:name/repo) passt.
    $teile = ($url -replace '\.git$', '' -replace ':', '/') -split '/'
    $name = $teile[-2]
    $repo = $teile[-1]

    Write-Host ''
    Write-Host 'Hochgeladen.' -ForegroundColor Green
    Write-Host ''
    Write-Host 'Link fuer Dalamud:'
    Write-Host "    https://raw.githubusercontent.com/$name/$repo/main/repo.json" -ForegroundColor Cyan
    Write-Host ''
    Write-Host 'Im Spiel dauert es ein paar Minuten, bis GitHub den neuen Stand ausliefert.'
    Write-Host 'Schneller: im Plugin-Fenster oben auf Aktualisieren, oder einmal aus- und einloggen.'
}
finally {
    Pop-Location
}
