using System;
using System.IO;
public class Debug
{
    /// <summary>
    /// Loads the debug shit, creates log files etc etc 
    /// </summary>
    public static void Init()
    {
        string path = Path.Combine(Directory.GetCurrentDirectory() + "\\logs\\");
        //check if old logs exist and clear em
        try
        {
            if (Directory.Exists(path))
            {
                Console.WriteLine("Log directory already exists... continuing");
            }
            else
            {
                Console.WriteLine("Creating log directory...");
                Directory.CreateDirectory(path);
            }

            if (File.Exists(path + "debug_normal.log"))
            {
                foreach (FileInfo f in new DirectoryInfo(path).GetFiles("debug*.log"))
                {
                    f.Delete();
                    File.Create(path + f.ToString()).Close();
                    Console.WriteLine(f.ToString() + " deleted and remade.");
                }
            }
            else
            {
                File.Create(path + "debug_normal.log").Close();
                File.Create(path + "debug_error.log").Close();
                File.Create(path + "debug_special.log").Close();

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;

        }
    }
    /// <summary>
    /// Outputs to the console with a timestamp before the message, and writes all this to a file. [string, bool, bool, bool]
    /// Usage: ConOut(Message, Error?, Special?, Really Special?)
    /// </summary>
    public static void ConOut(string Msg, bool ERR = false, bool SPC = false, bool SPC2 = false)
    {

        if (ERR)
        {
            string Message = (DateTime.Now.Hour + ":" + DateTime.Now.Minute + "." + DateTime.Now.Second + "|" + DateTime.Now.Millisecond + ": ERROR: " + Msg);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(Message);

            WriteToFile(Message, "error");
        }
        else if (SPC)
        {
            string Message = (DateTime.Now.Hour + ":" + DateTime.Now.Minute + "." + DateTime.Now.Second + "|" + DateTime.Now.Millisecond + ": SPECIAL: " + Msg);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(Message);

            WriteToFile(Message, "special");
        }
        else if (SPC2)
        {
            string Message = (DateTime.Now.Hour + ":" + DateTime.Now.Minute + "." + DateTime.Now.Second + "|" + DateTime.Now.Millisecond + ": SPECIAL: " + Msg);


            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(Message);

            WriteToFile(Message, "special");
        }
        else
        {
            string Message = (DateTime.Now.Hour + ":" + DateTime.Now.Minute + "." + DateTime.Now.Second + "|" + DateTime.Now.Millisecond + ": " + Msg);

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Message);

            WriteToFile(Message, "normal");
        }
    }

    private static void WriteToFile(string toWrite, string type)
    {
        try
        {

            string pathtowrite = Path.Combine(Directory.GetCurrentDirectory() + "\\logs\\");
            System.IO.StreamReader ReadMe = new System.IO.StreamReader(pathtowrite + "debug_" + type + ".log");
            string toWriteAppended = ReadMe.ReadToEnd();
            toWriteAppended += toWrite + Environment.NewLine;
            ReadMe.Close();


            System.IO.StreamWriter WriteMe = new System.IO.StreamWriter(pathtowrite + "debug_" + type + ".log");
            WriteMe.Write(toWriteAppended);
            WriteMe.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }
}
