using System.IO;
using System.Net;
using System;
using System.Threading;
using Chat = System.Net;
using System.Collections;
using Server;
using System.Net.Sockets;
using System.Security.Permissions;

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
                            Thread acceptConnection = new Thread( () => ValidateNickName( message ) );
                            acceptConnection.Name = "AcceptConnetion";
                            acceptConnection.Start();
                            break;
                        case "CloseConnection:":
                            Thread closeConnection = new Thread( () => CloseConnection() );
                            closeConnection.Name = "CloseConnetion";
                            closeConnection.Start();
                            break;
                    }
                }
            } catch(Exception) { }
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
            Server._nickName.Add( client.NickName, client.TcpClient );
            Server._nickNameByConnect.Add( client.TcpClient, client.NickName );
            client.isConnected = true;
            client.WriteLine( "ConnectionAccepted:" + client.NickName );
            ConsoleManager.Communication( client.NickName + " is now connected to server." + " Address " + client.GetIp() + " Port:" + client.GetPort() );
        }

        private void CloseConnection() {
            ConsoleManager.Communication( client.NickName + " is now disconnected from the server." );
            Server._nickName.Remove( client.NickName );
            Server._nickNameByConnect.Remove( client.TcpClient );
            client.Dispose();
        }
    }
}
