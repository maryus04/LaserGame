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
using System.Windows.Shapes;
using Client.LaserComponents;
using Client.CanvasBehavior;
using System.Threading;

namespace Client {
    public partial class GameWindow : Window {

        private Laser _laser;
        private Portal _portal;

        private static bool gameStarted = false;

        public static bool isGameStarted { set { gameStarted = value; } get { return gameStarted; } }

        public GameWindow() {
            isGameStarted = true;
            InitializeComponent();

            _laser = new Laser( gameCanvas, debugCanvas );
            _portal = new Portal( gameCanvas );

            _laser.buildFirstLine();

            this.Show();
        }

        protected void Canvas_Clicked( object sender, System.Windows.Input.MouseEventArgs e ) {
            Rectangle rect = _portal.CreatePortal( e.GetPosition( gameCanvas ) );

            _portal.AddPortal( rect );

            Behavior.LaserIntersectionAllCanvasRects( _laser.GetAllLines() );
        }


        private void Window_Closing( object sender, System.ComponentModel.CancelEventArgs e ) {
            Player.CloseConnection();
        }
    }
}
