using System.IO;
using System.Net;
using System;
using System.Threading;
using Chat = System.Net;
using System.Collections;

namespace Server {
    public class ChatServer {
        System.Net.Sockets.TcpListener _chatServer;
        public static Hashtable _nickName;
        public static Hashtable _nickNameByConnect;

        public ChatServer() {
            //create out nickname and nickname by connection variables
            _nickName = new Hashtable( 100 );
            _nickNameByConnect = new Hashtable( 100 );
            //create our TCPListener object
            _chatServer = new System.Net.Sockets.TcpListener( IPAddress.Parse( "127.0.0.1" ) , 4296 );
            //check to see if the server is running
            while(true) {
                //start the char servet
                _chatServer.Start();
                //check if there are pengin requests create a new connection
                if(_chatServer.Pending()) {
                    Chat.Sockets.TcpClient chatConnection = _chatServer.AcceptTcpClient();
                    //display a message letting the user know they're connected
                    Console.WriteLine( "A new client has connected." );
                    //create a new DoComunicate Object
                    Communication comm = new Communication( chatConnection );
                }
            }
        }

        public static void Main() {
            new ChatServer();
        }

        public static void SendMsgToAll( string nickName, String msg ) {
            //create a SteaWriter Object
            StreamWriter write;
            ArrayList ToRemove = new ArrayList( 0 );
            //Create a new TCPClient Array
            Chat.Sockets.TcpClient[] tcpClient = new Chat.Sockets.TcpClient[ChatServer._nickName.Count];
            //copy the users nickname to the ChatServer values
            ChatServer._nickName.Values.CopyTo( tcpClient, 0 );
            for(int count = 0; count < tcpClient.Length; count++) {
                try {
                    //check if the message is empty, of the particular
                    //index of out array is null,if it is then continue
                    if(msg.Trim() == "" || tcpClient[count] == null)
                        continue;
                    //use the GetStream method to get the current memory
                    //stream for this index of our tcpClientArray
                    write = new StreamWriter( tcpClient[count].GetStream() );
                    //white our message to the window
                    write.WriteLine( nickName + ":" + msg );
                    //make sure all bytes are written
                    write.Flush();
                    //dispose of the writer object until needed again
                    write = null;
                } catch(Exception ) {
                    string str = (string)ChatServer._nickNameByConnect[tcpClient[count]];
                    //send the message that the user has left
                    ChatServer.SendSysMsg( "*** " + str + " *** left the chat" );
                    //remove the nickname from the list
                    ChatServer._nickName.Remove( str );
                    //remove that index of the array,thus freezing it up for another user
                    ChatServer._nickNameByConnect.Remove( tcpClient[count] );
                }
            }
        }

        public static void SendSysMsg(String msg ) {
            //create a SteaWriter Object
            StreamWriter write;
            ArrayList ToRemove = new ArrayList( 0 );
            //Create a new TCPClient Array
            Chat.Sockets.TcpClient[] tcpClient = new Chat.Sockets.TcpClient[ChatServer._nickName.Count];
            //copy the users nickname to the ChatServer values
            ChatServer._nickName.Values.CopyTo( tcpClient, 0 );
            for(int count = 0; count < tcpClient.Length; count++) {
                try {
                    //check if the message is empty, of the particular
                    //index of out array is null,if it is then continue
                    if(msg.Trim() == "" || tcpClient[count] == null)
                        continue;
                    //use the GetStream method to get the current memory
                    //stream for this index of our tcpClientArray
                    write = new StreamWriter( tcpClient[count].GetStream() );
                    //white our message to the window
                    write.WriteLine("System: " + msg );
                    //make sure all bytes are written
                    write.Flush();
                    //dispose of the writer object until needed again
                    write = null;
                } catch(Exception ) {
                    string str = (string)ChatServer._nickNameByConnect[tcpClient[count]];
                    //send the message that the user has left
                    //remove the nickname from the list
                    ChatServer._nickName.Remove( str );
                    //remove that index of the array,thus freezing it up for another user
                    ChatServer._nickNameByConnect.Remove( tcpClient[count] );
                }
            }
        }
    }
}
