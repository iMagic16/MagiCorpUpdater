using System;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;
using System.Threading;
using System.Security.Cryptography;
using MagiCorpDevTools_Debugger;

//                  TO DO
// MD5 check / file downloaded integrity check [DONE]
// Check if the program is running before unzip, ask to close or force kill [DONE]
//Copy out of tmp dir into working dir [DONE]
//backup old ver [DONE]
//add server to sw [DONE]
//File "magic" must exist for the prog to halt on finish
//write logs to file [wip]
//10-mar-15

namespace MagiCorpUpdater
{
    class Program
    {
        public static string sha256generated = "", serverURL;
        public static bool fileIntegrity;
        public static bool UrlFromSw = false;
        public static bool Check4Update = false;
        static void Main(string[] args)
        {
            string ProgName = "";
            string ProgVer = "";
            if (!(File.Exists("MagiCorpDevTools_Debugger.dll")))
            {
                File.WriteAllBytes("MagiCorpDevTools_Debugger.dll", Properties.Resources.MagiCorpDevTools_Debugger);
            }
            //Init debug
            M_Debugger.Init();

            //Argument Checker
            if (args == null)
            {
                M_Debugger.ConOut("No Arguments taken"); // no args? gg
            }
            else
            {
                M_Debugger.ConOut(args.Length + " Arguments Found");

                for (int i = 0; i < args.Length; i++) // Loop through array
                {
                    string argument = args[i];
                    M_Debugger.ConOut("Argument " + i + " is [" + argument + "]");

                    //-p: (program name) via switch
                    if (argument.Contains("-p:"))
                    {
                        M_Debugger.ConOut("Argument found for Program Name: " + argument);
                        ProgName = argument.Trim(new char[] { '-', 'p', ':' });//cut the -p: from the switch
                        M_Debugger.ConOut("P: " + ProgName, false, true); //output trimmed switch input to console
                    }
                    //-v: (program veresion) via switch
                    if (argument.Contains("-v:"))
                    {
                        M_Debugger.ConOut("Argument found for Program Version: " + argument);
                        ProgVer = argument.Trim(new char[] { '-', 'v', ':' }); //cut the -v: from the switch
                        M_Debugger.ConOut("V: " + ProgVer, false, true); //output trimmed switch input to console
                    }
                    //-s: (server url) via switch
                    if (argument.Contains("-s:"))
                    {
                        M_Debugger.ConOut("Argument found for Server: " + argument);
                        serverURL = argument.Trim(new char[] { '-', 's', ':' }); //cut the -s: from the switch
                        M_Debugger.ConOut("S: " + serverURL, false, true); //output trimmed switch input to console
                        UrlFromSw = true;
                    }
                    //-c: (only check for updates
                    if (argument.Contains("-c"))
                    {
                        M_Debugger.ConOut("Argument found for 'only check for updates'");
                        Check4Update = true; //set true
                        M_Debugger.ConOut("C: " + Check4Update, false, true); //output trimmed switch input to console
                    }

                }
            }

            M_Debugger.ConOut("PROGRAM START_");

            //Checking if we have had args set our names and versions
            if (ProgName.Length > 1)
            {
                //Do nothing
            }
            else
            {
                M_Debugger.ConOut("Program to update?");
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
                    ProgVer = File.ReadAllText("version.mup");
                }
                catch (Exception e)
                {
                    M_Debugger.ConOut(e.Message, true);

                    //Try manually..
                    if(ProgVer == "")
                    {
                        M_Debugger.ConOut("Version?");
                        ProgVer = Console.ReadLine();
                    }
                    
                }
            }

            //Call subroutine to check for updates
            CheckForUpdates(ProgName, ProgVer);

            M_Debugger.ConOut("DONE");
            M_Debugger.ConOut("Launching...");
            try
            {
                if (!File.Exists("magic")) 
                    Process.Start(ProgName + ".exe");
            }
            catch (Exception ex)
            {
                M_Debugger.ConOut(ex.Message, true);
            }

            if (File.Exists("magic"))
                Console.ReadKey();

        }

        static void CheckForUpdates(string ProgramName, string CurrentVersion)
        {
            string UpdateURL;

            if (UrlFromSw == true)
            {
                UpdateURL = serverURL + @"/" + ProgramName + @"/";
            }
            else
            {
                UpdateURL = "http://magicorp.me/Updater" + ProgramName + @"/";
            }

            string FileToCheck = "version.mup";
            string NewVersion = "null";
            //create new net web interf
            WebClient UpdateClient = new WebClient();
            M_Debugger.ConOut("Web Client Created");
            //res to dl
            M_Debugger.ConOut("Downloading version check...");
            M_Debugger.ConOut(UpdateURL + FileToCheck);
            //check for the update
            try
            {
                NewVersion = UpdateClient.DownloadString(UpdateURL + FileToCheck);
            }
            catch (Exception e)
            {
                M_Debugger.ConOut(e.Message, true);
            }

            //Compare
            M_Debugger.ConOut("Checking versions...");
            try
            {
                double intNewVersion = Convert.ToDouble(NewVersion);
                double intCurrentVersion = Convert.ToDouble(CurrentVersion);
                bool DownloadedOK = false;
                M_Debugger.ConOut(NewVersion + ">" + CurrentVersion + " (new>current)");

                if (intNewVersion > intCurrentVersion)
                {
                    M_Debugger.ConOut("New version found... " + NewVersion);
                    if (Check4Update == true)
                    {
                        M_Debugger.ConOut("Only checking for updates, time to exit!");
                        if (File.Exists("magic")) 
                            Console.ReadKey();
                        Environment.Exit(1);
                    }
                    M_Debugger.ConOut("Downloading update package...");
                    M_Debugger.ConOut(UpdateURL + intNewVersion + ".zip");
                    //Download package 
                    try
                    {
                        UpdateClient.DownloadFile(UpdateURL + intNewVersion + ".zip", NewVersion + ".zip");
                        UpdateClient.DownloadFile(UpdateURL + intNewVersion + ".sha256", NewVersion + ".sha256");
                        DownloadedOK = true;
                    }
                    catch (Exception e)
                    {
                        M_Debugger.ConOut(e.Message, true);
                        // throw;
                    }

                    if (DownloadedOK == true)
                    {
                        //Compare the MD5 of downloaded with checksum from the server only if it downloaded fine.
                        M_Debugger.ConOut("File downloaded OK, Calling sha256 check");
                        SHA256Check(NewVersion + ".sha256", NewVersion + ".zip", NewVersion);
                    }
                    else
                    {
                        M_Debugger.ConOut("DOWNLOAD FAILED!", true);
                        if (File.Exists("magic")) 
                            Console.ReadKey();
                        Environment.Exit(1);
                    }
                    //CALL EXTR only if sha passed.
                    if (fileIntegrity == true)
                    {
                        M_Debugger.ConOut("SHA256 check passed, Calling zip file extraction");
                        ExtractPackage(NewVersion + ".zip", ProgramName);
                    }
                    else
                    {
                        M_Debugger.ConOut("DOWNLOADED FILE DOES NOT MATCH CHECKSUMS", true);
                        if (File.Exists("magic")) 
                            Console.ReadKey();
                        Environment.Exit(1);
                    }
                }
                else
                {
                    M_Debugger.ConOut("No new version found, you have the latest! (" + CurrentVersion + ")!", false, true);
                    M_Debugger.ConOut("No new version found, you have the latest! (" + CurrentVersion + ")!");
                    if (File.Exists("magic")) 
                        Console.ReadKey();
                    Environment.Exit(1);
                }
            }
            catch (Exception e)
            {
                M_Debugger.ConOut(e.Message, true);
            }
        }

        //extract zip into tmp
        static void ExtractPackage(string PackageName, string ProgramName)
        {

            //Check if program is running first...
            if (Process.GetProcessesByName(ProgramName).Length > 0)
            {
                M_Debugger.ConOut(ProgramName + ".exe is running, please close before updating");
                M_Debugger.ConOut("Close program, or type y for a force closure");
                string input = Console.ReadLine();

                if (input == "y")
                {
                    //kill it with fire.
                    try
                    {
                        foreach (Process proc in Process.GetProcessesByName(ProgramName))
                        {
                            proc.Kill();
                            M_Debugger.ConOut("Waiting for program to close...");
                            Thread.Sleep(2000);
                        }
                    }
                    catch (Exception e)
                    {
                        M_Debugger.ConOut(e.Message);
                    }
                }
                else
                {
                    M_Debugger.ConOut("Please close the program before continuing...", true);
                    if (File.Exists("magic")) 
                        Console.ReadKey();
                    Environment.Exit(1);
                }
            }
            else
            {
                M_Debugger.ConOut("Program not running, clear for go ahead.");
            }

            //clear old data
            M_Debugger.ConOut("Clearing old data...");
            if (Directory.Exists("tmp"))
            {
                M_Debugger.ConOut("tmp dir found, deleting", true);
                Directory.Delete("tmp", true);
            }
            else
            {
                M_Debugger.ConOut("No leftovers found.");
            }

            //EXTRACT TIME!
            M_Debugger.ConOut("Extracting package: " + PackageName);
            ZipFile.ExtractToDirectory(PackageName, Directory.GetCurrentDirectory() + @"\tmp");

            //Backup time...
            BackupExistingProgram();

            //aaand time to update
            CopyToLive();
        }

        //backup existing live program
        static void BackupExistingProgram()
        {
            M_Debugger.ConOut("Backing up existing program...", false, true);
            string fileName = "";

            //string sourcePath = Directory.GetCurrentDirectory();

            string targetPath = Directory.GetCurrentDirectory() + @"\backup";

            //Use Path class to manipulate file and directory paths.
            //string sourceFile = System.IO.Path.Combine(sourcePath, fileName);

            string destFile = Path.Combine(targetPath, fileName);

            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory());

            //https://msdn.microsoft.com/en-us/library/system.io.path.getfilename(v=vs.110).aspx
            try
            {
                if (Directory.Exists("backup"))
                {
                    M_Debugger.ConOut("Backup dir already exists, continuing...");
                }
                else
                {
                    M_Debugger.ConOut("Backup dir not found, creating...");
                    Directory.CreateDirectory("backup");
                }

                foreach (string s in files)
                {
                    fileName = Path.GetFileName(s);
                    destFile = Path.Combine(targetPath, fileName);
                    File.Copy(s, destFile, true);
                    M_Debugger.ConOut("B: Moved " + s + " to " + targetPath, false, false, true);
                }
            }
            catch (Exception e)
            {
                M_Debugger.ConOut(e.Message, true);
                throw;
            }

        }

        //copy the files from tmp to "live" system
        static void CopyToLive()
        {
            M_Debugger.ConOut("Copy program to live...", false, true);
            // Thread.Sleep(2000);

            string fileName = "";

            string sourcePath = Directory.GetCurrentDirectory() + @"\tmp";
            string targetPath = Directory.GetCurrentDirectory();

            //Use Path class to manipulate file and directory paths.
            //string sourceFile = System.IO.Path.Combine(sourcePath, fileName);

            string destFile = Path.Combine(targetPath, fileName);

            string[] files = Directory.GetFiles(sourcePath);

            //https://msdn.microsoft.com/en-us/library/system.io.path.getfilename(v=vs.110).aspx

            try
            {
                foreach (string s in files)
                {
                    fileName = Path.GetFileName(s);
                    destFile = Path.Combine(targetPath, fileName);
                    File.Copy(s, destFile, true);
                    M_Debugger.ConOut("C2L: Moved " + s + " to " + targetPath, false, false, true);
                }
            }
            catch (Exception e)
            {
                M_Debugger.ConOut(e.Message, true);
            }

        }

        //check file integrity
        static void SHA256Check(string sha256, string FileToCheck, string version)
        {
            M_Debugger.ConOut("SHA256 check in progress...", false, true);

            M_Debugger.ConOut("Opening file to generate sha256...");
            FileStream fs = File.Open(FileToCheck, FileMode.Open, FileAccess.Read, FileShare.None);

            M_Debugger.ConOut("Create sha256 instance");
            SHA256 sha256r = SHA256Managed.Create();

            M_Debugger.ConOut("Create hash from file...");
            byte[] hashvalue = sha256r.ComputeHash(fs);

            M_Debugger.ConOut("Closing file");
            fs.Close();

            M_Debugger.ConOut("Convert byte array into a string");
            PrintByteArray(hashvalue);

            M_Debugger.ConOut("Read sha256 from the server");
            string sha256fromweb = File.ReadAllText(sha256);

            M_Debugger.ConOut("Write generated sha256 to a file");
            File.WriteAllText(version + "_generated.sha256", sha256generated);

            M_Debugger.ConOut("Comparing " + sha256generated + " to " + sha256fromweb);
            if (sha256generated == sha256fromweb)
            {
                M_Debugger.ConOut("File integrity: OK", false, false, true);
                fileIntegrity = true;
            }
            else
            {
                M_Debugger.ConOut("File integrity: BAD", true);
                fileIntegrity = false;
            }

        }

        // Print the byte array in a readable format. 
        public static string PrintByteArray(byte[] array)
        {
            int i;
            for (i = 0; i < array.Length; i++)
            {
                //Console.Write(String.Format("{0:X2}", array[i]));
                sha256generated += String.Format("{0:X2}", array[i]);
                M_Debugger.ConOut(sha256generated, false, false, true);
                if ((i % 4) == 3)
                {
                    M_Debugger.ConOut(" ", false, false, true);
                    sha256generated += " ";
                }
            }
            Console.WriteLine();
            sha256generated = sha256generated.ToLower();
            sha256generated = sha256generated.Replace(" ", "");
            return sha256generated;
        }

    }
}
