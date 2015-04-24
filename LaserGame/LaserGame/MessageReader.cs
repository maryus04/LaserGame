using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Client {
    class MessageReader {

        private static MainWindow _main;
        private static GameWindow game;

        public static void SetMainWindow( MainWindow main ) {
            _main = main;
        }

        public static void ReadMessages() {
            while(true) {
                string message = Player.ReadLine();
                if(message.Length == 0) {
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
                        _main.Dispatcher.Invoke( (Action)(() => { game = new GameWindow(); }) );
                        _main.Dispatcher.Invoke( (Action)(() => { _main.Close(); }) );
                        break;
                    case "NickNameInUse:":
                        _main.Dispatcher.Invoke( (Action)(() => { _main.SetError( "Nickname already in use" ); }) );
                        DebugManager.GameWarn( "Nickname already in use" );
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
