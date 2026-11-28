@echo off
rem Prueft alle Plugins gegen die Attrappen. Braucht nur das .NET SDK, kein Spiel.
setlocal enabledelayedexpansion
cd /d "%~dp0"

set PLUGINS=Glacier Clutter MarketPager LevelForecast ReturnersNote WayfarersDiary SessionPlanner
if not "%~1"=="" set PLUGINS=%~1

set FEHLER=0

for %%P in (%PLUGINS%) do (
    echo.
    echo === %%P
    dotnet build check.csproj -p:Plugin=%%P --nologo -v quiet 2>&1 | findstr /C:"error" && set FEHLER=1
    if not errorlevel 1 echo    in Ordnung
)

echo.
if "%FEHLER%"=="1" (
    echo Es gibt Fehler. Siehe oben.
) else (
    echo Alles uebersetzt sauber.
)
pause
