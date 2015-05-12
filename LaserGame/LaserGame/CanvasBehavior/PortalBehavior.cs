using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Client.CanvasBehavior;
using Client.CanvasComponents;

namespace Client.CanvasBehavior {
    class PortalBehavior {

        public static void AddPlayerPortal( Point centerPoint ) {
            RemovePortalByRectangle( GetMyCurrentPortalCenterPoint() );
            SetMyCurrentPortal( centerPoint );
            DrawPortal( centerPoint );
        }

        public static void DrawPortal( Point centerPoint ) {
            DebugManager.Game( "Added portal at (" + centerPoint.X + "," + centerPoint.Y + ")");
            Rectangle portal = CreatePortal( centerPoint );
            Portals.getInstance().OtherPortals.Add( centerPoint, portal );
            GameWindow.getInstance().gameCanvas.Children.Add( portal );
        }

        public static void RemovePortalByRectangle( Point centerPoint ) {
            GameWindow.getInstance().gameCanvas.Children.Remove( GetPortalByInsidePoint( centerPoint ) );
            Portals.getInstance().OtherPortals.Remove( centerPoint );
        }

        public static Rectangle CreatePortal( Point centerPoint ) {
            Rectangle rect = new Rectangle {
                Width = 10,
                Height = 10,
                Stroke = Brushes.Purple,
                StrokeThickness = 10
            };
            Canvas.SetLeft( rect, centerPoint.X - 5 );
            Canvas.SetTop( rect, centerPoint.Y - 5 );

            return rect;
        }

        public static void SetMyCurrentPortal( Point centerPoint ) {
            if(Player.getInstance().FirstClick) {
                Portals.getInstance().FirstPortal = CreatePortal( centerPoint );
                Player.getInstance().FirstClick = false;
            } else {
                Portals.getInstance().SecondPortal = CreatePortal( centerPoint );
                Player.getInstance().FirstClick = true;
            }
        }

        public static Point GetMyCurrentPortalCenterPoint() { // TODO: TRY TO MAKE IT BETTER
            if(Player.getInstance().FirstClick && Portals.getInstance().FirstPortal != null && Portals.getInstance().SecondPortal != null) {
                return new Point(Canvas.GetLeft( Portals.getInstance().FirstPortal )+5 , Canvas.GetTop( Portals.getInstance().FirstPortal )+5);
            } else if(!Player.getInstance().FirstClick && Portals.getInstance().FirstPortal != null && Portals.getInstance().SecondPortal != null) {
                return new Point( Canvas.GetLeft( Portals.getInstance().SecondPortal ) + 5, Canvas.GetTop( Portals.getInstance().SecondPortal ) + 5 ); ;
            } else {
                return new Point(-1,-1);
            }
        }

        public static Rectangle GetPortalByInsidePoint( Point point ) {
            return (Rectangle)Portals.getInstance().OtherPortals[point];
        }

    }
}
