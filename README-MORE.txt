More Information
================

Cactus File Structure
---------------
The directory structure that Cactus uses is as follows:

Diablo II/                        - Your Diablo II Root Directory
Diablo II/Entries.json            - Your settings will be saved here.

Diablo II/Classic                 - All your classic versions will be in here
Diablo II/Classic/<label>         - Location of your Diablo II files for this version (Storage Directory)
Diablo II/Classic/<label>/data    - Data folder (If you have one)
Diablo II/Classic/<label>/save    - Location of your save file for this specific version of classic

The same applies for expansion:

Diablo II/Expansion
Diablo II/Expansion/<label>
Diablo II/Expansion/<label>/data
Diablo II/Expansion/<label>/save

Moving Existing Characters and DLLs to new locations:
---------------
If you have any existing characters, just take the characters from your
specific version's save folder and put it into its new location (Example,
if you already had some 1.07 expansion characters, you can place them in:
C:\Program Files\Diablo II\Expansion\1.07\save).

Path A) If you are using any custom .dlls (Like d2gfx.dll), you can place
        this dll in its respective folder (Example: ..\Expansion\1.07 folder
        *before switching to that version*).

Path B) If you already switched into that version, you can drop the dll into
        the Diablo II folder and also the respective version folder (or switch
        to a different version and switch back into the version you want after
        you followed Path A.

Blizzard released a new patch but it isn't in the Bliss Complete Collection
---------------
Simply run the LOD updater either from the Standalone updater or from Battle.net.

1. Update the game either through Battle.net or the Standalone Updater.
2. Go to your Diablo II root directory
3. Copy the required files that the new updater installed into a folder
   with the label you want to use in Cactus later.
4. Create an empty 'save' folder in the Storage Directory.
5. Switch to this new version.
   
   In this example we are assuming that:
   Diablo II Root Directory = D:\Games\Diablo II
   Storage Directory = D:\Games\Diablo II\Expansion\MooMooFarm 1.15\
   
   1. Update the game
   2. Go to the Diablo II Root Directory
   3. Copy the required files to your Storage Directory. Since this directory
      doesn't exist, go ahead and create it. If you don't know which are the
      required files, then go to any entry in the Storage Directory and copy
      whatever files are listed there from your Root Directory.
   4. Create an empty "save" folder in the Storage Directory
   5. Add an entry to Cactus with the Label "MooMooFarm 1.15" and version "1.15".
   6. Done!

   If the version doesn't exist in Cactus because the application hasn't been updated,
   you might be able to get everything working by using the previous version (Since
   maybe the requires files stayed the same.

Troubleshooting
================

"I click Launch but nothing is happening"
----------------
This can happen for multiple reasons. One of them is that the files in
your D2 root folder are not correct. Cactus will copy the files from the
corresponding "[Classic/Expansion] / <label>" folder into your Diablo II
root folder before launching if needed. If the files in your back up dirs
are incorrect, then this may cause an issue.

Lastly the game may not be launch because you installed Diablo II through
Blizzard's new 1.12 installer and you are trying to play a version of
Diablo II older than 1.12. The reason this problem exists is because starting with
the 1.12 installer, Blizzard updated the MPQ file format that Diablo II uses.
Due to this, only versions of Diablo II that understand this format can be played
using the Blizzard installer (1.12+). If you try to use 1.11, 1.10, etc, those versions
only understand the old MPQ file format that Blizzard used between versions 1.00 - 1.11.
In order to fix this, you can download my Singling mod (or Bliss Complete Collection)
which contains the fix.

"Oh My... I broke it"
----------------
If you screw up the install and your Diablo II directory files get messed up,
just relax.. you don't need to reinstall. Just extract the files from the Bundle
for the version you want and drop them into your Diablo II folder (Replacing the
existing files in there). You already have the MPQ files, so simply replacing
the dlls should do the trick, and re-running that version in BVS should fix it.

If your MPQs get screwed... well then good game. Go get the disks.

Multiple Versions
---------------
If you want to run multiple versions of Diablo II, It is the same process as before.
You can get this feature by downloading Singling. It contains the changes needed in
order to allow multiple D2 windows to be launched.