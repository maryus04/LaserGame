using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
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
using Client.MessageControl;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using System.Diagnostics;
using System.Windows.Controls;
using System.Text.RegularExpressions;

namespace Client {
    public partial class MainWindow : Window {

        private static MainWindow instance;
        int port = 4296;


        private void Refresh_Click( object sender, RoutedEventArgs e ) {
            serverListBox.Items.Clear();
            serverListBox.IsEnabled = false;
            RefreshServerList();
        }

        private void RefreshServerList() {
            IPAddress ipAddress = Array.FindAll( Dns.GetHostAddresses( Dns.GetHostName() ), a => a.AddressFamily == AddressFamily.InterNetwork ).First();
            string[] baseIP = ipAddress.ToString().Split( '.' );

            for(int j = Int32.Parse( baseIP[2] ); j < Int32.Parse( baseIP[2] ) + 5; j++) {
                for(int i = 0; i < 255; i++) {
                    try {
                        IPAddress ip = IPAddress.Parse( baseIP[0] + "." + baseIP[1] + "." + j + "." + i );

                        Ping ping = new Ping();
                        ping.PingCompleted += new PingCompletedEventHandler( PingCompleted );
                        ping.SendAsync( ip, 100, ip );
                    } catch(Exception) { }
                }
            }
            serverListBox.IsEnabled = true;
        }

        private void PingCompleted( object sender, PingCompletedEventArgs e ) {
            if(e.Reply != null && e.Reply.Status == IPStatus.Success) {
                Thread SearchServer = new Thread( () => CheckServer( e ) );
                SearchServer.SetApartmentState( ApartmentState.STA );
                SearchServer.Name = "SearchServer";

                SearchServer.Start();
            }
        }

        private void CheckServer( PingCompletedEventArgs e ) {
            TcpClient connection = new TcpClient();
            try {
                connection.Connect( "" + e.UserState, port );
                if(connection.Connected) {
                    this.Dispatcher.Invoke( (Action)(() => {
                        ListBoxItem item = new ListBoxItem();
                        item.Content = "" + e.UserState;
                        serverListBox.Items.Add( item );
                    }) );
                }
            } catch(Exception) { }
        }

        public MainWindow() {
            instance = this;
            DebugManager.EnableConsole();
            InitializeComponent();
            RefreshServerList();
        }

        private void InitiateConnection( object sender, RoutedEventArgs e ) {
            InitConnection();
            SendNickName();
        }

        public void StartGame( object sender, RoutedEventArgs e ) {
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

        public void SetError( string error ) {
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

        private void ServerListView_SelectionChanged( object sender, System.Windows.Controls.SelectionChangedEventArgs e ) {
            ListBoxItem item = (ListBoxItem)serverListBox.SelectedItem;
            IpAddTB.Text = "" + item.Content;
        }

        private void SendMessage( object sender, RoutedEventArgs e ) {
            if(textName.Text != "") {
                Player.getInstance().WriteLine( "MainWindowMessage:" + textName.Text );
            }
            textName.Text = "";
        }

        private void ReadyButtonClick( object sender, RoutedEventArgs e ) {
            Player.getInstance().WriteLine( "ReadyPressed:" + readyButton.IsChecked );
        }

        public void SetAvaibility() {
            this.Dispatcher.Invoke( (Action)(() => {
                ipGrid.Visibility = System.Windows.Visibility.Hidden;
                nameGrid.Visibility = System.Windows.Visibility.Hidden;
                mapGrip.Visibility = System.Windows.Visibility.Visible;
                textName.KeyDown += textName_KeyDown;
                sendMessageButton.IsEnabled = true;
                enterRoomButton.Visibility = System.Windows.Visibility.Hidden;
                readyButton.Visibility = System.Windows.Visibility.Visible;
                readyButton.IsEnabled = true;
                playersList.Visibility = System.Windows.Visibility.Visible;
                server_list.Visibility = System.Windows.Visibility.Hidden;
            }) );
        }

        public void AppendText( string message ) {
            this.Dispatcher.Invoke( (Action)(() => { textBox.Text += message; }) );
        }

        private void textName_KeyDown( object sender, System.Windows.Input.KeyEventArgs e ) {
            if(e.Key == Key.Enter) {
                SendMessage( null, null );
            }
        }

        public void AllPlayersReady() {
            this.Dispatcher.Invoke( (Action)(() => {
                StartGame( null, null );
            }) );
        }

        public void SetPlayerList( string[] message ) {
            this.Dispatcher.Invoke( (Action)(() => {
                playersList.Items.Clear();
                foreach(string name in message) {
                    ListBoxItem item = new ListBoxItem();
                    item.Content = name;
                    playersList.Items.Add( item );
                }
            }) );
        }

        public void UpdatePlayerStatus( string playerName, string status ) {
            this.Dispatcher.Invoke( (Action)(() => {
                ListBoxItem item = new ListBoxItem();
                if(Boolean.Parse( status ) == true) {
                    item.Content = playerName + " -- Ready";
                } else {
                    item.Content = playerName;
                }
                int index = -1;
                foreach(ListBoxItem curItem in playersList.Items) {
                    string itemContent = curItem.Content.ToString();
                    if(itemContent.Equals( playerName ) || itemContent.Equals( playerName + " -- Ready" )) {
                        index = playersList.Items.IndexOf( curItem );
                    }
                }
                if(index != -1) {
                    playersList.Items.RemoveAt( index );
                    playersList.Items.Insert( index, item );
                }
            }) );
        }

        private void changeMapButton_Click( object sender, RoutedEventArgs e ) {
            OpenFileDialog dialog = new OpenFileDialog();
            if(dialog.ShowDialog() == Forms.DialogResult.OK) {
                if(dialog.FileName.Contains( ".map" )) {
                    string filename = System.IO.Path.GetFileName( dialog.FileName );
                    Regex rgx = new Regex( @"(\n(.*\s*)*)" );

                    int charPerLine = rgx.Replace( File.ReadAllText( dialog.FileName ), "" ).Count() - 2;
                    string map = File.ReadAllText( dialog.FileName ).Replace( "\r\n", "," );

                    mapNameLabel.Content = filename.Replace( ".map", "" );
                    Player.getInstance().WriteLine( "MapChanged:" + charPerLine + "," + map );
                }
            }
        }
    }
}
