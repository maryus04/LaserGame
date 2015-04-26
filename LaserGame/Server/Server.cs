using System.IO;
using System.Net;
using System;
using System.Threading;
using Chat = System.Net;
using System.Collections;
using System.Net.Sockets;
using System.Windows.Forms;

namespace Server {
    public class Server {
        TcpListener _chatServer;
        public static Hashtable _nickName;

        public static void Main() {
            if(Environment.MachineName == "NWRMP01") {
                DialogResult dr = MessageBox.Show( "Would you like to run in debug mode?", "Debug", MessageBoxButtons.YesNo );
                switch(dr) {
                    case DialogResult.Yes:
                        ConsoleManager.debugMode = true;
                        break;
                    case DialogResult.No:
                        ConsoleManager.debugMode = false;
                        break;
                }
            }
            new Server();
        }

        public Server() {
            MapParser.SetMapName( "initial.map1" );
            //Player.width = MapParser.ParseMapDimensions().Item1;
            //Player.height = MapParser.ParseMapDimensions().Item2;

            _nickName = new Hashtable( 100 );
            _chatServer = new TcpListener( IPAddress.Parse( "127.0.0.1" ) , 4296 );
            _chatServer.Start();
            ConsoleManager.Server( "Server started on " + Environment.MachineName );

            while(true) {
                if(_chatServer.Pending()) {
                    TcpClient connection = _chatServer.AcceptTcpClient();
                    ConsoleManager.Server( "New client is pending..." );

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
