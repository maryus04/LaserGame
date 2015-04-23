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
using System.Threading;

namespace Client {
    public partial class GameWindow : Window {
        
        public GameWindow() {
            InitializeComponent();
            InitializeComponents();
        }

        private void InitializeComponents(){
            Laser laser = new Laser( gameCanvas, debugCanvas );

            laser.buildFirstLine();

            this.Show();
        }

        protected void Canvas_Clicked( object sender, System.Windows.Input.MouseEventArgs e ) {
            Rectangle rect = CreatePortal(e.GetPosition(gameCanvas));

            if(Player.FirstClick) {
                gameCanvas.Children.Remove( Player.FirstPortal );

                Player.FirstPortal = rect;
                Player.FirstClick = false;

                gameCanvas.Children.Add( rect );
            } else {
                gameCanvas.Children.Remove( Player.SecondPortal );
                
                Player.SecondPortal = rect;
                Player.FirstClick = true;

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
