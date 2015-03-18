using System.IO;
using System.Net;
using System;
using System.Threading;
using Chat = System.Net;
using System.Collections;
using Server;

namespace Server{
    class Communication {
        System.Net.Sockets.TcpClient _chatClient;
        StreamReader _reader;
        StreamWriter _writer;
        string _nickName;

        public Communication( System.Net.Sockets.TcpClient tcpClient ) {
            //create our tcpClient
            _chatClient = tcpClient;
            //create a new thread
            Thread chatThread = new Thread( new ThreadStart( StartChat ) );
            chatThread.Start();
        }

        private string GetNick() {
            _writer.WriteLine( "Please type your nickname" );
            _writer.Flush();
            return _reader.ReadLine();
        }

        private void StartChat() {
            //create our StreamReader object to read the current stream
            _reader = new StreamReader( _chatClient.GetStream() );
            //create our StreamWriter object to write to the current stream
            _writer = new StreamWriter( _chatClient.GetStream() );
            _nickName = GetNick();
            //while(Server.ChatServer._nickName.Contains( _nickName )) {
            //    //since the nickname is in use we display that message
            //    _writer.WriteLine( "ERROR: Chose another nickname" );
            //    _nickName = GetNick();
            //}
            //add their nickname to the server
            Server.ChatServer._nickName.Add( _nickName, _chatClient );
            Server.ChatServer._nickNameByConnect.Add( _chatClient, _nickName );
            //send a system message letting the others know
            Server.ChatServer.SendSysMsg( "*** " + _nickName + " *** joined the room" );
            _writer.Flush();
            //create a new thread that reads and writes the messages
            Thread chatThread = new Thread( new ThreadStart( RunChat ) );
            chatThread.Start();
        }

        private void RunChat() {
            try {
                //set out line variable to an empty string
                string message = "";
                while(true) {
                    message = _reader.ReadLine();
                    Server.ChatServer.SendMsgToAll( _nickName, message );
                }
            } catch(Exception e) {
                Console.WriteLine( e );
            }
        }
    }
}
