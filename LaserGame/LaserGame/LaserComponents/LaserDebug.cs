using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace Client {
    class LaserDebug {

        public static List<Point> pointsToBeDrawn = new List<Point>();

        public static void DrawPoint(Canvas canvas, Point point) {
            var ellipse = new Ellipse() { Width = 8, Height = 8, Fill= new SolidColorBrush( Colors.Blue ), Stroke = new SolidColorBrush( Colors.Blue ) };
            Canvas.SetLeft( ellipse, point.X-4 );
            Canvas.SetTop( ellipse, point.Y-4 );
            canvas.Children.Add( ellipse );
        }

        public static void DeletePointList( Canvas canvas ) {
            pointsToBeDrawn = new List<Point>();
        }

        public static void DeletePointsFromCanvas( Canvas canvas ) {
            canvas.Children.RemoveRange( 0, canvas.Children.Count );
        }

    }
}
