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

        private static string _map;

        private static bool _gameStarted = false;

        private static int _startCountDown = 3;

        private static System.Threading.Timer _timer;

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
            _nickName = new Hashtable( 100 );
            _chatServer = new TcpListener( IPAddress.Any, 4296 );
            _chatServer.Start();
            ConsoleManager.Server( "Server started on " + Environment.MachineName );

            while(true) {
                if(_chatServer.Pending()) {
                    TcpClient connection = _chatServer.AcceptTcpClient();
                    ConsoleManager.Server( "New client is pending..." );

                    Thread startCommunication = new Thread( () => newCommunication( connection ) );
                    startCommunication.Name = "StartConnetion";
                    startCommunication.Start();
                }
            }
        }

        public static void SetCurrentMap( string map ) {
            _map = map;
        }

        public static string GetCurrectMap() {
            return _map;
        }

        public static void SendServerMessageExcept( ServerClient sendingClient, string message ) {
            foreach(ServerClient client in _nickName.Values) {
                if(client == sendingClient) {
                    continue;
                }
                client.WriteLine( message );
            }
        }

        public static void SendServerToAll( string message ) {
            foreach(ServerClient client in _nickName.Values) {
                client.WriteLine( message );
            }
        }

        public static void SendPlayerToAll( ServerClient sendingClient, string message ) {
            foreach(ServerClient client in _nickName.Values) {
                client.WriteLine( message + "NICK:" + sendingClient.NickName + "ENDNICK" );
            }
        }

        public static void SendReady() {
            foreach(ServerClient client in _nickName.Values) {
                client.WriteLine( "AllPlayersAreReady:" );
            }
        }

        public static string SendPlayerNames() {
            string names = "";
            foreach(ServerClient client in _nickName.Values) {
                names += client.NickName + client.Status + ",";
            }
            if(names.Equals( "" )) {
                return "";
            }
            names = names.Remove( names.Length - 1 );
            foreach(ServerClient client in _nickName.Values) {
                client.WriteLine( "Players:" + names );
            }
            return names;
        }

        public static void PlayersAreReady() {
            bool ready = true;
            foreach(ServerClient client in _nickName.Values) {
                if(!"True".Equals( client.Status )) {
                    ready = false;
                }
            }
            if(ready == true && !_gameStarted && MapExists()) {
                ConsoleManager.Server( "All players are ready. Starting the game." );
                _timer = new System.Threading.Timer( TimerCallback, null, 0, 1000 );
            } else if(_gameStarted) {
                ConsoleManager.Server( "Someone is tring to connect while the game is already started." );
                Server.SendServerToAll( "MainWindowServerMessage:** Game already started." );
            } else if(ready == true && !MapExists()) {
                ConsoleManager.ServerWarn( "No map found. Game wont start." );
                Server.SendServerToAll( "MainWindowServerMessage:** No map have been selected." );
            }
        }

        private static void TimerCallback( Object o ) {
            _startCountDown -= 1;

            if(_startCountDown == 0) {
                _timer.Dispose();
                SendReady();
                _gameStarted = true;
                Server.SendServerToAll( "MapAccepted:" + Server.GetCurrectMap() );
                foreach(ServerClient client in _nickName.Values) {
                    client.Status = "-- In game";
                }
            }
            Server.SendServerToAll( "MainWindowServerMessage:** Game starts in " + _startCountDown );
        }

        private static bool MapExists() {
            if(_map != null) {
                return true;
            }
            return false;
        }

        public static void CheckResolution() {
            bool ready = false;
            Tuple<int, int> resolution = Tuple.Create( 9999, 9999 );
            foreach(ServerClient client in _nickName.Values) {
                if(client.Resolution == null) {
                    return;
                }
                if(client.Resolution.Item1 < resolution.Item1 && client.Resolution.Item2 < resolution.Item2) {
                    resolution = client.Resolution;
                    ready = true;
                }
            }
            if(ready == true) {
                Server.SendServerToAll( "Resolution:COORD:" + resolution.Item1 + "," + resolution.Item2 + "ENDCOORD" );
            }
        }

        public static void UpdateReadyStatus( ServerClient player ) {
            foreach(ServerClient client in _nickName.Values) {
                client.WriteLine( "PlayerReady:" + "NICK:" + player.NickName + "ENDNICK" + "VALUE:" + player.Status + "ENDVALUE" );
            }
        }

        private void newCommunication( TcpClient connection ) {
            new Communication( connection );
        }

        internal static void Win() {
            ConsoleManager.Server( "Game finished" );
        }
    }
}
