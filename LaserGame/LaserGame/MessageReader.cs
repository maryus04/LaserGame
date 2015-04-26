using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows;
using System.Windows.Shapes;

namespace Client {
    class MessageReader {

        private static MainWindow _main;
        private static GameWindow _game;

        public static void SetMainWindow( MainWindow main ) {
            _main = main;
        }

        public static void SetGameWindow( GameWindow game ) {
            _game = game;
        }

        public static void ReadMessages() {
            while(true) {
                string message = Player.ReadLine();
                if(message == null || message.Length == 0) {
                    break;
                }

                DebugManager.DebugGame( "Server sent:" + message );

                string method = message.Substring( 0, message.IndexOf( ":" ) + 1 );
                message = message.Replace( method, "" );

                switch(method) {
                    case "ConnectionAccepted:":
                        Player.Name = message;
                        DebugManager.Game( "Connected as " + Player.Name );
                        Player.Connected = true;
                        _main.Dispatcher.Invoke( (Action)(() => { new GameWindow(); _main.Close(); }) );
                        break;
                    case "NickNameInUse:":
                        _main.SetError( "Nickname already in use" );
                        DebugManager.GameWarn( "Nickname already in use" );
                        break;
                    case "PortalAccepted:":
                        Tuple<int,int> points = Portal.GetPointsFromMessage(message);
                        _game.PortalAccepted( points.Item1, points.Item2 );
                        _game.CanvasChanged();
                        break;
                    case "PortalDenied:":
                        DebugManager.GameWarn( message );
                        break;
                }
            }
        }

        private static void StartGame() {
            Thread game = new Thread( () => new GameWindow() ) { Name = "GameWindow" };
            game.SetApartmentState( ApartmentState.STA );
            game.Start();
        }

    }
}
