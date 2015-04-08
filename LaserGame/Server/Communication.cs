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
            client.tcpClient = tcpClient;

            BeginConnection();
        }

        private void BeginConnection() {
            client.reader = new StreamReader( client.GetStream() );
            client.writer = new StreamWriter( client.GetStream() );
            client.isConnected = true;
            ReadMessages();
        }

        private void ReadMessages() {
            try {
                while(client.isConnected) {
                    string message = client.ReadLine();

                    ConsoleManager.DebugComunication( "--" + client.nickName + " sent:" + message );

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
            if(!Server._nickName.Contains( name )) {
                client.nickName = name;
                AcceptConnection();
            } else {
                ConsoleManager.DebugComunication( "Name \"" + name + "\" already in use. Connection refused." );
                client.WriteLine( "NickNameInUse:" );
            }
        }

        private void AcceptConnection() {
            Server._nickName.Add( client.nickName, client.tcpClient );
            Server._nickNameByConnect.Add( client.tcpClient, client.nickName );
            client.WriteLine( "ConnectionAccepted:" + client.nickName );
            ConsoleManager.Comunication( client.nickName + " is now connected to server. Port:" + client.GetPort() );
        }

        private void CloseConnection() {
            Server._nickName.Remove( client.nickName );
            Server._nickNameByConnect.Remove( client.tcpClient );
            ConsoleManager.Comunication( client.nickName + " is now disconnected from the server." );
            client.Dispose();

        }
    }
}
