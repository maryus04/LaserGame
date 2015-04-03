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

namespace Client {
    public partial class GameWindow : Window {
        List<Player> playerList = new List<Player>();

        public GameWindow() {
            InitializeComponent();
        }

        public GameWindow( int width, int height, string name, string ipAdd ) {
            InitializeComponent();

            GlobalVariable.debugMode = false;

            Laser laser = new Laser( gameCanvas, debugCanvas );

            playerList.Add( new Player() );

            laser.buildFirstLine();

            this.Show();
        }

        protected void Canvas_Clicked( object sender, System.Windows.Input.MouseEventArgs e ) {
            Rectangle rect = CreatePortal(e.GetPosition(gameCanvas));

            if(Player.firstClick) {
                gameCanvas.Children.Remove( Player.FirstPortal );

                Player.FirstPortal = rect;
                Player.firstClick = false;

                gameCanvas.Children.Add( rect );
            } else {
                gameCanvas.Children.Remove( Player.SecondPortal );
                
                Player.SecondPortal = rect;
                Player.firstClick = true;

                gameCanvas.Children.Add( rect );
            }

            Laser.ExctractIntersectionPoints();
        }

        private Rectangle CreatePortal( Point point ) {
            Rectangle rect = new System.Windows.Shapes.Rectangle {
                Width = 10,
                Height = 10,
                Stroke = System.Windows.Media.Brushes.Purple,
                StrokeThickness = 10
            };
            Canvas.SetLeft( rect, point.X - 5 );
            Canvas.SetTop( rect, point.Y - 5 );

            return rect;
        }
    }
}
