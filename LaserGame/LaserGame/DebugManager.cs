using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Forms = System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace Client {
    public class DebugManager {

        [DllImport( "kernel32.dll", SetLastError = true )]
        [return: MarshalAs( UnmanagedType.Bool )]
        static extern bool AllocConsole();

        private static readonly int PREFIX_LENGTH = 26;
        private static bool _debugMode = false;

        public static bool DebugMode { get { return _debugMode; } set { _debugMode = value; } }

        public static void EnableConsole() {
            if(Environment.MachineName == "NWRMP01") {
                Forms.DialogResult dr = Forms.MessageBox.Show( "Would you like to attach the console?", "DebugManager", Forms.MessageBoxButtons.YesNo );
                switch(dr) {
                    case Forms.DialogResult.Yes:
                        CreateConsole();
                        EnableDebug();
                        break;
                    case Forms.DialogResult.No:
                        break;
                }
            }
        }

        public static void EnableDebug() {
            Forms.DialogResult dr = Forms.MessageBox.Show( "Would you like to run in debug mode?", "Debug", Forms.MessageBoxButtons.YesNo );
            switch(dr) {
                case Forms.DialogResult.Yes:
                    DebugManager.DebugMode = true;
                    break;
                case Forms.DialogResult.No:
                    DebugManager.DebugMode = false;
                    break;
            }
        }

        private static void CreateConsole() {
            Forms.Application.EnableVisualStyles();
            Forms.Application.SetCompatibleTextRenderingDefault( false );
            Thread Console = new Thread( () => AllocConsole() );
            Console.Name = "ConsoleWindow";
            Console.Start();
        }

        public static void Game( string message ) {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine( Prefix( "(Info)(Game)" ) + message );
            Console.ResetColor();
        }

        public static void GameWarn( string message ) {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine( Prefix( "(Warn)(Game)" ) + message );
            Console.ResetColor();
        }

        public static void GameError( string message ) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine( Prefix( "(Error)(Game)" ) + message );
            Console.ResetColor();
        }

        public static void DebugGame( string message ) {
            if(!_debugMode) return;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine( Prefix( "(Debug)(Game)" ) + message );
            Console.ResetColor();
        }

        private static string Prefix( string prefix ) {
            String temp = prefix;
            for(int i = temp.Length; i < PREFIX_LENGTH; i++) {
                temp = temp + " ";
            }
            return temp;
        }
    }
}
