using System;
namespace Program
{
    public class Logo
    {
        public void ShowLogo()
        {
            Console.ForegroundColor = ConsoleColor.Green; //Change colors to green
            Console.WriteLine("================================================");
            Console.WriteLine("   ____      _               ____        _      ");//Start of ascii logo
            Console.WriteLine("  / ___|   _| |__   ___ _ __| __ )  ___ | |_   ");
            Console.WriteLine(" | |  | | | | '_ \\ / _ \\ '__|  _ \\ / _ \\| __|  ");
            Console.WriteLine(" | |__| |_| | |_) |  __/ |  | |_) | (_) | |_   ");
            Console.WriteLine("  \\____\\__, |_.__/ \\___|_|  |____/ \\___/ \\__|  ");
            Console.WriteLine("       |___/                                     ");//End of ascii log
            Console.WriteLine("   Cybersecurity Awareness Bot                  ");
            Console.WriteLine("================================================");
            Console.ResetColor();//Reset Color
        }
    }
}