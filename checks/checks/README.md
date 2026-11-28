# Typprüfung ohne Spiel

Übersetzt die Plugin-Quellen gegen nachgebaute Dalamud-, ImGui-, Lumina- und
NPSMLib-Typen. Dafür müssen weder das Spiel noch Dalamud installiert sein.

```
pruefen.cmd                 alle sieben
pruefen.cmd Glacier         nur eins
```

Oder von Hand:

```
dotnet build check.csproj -p:Plugin=Glacier
```

Erwartet die Plugin-Ordner eine Ebene über dem Repository, also `C:\dev\Glacier\Glacier`
wenn dieser Ordner `C:\dev\DalamudPlugins\checks` ist. Liegt es bei dir anders:

```
dotnet build check.csproj -p:Plugin=Glacier -p:Dev=C:\irgendwo
```

## Wozu das gut ist

Für dich ist es Beiwerk — du hast Dalamud installiert, dein `dotnet build` prüft
ohnehin gegen das echte Ding und prüft es besser.

Der Punkt ist ein anderer: **ich kann hier nicht gegen Dalamud bauen.** In diesem
Projekt sind dir deshalb dreimal Dateien zugestellt worden, die bei dir nicht
übersetzt haben — vier Fehler in Glacier, fünf in Returner's Note, und einmal eine
Datei, in die beim Einfügen eine Zeile an die falsche Stelle geraten war. Mit diesem
Ordner kann ich das vorher sehen.

Deshalb liegt er im Repository und nicht auf meiner Seite: so ist er in jeder
künftigen Sitzung da.

## Was es beweist und was nicht

**Beweist:** dass die Quellen syntaktisch heil sind, dass alle Namen existieren, dass
Typen, Argumentzahlen und Nullbarkeit zusammenpassen, und dass nichts mehr auf etwas
verweist, das es nicht gibt. Genau die Fehlerklasse, die oben dreimal durchgerutscht
ist.

**Beweist nicht,** dass Dalamud wirklich so aussieht. Die Attrappen sind nachgebaut:
teils aus der Dokumentation auf dalamud.dev, teils aus dem, was die Plugins benutzen
und was bei dir schon einmal übersetzt hat. Wenn Dalamud eine Eigenschaft `ushort`
nennt, wo hier `int` steht, merkt es dieser Ordner nicht. Für den üblichen Gebrauch
im Plugin macht das keinen Unterschied, aber eine Garantie ist es nicht. Und ob ein
Plugin im Spiel das Richtige tut, sagt eine Typprüfung ohnehin nicht.

Wenn eine Prüfung etwas meldet, das bei dir baut, ist die Attrappe falsch, nicht dein
Code. Dann gehört die Attrappe korrigiert.

## Aufbau

```
Stubs/Dalamud.cs      Dienste, Plugin-Schnittstelle, Inventar, Texturen
Stubs/Game.cs         Spielobjekte, Gruppe, Statuseffekte
Stubs/JobGauges.cs    die 21 Job-Anzeigen, erzeugt aus Glaciers Nutzung
Stubs/ImGui.cs        die ImGui-Oberfläche und das Fenstergerüst
Stubs/Lumina.cs       die Spieltabellen
Stubs/Npsm.cs         die Medienwiedergabe von Windows
```

Neues Plugin, neue API: der Compiler sagt, was fehlt, und es kommt in die passende
Datei. Die Job-Anzeigen sind mit einem Skript aus `JobGauge.cs` erzeugt worden, damit
keine vergessen wird.
