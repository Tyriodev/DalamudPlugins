# My FFXIV plugins

Seven Dalamud plugins I wrote for myself. They live in one repo so I don't have to
reinstall them by hand every time I rebuild the game. If you want them, help yourself.

## Installing

Copy this:

```
https://raw.githubusercontent.com/Tyriodev/DalamudPlugins/main/repo.json
```

In game, type `/xlsettings`, open the **Experimental** tab, paste the link under
**Custom Plugin Repositories**, tick the box next to it, save. They'll show up in
`/xlplugins` after that.

## What's in here

### Glacier — `/glacier`

A HUD of my own, because the default one has looked the same for a decade. Health with
shields, mana, target and its cast bar, focus target, party with role colours and job
icons, an alliance list, an enemy list, status effects with timers, and a job gauge that
follows whatever you're playing. Four colour themes, or pick every colour yourself.

It draws on top of the game instead of replacing anything, so nothing can break. You hide
the old bars yourself in the HUD layout editor and the new ones take over. Health bars
leave a pale trail behind a hit so you can see how much was taken, not just what's left,
and the cast bar marks the half second at the end where you can already move.

Layouts save as profiles and travel as a single line of text.

### Wayfarer's Diary — `/diary`

Keeps track of where the time actually goes. Which zones, which activities, how long the
session ran. I built it after wondering why an evening had vanished with nothing to show
for it. Turns out I'd spent two hours standing in Limsa.

### Returner's Note — `/comeback`

For coming back after a break. Leave yourself a note before you stop playing, and it's
waiting when you return, along with what you'd planned to do next and a checklist that
gets longer the longer you were gone.

### Session Planner — `/plan`

I have forty minutes. What actually fits? It times your duties and your queues while you
play, so after a while it knows how long things really take for you rather than what some
guide claims.

### Level Forecast — `/xp`

Measures how fast you're actually gaining XP right now and tells you when you'll hit the
next level at that rate. Useful for deciding whether to do one more run or call it a
night.

### Market Pager — `/pager`

Watches the market board through Universalis and sends your phone a message when something
you want drops below your price, or when someone undercuts your listing. Works with ntfy,
Pushover, or any webhook. Quiet hours so it doesn't wake you up.

### Clutter — `/clutter`

Goes through your inventory and sorts everything into keep, crafting material, outgrown,
sell, junk, or not sure. The last one matters: it says so instead of guessing, because
I'd rather check twice than vendor something I needed.

## Some things worth saying

I wrote these for my own game. They work for me; I can't promise they work for you.
There's no support and no schedule, and if one of them breaks after a patch I'll fix it
when I feel like it.

None of them automate anything. No combat help, no doing things for you. They read what
the game already shows and display it differently, or keep notes.

Dalamud plugins are third-party software and have nothing to do with Square Enix.

Setup notes for my own machine are in [ANLEITUNG.md](ANLEITUNG.md), in German.
