using System.IO;
using System.Net;
using System;
using System.Threading;
using Chat = System.Net;
using System.Collections;
using Server;
using System.Net.Sockets;
using System.Security.Permissions;
using Server.Handlers;
using System.Drawing;
using Server.MessageControl;

namespace Server {
    class Communication {
        ServerClient _client = new ServerClient();

        private static string _message, _method;

        public Communication( TcpClient tcpClient ) {
            _client.TcpClient = tcpClient;

            BeginConnection();
        }

        private void BeginConnection() {
            _client.Reader = new StreamReader( _client.GetStream() );
            _client.Writer = new StreamWriter( _client.GetStream() );

            ReadMessages();
        }

        private void ReadMessages() {
            while(true) {
                string message = _client.ReadLine();
                if(message == null || message.Length == 0) {
                    break;
                }

                ConsoleManager.CommunicationDebug( _client.NickName + " sent:" + message );

                SetMethodMessage( message );

                switch(_method) {
                    case "MyName:":
                        ValidateNickName( _message );
                        break;
                    case "CloseConnection:":
                        Server.SendServerToAll( "MainWindowServerMessage:** " + _client.NickName + " left the room." );
                        CloseConnection();
                        Server.SendPlayerNames();
                        break;
                    case "PortalCreated:":
                        PortalCreated( _message );
                        break;
                    case "LaserCreated:":
                        LaserCreated( _message );
                        break;
                    case "LaserRemoved:":
                        Server.SendServerMessageExcept( _client, "LaserRemoved:" + _message );
                        break;
                    case "MainWindowMessage:":
                        Server.SendPlayerToAll( _client, "MainWindowMessage:" + _message );
                        break;
                    case "ReadyPressed:":
                        _client.Ready = Boolean.Parse( _message );
                        Server.PlayersAreReady();
                        Server.UpdateReadyStatus( _client );
                        break;
                    case "MapChanged:":
                        Server.SetCurrentMap( _message );
                        break;
                    case "MyResolution:":
                        _client.SetResolution( MessageParser.GetPointsFromMessage( _message ) );
                        Server.CheckResolution();
                        break;
                }
            }
        }

        private static void SetMethodMessage( string message ) {
            _method = message.Substring( 0, message.IndexOf( ":" ) + 1 );
            _message = message.Replace( _method, "" );
        }

        private void LaserCreated( string message ) {
            Tuple<double, double, double, double> line = MessageParser.GetLine( message );
            string buildingDirection = MessageParser.GetValue( message );
            Server.SendServerMessageExcept( _client, "LaserCreated:VALUE:" + buildingDirection + "ENDVALUE" + "COORD2:" + line.Item1 + "," + line.Item2 + "," + line.Item3 + "," + line.Item4 + "ENDCOORD2" );

            ConsoleManager.Communication( _client.NickName + " created a laser at (" + line.Item1 + "," + line.Item2 + ") (" + line.Item3 + "," + line.Item4 + ")" );
        }

        private void PortalCreated( string message ) {
            Tuple<int, int> points = MessageParser.GetPointsFromMessage( message );
            Rectangle portal = new Rectangle( points.Item1 - 5, points.Item2 - 5, 10, 10 );
            if(!PortalHandler.IsPortalIntersectingPortal( portal )) {
                PortalHandler.SetCurrentPortal( _client, portal );
                _client.WriteLine( "PortalAccepted:COORD:" + points.Item1 + "," + points.Item2 + "ENDCOORD" );
                PortalHandler.SendPortalExcept( _client, new Point( points.Item1, points.Item2 ) );
                ConsoleManager.Communication( _client.NickName + " created a portal at " + points.Item1 + "," + points.Item2 );
            } else {
                _client.WriteLine( "PortalDenied:" + points.Item1 + "," + points.Item2 + " intersecting another portal." );
            }
        }

        private void ValidateNickName( string name ) {
            if(!Server._nickName.Contains( name ) && !"NotYetSet".Equals( name )) {
                _client.NickName = name;
                AcceptConnection();
                Server.SendServerToAll( "MainWindowServerMessage:** " + _client.NickName + " joined the room." );
                Server.SendPlayerNames();
            } else {
                ConsoleManager.Communication( "Name \"" + name + "\" already in use. Connection refused." );
                _client.WriteLine( "NickNameInUse:" );
            }
        }

        private void AcceptConnection() {
            Server._nickName.Add( _client.NickName, _client );
            _client.isConnected = true;
            _client.WriteLine( "ConnectionAccepted:" + _client.NickName );
            ConsoleManager.Communication( _client.NickName + " is now connected to server." + " Address " + _client.GetIp() + " Port:" + _client.GetPort() );
        }

        private void CloseConnection() {
            ConsoleManager.Communication( _client.NickName + " is now disconnected from the server." );
            Server._nickName.Remove( _client.NickName );
            _client.Dispose();
        }
    }
}
