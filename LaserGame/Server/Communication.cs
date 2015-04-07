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
            client.IsConnected = true;
            ReadMessages();
        }

        private void ReadMessages() {
            try {
                while(client.IsConnected) {
                    string message = client.ReadLine();

                    if(GlobalVariable.debugMode) {
                        Console.WriteLine( message );
                    }

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
                client.NickName = name;
                AcceptConnection();
            } else {
                client.WriteLine( "NickNameInUse:" );
            }
        }

        private void AcceptConnection() {
            Server._nickName.Add( client.NickName, client.TcpClient );
            Server._nickNameByConnect.Add( client.TcpClient, client.NickName );
            client.WriteLine( "ConnectionAccepted:" + client.NickName );
        }

        private void CloseConnection() {
            Server._nickName.Remove( client.NickName );
            Server._nickNameByConnect.Remove( client.TcpClient );
            client.Dispose();
        }
    }
}
