using System;
using System.IO;
using System.Media;

namespace ai_response
{
    public static class AudioGreeting
    {
        public static void PlayGreeting()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;//String to find the Greeting recording directory

            string joined_path = Path.Combine(path, "greeting.wav");//Greeting is in Bin/Debug so the system will retrieve the file using this path
            Console.WriteLine(joined_path);
            SoundPlayer player = new SoundPlayer(joined_path);
            player.Load();//Load Recording
            player.Play();//Play recording 
        }
    }
}