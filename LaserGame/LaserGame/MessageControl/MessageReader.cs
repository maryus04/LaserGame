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
                        GameWindow.GetInstance().DeleteMyLaser();
                        GameWindow.GetInstance().PortalAccepted( MessageParser.GetPoint( entireMessage ) );
                        GameWindow.GetInstance().ConstructLaser();
                        break;
                    case "PortalDenied:":
                        DebugManager.GameWarn( _message );
                        break;
                    case "PortalSpawned:":
                        GameWindow.GetInstance().PortalSpawnedByOtherPlayer( MessageParser.GetPoint( entireMessage ) );
                        GameWindow.GetInstance().ConstructLaser();
                        break;
                    case "PortalRemoved:":
                        GameWindow.GetInstance().CanvasChanged();
                        GameWindow.GetInstance().PortalRemovedByOtherPlayer( MessageParser.GetPoint( entireMessage ) );
                        GameWindow.GetInstance().ConstructLaser();
                        break;
                    case "LaserCreated:":
                        GameWindow.GetInstance().Dispatcher.Invoke( (Action)(() => {
                            Line line = MessageParser.GetLine( entireMessage );
                            Laser.GetInstance().BuildLaserLine( MessageParser.GetValue( entireMessage ), new Point( line.X1, line.Y1 ), new Point( line.X2, line.Y2 ) );
                        }) );
                        GameWindow.GetInstance().ConstructLaser();
                        break;
                    case "LaserRemoved:":
                        Line laser = MessageParser.GetLine( _message );
                        GameWindow.GetInstance().RemoveFromGameCanvas( Laser.GetInstance().GetLine( "" + laser.X1 + laser.Y1 + laser.X2 + laser.Y2 ) );
                        Laser.GetInstance().RemoveLine( "" + laser.X1 + laser.Y1 + laser.X2 + laser.Y2 );
                        break;
                    case "MainWindowMessage:":
                        string name = MessageParser.GetNick( _message );
                        _message = MessageParser.RemoveNickFrom( _message );
                        MainWindow.getInstance().AppendText( name + _message + "\r\n" );
                        break;
                    case "MainWindowServerMessage:":
                        if(MainWindow.getInstance() != null) {
                            MainWindow.getInstance().AppendText( _message + "\r\n" );
                        }
                        break;
                    case "GameWindowMessage:":
                        name = MessageParser.GetNick( _message );
                        _message = MessageParser.RemoveNickFrom( _message );
                        GameWindow.GetInstance().AppendText( name + _message + "\r\n" );
                        break;
                    case "GameWindowServerMessage:":
                        if(GameWindow.GetInstance() != null) {
                            GameWindow.GetInstance().AppendText( _message + "\r\n" );
                        }
                        break;
                    case "AllPlayersAreReady:":
                        if(MainWindow.getInstance() != null) {
                            MainWindow.getInstance().AllPlayersReady();
                        }
                        break;
                    case "Players:":
                        MainWindow.getInstance().SetPlayerList( _message.Split( ',' ) );
                        break;
                    case "PlayerReady:":
                        MainWindow.getInstance().UpdatePlayerStatus( MessageParser.GetName( _message ), MessageParser.GetValue( _message ) );
                        break;
                    case "MapAccepted:":
                        GameWindow.GetInstance().CreateMap( _message );
                        break;
                    case "Resolution:":
                        GameWindow.GetInstance().Dispatcher.Invoke( (Action)(() => { MapParser.SetResolution( MessageParser.GetPoint( _message ) ); }) );
                        break;
                    case "MapName:":
                        MainWindow.getInstance().SetMapName( _message );
                        break;
                    case "GameFinished:":
                        GameWindow.GetInstance().GameFinished();
                        break;
                }
            }
        }

        private static void SetMethodMessage( string message ) {
            if(message.Contains( ":" )) {
                _method = message.Substring( 0, message.IndexOf( ":" ) + 1 );
                _message = message.Replace( _method, "" );
            }
        }
    }
}
