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

        static TcpClient _tcpClient;
        StreamWriter _writer;
        StreamReader _reader;
        int _width;
        int _height;

        public MainWindow() {
            InitializeComponent();
        }

        private void SendNickName() {
            _writer.WriteLine("MyName:" + PlayerNameTB.Text );
            _writer.Flush();
        }

        private void NewGame( object sender, RoutedEventArgs e ) {
            MapParser.SetMapName( "initial.map1" );
            _width = MapParser.ParseMapDimensions().Item1;
            _height = MapParser.ParseMapDimensions().Item2;

            _tcpClient = new TcpClient();
            _tcpClient.Connect( IpAddTB.Text, 4296 );

            _writer = new StreamWriter( _tcpClient.GetStream() );
            _reader = new StreamReader( _tcpClient.GetStream() );

            SendNickName();

            //Thread ReadIncomming = new Thread( new ThreadStart( ReadMessages ) );
            //ReadIncomming.Start();

            //new GameWindow( _width, _height, PlayerNameTB.Text, IpAddTB.Text );

            //this.Close();
        }

        private void ReadMessages() {
            while(true) {
                string message = _reader.ReadLine();

                if(GlobalVariable.debugMode) {
                    Console.WriteLine( message );
                }

                string method = message.Substring( 0, message.IndexOf( ":" ) );
                message.Replace( method, "" );

                switch(method) {
                    case "ConnectionAccepted:":
                        break;
                }

            }
        }

        private void ExitGame( object sender, RoutedEventArgs e ) {
            this.Close();
        }

        private void Window_Closing( object sender, System.ComponentModel.CancelEventArgs e ) {
            _writer.WriteLine( "CloseConnection:" );
            _writer.Flush();
            System.Threading.Thread.Sleep( 2000 );
        }

    }
}
