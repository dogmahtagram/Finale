using System;

namespace TheFinale
{
#if WINDOWS || XBOX
    class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            using (TheFinale game = new TheFinale())
            {
                game.Run();
            }
        }
    }
#endif
}

