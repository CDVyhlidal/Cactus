## Cactus 1.2.1
##### Jonathan Vasquez (fearedbliss)
##### Apache License 2.0
##### Released on Thursday, June 7, 2018

## Description

This is a C#/.NET based application that will help you manage/launch
Multiple Versions or _Compatible Mods_ of Diablo II (Which I will call "Platforms")
from a single application.

This means you can easily install and play every single version of Diablo II
from 1.00 to the latest 1.14d (and any other future versions) while maximizing
your disk space (Since you won't have to keep having multiple copies of your MPQs),
and having complete character isolation.

## Requirements

- .NET Framework 4.6.1 +

## Links

- [Discord](https://discord.gg/B59qDKy)
- [Cactus](https://github.com/fearedbliss/Cactus)
- [Succulent](https://github.com/fearedbliss/Succulent)
- [Alpaca](https://github.com/fearedbliss/Alpaca)
- [Singling](https://github.com/fearedbliss/Singling)
- [Bliss Complete Collection](https://xyinn.org/diablo/Bliss_Complete_Collection.7z)

## Installation/Instructions ([Video](https://www.youtube.com/watch?v=i6SfYy1kCJ0))

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

Platform: "1.14d Vanilla"
Path: "D:\Games\Diablo II\Game.exe"
Flags: "-w -ns"
Expansion: [Checked]

What this says is that I want to run a platform of the game in a folder
called "1.14d Vanilla". The game will be launched with the "-w -ns" flags,
and the path of main executable is "D:\Games\Diablo II\Game.exe". This platform
is also requires the Expansion MPQs since the Expansion checkbox is checked.
What Cactus does is that when it switches between versions, it deletes all of the files
that the old entry required from the "D:\Games\Diablo II\ directory, and then it goes
to the Platform with the associated name, and copies the required files for that particular
platform to the "D:\Games\Diablo II\" directory. Cactus always copies files in one direction,
thus the files in your Platforms directory never get modified or removed and thus are always
safe. Cactus also keeps all of your character files for all of your platforms in separate
directories named after the Platform in question, inside of the "Saves" folder. This means
that you can easily backup all of your characters quickly by simply copying this folder.
The "Saves" directory doesn't have to be manually created and will be automatically created
when you switch an entry (or launch for the first time).

4. Go to your Diablo II folder and make a folder called "Platforms" and inside of
   the "Platforms" folder make another folder called "1.14d Vanilla".

5. Go back to your Diablo II root folder and copy all of the .exes, .dlls,
   and Patch_D2.mpq (The other MPQs should not be copied) to the "Platforms\1.14d Vanilla"
   folder.

6. Go back to Cactus, Click the new entry, and click Launch.

7. Enjoy.

Any other versions, just make another folder for it in Platforms and throw your files in
there. Each platform is completely separate from the previous one so all of the files
that you need to run that version should be in that folder (Except for the shared MPQs).

## Playing Previous Versions ([Video](https://www.youtube.com/watch?v=tz1J4FyuBHE))

If you installed the game with a 1.00, 1.03, or 1.07 CDs/ISOs,
then there is nothing special you need to do to play older versions
(other than possible graphical tweaks).

However, if you installed Diablo II using the new Blizzard Installers (1.12-1.14+),
then you will have updated MPQ files with special attributes that the old Diablo II
executables will not understand and will cause errors.

To fix your MPQs, you can either download my Singling mod that contains the fix,
or you can follow my video tutorial below to manually fix the DLLs. If you don't
fix your DLLs, you won't be able to play most versions before 1.14 and you will
get a blank error message from the application, an error message saying that
your mpq is corrupted, and D2VidTst won't run and will not allow you to change video
modes. It actually isn't corrupted, it's just that the old Diablo II version you are
trying to switch to doesn't understand the new MPQ attributes.

### Fixing Graphical Issues (D2VidTst)

Another thing to keep in mind is that starting in 1.14, Blizzard removed
the "D2VidTst" application and I believe they are defaulting to the Direct 3D
driver. If you are switching back down to pre 1.14, make sure that you run the
D2VidTst application and set it to either Direct 3D or Glide. If you are using
the GlideWrapper, make sure you install the glide-init.exe and its corresponding
DLL to the root of your Diablo II directory and add the "-3dfx" flag to Cactus.

Keep in mind that Diablo II works perfectly fine while using window mode (-w)
for Direct 3D and Glide. It's only when attempting to run in Full Screen mode
without Glide (for versions before 1.14) that issues arise. Below are some of
the steps you can take to fix this.

#### On Windows 7

You will want to "Disable Desktop Composition" (Disables Aero) for your Diablo II
executable (Game.exe), since older versions will conflict with this. If you are
experiencing the spinning characters at the top left of your monitor, and your game
is not starting, it's most likely related to this. I would also recommend to use
Glide Wrapper as well (for the improve colors, visuals, and compatibility) but it
isn't necessary on Windows 7 since the older versions still run without an issue.

#### On Windows 10

I haven't been able to get DirectDraw or Direct 3D to work in Full Screen mode
for Diablo II versions before 1.14 (1.14 was Blizzard's Compatibility Update).
However, I was able to get Full Screen working perfectly fine with Glide.
All you need to do is run the "D2VidTst" application in Windows 7 compatibility
mode. If you don't run it in compatibility mode, the application will not work
(and you won't see any visual indications). Once the D2VidTst runs, select Glide,
and that's it. You'll still see the spinning characters at the top left but it
will work.

The errors you will get on Windows 10 pre 1.14 if you try to run it without
running D2VidTst (and without Glide) are:

"Error 22: A critical error has occurred while initializing DirectDraw"
"Error 25: A critical error has occurred while initializing Direct3D".

I will repeat one more time, all of the above stuff needs to be done with MPQ files
that have been fixed either by following my video tutorial or by using my Singling mod
that contains the fix. You only need to fix the MPQ files if you installed Diablo II
using Blizzard's new installer. If you installed using old 1.00, 1.03, or 1.07 CDs/ISOs,
then you don't need to do this.

## [Alpaca Support (PlugY Rearchitected)](https://github.com/fearedbliss/Alpaca)

If you are using Alpaca, then make sure to point your Path to "Alpaca.exe"
rather than "Game.exe". Also, Cactus copies all of the Alpaca information
in one direction only (From the Platform Directory -> Root Directory).
So, if you wanna make any changes to Alpaca, make sure that you switch to
another entry, edit the Alpaca files in the platform directory, and then
switch back to it. If you don't have more than one platform, then just
edit the settings in both the Platform Directory, and the Root Directory.

Cactus also slightly delays the execution of automatically launching Alpaca
when you first switch to an entry. This is because the filesystem might
copy the files in an asynchronous way which means that Alpaca will try to
launch before all of its files are available, if this happens, you
will get a "Read memory error". If you get this, just try to launch it again..
Hopefully you won't get this often/never considering I've implemented the delay.

## Median XL support (Tested on 13.2 with 1.13c base)

Median XL is fully supported. When launching the game, Median XL requires
admin privileges or else it will give you a "failed to retrieve process" error.
Just run Cactus in admin mode and the permissions will trickle down.

Also on Windows 10 I was getting an access violation after I closed the
Median XL process. You can solve this by running the Game.exe in either
Windows XP (SP3) or Windows 7 compatibility mode.

## Moving Cactus over to a new computer

If you want to move all of your platforms, characters, and Diablo II folder
to another machine, you can just copy your entire Diablo II folder as is,
transport it to your new machine (USB, SSH, etc), and then after that edit
the Last Ran entry and uncheck the "Last Ran" box. Then run whatever entry
you want. Unchecking the "Last Ran" box will cause Cactus to reconfigure itself
(Including registry locations).

## Updating files in your Platforms directory

If you update any files in your Platforms directory, then if that entry
that you updated is the currently running version (Last Ran is checked),
then you will need to edit the entry, uncheck the "Last Ran" box, and re-run
it. This will cause Cactus to reinstall that platform so that you will have
the new files in the Diablo II root directory.  If the platform you updated
isn't the last ran, then you don't need to do anything else since it will
install those files properly whenever you switch to it.
