using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Client {
    class Portal {

        private static Canvas _gameCanvas;

        public static bool FirstClick { get; set; }

        public static Rectangle FirstPortal { get; set; }
        public static Rectangle SecondPortal { get; set; }

        public Portal( Canvas gameCanvas ) {
            _gameCanvas = gameCanvas;
        }

        public static void AddPlayerPortal( Rectangle rect ) {
            RemovePortalByRectangle( GetCurrentPortal() );
            SetCurrentPortal( rect );
            DrawPortal( rect );
        }

        public static void DrawPortal( Rectangle rect ) {
            _gameCanvas.Children.Add( rect );
        }

        public static void RemovePortalByRectangle( Rectangle rect ) {
            _gameCanvas.Children.Remove( rect );
        }

        public static Rectangle CreatePortal( Point point ) {
            Rectangle rect = new Rectangle {
                Width = 10,
                Height = 10,
                Stroke = Brushes.Purple,
                StrokeThickness = 10
            };
            Canvas.SetLeft( rect, point.X - 5 );
            Canvas.SetTop( rect, point.Y - 5 );

            return rect;
        }

        public static void SetCurrentPortal( Rectangle rect ) {
            if(FirstClick) {
                FirstPortal = rect;
                FirstClick = false;
            } else {
                SecondPortal = rect;
                FirstClick = true;
            }
        }

        public static Rectangle GetCurrentPortal() {
            if(FirstClick && FirstPortal != null && SecondPortal != null) {
                return FirstPortal;
            } else if(!FirstClick && FirstPortal != null && SecondPortal != null) {
                return SecondPortal;
            } else {
                return null;
            }
        }

        public static Point GetPointsFromMessage( string message ) {
            message = message.Substring( message.IndexOf( "COORD:" ) + 6, (message.IndexOf( "ENDCOORD" )) - (message.IndexOf( "COORD:" ) + 6) );
            string x = message.Substring( 0, (message.IndexOf( "," )) );
            message = message.Replace( x + ",", "" );
            string y = message.Substring( 0, message.Length );

            return new Point( Int32.Parse( x ), Int32.Parse( y ) );
        }

        public static Rectangle GetPortalByInsidePoint(Point point) {
            Rectangle portal = CreatePortal( point );
            
            foreach(Rectangle rect in _gameCanvas.Children.OfType<Rectangle>()) {
                if(rect.Stroke != Brushes.Purple) {
                    continue;
                }
                double rectX = Canvas.GetLeft( rect );
                double rectY = Canvas.GetTop( rect );

                if(point.X >= rectX && point.X <= rectX + rect.Width &&
                    point.Y >= rectY && point.Y <= rectY + rect.Height) {
                        return rect;
                }
            }
            return null;
        }

    }
}
