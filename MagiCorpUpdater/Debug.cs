using System;
using System.IO;
public class Debug
{
    /// <summary>
    /// Loads the debug shit, creates log files etc etc 
    /// </summary>
    public static void Init()
    {
         //check if old logs exist and clear em
            if (File.Exists("debug_normal.log"))
            {
                foreach (FileInfo f in new DirectoryInfo(Directory.GetCurrentDirectory()).GetFiles("debug*.log"))
                {
                    f.Delete();
                    Console.WriteLine(f.ToString() + "deleted and remade.");
                    File.Create(f.ToString()).Close();
                }
            }
            else
            {
                File.Create("debug_normal.log").Close();
                File.Create("debug_error.log").Close();
                File.Create("debug_special.log").Close();
                
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
        System.IO.StreamReader ReadMe = new System.IO.StreamReader("debug_" + type + ".log");
        string toWriteAppended = ReadMe.ReadToEnd();
        toWriteAppended += toWrite + Environment.NewLine;
        ReadMe.Close();

        System.IO.StreamWriter WriteMe = new System.IO.StreamWriter("debug_" + type + ".log");
        WriteMe.Write(toWriteAppended);
        WriteMe.Close();
    }
}
