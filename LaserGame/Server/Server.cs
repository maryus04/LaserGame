using System.IO;
using System.Net;
using System;
using System.Threading;
using Chat = System.Net;
using System.Collections;
using System.Net.Sockets;

namespace Server {
    public class Server {
        TcpListener _chatServer;
        public static Hashtable _nickName;
        public static Hashtable _nickNameByConnect;

        public static void Main() {
            ConsoleManager.debugMode = true; //debug mode
            new Server();
        }

        public Server() {
            _nickName = new Hashtable( 100 );
            _nickNameByConnect = new Hashtable( 100 );
            _chatServer = new TcpListener( IPAddress.Parse( "127.0.0.1" ) , 4296 );
            _chatServer.Start();
            ConsoleManager.ServerInfo( "Server started" );

            while(true) {
                if(_chatServer.Pending()) {
                    TcpClient connection = _chatServer.AcceptTcpClient();
                    ConsoleManager.ServerInfo( "New client is pending..." );

                    Thread startCommunication = new Thread(() => newCommunication(connection));
                    startCommunication.Name = "StartConnetion";
                    startCommunication.Start();
                }
            }
        }

        private void newCommunication(TcpClient connection){
            new Communication( connection );
        }
    }
}
