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
        TcpClient _client;
        StreamReader _reader;
        StreamWriter _writer;
        string _nickName = "___";

        Thread ReadIncomming;

        public Communication( TcpClient tcpClient ) {
            _client = tcpClient;
            Thread chatThread = new Thread( new ThreadStart( BeginConnection ) );
            chatThread.Start();
        }

        private void BeginConnection() {
            _reader = new StreamReader( _client.GetStream() );
            _writer = new StreamWriter( _client.GetStream() );

            ReadIncomming = new Thread( new ThreadStart( ReadMessages ) );
            ReadIncomming.Start();
        }

        private void ReadMessages() {
            try {
                while(_client.Connected) {
                    string message = _reader.ReadLine();

                    if(GlobalVariable.debugMode) {
                        Console.WriteLine( message );
                    }

                    string method = message.Substring( 0, message.IndexOf( ":" ) );
                    message.Replace( method, "" );

                    switch(method) {
                        case "MyName:":
                            if(ValidateNickName( message )) {
                                AcceptConnection();
                            }
                            break;
                        case "CloseConnection:":
                            CloseConnection();
                            break;
                    }
                }
            } catch(Exception) { }
        }

        private string GetNick() {
            return _reader.ReadLine();
        }

        private bool ValidateNickName( string name ) {
            if(GlobalVariable.debugMode) {
                Console.WriteLine( name );
            }
            if(!Server._nickName.Contains( name )) {
                _nickName = name;
                return true;
            }
            return false;
        }

        private void CloseConnection() {
            _reader.Close();
            _client.Close();
        }

        private void AcceptConnection() {
            Server._nickName.Add( _nickName, _client );
            Server._nickNameByConnect.Add( _client, _nickName );
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
