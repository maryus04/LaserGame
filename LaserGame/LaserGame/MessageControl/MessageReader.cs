using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows;
using System.Windows.Shapes;

namespace Client.MessageControl {
    class MessageReader {

        private static string _message, _method;

        public static void ReadMessages() {
            while(true) {
                string message = Player.getInstance().ReadLine();
                if(message == null || message.Length == 0) {
                    break;
                }
                DebugManager.DebugGame( "Server sent:" + message );

                SetMethodMessage( message );

                switch(_method) {
                    case "ConnectionAccepted:":
                        Player.getInstance().Name = _message;
                        Player.getInstance().Connected = true;
                        MainWindow.getInstance().StartGame();
                        DebugManager.Game( "Connected as " + Player.getInstance().Name );
                        break;
                    case "NickNameInUse:":
                        MainWindow.getInstance().SetError( "Nickname already in use" );
                        DebugManager.GameWarn( "Nickname already in use" );
                        break;
                    case "PortalAccepted:":
                        GameWindow.getInstance().PortalAccepted( MessageParser.GetPoint( message ) );
                        GameWindow.getInstance().CanvasChanged();
                        break;
                    case "PortalDenied:":
                        DebugManager.GameWarn( _message );
                        break;
                    case "PortalSpawned:":
                        GameWindow.getInstance().PortalSpawnedByOtherPlayer( MessageParser.GetPoint( message ) );
                        GameWindow.getInstance().CanvasChanged();
                        break;
                    case "PortalRemoved:":
                        GameWindow.getInstance().PortalRemovedByOtherPlayer( MessageParser.GetPoint( message ) );
                        GameWindow.getInstance().CanvasChanged();
                        break;
                }
            }
        }

        private static void SetMethodMessage( string message ) {
            _method = message.Substring( 0, message.IndexOf( ":" ) + 1 );
            _message = message.Replace( _method, "" );
        }
    }
}
