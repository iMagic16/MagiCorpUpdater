using System;

public class Debug
{
    /// <summary>
    /// Outputs to the console with a timestamp before the message.
    /// Usage: ConOut(Message, Error?, Special?)
    /// </summary>
    public static void ConOut(string Msg, bool ERR = false, bool SPC = false, bool SPC2 = false)
    {
        if (ERR)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("{0}:{1}.{2}|{3}: ERROR: " + Msg, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
        }
        else if (SPC)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("{0}:{1}.{2}|{3}: SPECIAL: " + Msg, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
        }
        else if (SPC2)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("{0}:{1}.{2}|{3}: SPECIAL: " + Msg, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("{0}:{1}.{2}|{3}: " + Msg, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
        }
    }
}
