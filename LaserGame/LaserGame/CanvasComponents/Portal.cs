using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;

namespace Client {
    class Portal {

        private Canvas _gameCanvas;

        public Portal( Canvas gameCanvas ) {
            _gameCanvas = gameCanvas;
        }

        public void AddPortal( Rectangle rect ) {
            _gameCanvas.Children.Remove( Player.GetCurrentPortal() );
            Player.SetCurrentPortal( rect );
            _gameCanvas.Children.Add( rect );
        }

        public Rectangle CreatePortal( Point point ) {
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
