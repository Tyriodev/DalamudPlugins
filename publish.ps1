<#
    publish.ps1 - builds one plugin and files it into this repository.

    Usage, from inside the repository folder:

        .\publish.ps1 -Project C:\dev\Clutter\Clutter\Clutter.csproj

    What it does, in order:
      1. reads <Version> from the .csproj - that is the single source of truth
      2. writes the same value into the plugin manifest next to it
      3. builds Release
      4. copies the packaged latest.zip to plugins\<InternalName>\latest.zip
      5. updates AssemblyVersion and LastUpdate in repo.json

    Afterwards: git add . ; git commit ; git push - Dalamud offers the update
    as soon as the new repo.json is live.
#>

param(
    [Parameter(Mandatory = $true)]
    [string] $Project,

    [string] $RepoRoot = $PSScriptRoot
)

$ErrorActionPreference = 'Stop'

function Write-TextNoBom {
    <#
        Windows PowerShell 5.1 schreibt mit -Encoding UTF8 ein BOM an den Dateianfang.
        Dalamud laedt repo.json als Text und laesst sie durch den JSON-Leser laufen -
        und der stolpert ueber genau dieses unsichtbare Zeichen ("Unexpected character").
        Deswegen hier von Hand ohne BOM schreiben.
    #>
    param(
        [Parameter(Mandatory = $true)] [string] $Path,
        [Parameter(Mandatory = $true)] [string] $Text
    )

    $utf8NoBom = New-Object System.Text.UTF8Encoding $false
    [System.IO.File]::WriteAllText($Path, $Text, $utf8NoBom)
}


if (-not (Test-Path $Project)) {
    throw "Project file not found: $Project"
}

$projectFile = Resolve-Path $Project
$projectDir = Split-Path $projectFile -Parent
$projectName = [System.IO.Path]::GetFileNameWithoutExtension($projectFile)

# --- 1. version from the csproj -------------------------------------------------
$csproj = Get-Content $projectFile -Raw
$versionMatch = [regex]::Match($csproj, '<Version>\s*([^<]+?)\s*</Version>')
if (-not $versionMatch.Success) {
    throw "No <Version> element in $projectFile"
}

$version = $versionMatch.Groups[1].Value
Write-Host "Version from csproj: $version"

# --- 2. keep the manifest in step ----------------------------------------------
$manifestPath = Join-Path $projectDir "$projectName.json"
if (-not (Test-Path $manifestPath)) {
    throw "Manifest not found: $manifestPath"
}

$manifest = Get-Content $manifestPath -Raw | ConvertFrom-Json
$internalName = $manifest.InternalName
if ([string]::IsNullOrWhiteSpace($internalName)) {
    throw "Manifest has no InternalName: $manifestPath"
}

if ($manifest.AssemblyVersion -ne $version) {
    Write-Host "Manifest said $($manifest.AssemblyVersion), setting it to $version"
    $manifest.AssemblyVersion = $version
    Write-TextNoBom -Path $manifestPath -Text ($manifest | ConvertTo-Json -Depth 10)
}

# --- 3. build -------------------------------------------------------------------
Write-Host "Building $projectName..."
& dotnet build $projectFile -c Release
if ($LASTEXITCODE -ne 0) {
    throw "Build failed."
}

# --- 4. collect the packaged zip ------------------------------------------------
$binDir = Join-Path $projectDir 'bin'
$zip = Get-ChildItem -Path $binDir -Recurse -Filter 'latest.zip' -ErrorAction SilentlyContinue |
    Sort-Object LastWriteTime |
    Select-Object -Last 1

if ($null -eq $zip) {
    throw "No latest.zip found under $binDir - did DalamudPackager run?"
}

$targetDir = Join-Path (Join-Path $RepoRoot 'plugins') $internalName
New-Item -ItemType Directory -Force -Path $targetDir | Out-Null
Copy-Item $zip.FullName (Join-Path $targetDir 'latest.zip') -Force
Write-Host "Copied $($zip.FullName)"

# --- 5. update repo.json --------------------------------------------------------
$repoPath = Join-Path $RepoRoot 'repo.json'
if (-not (Test-Path $repoPath)) {
    throw "repo.json not found in $RepoRoot"
}

$entries = Get-Content $repoPath -Raw | ConvertFrom-Json
$entry = $entries | Where-Object { $_.InternalName -eq $internalName }

if ($null -eq $entry) {
    throw "repo.json has no entry with InternalName '$internalName'. Add one first."
}

$entry.AssemblyVersion = $version
$entry.LastUpdate = [string][DateTimeOffset]::UtcNow.ToUnixTimeSeconds()

Write-TextNoBom -Path $repoPath -Text (ConvertTo-Json -InputObject @($entries) -Depth 10)

Write-Host ""
Write-Host "$internalName $version is ready in the repository."
Write-Host "Next:  git add . ; git commit -m `"$internalName $version`" ; git push"
