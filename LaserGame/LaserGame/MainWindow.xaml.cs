using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using Client.Map.Sprite;
using System.Drawing;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace Client {
    public partial class MainWindow : Window {

        public MainWindow() {
            ConsoleManager.DebugMode = true; //debug mdoe
            InitializeComponent();
        }

        private void NewGame( object sender, RoutedEventArgs e ) {
            MapParser.SetMapName( "initial.map1" );
            Player.width = MapParser.ParseMapDimensions().Item1;
            Player.height = MapParser.ParseMapDimensions().Item2;

            Player.tcpClient = new TcpClient();
            Player.tcpClient.Connect( IpAddTB.Text, 4296 );

            Player.writer = new StreamWriter( Player.tcpClient.GetStream() );
            Player.reader = new StreamReader( Player.tcpClient.GetStream() );

            Thread ReadIncomming = new Thread( new ThreadStart( ReadMessages ) );
            ReadIncomming.Name = "ReadMessages";
            ReadIncomming.Start();

            SendNickName();

            //new GameWindow( _width, _height, PlayerNameTB.Text, IpAddTB.Text );

            //this.Close();
        }

        private void SendNickName() {
            Player.WriteLine( "MyName:" + PlayerNameTB.Text );
        }

        private void ReadMessages() {
            while(true) {
                string message = Player.ReadLine();
                if(message.Length == 0) {
                    break;
                }

                ConsoleManager.DebugGame( "Server sent:" + message );

                string method = message.Substring( 0, message.IndexOf( ":" ) + 1 );
                message = message.Replace( method, "" );

                switch(method) {
                    case "ConnectionAccepted:":
                        Player.name = message;
                        ConsoleManager.Game( "Connected as " + Player.name );
                        Player.connected = true;
                        break;
                    case "NickNameInUse:":
                        this.Dispatcher.Invoke( (Action)(() => { ErrorLabel.Content = "Nickname already in use"; }) );
                        ConsoleManager.GameWarn(  "Nickname already in use" );
                        break;
                }
            }
        }

        private void ExitGame( object sender, RoutedEventArgs e ) {
            this.Close();
        }

        private void Window_Closing( object sender, System.ComponentModel.CancelEventArgs e ) {
            if(Player.tcpClient != null) {
                Player.connected = false;
                Player.WriteLine( "CloseConnection:" );
                Player.reader.Close();
                Player.writer.Close();
                Player.tcpClient.Close();
            }
        }

        private void Window_KeyDown( object sender, System.Windows.Input.KeyEventArgs e ) {
            ErrorLabel.Content = "";
        }

    }
}
