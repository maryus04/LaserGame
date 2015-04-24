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
using Forms = System.Windows.Forms;
using System.Drawing;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Client {
    public partial class MainWindow : Window {

        

        public MainWindow() {
            DebugManager.SetDebugMode();
            InitializeComponent();
        }

        private void NewGame( object sender, RoutedEventArgs e ) {
            InitConnection();
            SendNickName();
        }

        private bool InitConnection() {
            if(Player.TcpClient != null) return false;
            if(!InitializeConnection()) return false;

            Player.InitializeStream();

            ReadMessages();

            return true;
        }

        

        private bool InitializeConnection() {
            try {
                Player.TcpClient = new TcpClient();
                Player.TcpClient.Connect( IpAddTB.Text, 4296 );
            } catch {
                this.Dispatcher.Invoke( (Action)(() => { ErrorLabel.Content = "Server not found."; }) );
                DebugManager.GameError( "Server not found." );
                return false;
            }
            return true;
        }

        private void ReadMessages() {
            Thread ReadIncomming = new Thread( () => MessageReader.ReadMessages() );
            
            MessageReader.SetMainWindow( this );
            ReadIncomming.SetApartmentState( ApartmentState.STA );
            ReadIncomming.Name = "ReadMessages";

            ReadIncomming.Start();
        }

        private void SendNickName() {
            Player.WriteLine( "MyName:" + PlayerNameTB.Text );
        }

        public void SetError(string error) {
            ErrorLabel.Content = error;
        }

        private void ExitGame( object sender, RoutedEventArgs e ) {
            this.Close();
        }

        private void Window_Closing( object sender, System.ComponentModel.CancelEventArgs e ) {
            if(!GameWindow.isGameStarted) {
                Player.CloseConnection();
            }
        }

        private void Window_KeyDown( object sender, System.Windows.Input.KeyEventArgs e ) {
            ErrorLabel.Content = "";
        }

    }
}
