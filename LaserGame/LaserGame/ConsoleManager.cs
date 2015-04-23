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

        private static readonly int PREFIX_LENGTH = 26;
        private static bool debugMode = false;
        private static bool infoMode = false;

        public static bool DebugMode {
            get { return debugMode; }
            set {
                if(value == true) {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault( false );
                    Thread Console = new Thread( () => AllocConsole() );
                    Console.Name = "ConsoleWindow";
                    Console.Start();
                    debugMode = true;
                }
            }
        }

        public static void SetDebugMode() {
            if(Environment.MachineName == "NWRMP01") {
                DialogResult dr = MessageBox.Show( "Would you like to run in debug mode?", "Debug", MessageBoxButtons.YesNo );
                switch(dr) {
                    case DialogResult.Yes:
                        ConsoleManager.DebugMode = true;
                        break;
                    case DialogResult.No:
                        ConsoleManager.DebugMode = false;
                        break;
                }
            }
        }

        public static void Game( string message ) {
            if(infoMode) return;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine( Prefix( "(Info)(Game)" ) + message );
            Console.ResetColor();
        }

        public static void GameWarn( string message) {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine( Prefix( "(Warn)(Game)" ) + message );
            Console.ResetColor();
        }

        public static void GameError( string message) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine( Prefix( "(Error)(Game)" ) + message );
            Console.ResetColor();
        }

        public static void DebugGame( string message) {
            if(debugMode) return;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine( Prefix( "(Debug)(Game)" ) + message );
            Console.ResetColor();
        }

        private static string Prefix(string prefix) {
            String temp=prefix;
            for(int i = temp.Length; i < PREFIX_LENGTH; i++) {
                temp = temp + " ";
            }
            return temp;
        }

    }
}
