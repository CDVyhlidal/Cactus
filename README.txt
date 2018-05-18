Cactus 0.0.1
Jonathan Vasquez (fearedbliss) <jon@xyinn.org>
Apache License 2.0
Thursday, May 17, 2018
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
2. Have fun.











--------------
Initial Setup
---------------
The following YouTube video is now available: https://www.youtube.com/watch?v=JgUVmZmnjSI

Since BVS doesn't know what version you have installed, we will need to set things up correctly
for the first launch. After this, everything will be synchronized and work correctly.

Assumptions
-------------
- You installed Diablo II in C:\Program Files\Diablo II\
- The current version of Diablo II in that directory is 1.07.

1. Drop the "Bliss_Version_Switcher.jar" file into your Diablo II folder.
2. Open BVS
3. Click "Add" and fill in the information required and press Add.
   > Check the "Expansion" check box if you want to play LoD (This doesn't affect the character creation, just what files the game uses).
   > Select the version that is currently in the Diablo II folder (In our example, it is 1.07).
   > Type in the label for what you want call this folder. Your saves and backed up files for this version will be located in here.
   > Click "Set Path" and navigate to where your Game.exe is located and select it.
   > Type in any flags you want if needed (Example: -w -ns -direct -txt). You can leave this blank.

4. Start Diablo II by clicking "Launch"

That's it. If everything went well, you should see your Diablo II game launch. If you aren't using the modified Game.exes in the Singling pack
that backported the no-cd changes from 1.12 back to older versions, then make sure you have
your Play Disc/LoD Disc inside your CD Drive (Or use ISOs that are mounted).

If you downloaded the Bliss Complete Collection, then you can just drop any version
of Diablo II from in there into the corresponding [Classic/Expansion] folder. Add another entry in BVS, and then will be able to switch to that
version as well. The "Bliss Complete Collection" also includes Singling, GlideWrapper, and any other needed utilities.

Other Information
======
Please check out the HELP.txt file for more scenarios, help, and advice.