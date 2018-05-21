Cactus 1.0.2
Jonathan Vasquez (fearedbliss) <jon@xyinn.org>
Apache License 2.0
Monday, May 21, 2018
------------------------------------------------

Description
---------------
This is a C#/.NET based application that will help you manage/launch
multiple versions of Diablo II from a single window.

This means you can easily install and play every single version of Diablo II
from 1.00 to the latest 1.14d (and any other future versions) while maximizing
your disk space (Since you won't have to keep having multiple copies of your MPQs),
and having complete character isolation.

Requirements
---------------
- .NET Framework 4.6.1 +

Installation/Instructions
---------------
1. Place the "Cactus.exe" and all of the .dlls in the "Release" folder into the
   root of your Diablo II directory. If you don't do this, you will have issues
   with using any 'data' folders (that are used with -direct -txt), and also
   any battle.net updates you do will fail with an error saying that your
   "D2XP_IX86_1xx_114a.mpq" file is corrupted. You don't need to copy any .pdb, .xml,
   or any other files since those are only used for debugging purposes.

2. Run Cactus

3. Click "Add"

The first version that you add AND run must be the version that's currently
in your Diablo II directory. So for example, if my D:\Games\Diablo II is where
I installed Diablo II, and it currently has 1.14d, I would want to add, and run
this version immediately through Cactus. This allows Cactus to register everything
correctly so that things can switch properly.

My first version will look like this:

Label: "1.14d Vanilla"
Version: "1.14d"
Path: "D:\Games\Diablo II\Game.exe"
Flags: "-w -ns"
Expansion: [Checked]

What this says is that I want to run version 1.14d of the game,
and the files that I will be using are located in a folder called,
"1.14d Vanilla" inside of the "Expansion" sub directory (The check box controls
where it looks in Classic/ or Expansion folders). The flags the game will be
launched are "-w -ns" and the path of main executable is "D:\Games\Diablo II\Game.exe".
What Cactus does is that when it switches between versions, it deletes all of the files
that the old entry requires from the "D:\Games\Diablo II\ directory, and then it goes
to the directory with the Label, and copies the requires files for that particular
version to the "D:\Games\Diablo II\" directory.

If it's the first time you are running the game and you don't have the files backed up
for this version, Cactus will automatically create a folder and copy all of those files
over.

4. Click the new entry and click Launch.

5. Enjoy.

Playing Previous Versions
---------------
- In order to play versions before 1.12, there is nothing you need to do other than
  install the game with a 1.00, 1.03, or 1.07 disk (Basically any installer before
  the 1.12+ bnet installer).
  
  If you installed D2 with the Battle.net installer, you can install Singling
  (or do it manually by following the video below) which has the dlls that
  includes the fix.
  
  Corresponding video tutorial:
  https://www.youtube.com/watch?v=tz1J4FyuBHE
  
Useful Additions
---------------
- Bliss Complete Collection - https://xyinn.org/diablo/Bliss_Complete_Collection.7z

  Contains:
  > All the versions of Diablo II from 1.00 - 1.14d.
  > Singling - No Gameplay Changes. Only Game Fixes and Minor Enhancements
               [Including Multiple D2s, No CD, Allow Hardcore, Etc]
  > Vanilla Frosting - My Diablo II Single Player mod that merges the best from all the versions in a balanced way.
  > GlideWrapper