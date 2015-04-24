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
        private static bool infoMode = false;

        public static bool DebugMode {
            get { return _debugMode; }
            set {
                if(value == true) {
                    Forms.Application.EnableVisualStyles();
                    Forms.Application.SetCompatibleTextRenderingDefault( false );
                    Thread Console = new Thread( () => AllocConsole() );
                    Console.Name = "ConsoleWindow";
                    Console.Start();
                    _debugMode = true;
                }
            }
        }

        public static void SetDebugMode() {
            if(Environment.MachineName == "NWRMP01") {
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
            if(_debugMode) return;
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

        // ===========================================================LASER DEBUG
        private static List<Point> _pointsToBeDrawn = new List<Point>();

        public static List<Point> PointsToBeDrawn {
            set {
                if(_debugMode) {
                    _pointsToBeDrawn = value;
                }
            }
            get { return _pointsToBeDrawn; }
        }

        public static void DebugLaser( Canvas debugCanvas) {
            if(!_debugMode) return;
            DeletePointsFromCanvas( debugCanvas );
            foreach(Point intersection in _pointsToBeDrawn) {
                DrawPoint( debugCanvas, intersection );
            }
            DeletePointList( debugCanvas );
        }

        private static void DrawPoint( Canvas canvas, Point point ) {
            if(!DebugManager.DebugMode) return;
            var ellipse = new Ellipse() { Width = 8, Height = 8, Fill = new SolidColorBrush( Colors.Blue ), Stroke = new SolidColorBrush( Colors.Blue ) };
            Canvas.SetLeft( ellipse, point.X - 4 );
            Canvas.SetTop( ellipse, point.Y - 4 );
            canvas.Children.Add( ellipse );
        }

        private static void DeletePointList( Canvas canvas ) {
            if(!DebugManager.DebugMode) return;
            _pointsToBeDrawn = new List<Point>();
        }

        private static void DeletePointsFromCanvas( Canvas canvas ) {
            if(!DebugManager.DebugMode) return;
            canvas.Children.RemoveRange( 0, canvas.Children.Count );
        }

    }
}
