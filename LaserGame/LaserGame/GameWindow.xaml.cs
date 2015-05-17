using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Client.CanvasBehavior;
using System.Threading;
using Client.Map;
using Client.CanvasComponents;

namespace Client {
    public partial class GameWindow : Window {

        private static GameWindow instance;

        private static readonly int SECONDS_OF_DISPALY = 5;

        private static int currentHitStarts = 0;
        private static bool _firstEnter = true;

        private int _startCountDown = SECONDS_OF_DISPALY;
        private System.Threading.Timer _timer;

        public static bool IsGameStarted {
            get {
                if(instance == null) {
                    return false;
                }
                return true;
            }
        }

        public GameWindow() {
            this.KeyDown += new KeyEventHandler( GameWindow_KeyDown );
            instance = this;

            InitializeComponent();

            new Laser();
            new Portals();
        }

        private void GameWindow_KeyDown( object sender, KeyEventArgs e ) {
            if((Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt) {
                if(Keyboard.IsKeyDown( Key.Enter )) {
                    if(this.WindowState == System.Windows.WindowState.Normal) {
                        this.WindowStyle = System.Windows.WindowStyle.None;
                        this.WindowState = System.Windows.WindowState.Maximized;
                    } else {
                        this.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
                        this.WindowState = System.Windows.WindowState.Normal;
                    }
                }
            }
            if(Keyboard.IsKeyDown( Key.Enter )) {
                if(_firstEnter) {
                    inputMessageBox.Visibility = System.Windows.Visibility.Visible;
                    messageBox.Visibility = System.Windows.Visibility.Visible;
                    _firstEnter = false;
                    ResetTimer();
                } else {
                    _firstEnter = true;
                    SendMessage();
                    inputMessageBox.Visibility = System.Windows.Visibility.Hidden;
                    _timer = new System.Threading.Timer( TimerCallback, null, 0, 1000 );
                }
            }
            if(messageBox.Visibility == System.Windows.Visibility.Visible && e.Key.ToString().Length == 1) {
                if(Char.IsLetterOrDigit( Char.Parse( e.Key.ToString() ) )) {
                    if("".Equals( inputMessageBox.Text )) {
                        inputMessageBox.Text += Char.ToUpper( Char.Parse( "" + e.Key ) );
                    } else {
                        inputMessageBox.Text += Char.ToLower( Char.Parse( "" + e.Key ) );
                    }
                }
            } else if(e.Key.ToString() == "Space") {
                inputMessageBox.Text += " ";
            }
        }

        private void ResetTimer() {
            if(_timer != null) {
                _timer.Dispose();
                _startCountDown = SECONDS_OF_DISPALY;
            }
        }

        private void SendMessage() {
            if(inputMessageBox.Text != "") {
                Player.getInstance().WriteLine( "GameWindowMessage:" + inputMessageBox.Text );
            }
            inputMessageBox.Text = "";
        }

        private void TimerCallback( Object o ) {
            _startCountDown -= 1;

            if(_startCountDown == 0) {
                this.Dispatcher.Invoke( (Action)(() => {
                    messageBox.Visibility = System.Windows.Visibility.Hidden;
                    ResetTimer();
                }) );
            }
        }

        public void SetGridLayout( int width, int height ) {
            this.Dispatcher.Invoke( (Action)(() => {
                gameCanvas.Width = width;
                gameCanvas.Height = height;

                inputMessageBox.Width = width / 3;
                inputMessageBox.Height = height / 3;
            }) );
        }

        public void CanvasChanged() {
            DebugManager.GameWarn( "Canvas changed.\n\t\t\tRemoving all lasers.\n\t\t\tStar count set to zero." );
            this.Dispatcher.Invoke( (Action)(() => { Laser.getInstance().RemoveAll(); }) );
        }

        public void ResetStars() {
            currentHitStarts = 0;
        }

        public void CreateMap( string map ) {
            this.Dispatcher.Invoke( (Action)(() => { MapParser.ParseMap( map.Split( ',' ) ); }) );
        }

        public void ShowGame() {
            this.Dispatcher.Invoke( (Action)(() => { this.Show(); }) );
        }

        public void ConstructLaser() {
            DebugManager.GameWarn( "Updating laser intersections." );
            if(Laser.getInstance().BuildLaser()) {
                this.Dispatcher.Invoke( (Action)(() => { Portals.getInstance().BuildLaserIfIntersect(); }) );
            }
        }

        public void DeleteMyLaser() {
            DebugManager.GameWarn( "Canvas changed. Removing my laser." );
            this.Dispatcher.Invoke( (Action)(() => { Laser.getInstance().RemoveMyLaser(); }) );
        }

        public void PortalAccepted( Point centerPoint ) {
            this.Dispatcher.Invoke( (Action)(() => { PortalBehavior.AddPlayerPortal( centerPoint ); }) );
        }

        public void AddToGameCanvas( UIElement block ) {
            this.Dispatcher.Invoke( (Action)(() => { gameCanvas.Children.Add( block ); }) );
        }

        public void RemoveFromGameCanvas( UIElement block ) {
            this.Dispatcher.Invoke( (Action)(() => { gameCanvas.Children.Remove( block ); }) );
        }

        protected void Canvas_Clicked( object sender, System.Windows.Input.MouseEventArgs e ) {
            ResetStars();
            Player.getInstance().WriteLine( "PortalCreated:COORD:" + Convert.ToInt64( e.GetPosition( gameCanvas ).X ) + "," + Convert.ToInt64( e.GetPosition( gameCanvas ).Y ) + "ENDCOORD" );
        }

        private void Window_Closing( object sender, System.ComponentModel.CancelEventArgs e ) {
            Player.getInstance().CloseConnection();
        }

        public void PortalSpawnedByOtherPlayer( Point centerPoint ) {
            this.Dispatcher.Invoke( (Action)(() => { PortalBehavior.DrawPortal( centerPoint ); }) );
            DebugManager.DebugGame( "Other player added portal from (" + centerPoint.X + "," + centerPoint.Y + ")" );
        }

        public void PortalRemovedByOtherPlayer( Point centerPoint ) {
            this.Dispatcher.Invoke( (Action)(() => { PortalBehavior.RemovePortalByRectangle( centerPoint ); }) );
            DebugManager.DebugGame( "Removing other player portal from (" + centerPoint.X + "," + centerPoint.Y + ")" );
        }

        public static GameWindow getInstance() {
            if(instance == null) {
                instance = new GameWindow();
            }
            return instance;
        }

        public void StarHit() {
            currentHitStarts++;
            if(MapParser._starNumber == currentHitStarts) {
                Player.getInstance().WriteLine( "MaxStarHit:VALUE:" + currentHitStarts + "ENDVALUE" );
            }
        }

        public void AppendText( string message ) {
            this.Dispatcher.Invoke( (Action)(() => {
                messageBox.Text = message + messageBox.Text;
                messageBox.Visibility = System.Windows.Visibility.Visible;
                ResetTimer();
                _timer = new System.Threading.Timer( TimerCallback, null, 0, 1000 );
            }) );
        }
    }
}
