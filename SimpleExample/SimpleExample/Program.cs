using System;

namespace SimpleExample
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (SimpleExample game = new SimpleExample())
            {
                game.Run();
            }
        }
    }
#endif
}

