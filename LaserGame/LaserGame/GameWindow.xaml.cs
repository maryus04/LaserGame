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
using Client.LaserComponents;
using Client.CanvasBehavior;
using System.Threading;
using Client.CanvasComponents;

namespace Client {
    public partial class GameWindow : Window {

        private Laser _laser;
        private Portal _portal;

        private static bool gameStarted = false;

        public static bool isGameStarted { set { gameStarted = value; } get { return gameStarted; } }

        public GameWindow() {
            isGameStarted = true;
            MessageReader.SetGameWindow( this );
            
            InitializeComponent();

            _laser = new Laser( gameCanvas, debugCanvas );
            _portal = new Portal( gameCanvas );

            BuildMap();

            _laser.buildFirstLine();

            this.Show();
        }

        public void CanvasChanged( ) {
            DebugManager.DebugGame( "Canvas changed. Updating laser intersections" );
            this.Dispatcher.Invoke( (Action)(() => { LaserBehavior.LaserIntersectionAllCanvasRects( _laser.GetAllLines() ); }) );
        }

        public void PortalAccepted(int x,int y) {
            this.Dispatcher.Invoke( (Action)(() => { Portal.AddPlayerPortal( Portal.CreatePortal( new Point( x, y ) ) ); }) );
        }

        private void BuildMap() { // TODO: should have a string mapName parameter and use the map parser (the map should be recieved from server if it dosent have)
            gameCanvas.Children.Add( Block.Create( 200, 300, 400, 0, Brushes.Green ));
            gameCanvas.Children.Add( Block.Create( 188,127,109,126,Brushes.Green ));
        }

        protected void Canvas_Clicked( object sender, System.Windows.Input.MouseEventArgs e ) {
            Player.WriteLine( "PortalCreated:COORD:" + e.GetPosition( gameCanvas ).X + "," + e.GetPosition( gameCanvas ).Y + "ENDCOORD" );
        }


        private void Window_Closing( object sender, System.ComponentModel.CancelEventArgs e ) {
            Player.CloseConnection();
        }
    }
}
