using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows;
using System.Windows.Shapes;
using Client.Map;
using Client.CanvasComponents;
using Client.CanvasBehavior;

namespace Client.MessageControl {
    class MessageReader {

        private static string _message, _method;

        public static void ReadMessages() {
            while(true) {
                string entireMessage = Player.getInstance().ReadLine();
                if(entireMessage == null || entireMessage.Length == 0) {
                    break;
                }
                DebugManager.DebugGame( "Server sent:" + entireMessage );

                SetMethodMessage( entireMessage );

                switch(_method) {
                    case "ConnectionAccepted:":
                        Player.getInstance().Name = _message;
                        Player.getInstance().Connected = true;
                        MainWindow.getInstance().SetAvaibility();
                        DebugManager.Game( "Connected as " + Player.getInstance().Name );
                        break;
                    case "NickNameInUse:":
                        MainWindow.getInstance().SetError( "Nickname already in use" );
                        DebugManager.GameWarn( "Nickname already in use" );
                        break;
                    case "PortalAccepted:":
                        GameWindow.getInstance().DeleteMyLaser();
                        GameWindow.getInstance().PortalAccepted( MessageParser.GetPoint( entireMessage ) );
                        GameWindow.getInstance().ConstructLaser();
                        break;
                    case "PortalDenied:":
                        DebugManager.GameWarn( _message );
                        break;
                    case "PortalSpawned:":
                        GameWindow.getInstance().PortalSpawnedByOtherPlayer( MessageParser.GetPoint( entireMessage ) );
                        GameWindow.getInstance().ConstructLaser();
                        break;
                    case "PortalRemoved:":
                        GameWindow.getInstance().CanvasChanged();
                        GameWindow.getInstance().PortalRemovedByOtherPlayer( MessageParser.GetPoint( entireMessage ) );
                        GameWindow.getInstance().ConstructLaser();
                        break;
                    case "LaserCreated:":
                        GameWindow.getInstance().Dispatcher.Invoke( (Action)(() => {
                            Line line = MessageParser.GetLine( entireMessage );
                            Laser.getInstance().BuildLaserLine( new Point( line.X1, line.Y1 ), new Point( line.X2, line.Y2 ) );
                        }) );
                        GameWindow.getInstance().ConstructLaser();
                        break;
                    case "LaserRemoved:":
                        Line laser = MessageParser.GetLine( _message );
                        GameWindow.getInstance().RemoveFromGameCanvas( Laser.getInstance().GetLine( "" + laser.X1 + laser.Y1 + laser.X2 + laser.Y2 ) );
                        Laser.getInstance().RemoveLine( "" + laser.X1 + laser.Y1 + laser.X2 + laser.Y2 );
                        break;
                    case "MainWindowMessage:":
                        string name = MessageParser.GetNick( _message );
                        _message = MessageParser.RemoveNickFrom( _message );
                        MainWindow.getInstance().AppendText( name + _message + "\r\n" );
                        break;
                    case "MainWindowServerMessage:":
                        MainWindow.getInstance().AppendText( _message + "\r\n" );
                        break;
                    case "AllPlayersAreReady:":
                        MainWindow.getInstance().AllPlayersReady();
                        break;
                    case "Players:":
                        MainWindow.getInstance().SetPlayerList( _message.Split( ',' ) );
                        break;
                    case "PlayerReady:":
                        MainWindow.getInstance().UpdatePlayerStatus( MessageParser.GetName( _message ), MessageParser.GetValue( _message ) );
                        break;
                    case "MapAccepted:":
                        GameWindow.getInstance().CreateMap( _message );
                        break;
                    case "Resolution:":
                        GameWindow.getInstance().Dispatcher.Invoke( (Action)(() => { MapParser.SetResolution( MessageParser.GetPoint( _message ) ); }) );
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
