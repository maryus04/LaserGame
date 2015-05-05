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
            Laser.getInstance().BuildLaserLine( new Point( 0, 400 ), new Point( 500, 400 ) ); //x
            //Laser.getInstance().BuildLaserLine( new Point( 500, 500 ), new Point( 500, 0 ) ); //x
            //Laser.getInstance().BuildLaserLine( new Point( 200, 0 ), new Point( 200, 300 ) ); //x
            //Laser.getInstance().BuildLaserLine( new Point( 700, 300 ), new Point( 0, 300 ) ); //x
            //Laser.getInstance().BuildLaserLine( new Point( 0, 0 ), new Point( 300, 300 ) );

            this.Show();
        }

        public void CanvasChanged() {
            DebugManager.GameWarn( "Canvas changed. Removing all lasers." );
            this.Dispatcher.Invoke( (Action)(() => { Laser.getInstance().RemoveAll(); ; }) );
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
            DebugManager.DebugGame( "Other player added portal from (" + centerPoint.X + "," + centerPoint.Y + ")" );
        }

        public void PortalRemovedByOtherPlayer( Point centerPoint ) {
            this.Dispatcher.Invoke( (Action)(() => { PortalBehavior.RemovePortalByRectangle( centerPoint ); }) );
            DebugManager.DebugGame( "Removing other player portal from (" + centerPoint.X + "," + centerPoint.Y +")");
        }

        public static GameWindow getInstance() {
            if(instance == null) {
                instance = new GameWindow();
            }
            return instance;
        }
    }
}
