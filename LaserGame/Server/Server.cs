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

        public Server() {
            _nickName = new Hashtable( 100 );
            _nickNameByConnect = new Hashtable( 100 );
            _chatServer = new TcpListener( IPAddress.Parse( "127.0.0.1" ) , 4296 );


            GlobalVariable.debugMode = true;

            while(true) {
                _chatServer.Start();
                if(_chatServer.Pending()) {
                    TcpClient chatConnection = _chatServer.AcceptTcpClient();
                    Console.WriteLine( "A new client has connected." );
                    Communication comm = new Communication( chatConnection );
                }
            }
        }

        public static void Main() {
            new Server();
        }

        public static void SendMsgToAll( string nickName, String msg ) {
            StreamWriter write;
            ArrayList ToRemove = new ArrayList( 0 );
            TcpClient[] tcpClient = new TcpClient[Server._nickName.Count];
            Server._nickName.Values.CopyTo( tcpClient, 0 );
            for(int count = 0; count < tcpClient.Length; count++) {
                try {
                    if(msg.Trim() == "" || tcpClient[count] == null)
                        continue;
                    write = new StreamWriter( tcpClient[count].GetStream() );
                    write.WriteLine( nickName + ":" + msg );
                    write.Flush();
                    write = null;
                } catch(Exception ) {
                    string str = (string)Server._nickNameByConnect[tcpClient[count]];
                    Server.SendSysMsg( "*** " + str + " *** left the chat" );
                    Server._nickName.Remove( str );
                    Server._nickNameByConnect.Remove( tcpClient[count] );
                }
            }
        }

        public static void SendSysMsg(String msg ) {
            StreamWriter write;
            ArrayList ToRemove = new ArrayList( 0 );
            TcpClient[] tcpClient = new TcpClient[Server._nickName.Count];
            Server._nickName.Values.CopyTo( tcpClient, 0 );
            for(int count = 0; count < tcpClient.Length; count++) {
                try {
                    if(msg.Trim() == "" || tcpClient[count] == null)
                        continue;
                    write = new StreamWriter( tcpClient[count].GetStream() );
                    write.WriteLine("System: " + msg );
                    write.Flush();
                    write = null;
                } catch(Exception ) {
                    string str = (string)Server._nickNameByConnect[tcpClient[count]];
                    Server._nickName.Remove( str );
                    Server._nickNameByConnect.Remove( tcpClient[count] );
                }
            }
        }
    }
}
