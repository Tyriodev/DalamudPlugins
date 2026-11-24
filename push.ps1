<#
    push.ps1 - schiebt den aktuellen Stand zu GitHub.

    Aufruf, aus diesem Ordner heraus:

        .\push.ps1
        .\push.ps1 -Text "Glacier 0.4.1"

    Prueft zweierlei: ob Dateien geaendert sind, UND ob noch Commits unverschickt
    sind. Lehnt GitHub ab, weil dort etwas liegt, das lokal fehlt (typisch nach einer
    Aenderung im Browser), holt das Skript den Stand selbst und versucht es erneut.
#>

param(
    [string] $Text,

    [string] $RepoRoot = $PSScriptRoot
)

$ErrorActionPreference = 'Stop'

if (-not (Test-Path (Join-Path $RepoRoot '.git'))) {
    throw 'Hier ist noch kein Git-Repository. Einmal .\setup.ps1 -GitHubName DEINNAME laufen lassen.'
}

function Get-Ahead {
    <# Wie viele Commits liegen lokal, die GitHub noch nicht hat? #>
    $upstream = & git rev-parse --abbrev-ref --symbolic-full-name '@{u}' 2>$null
    if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($upstream)) {
        return -1   # Fernziel noch nie gesetzt
    }

    $count = & git rev-list --count '@{u}..HEAD' 2>$null
    if ($LASTEXITCODE -ne 0) {
        return -1
    }

    return [int]$count
}

Push-Location $RepoRoot
try {
    $status = & git status --porcelain
    $ahead = Get-Ahead

    if ([string]::IsNullOrWhiteSpace($status) -and $ahead -eq 0) {
        Write-Host 'Nichts geaendert und nichts offen. GitHub hat bereits den aktuellen Stand.' -ForegroundColor Green
        return
    }

    if (-not [string]::IsNullOrWhiteSpace($status)) {
        Write-Host 'Geaendert:' -ForegroundColor Cyan
        $status | ForEach-Object { Write-Host "  $_" }
        Write-Host ''

        if ([string]::IsNullOrWhiteSpace($Text)) {
            $eintraege = @(Get-Content (Join-Path $RepoRoot 'repo.json') -Raw | ConvertFrom-Json)
            $Text = 'Stand ' + (Get-Date -Format 'yyyy-MM-dd HH:mm') +
                    ' (' + (($eintraege | ForEach-Object { "$($_.InternalName) $($_.AssemblyVersion)" }) -join ', ') + ')'
        }

        & git add -A
        & git commit -m $Text | Out-Null
        Write-Host 'Commit angelegt.'
    }
    else {
        Write-Host "Keine neuen Aenderungen, aber $ahead Commit(s) warten noch auf GitHub." -ForegroundColor Cyan
    }

    # --- hochladen, und bei Ablehnung einmal nachholen ---------------------------
    if ($ahead -lt 0) {
        & git push -u origin main
    }
    else {
        & git push
    }

    if ($LASTEXITCODE -ne 0) {
        Write-Host ''
        Write-Host 'GitHub hat abgelehnt. Meist liegt dort etwas, das lokal fehlt -' -ForegroundColor Yellow
        Write-Host 'zum Beispiel eine Datei, die du im Browser bearbeitet hast. Ich hole das jetzt.' -ForegroundColor Yellow
        Write-Host ''

        & git pull --rebase
        if ($LASTEXITCODE -ne 0) {
            throw 'Das Zusammenfuehren hat nicht geklappt. Wahrscheinlich ein Konflikt - "git status" zeigt, welche Datei.'
        }

        & git push
        if ($LASTEXITCODE -ne 0) {
            throw 'Der Push ist auch nach dem Nachholen fehlgeschlagen. Steht die Anmeldung bei GitHub noch?'
        }
    }

    $url = & git remote get-url origin
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
