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

namespace Client {
    public partial class MainWindow : Window {

        static TcpClient _tcpClient;
        int _width;
        int _height;

        public MainWindow() {
            InitializeComponent();
        }

        private void SendNickName() {
            StreamWriter writer = new StreamWriter( _tcpClient.GetStream() );
            writer.WriteLine("MyName:" + PlayerNameTB.Text );
            writer.Flush();
        }

        private void NewGame( object sender, RoutedEventArgs e ) {
            MapParser.SetMapName( "initial.map1" );
            _width = MapParser.ParseMapDimensions().Item1;
            _height = MapParser.ParseMapDimensions().Item2;

            _tcpClient = new TcpClient();
            _tcpClient.Connect( IpAddTB.Text, 4296 );

            SendNickName();

            //new GameWindow( _width, _height, PlayerNameTB.Text, IpAddTB.Text );

            //this.Close();
        }

        private void ExitGame( object sender, RoutedEventArgs e ) {
            this.Close();
        }

    }
}
