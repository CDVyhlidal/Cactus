Cactus 0.0.1
Jonathan Vasquez (fearedbliss) <jon@xyinn.org>
Apache License 2.0
Friday, May 18, 2018
------------------------------------------------

Description
---------------
This is a C#/.NET based application that will help you manage/launch multiple versions of Diablo II from a single window.

This means you can easily install and play every single version of Diablo II from 1.00 to the latest 1.14d (and any other future versions)
while maximizing your disk space (Since you won't have to keep having multiple copies of your MPQs), and having complete character isolation.

Requirements
---------------
- .NET Framework 4.6.1 +
- In order to play versions before 1.12, there is nothing you need to do other than install the game with a 1.00, 1.03, or 1.07 disk
  (Basically any installer before the 1.12+ bnet installer).
  
  If you installed D2 with the bnet installer, you will need to read the following thread to get it to work with older versions:
  https://www.diabloii.net/forums/threads/tech-support-installing-from-bnet-1-14d-installer-doesnt-allow-switching-back-to-1-13d.960439/
  
  Corresponding video tutorial:
  https://www.youtube.com/watch?v=tz1J4FyuBHE
  
Useful Additions
---------------
- Bliss Complete Collection - https://xyinn.org/diablo/Bliss_Complete_Collection.7z
  > Contains all the versions of Diablo II from 1.00 - 1.14d.
  > Contains Singling [Game Fixes and Minor Enhancements [Including Multiple D2s, No CD, Allow Hardcore, Etc]
  > GlideWrapper
  > Additional Utilities

- Singling - https://github.com/fearedbliss/singling

Instructions
---------------
1. Run the "Cactus.exe" that is located in the "Release" folder.
2. Click "Add"

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

3. Click the new entry and click Launch.

4. Enjoy.
