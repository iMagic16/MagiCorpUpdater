using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;
using System.Threading;
using System.Security.Cryptography;

//                  TO DO
// MD5 check / file downloaded integrity check
// Check if the program is running before unzip, ask to close or force kill [DONE]
//Copy out of tmp dir into working dir
//backup old ver
//

namespace MagiCorpUpdater
{
    class Program
    {
        static void Main(string[] args)
        {

            string ProgName = "";
            string ProgVer = "";

            //Argument Checker
            if (args == null)
            {
                Debug.ConOut("No Arguments taken"); // no args? gg
            }
            else
            {
                Debug.ConOut(args.Length + " Arguments Found");

                for (int i = 0; i < args.Length; i++) // Loop through array
                {
                    string argument = args[i];
                    Debug.ConOut("Argument " + i + " is [" + argument + "]");

                    //-p: (program name) via switch
                    if (argument.Contains("-p:"))
                    {
                        Debug.ConOut("Argument found for Program Name: " + argument);
                        ProgName = argument.Trim(new char[] { '-', 'p', ':' });//cut the -p: from the switch
                        Debug.ConOut("P: " + ProgName, false, true); //output trimmed switch input to console
                    }
                    //-v: (program veresion) via switch
                    if (argument.Contains("-v:"))
                    {
                        Debug.ConOut("Argument found for Program Version: " + argument);
                        ProgVer = argument.Trim(new char[] { '-', 'v', ':' }); //cut the -v: from the switch
                        Debug.ConOut("V: " + ProgVer, false, true); //output trimmed switch input to console
                    }

                }
            }

            Debug.ConOut("PROGRAM START_");


            //Checking if we have had args set our names and versions

            if (ProgName.Length > 1)
            {
                //Do nothing
            }
            else
            {
                Debug.ConOut("Program to update?");
                ProgName = Console.ReadLine();
            }
            if (ProgVer.Length > 1)
            {
                //Do nothing
            }
            else
            {
                try
                {
                    //Try to read version from version file
                    ProgVer = System.IO.File.ReadAllText("version.mup");
                }
                catch (Exception e)
                {
                    Debug.ConOut(e.Message, true);

                    //Try manually..
                    Debug.ConOut("Version?");
                    ProgVer = Console.ReadLine();
                }

            }


            //Call subroutine to check for updates
            CheckForUpdates(ProgName, ProgVer);

            Debug.ConOut("DONE");
            Console.ReadLine();
        }

        static void CheckForUpdates(string ProgramName, string CurrentVersion)
        {
            string UpdateURL = "http://magicorp.comuv.com/updater/" + ProgramName + "/";
            string FileToCheck = "version.mup";
            string NewVersion = "null";
            //create new net web interf
            WebClient UpdateClient = new WebClient();
            Debug.ConOut("Web Client Created");
            //res to dl
            Debug.ConOut("Downloading version check...");
            Debug.ConOut(UpdateURL + FileToCheck);
            //check for the update
            try
            {
                NewVersion = UpdateClient.DownloadString(UpdateURL + FileToCheck);
            }

            catch (Exception e)
            {
                Debug.ConOut(e.Message, true);
                //throw;
            }

            //Compare
            Debug.ConOut("Checking versions...");
            try
            {
                double intNewVersion = Convert.ToDouble(NewVersion);
                double intCurrentVersion = Convert.ToDouble(CurrentVersion);


                Debug.ConOut(NewVersion + ">" + CurrentVersion);

                if (intNewVersion > intCurrentVersion)
                {
                    Debug.ConOut("New version found... Downloading update package...");
                    Debug.ConOut(UpdateURL + intNewVersion + ".zip");
                    //Download package 
                    try
                    {
                        UpdateClient.DownloadFile(UpdateURL + intNewVersion + ".zip", "update.zip");
                        UpdateClient.DownloadFile(UpdateURL + intNewVersion + ".md5", "update.md5");

                    }
                    catch (Exception e)
                    {
                        Debug.ConOut(e.Message, true);
                        // throw;
                    }


                    //Compare the MD5 of downloaded with checksum from the server
                    MD5Check("update.md5");



                    //CALL EXTR
                    ExtractPackage("update.zip", ProgramName);
                    //ProgramName

                }
                else
                {
                    Debug.ConOut("No new version found, you have the latest! (" + CurrentVersion + ")!", false, true);
                }
            }
            catch (Exception e)
            {
                Debug.ConOut(e.Message, true);
            }


        }

        static void ExtractPackage(string PackageName, string ProgramName)
        {

            //Check if program is running first...
            if (Process.GetProcessesByName(ProgramName).Length > 0)
            {
                Debug.ConOut(ProgramName + ".exe is running, please close before updating");
                Debug.ConOut("Close program, or type y for a force closure");
                string input = Console.ReadLine();

                if (input == "y")
                {
                    //kill it with fire.
                    try
                    {
                        foreach (Process proc in Process.GetProcessesByName(ProgramName))
                        {
                            proc.Kill();
                            Debug.ConOut("Waiting for program to close...");
                            Thread.Sleep(2000);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.ConOut(e.Message);
                    }
                }
            }
            else
            {
                Debug.ConOut("Program not running, clear for go ahead.");
            }

            //clear old data
            Debug.ConOut("Clearing old data...");
            if (Directory.Exists("tmp"))
            {
                Debug.ConOut("tmp dir found, deleting");
                Directory.Delete("tmp", true);
            }
            else
            {
                Debug.ConOut("No leftovers found.");
            }


            //EXTRACT TIME!
            Debug.ConOut("Extracting package: " + PackageName);
            ZipFile.ExtractToDirectory(PackageName, Directory.GetCurrentDirectory() + "/tmp");

            //Backup time...
            BackupExistingProgram();

            //aaand time to update
            CopyToLive();
        }


        static void BackupExistingProgram()
        {
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory());
            //https://msdn.microsoft.com/en-us/library/system.io.path.getfilename(v=vs.110).aspx
            try
            {
                foreach (string filename in files)
                {
                    string MoveMeHere = Directory.GetCurrentDirectory() + "\\bak\\";
                    //raw filename
                    Debug.ConOut(filename, false, true);
                    //amended filename
                    string AmendedFilename = Path.GetFileName(filename);
                    Debug.ConOut(AmendedFilename, false, true);
                    //Move to here
                    Debug.ConOut(MoveMeHere, false, true);

                    //check if file is legit
                    if (AmendedFilename.EndsWith(@"\"))
                    {
                        //no legit files found
                        Debug.ConOut("No files found to backup");
                    }
                    else
                    {
                        //actually move the file
                        File.Move(AmendedFilename, MoveMeHere);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.ConOut(e.Message, true);
            }
        }

        static void CopyToLive()
        {
            Debug.ConOut("C2L not implemented yet", false, true);
        }

        static void MD5Check(string md5, string FileToCheck)
        {
            Debug.ConOut("MD5 check not implemented yet", false, true);
        }

    }
}
