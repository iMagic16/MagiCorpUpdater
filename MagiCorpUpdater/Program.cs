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
// MD5 check / file downloaded integrity check [DONE]
// Check if the program is running before unzip, ask to close or force kill [DONE]
//Copy out of tmp dir into working dir [DONE]
//backup old ver [DONE]
//

namespace MagiCorpUpdater
{
    class Program
    {
        public static string sha256generated = "";
        public static bool fileIntegrity;

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
            Debug.ConOut("Launching...");
            Process.Start(ProgName + ".exe");
            Console.ReadKey();
        }

        static void CheckForUpdates(string ProgramName, string CurrentVersion)
        {
            string UpdateURL = "http://magicorp.comuv.com/Updater/" + ProgramName + "/";
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
            }

            //Compare
            Debug.ConOut("Checking versions...");
            try
            {
                double intNewVersion = Convert.ToDouble(NewVersion);
                double intCurrentVersion = Convert.ToDouble(CurrentVersion);
                bool DownloadedOK = false;
                Debug.ConOut(NewVersion + ">" + CurrentVersion);

                if (intNewVersion > intCurrentVersion)
                {
                    Debug.ConOut("New version found... Downloading update package...");
                    Debug.ConOut(UpdateURL + intNewVersion + ".zip");
                    //Download package 
                    try
                    {
                        UpdateClient.DownloadFile(UpdateURL + intNewVersion + ".zip", "update.zip");
                        UpdateClient.DownloadFile(UpdateURL + intNewVersion + ".sha256", "update.sha256");
                        DownloadedOK = true;
                    }
                    catch (Exception e)
                    {
                        Debug.ConOut(e.Message, true);
                        // throw;
                    }

                    if (DownloadedOK == true)
                    {
                        //Compare the MD5 of downloaded with checksum from the server only if it downloaded fine.
                        Debug.ConOut("File downloaded OK, Calling sha256 check");
                        SHA256Check("update.sha256", "update.zip");
                    }
                    else
                    {
                        Debug.ConOut("DOWNLOAD FAILED!", true);
                    }
                    //CALL EXTR only if sha passed.
                    if (fileIntegrity == true)
                    {
                        Debug.ConOut("SHA256 check passed, Calling zip file extraction");
                        ExtractPackage("update.zip", ProgramName);
                    }
                    else
                    {
                        Debug.ConOut("DOWNLOADED FILE DOES NOT MATCH CHECKSUMS", true);
                    }
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

        //extract zip into tmp
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
                Debug.ConOut("tmp dir found, deleting", true);
                Directory.Delete("tmp", true);
            }
            else
            {
                Debug.ConOut("No leftovers found.");
            }

            //EXTRACT TIME!
            Debug.ConOut("Extracting package: " + PackageName);
            ZipFile.ExtractToDirectory(PackageName, Directory.GetCurrentDirectory() + @"\tmp");

            //Backup time...
            BackupExistingProgram();

            //aaand time to update
            CopyToLive();
        }

        //backup existing live program
        static void BackupExistingProgram()
        {
            Debug.ConOut("Backing up existing program...", false, true);
            string fileName = "";

            //string sourcePath = Directory.GetCurrentDirectory();

            string targetPath = Directory.GetCurrentDirectory() + @"\Backup";

            //Use Path class to manipulate file and directory paths.
            //string sourceFile = System.IO.Path.Combine(sourcePath, fileName);

            string destFile = System.IO.Path.Combine(targetPath, fileName);

            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory());

            //https://msdn.microsoft.com/en-us/library/system.io.path.getfilename(v=vs.110).aspx
            try
            {
                if (Directory.Exists("Backup"))
                {
                    Debug.ConOut("Backup dir already exists, continuing...");
                }
                else
                {
                    Debug.ConOut("Backup dir not found, creating...");
                    Directory.CreateDirectory("Backup");
                }

                foreach (string s in files)
                {
                    fileName = Path.GetFileName(s);
                    destFile = Path.Combine(targetPath, fileName);
                    System.IO.File.Copy(s, destFile, true);
                    Debug.ConOut("B: Moved " + s + " to " + targetPath, false, false, true);
                }
            }
            catch (Exception e)
            {
                Debug.ConOut(e.Message, true);
                throw;
            }

        }

        //copy the files from tmp to "live" system
        static void CopyToLive()
        {
            Debug.ConOut("Copy2Live is heavily WIP", false, true);
            Thread.Sleep(2000);

            string fileName = "";

            string sourcePath = Directory.GetCurrentDirectory() + @"\tmp";
            string targetPath = Directory.GetCurrentDirectory();

            //Use Path class to manipulate file and directory paths.
            //string sourceFile = System.IO.Path.Combine(sourcePath, fileName);

            string destFile = System.IO.Path.Combine(targetPath, fileName);

            string[] files = Directory.GetFiles(sourcePath);

            //https://msdn.microsoft.com/en-us/library/system.io.path.getfilename(v=vs.110).aspx

            try
            {
                foreach (string s in files)
                {
                    fileName = Path.GetFileName(s);
                    destFile = Path.Combine(targetPath, fileName);
                    System.IO.File.Copy(s, destFile, true);
                    Debug.ConOut("C2L: Moved " + s + " to " + targetPath, false, false, true);
                }
            }
            catch (Exception e)
            {
                Debug.ConOut(e.Message, true);
            }

        }

        //check file integrity
        static void SHA256Check(string sha256, string FileToCheck)
        {
            Debug.ConOut("SHA256 check in progress...", false, true);

            Debug.ConOut("Opening file to generate sha256...");
            FileStream fs = File.Open(FileToCheck, FileMode.Open, FileAccess.Read, FileShare.None);

            Debug.ConOut("Create sha256 instance");
            SHA256 sha256r = SHA256Managed.Create();

            Debug.ConOut("Create hash from file...");
            byte[] hashvalue = sha256r.ComputeHash(fs);

            Debug.ConOut("Closing file");
            fs.Close();

            Debug.ConOut("Convert byte array into a string");
            PrintByteArray(hashvalue);

            Debug.ConOut("Read sha256 from the server");
            string sha256fromweb = File.ReadAllText(sha256);

            Debug.ConOut("Write generated sha256 to a file");
            File.WriteAllText("update_generated.sha256", sha256generated);

            Debug.ConOut("Comparing " + sha256generated + " to " + sha256fromweb);
            if (sha256generated == sha256fromweb)
            {
                Debug.ConOut("File integrity: OK", false, false, true);
                fileIntegrity = true;
            }
            else
            {
                Debug.ConOut("File integrity: BAD", true);
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
                Debug.ConOut(sha256generated, false, false, true);
                if ((i % 4) == 3)
                {
                    Debug.ConOut(" ", false, false, true);
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
