## Singling 3.0.1
##### Jonathan Vasquez (fearedbliss)
##### Released on Wednesday, February 20, 2019

## Synopsis

A collection of non-gameplay modifications and fixes in
order to improve the Vanilla Diablo II Single Player Experience.

## Supported Versions

- **`1.00`**
- **`1.05b`**
- **`1.07`**
- **`1.09b`**
- **`1.10`**
- **`1.14d`**

## Features

- You can now run multiple clients of Diablo II.
- You are now able to quickly join LAN games.
- Fixed CPU usage bug in Main Menu, Single Player, and LAN games.
- The Battle.net button has been disabled for safety reasons.
- The introduction cinematics are now automatically skipped.

- **`[1.00-1.07]`** The **`players`** command is now implemented (up to 8 players).

- **`[1.00-1.10]`** You no longer need the CD in order to play the game.
- **`[1.00-1.10]`** You can now make Hardcore characters without beating Softcore.
- **`[1.00-1.10]`** The window will no longer minimize when you click out of it.
- **`[1.00-1.10]`** The window will now have Minimize and Close buttons.

- **`[1.10-1.14d]`** Battle.net-only Runewords are now enabled on Single Player.

## Notes

- The Main Menu CPU fix will only be applied for **`v1.10+`**, since people not
  using Glide in versions before that would experience massive lag. Most people
  don't spend their time sitting in the Main Menu so this isn't a big deal.

- If you are using Glide, make sure to disable VSYNC in your Glide settings
  or you will still experience high CPU usage in LAN games.

- Due to a Windows bug, the Minimize/Close buttons will not show if you are
  using Glide for versions below **`1.14d`**.
  
- The **`players`** command works similarly to **`1.09b`**. You type **`players #`**
  with no slash and the game will simulate that. No confirmation will be displayed.
  If you are in a LAN game, the host will need to set the **`players #`** explicitly.
  The Monster's HP, Experience, and Item Drops (Including Chests) will be affected.
  
  For versions before **`1.07`**, the game was designed with monsters and chests
  having a chance to drop only a single item, and chests are not affected by
  player count at all. Monsters however will have an increased chance of dropping
  a single item for every additional player in the game (up to players 5, everything
  after that player count doesn't matter).

## Patch Rational

#### Patch 1.00

This patch was selected due to it simply being the first version of Diablo II released.
There are a few things this patch has that you can't do in **`1.05b`**, however it also
is obviously an unpolished version with many bugs and some crashes.

#### Patch 1.05b

This patch was picked over **`1.06`** since the only reason **`1.06`** was released
was to implement anti-duping code. The dupes that the game would delete on Single Player
could have been legitimately found. This is because the game sometimes assigns the same
fingerprint to a new drop. It was also kept because it is the most stable version of the game
before **`Lord of Destruction`**.

#### Patch 1.07

This patch was selected because it is the first version of **`Lord of Destruction`**
that was released. Thus, it contains a lot of features that were immediately patched out
in Patches **`1.08`** and **`1.09`**.

#### Patch 1.09b

This patch was picked over **`1.09d`** because it contains **`players 64`** and
also working CtC. **`1.09d`** has broken CtC which means that you will
see the animation of your CtC effect, but it actually won't do anything.

#### Patch 1.10

This patch was picked because it was the last patch released by Blizzard North,
it was the first patch in the series that massively redesigned the game and made
it primarily balanced for Multiplayer, and lastly it is the patch that has the
most mod support.

#### Patch > 1.10

Since nothing major has been implemented in the game (other than respec and uber trist) after
version **`1.10`**, the latest version of the game will be used.
