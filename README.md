# Marcels Plugin-Repository

Hier liegt alles, was Dalamud braucht, um deine sieben Plugins wie ganz normale Plugins
anzubieten: mit Installieren-Knopf, mit Update-Hinweis, mit Icon in der Liste.

Das Ganze ist am Ende **ein einziger Link**, den du in Dalamud einträgst. Dieser Link zeigt
auf `repo.json`, und darin steht für jedes Plugin, wo seine `latest.zip` liegt.

---

## Was in diesen Ordner gehört

```
repo.json                      Der Katalog. Das ist der Link, den du einträgst.
setup.ps1                      Einmalige Einrichtung: Name eintragen, Git, erster Push.
build-all.ps1                  Baut alle Plugins und legt sie hier ab.
publish.ps1                    Baut ein einzelnes Plugin (wird von build-all aufgerufen).
push.ps1                       Änderungen hochladen.
images/<Name>.png              Icons für die Plugin-Liste (7 Stück, schon da)
plugins/<Name>/latest.zip      Das gebaute Plugin. Musst du noch hineinlegen.
```

Die Ordner unter `plugins/` sind schon angelegt und enthalten jeweils eine
`PLATZHALTER.md`. Die kannst du löschen, sobald die echte `latest.zip` drin liegt.

---

## Teil 1 — Einmalig: Repository anlegen

1. Konto auf **github.com** anlegen, falls noch nicht geschehen. Kostenlos.
2. Oben rechts auf **+** → **New repository**.
3. **Repository name:** `DalamudPlugins`
   (Wenn du einen anderen Namen willst: gut, aber dann musst du ihn gleich in Teil 2
   überall mitändern.)
4. **Public** auswählen. Das ist wichtig — bei einem privaten Repository kommt Dalamud
   nicht an die Dateien und zeigt dir nur eine Fehlermeldung.
5. Haken bei „Add a README file" **weg**, sonst gibt es gleich einen Konflikt.
6. **Create repository**.

---

## Teil 2 — Deinen Namen eintragen

> Das erledigt `setup.ps1` für dich (siehe Teil 4). Dieser Abschnitt beschreibt, was
> dabei passiert — und den Weg von Hand, falls du lieber selbst in die Datei schaust.

In `repo.json` steht an 35 Stellen der Platzhalter `DEIN-GITHUB-NAME`. Der muss durch
deinen echten GitHub-Benutzernamen ersetzt werden — genau so geschrieben wie oben in der
Adresszeile, Groß- und Kleinschreibung egal, aber ohne Leerzeichen.

Öffne `repo.json` in einem Editor (Notepad reicht, besser Notepad++ oder VS Code) und
mach ein **Suchen & Ersetzen über die ganze Datei**:

```
Suchen:   DEIN-GITHUB-NAME
Ersetzen: marcel123          <- dein echter Name
```

Danach einmal kurz kontrollieren, dass eine Zeile so aussieht:

```json
"DownloadLinkInstall": "https://raw.githubusercontent.com/marcel123/DalamudPlugins/main/plugins/Glacier/latest.zip",
```

---

## Teil 3 — Die Plugins bauen und ablegen

> Das erledigt `build-all.ps1` für dich (siehe Teil 4). Hier steht, was es tut.

Für jedes Plugin einmal:

1. Im Plugin-Ordner bauen:
   ```powershell
   dotnet build -c Release
   ```
2. Die entstandene Datei suchen:
   ```powershell
   dir -Recurse -Filter latest.zip bin
   ```
   Sie liegt unter `bin\Release\net10.0-windows\<Name>\latest.zip`.
3. Diese `latest.zip` nach `plugins\<Name>\latest.zip` in diesem Repository kopieren.

Wenn alle sieben drin sind, sieht es so aus:

```
plugins/WayfarersDiary/latest.zip
plugins/ReturnersNote/latest.zip
plugins/MarketPager/latest.zip
plugins/Clutter/latest.zip
plugins/LevelForecast/latest.zip
plugins/SessionPlanner/latest.zip
plugins/Glacier/latest.zip
```

Du musst nicht alle sieben auf einmal machen. Ein Plugin, dessen `latest.zip` noch fehlt,
wird in der Liste angezeigt, lässt sich aber nicht installieren. Wenn dich das stört, setze
bei diesem Eintrag in `repo.json` vorübergehend `"IsHide": true`.

---

## Teil 4 — Hochladen

### Vorbereitung: PowerShell darf Skripte ausführen

Zwei Dinge sperrt Windows standardmäßig, und beides einmalig in PowerShell lösen:

```powershell
# 1. Skripte aus eigenen Dateien erlauben (gilt nur für deinen Benutzer)
Set-ExecutionPolicy -Scope CurrentUser RemoteSigned

# 2. Die Markierung "aus dem Internet geladen" von den Skripten nehmen
cd C:\dev\DalamudPlugins        # dorthin, wo du dieses Paket entpackt hast
Unblock-File .\*.ps1
```

Ohne Schritt 2 bekommst du sonst „kann nicht geladen werden, da die Ausführung von
Skripts auf diesem System deaktiviert ist", obwohl die Policy stimmt — das liegt an der
Internet-Markierung, nicht an der Policy.

Git brauchst du außerdem:

```powershell
winget install --id Git.Git -e
```

Danach PowerShell einmal neu öffnen, sonst kennt sie `git` noch nicht.

### Der ganze Vorgang in drei Befehlen

Das GitHub-Repository muss vorher im Browser existieren (Teil 1). Dann, aus dem Ordner
dieses Pakets heraus:

```powershell
# 1. Alle sieben Plugins bauen und hier ablegen
.\build-all.ps1 -DevRoot C:\dev

# 2. Namen eintragen, Git einrichten, hochladen, Link ausgeben
.\setup.ps1 -GitHubName marcel123
```

`-DevRoot` ist der Ordner, unter dem deine Plugin-Projekte liegen. Das Skript sucht
darunter rekursiv nach `Glacier.csproj`, `Clutter.csproj` und so weiter — wie tief sie
liegen, ist egal, Build-Ordner werden übersprungen.

Am Ende steht der fertige Dalamud-Link auf dem Bildschirm und liegt in der
Zwischenablage.

### Ab dann: jedes weitere Update

```powershell
.\build-all.ps1 -DevRoot C:\dev -Nur Glacier
.\push.ps1 -Text "Glacier 0.4.1"
```

`push.ps1` zeigt vorher, was sich geändert hat, und nennt hinterher wieder den Link.
Ohne `-Text` schreibt es selbst eine Beschriftung mit Datum und allen Versionsnummern.

### Was die Skripte tun

| Skript | Aufgabe |
| --- | --- |
| `setup.ps1` | Platzhalter ersetzen, `git init`, Fernziel eintragen, erster Push, Link ausgeben |
| `build-all.ps1` | Projekte suchen, für jedes `publish.ps1` aufrufen, am Ende eine Übersicht |
| `publish.ps1` | Ein Plugin: Version lesen, ins Manifest schreiben, bauen, Zip kopieren, `repo.json` nachziehen |
| `push.ps1` | Änderungen zeigen, committen, pushen, Link ausgeben |

Alle vier kann man gefahrlos mehrfach laufen lassen. `setup.ps1` merkt, wenn der
Platzhalter schon ersetzt oder das Repository schon eingerichtet ist, und überspringt den
Teil. Wenn du nur die Datei anpassen und noch nichts hochladen willst:

```powershell
.\setup.ps1 -GitHubName marcel123 -NurErsetzen
```

### Ohne PowerShell, über die Webseite

Geht genauso, ist beim ersten Mal sogar schneller:

1. Im leeren Repository auf **uploading an existing file** klicken.
2. Den **Inhalt** dieses Ordners ins Browserfenster ziehen — also `repo.json`,
   die `.ps1`-Dateien, `README.md`, und die Ordner `images` und `plugins`.
   Nicht den Ordner selbst, sondern das, was darin liegt.
3. Unten **Commit changes**.

Den Platzhalter in `repo.json` musst du dann von Hand ersetzen, vor dem Hochladen.

### Zu `git config user.name`

Taucht in jeder Git-Anleitung auf, deswegen kurz: das sind reine Beschriftungen, die
neben jeder Änderung stehen. Mit dem Login haben sie nichts zu tun. `setup.ps1` setzt sie
selbst, falls sie noch nie gesetzt wurden — und zwar auf die noreply-Adresse
`deinname@users.noreply.github.com`, damit deine echte Mailadresse nicht öffentlich im
Verlauf steht.

Beim ersten `push` fragt Git nach Zugangsdaten. Ein normales Passwort nimmt GitHub nicht
mehr an; es öffnet sich stattdessen ein Browserfenster zum Anmelden, das reicht.

## Teil 5 — Den Link in Dalamud eintragen

Dein Link lautet:

```
https://raw.githubusercontent.com/DEIN-GITHUB-NAME/DalamudPlugins/main/repo.json
```

Prüfe ihn zuerst im Browser. Du musst dort nackten JSON-Text sehen, der mit `[` anfängt.
Siehst du stattdessen die GitHub-Oberfläche mit Zeilennummern drumherum, hast du den
falschen Link erwischt — achte auf **raw.githubusercontent.com**, nicht `github.com`.

Dann im Spiel:

1. `/xlsettings` eintippen.
2. Reiter **Experimental**.
3. Unter **Custom Plugin Repositories** den Link einfügen.
4. Haken daneben setzen, damit er aktiv ist.
5. **Save and Close**.
6. `/xlplugins` öffnen. Deine sieben stehen jetzt in der Liste, mit Icon.

---

## Teil 6 — Ein Update veröffentlichen

Das ist der Teil, den du danach immer wieder machst. **Entscheidend ist nur eines:
Dalamud erkennt ein Update ausschließlich daran, dass `AssemblyVersion` in `repo.json`
größer geworden ist.** Eine neue Zip mit gleicher Nummer wird niemals ausgeliefert.

Von Hand:

1. Im Plugin die `<Version>` in der `.csproj` hochzählen, z. B. `0.4.0.0` → `0.4.1.0`.
2. Dieselbe Nummer in die `<Name>.json` des Plugins schreiben (`AssemblyVersion`).
3. `dotnet build -c Release`, neue `latest.zip` nach `plugins\<Name>\` kopieren.
4. In `repo.json` beim passenden Eintrag `AssemblyVersion` auf dieselbe Nummer setzen.
5. Hochladen.

Oder mit den Skripten, die die Schritte 2 bis 5 selbst erledigen:

```powershell
.\build-all.ps1 -DevRoot C:\dev -Nur Glacier
.\push.ps1 -Text "Glacier 0.4.1"
```

Das Skript liest die `<Version>` aus der `.csproj` als einzige Wahrheit, schreibt sie ins
Manifest, baut, kopiert die Zip hierher und trägt Version und Zeitstempel in `repo.json`
ein. Du zählst also nur noch **eine** Zahl an **einer** Stelle hoch.

Im Spiel dauert es danach ein paar Minuten, bis GitHub den neuen Stand ausliefert.
Wenn es eilig ist: im Plugin-Fenster oben auf das Aktualisieren-Symbol, oder einmal
aus- und wieder einloggen.

---

## Muss mein PC laufen, damit das funktioniert?

Nein. GitHub speichert die Dateien auf eigenen Servern. Sobald du hochgeladen hast, kann
der Link von überall abgerufen werden, auch wenn dein Rechner aus ist. Dein PC wird nur
zum **Bauen** gebraucht.

Hochladen und Bearbeiten geht auch vom Tablet oder Handy über die GitHub-Webseite —
bauen allerdings nicht, dafür brauchst du Windows mit dem .NET SDK.

---

## Wenn etwas nicht klappt

| Symptom | Ursache |
| --- | --- |
| Dalamud zeigt das Repository rot / „failed" | Link zeigt nicht auf `raw.githubusercontent.com`, oder das Repository ist privat. |
| Liste bleibt leer | `repo.json` ist kein gültiges JSON. Einmal durch einen JSON-Prüfer schicken — meist ein Komma zu viel oder zu wenig. |
| Liste bleibt leer, JSON sieht aber korrekt aus | Ein unsichtbares BOM am Dateianfang, das Windows PowerShell beim Speichern mit `-Encoding UTF8` einfügt. Die mitgelieferten Skripte schreiben bewusst ohne BOM. Wenn du die Datei selbst in einem Editor bearbeitest: als „UTF-8 ohne BOM" speichern. |
| `.\setup.ps1` lässt sich nicht starten | Execution Policy oder Internet-Markierung. Siehe Teil 4 oben: `Set-ExecutionPolicy -Scope CurrentUser RemoteSigned` und `Unblock-File .\*.ps1`. |
| Plugin erscheint, Installieren schlägt fehl | Unter `plugins/<Name>/` liegt keine `latest.zip`, oder der Name ist anders geschrieben als `InternalName`. Groß- und Kleinschreibung zählt. |
| Update wird nicht angeboten | `AssemblyVersion` in `repo.json` ist unverändert. |
| Falsches Icon oder keins | `images/<Name>.png` fehlt oder heißt anders. Die Datei muss quadratisch sein. |

---

## Noch ein Hinweis

Dieses Repository ist deine private Verteilstelle. Es hat nichts mit dem offiziellen
Dalamud-Repository zu tun, es wird von niemandem geprüft, und es gelten keine fremden
Regeln — es ist deine Sammlung, die du dir selbst installierst. Wer den Link hat, kann
die Plugins allerdings ebenfalls installieren, also lade nichts hoch, was du nicht
weitergeben willst.
