using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;

namespace Client {
    public class ConsoleManager {

        [DllImport( "kernel32.dll", SetLastError = true )]
        [return: MarshalAs( UnmanagedType.Bool )]
        static extern bool AllocConsole();

        private static bool debugMode;

        public static bool DebugMode {
            get { return debugMode; }
            set {
                if(value == true) {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault( false );
                    Thread Console = new Thread( () => CreateConsoleWindow() );
                    Console.Start();
                    debugMode = true;
                }
            }
        }

        public static void CreateConsoleWindow() {
            AllocConsole();
        }

        public static void WriteLine( string message ) {
            Console.WriteLine( message );
        }

    }
}
