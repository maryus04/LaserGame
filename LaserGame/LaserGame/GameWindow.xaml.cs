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

namespace Client {
    public partial class GameWindow : Window {

        private static GameWindow instance;

        public static bool IsGameStarted {
            get {
                if(instance == null) {
                    return false;
                }
                return true;
            }
        }

        public GameWindow() {
            instance = this;

            InitializeComponent();

            new Laser( gameCanvas, debugCanvas );
            new Portals();

            BuildMap();

            Laser.getInstance().buildLaserLine( new Point( 0, 0 ), new Point( 300, 300 ) );

            this.Show();
        }

        public void CanvasChanged() {
            DebugManager.DebugGame( "Canvas changed. Updating laser intersections" );
            Laser.getInstance().RemoveAll();
            if(Portals.getInstance().FirstPortal == null || Portals.getInstance().SecondPortal == null) {
                return;
            }
            string temp = "";
            this.Dispatcher.Invoke( (Action)(() => {
                temp = LaserBehavior.IntersectionLinePortals( Laser.getInstance().GetLastLine() );
            }) );
            switch(temp) {
                case "first":
                    this.Dispatcher.Invoke( (Action)(() => {
                        Laser.getInstance().buildPortalLine( Portals.getInstance().SecondPortal );
                    }) );
                    break;
                case "second":
                    this.Dispatcher.Invoke( (Action)(() => {
                        Laser.getInstance().buildPortalLine( Portals.getInstance().FirstPortal );
                    }) );
                    break;
            }
        }

        public void PortalAccepted( Point centerPoint ) {
            this.Dispatcher.Invoke( (Action)(() => { PortalBehavior.AddPlayerPortal( centerPoint ); }) );
        }

        public void AddToGameCanvas( UIElement block ) {
            gameCanvas.Children.Add( block );
        }

        public void RemoveFromGameCanvas( UIElement block ) {
            gameCanvas.Children.Remove( block );
        }

        private void BuildMap() { // TODO: should have a string mapName parameter and use the map parser (the map should be recieved from server if it dosent have)
            HardBlock.Add( "HardBlock", Block.Create( 200, 300, 400, 0, Brushes.Green ) );
            HardBlock.Add( "HardBlock", Block.Create( 188, 127, 109, 126, Brushes.Green ) );
        }

        protected void Canvas_Clicked( object sender, System.Windows.Input.MouseEventArgs e ) {
            Player.getInstance().WriteLine( "PortalCreated:COORD:" + e.GetPosition( gameCanvas ).X + "," + e.GetPosition( gameCanvas ).Y + "ENDCOORD" );
        }


        private void Window_Closing( object sender, System.ComponentModel.CancelEventArgs e ) {
            Player.getInstance().CloseConnection();
        }

        public void PortalSpawnedByOtherPlayer( Point centerPoint ) {
            this.Dispatcher.Invoke( (Action)(() => { PortalBehavior.DrawPortal( centerPoint ); }) );
        }

        public void PortalRemovedByOtherPlayer( Point centerPoint ) {
            this.Dispatcher.Invoke( (Action)(() => { PortalBehavior.RemovePortalByRectangle( centerPoint ); }) );
        }

        public static GameWindow getInstance() {
            if(instance == null) {
                instance = new GameWindow();
            }
            return instance;
        }
    }
}
