using System.IO;
using System.Net;
using System;
using System.Threading;
using Chat = System.Net;
using System.Collections;
using Server;
using System.Net.Sockets;

namespace Server {
    class Communication {
        TcpClient _chatClient;
        StreamReader _reader;
        StreamWriter _writer;
        string _nickName = "___";

        public Communication( TcpClient tcpClient ) {
            _chatClient = tcpClient;
            Thread chatThread = new Thread( new ThreadStart( BeginConnection ) );
            chatThread.Start();
        }

        private void BeginConnection() {
            _reader = new StreamReader( _chatClient.GetStream() );
            _writer = new StreamWriter( _chatClient.GetStream() );

            Thread chatThread = new Thread( new ThreadStart( ReadIncomming ) );
            chatThread.Start();
        }

        private void ReadIncomming() {
            while(true) {
                string message = _reader.ReadLine();

                if(GlobalVariable.debugMode) {
                    Console.WriteLine( message );
                }

                string method = message.Substring( 0, message.IndexOf( ":" ) );
                message.Replace( method, "" );

                switch(method) {
                    case "MyName:":
                        ValidateNickName( message ); // ACCEPT CONNECTION IF VALIDATED
                        break;
                }

            }
        }

        private string GetNick() {
            return _reader.ReadLine();
        }

        private void ValidateNickName( string name ) {
            if(!Server._nickName.Contains( name )) {
                _nickName = name;
            }
            else if(GlobalVariable.debugMode) {
                Console.WriteLine( name );
            }
        }

        private void AcceptConnection() {
            Server._nickName.Add( _nickName, _chatClient );
            Server._nickNameByConnect.Add( _chatClient, _nickName );
            Server.SendSysMsg( "*** " + _nickName + " *** joined the room" );
            _writer.Flush();
            Thread chatThread = new Thread( new ThreadStart( RunChat ) );
            chatThread.Start();
        }

        private void RunChat() {
            try {
                string message = "";
                while(true) {
                    message = _reader.ReadLine();
                    Server.SendMsgToAll( _nickName, message );
                }
            } catch(Exception e) {
                Console.WriteLine( e );
            }
        }
    }
}
