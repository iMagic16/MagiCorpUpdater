using System;

public class Debug
{
    /// <summary>
    /// Outputs to the console with a timestamp before the message.
    /// Usage: ConOut(Message, Error?, Special?)
    /// </summary>
    public static void ConOut(string Msg, bool ERR = false, bool SPC = false)
    {
        if (ERR)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("{0}:{1}.{2}|{3}: ERROR: " + Msg, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
        }
        else if (SPC)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(Msg);
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("{0}:{1}.{2}|{3}: " + Msg, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
        }
    }
}
