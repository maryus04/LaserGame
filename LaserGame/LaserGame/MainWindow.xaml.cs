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
using Client.MessageControl;

namespace Client {
    public partial class MainWindow : Window {

        private static MainWindow instance;

        public MainWindow() {
            instance = this;
            DebugManager.EnableConsole();
            InitializeComponent();
        }

        private void NewGame( object sender, RoutedEventArgs e ) {
            InitConnection();
            SendNickName();
        }

        public void StartGame() {
            this.Dispatcher.Invoke( (Action)(() => { new GameWindow(); this.Close(); }) );
        }

        private bool InitConnection() {
            if(Player.getInstance().TcpClient != null) return false;
            if(!InitializeConnection()) return false;

            Player.getInstance().InitializeStream();

            ReadMessages();

            return true;
        }

        private bool InitializeConnection() {
            try {
                Player.getInstance().TcpClient = new TcpClient();
                Player.getInstance().TcpClient.Connect( IpAddTB.Text, 4296 );
            } catch {
                this.Dispatcher.Invoke( (Action)(() => { ErrorLabel.Content = "Server not found."; }) );
                DebugManager.GameError( "Server not found." );
                return false;
            }
            return true;
        }

        private void ReadMessages() {
            Thread ReadIncomming = new Thread( () => MessageReader.ReadMessages() );
            
            ReadIncomming.SetApartmentState( ApartmentState.STA );
            ReadIncomming.Name = "ReadMessages";

            ReadIncomming.Start();
        }

        private void SendNickName() {
            Player.getInstance().WriteLine( "MyName:" + PlayerNameTB.Text );
        }

        public void SetError(string error) {
            this.Dispatcher.Invoke( (Action)(() => { ErrorLabel.Content = error; ; }) );
        }

        private void ExitGame( object sender, RoutedEventArgs e ) {
            this.Close();
        }

        private void Window_Closing( object sender, System.ComponentModel.CancelEventArgs e ) {
            if(!GameWindow.IsGameStarted) {
                Player.getInstance().CloseConnection();
            }
        }

        private void Window_KeyDown( object sender, System.Windows.Input.KeyEventArgs e ) {
            ErrorLabel.Content = "";
        }

        public static MainWindow getInstance() {
            if(instance == null) {
                instance = new MainWindow();
            }
            return instance;
        }

    }
}
