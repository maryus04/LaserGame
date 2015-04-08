using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server {
    public class ConsoleManager {
        public static bool debugMode = false;

        public static void ServerInfo( string message ) {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine( message );
                Console.ResetColor();
        }

        public static void DebugComunication(string message) {
            if(ConsoleManager.debugMode) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine( message );
                Console.ResetColor();
            }
        }

        public static void Comunication( string message ) {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine( message );
            Console.ResetColor();
        }
    }
}
