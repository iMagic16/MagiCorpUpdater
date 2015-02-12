# MagiCorp Updater
Made By:JamesW/MagiCorp

#What this program does: 
This program handles updating in a modular way

You specify the web server holding the updates, versions and checksums

The application should do the rest


#Instructions:

To add this to your program you need to do a few things.

First: create a version.mup file holding the number of the latest version of your program (we'll compare this to your running version)

Second: Place a zip file with the same name as version. ex: version 1136 of a program's zip would be 1136.zip 

Third: Generate a sha256 checksum of the zip and place it inside a <version>.sha256 file

Finally: Place all of these inside your ://WebServer/Updater/ProgramName/

https://raw.githubusercontent.com/iMagic16/MagiCorpUpdater/master/layout.jpg

#To execute this from your program: 

Add a process.start for MagiCorpUpdater.exe

In this start, add the switches based on your programs details

-p: [Name of the program you're updating] _this must match the case and spacing as the web server_

-v: [Version of the program you're updating]

-s: [Web server holding data]


C# Example: 

```C#
Using System.Diagnostic;
Process.Start("MagiCorpUpdater.exe", "-p:CiscoOverwatch -v:1.0 -s:http://magicorp.comuv.com/Updater");
```

Note! You don't have to use C# to use this program!

Python Example:

```Python
import subprocess
subprocess.call(['MagiCorpUpdater.exe', '-p:CiscoOverwatch -v:1.0 -s:http://magicorp.comuv.com/Updater'])
```

#Known bugs and fixes
ERROR: Input string was not in a correct format.

FIXES: Your URL may have a trailing slash in the -s:

FIXES: The web server is responding in an unusual way


ERROR: 

FIXES:

#Contact: 
Gmail
jameswalsh5684@gmail.com


Steam
the_magical_one

