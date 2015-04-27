using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Client.CanvasComponents;

namespace Client.CanvasBehavior {
    class PortalBehavior {

        public static void AddPlayerPortal( Rectangle rect ) {
            RemovePortalByRectangle( GetMyCurrentPortal() );
            SetMyCurrentPortal( rect );
            DrawPortal( rect );
        }

        public static void DrawPortal( Rectangle rect ) {
            GameWindow.getInstance().gameCanvas.Children.Add( rect );
        }

        public static void RemovePortalByRectangle( Rectangle rect ) {
            GameWindow.getInstance().gameCanvas.Children.Remove( rect );
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

        public static void SetMyCurrentPortal( Rectangle rect ) {
            if(Player.getInstance().FirstClick) {
                Portals.getInstance().FirstPortal = rect;
                Player.getInstance().FirstClick = false;
            } else {
                Portals.getInstance().SecondPortal = rect;
                Player.getInstance().FirstClick = true;
            }
        }

        public static Rectangle GetMyCurrentPortal() {
            if(Player.getInstance().FirstClick && Portals.getInstance().FirstPortal != null && Portals.getInstance().SecondPortal != null) {
                return Portals.getInstance().FirstPortal;
            } else if(!Player.getInstance().FirstClick && Portals.getInstance().FirstPortal != null && Portals.getInstance().SecondPortal != null) {
                return Portals.getInstance().SecondPortal;
            } else {
                return null;
            }
        }

        public static Rectangle GetPortalByInsidePoint( Point point ) { // TODO: TRY USIG A HASHTABLE / HASHMAP KEY: CENTRAL POINT 
            foreach(Rectangle rect in GameWindow.getInstance().gameCanvas.Children.OfType<Rectangle>()) {
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
