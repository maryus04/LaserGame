using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using Drawing = System.Drawing;
using System.Windows.Shapes;

namespace Client.LaserComponents {
    class Laser {

        ObservableCollection<Line> _laserLines;
        Canvas _canvas;

        public Laser( Canvas canvas ) {
            _canvas = canvas;
            _laserLines = new ObservableCollection<Line>();
            _laserLines.CollectionChanged += ListChanged;
        }

        public void buildFirstLine() {
            Line myLine = new Line();
            myLine.Stroke = System.Windows.Media.Brushes.Red;
            myLine.X1 = 0;
            myLine.Y1 = 150;

            myLine.X2 = 500;
            myLine.Y2 = 150;

            myLine.StrokeThickness = 1;

            _laserLines.Add( myLine );
            _canvas.Children.Add( myLine );
        }

        private void ListChanged( object sender, EventArgs e ) {
            var child = _canvas.Children;
            var rectlist = child.OfType<Rectangle>();
            foreach(Rectangle rect in rectlist) {
                double rectX = Canvas.GetLeft( rect );
                double rectY = Canvas.GetTop( rect );

                Line item = _laserLines[_laserLines.Count - 1];
                Point firstLinePoint = new Point( item.X1, item.Y1 );
                Point secondLinePoint = new Point( item.X2, item.Y2 );

                LineIntersectionPoint( firstLinePoint, secondLinePoint, new Point( rectX, rectY ), new Point( rectX + rect.Width, rectY ) );
                LineIntersectionPoint( firstLinePoint, secondLinePoint, new Point( rectX + rect.Width, rectY ), new Point( rectX + rect.Width, rectY + rect.Height ) );
                LineIntersectionPoint( firstLinePoint, secondLinePoint, new Point( rectX + rect.Width, rectY + rect.Height ), new Point( rectX, rectY + rect.Height ) );
                LineIntersectionPoint( firstLinePoint, secondLinePoint, new Point( rectX, rectY + rect.Height ), new Point( rectX, rectY ) );
            }

            if(GlobalVariable.debugMode) {
                foreach(Point intersection in Debug.pointsToBeDrawn) {
                    Debug.DrawPoint( _canvas, intersection );
                }
            }
        }

        Point LineIntersectionPoint( Point lp1, Point lp2, Point rp1, Point rp2 ) {
            var A1 = lp2.Y - lp1.Y;
            var B1 = lp1.X - lp2.X;
            var C1 = A1 * lp1.X + B1 * lp1.Y;

            var A2 = rp2.Y - rp1.Y;
            var B2 = rp1.X - rp2.X;
            var C2 = A2 * rp1.X + B2 * rp1.Y;

            var delta = A1 * B2 - A2 * B1;
            if(delta == 0)
                return new Point( -1, -1 );

            Point intersection = new Point( (B2 * C1 - B1 * C2) / delta, (A1 * C2 - A2 * C1) / delta );

            if(!(IsOnLine( lp1, lp2, intersection ) && IsOnLine( rp1, rp2, intersection )))
                return new Point( -1, -1 );

            if(GlobalVariable.debugMode) {
                Debug.pointsToBeDrawn.Add( intersection );
            }

            return intersection;
        }

        bool IsOnLine( Point pt1, Point pt2, Point pt ) {
            return ((pt.X >= pt1.X && pt.X <= pt2.X) && (pt.Y >= pt1.Y && pt.Y <= pt2.Y)) ||
                ((pt.X <= pt1.X && pt.X >= pt2.X) && (pt.Y <= pt1.Y && pt.Y >= pt2.Y));
        }


    }
}
