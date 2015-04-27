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

namespace Server {
    class Communication {
        ServerClient client = new ServerClient();

        public Communication( TcpClient tcpClient ) {
            client.TcpClient = tcpClient;

            BeginConnection();
        }

        private void BeginConnection() {
            client.Reader = new StreamReader( client.GetStream() );
            client.Writer = new StreamWriter( client.GetStream() );
            
            ReadMessages();
        }

        private void ReadMessages() {
            try {
                while(true) {
                    string message = client.ReadLine();
                    if(message.Length == 0) {
                        break;
                    }

                    ConsoleManager.CommunicationDebug( "--" + client.NickName + " sent:" + message );

                    string method = message.Substring( 0, message.IndexOf( ":" ) + 1 );
                    message = message.Replace( method, "" );

                    switch(method) {
                        case "MyName:":
                            ValidateNickName( message );
                            break;
                        case "CloseConnection:":
                            CloseConnection();
                            break;
                        case "PortalCreated:":
                            PortalCreated(message);
                            break;
                    }
                }
            } catch(Exception) { }
        }

        private void PortalCreated(string message) {
            Tuple<int,int> points = PortalHandler.GetPointsFromMessage(message);
            Rectangle portal = new Rectangle( points.Item1 - 5, points.Item2 - 5, 10, 10 );
            if(!PortalHandler.IsPortalIntersectingPortal( portal )) {
                PortalHandler.SetCurrentPortal( client, portal );
                client.WriteLine( "PortalAccepted:COORD:" + points.Item1 + "," + points.Item2 + "ENDCOORD" );
                PortalHandler.SendPortalExcept( client, new Point(points.Item1 , points.Item2) );
                ConsoleManager.Communication( client.NickName + " created a portal at " + points.Item1 + "," + points.Item2 );
            } else {
                client.WriteLine( "PortalDenied:" + points.Item1 + "," + points.Item2 + " intersecting another portal." );
            }
        }

        private void ValidateNickName( string name ) {
            if(!Server._nickName.Contains( name ) && !"NotYetSet".Equals(name)) {
                client.NickName = name;
                AcceptConnection();
            } else {
                ConsoleManager.Communication( "Name \"" + name + "\" already in use. Connection refused." );
                client.WriteLine( "NickNameInUse:" );
            }
        }

        private void AcceptConnection() {
            Server._nickName.Add( client.NickName, client );
            client.isConnected = true;
            client.WriteLine( "ConnectionAccepted:" + client.NickName );
            ConsoleManager.Communication( client.NickName + " is now connected to server." + " Address " + client.GetIp() + " Port:" + client.GetPort() );
        }

        private void CloseConnection() {
            ConsoleManager.Communication( client.NickName + " is now disconnected from the server." );
            Server._nickName.Remove( client.NickName );
            client.Dispose();
        }
    }
}
