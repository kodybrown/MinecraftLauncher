MinecraftLauncher
=================

Provides a simple interface to launch multiple Minecraft installations.

The launcher reads client directories from its current directory and lists them for the user to choose. Once chosen a junction point is created in the %AppData%\.minecraft directory pointing to the chosen client version and Minecraft is executed from its normal location.

Setup
-----
The process is pretty simple and straight-forward.

* Create a new directory to hold all of the minecraft client versions. I use 'C:\Minecraft' but you (should be able to) use whatever you like.
* Move your %AppData%\.minecraft directory into 'C:\Minecraft'
* Rename the .minecraft directory to '\x.y.z-default' replacing the x, y, and z with your minecraft's version info. You can also change 'default' to whatever you want, for instance 'cc,gravgun,ic2,etc.' to indicate the mods that are applied.
* Place the mclauncher.exe in the 'C:\Minecraft' directory.

You can create as many client versions and mod configurations you want. There are no limits. (Technically, you can create up to 65000 give or take...)

