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
            InitializeComponent();
        }

        private void NewGame( object sender, RoutedEventArgs e ) {
            MapParser.SetMapName( "initial.map1" );
            Player.Width = MapParser.ParseMapDimensions().Item1;
            Player.Height = MapParser.ParseMapDimensions().Item2;

            Player.TcpClient = new TcpClient();
            Player.TcpClient.Connect( IpAddTB.Text, 4296 );

            Player.Writer = new StreamWriter( Player.TcpClient.GetStream() );
            Player.Reader = new StreamReader( Player.TcpClient.GetStream() );
            Player.Connected = true;

            Thread ReadIncomming = new Thread( new ThreadStart( ReadMessages ) );
            ReadIncomming.Start();

            SendNickName();

            //new GameWindow( _width, _height, PlayerNameTB.Text, IpAddTB.Text );

            //this.Close();
        }

        private void SendNickName() {
            Player.WriteLine( "MyName:" + PlayerNameTB.Text );
        }

        private void ReadMessages() {
            while(Player.Connected) {
                string message = Player.ReadLine();

                if(GlobalVariable.debugMode) {
                    Console.WriteLine( message );
                }

                string method = message.Substring( 0, message.IndexOf( ":" )+1 );
                message = message.Replace( method, "" );

                switch(method) {
                    case "ConnectionAccepted:":
                        Player.Name = message;
                        this.Dispatcher.Invoke( (Action)(() => { IpAddTB.Text = message; }) );
                        break;
                    case "NickNameInUse:":
                        this.Dispatcher.Invoke( (Action)(() => { ErrorLabel.Content = "Error: Nickname already in use"; }) );
                        break;
                }
            }
        }

        private void ExitGame( object sender, RoutedEventArgs e ) {
            this.Close();
        }

        private void Window_Closing( object sender, System.ComponentModel.CancelEventArgs e ) {
            Player.Connected = false;
            Player.WriteLine( "CloseConnection:" );
            Player.Reader.Close();
            Player.Writer.Close();
            Player.TcpClient.Close();
        }

        private void Window_KeyDown( object sender, System.Windows.Input.KeyEventArgs e ) {
            ErrorLabel.Content = "";
        }

    }
}
