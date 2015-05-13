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

        public static double WIDTH;
        public static double HEIGHT;
        private static readonly Point INVALID_POINT = new Point( -1, -1 );

        public static void AddPlayerPortal( Point centerPoint ) {
            RemovePortalByRectangle( GetMyCurrentPortalCenterPoint() );
            SetMyCurrentPortal( centerPoint );
            DrawPortal( centerPoint );
        }

        public static void SetDimensions( int width, int height ) {
            WIDTH = width;
            HEIGHT = height;
        }

        public static void DrawPortal( Point centerPoint ) {
            DebugManager.Game( "Added portal at (" + centerPoint.X + "," + centerPoint.Y + ")" );
            Rectangle portal = CreatePortal( centerPoint );
            Portals.getInstance().OtherPortals.Add( centerPoint, portal );
            GameWindow.getInstance().AddToGameCanvas( portal );
        }

        public static void RemovePortalByRectangle( Point centerPoint ) {
            GameWindow.getInstance().RemoveFromGameCanvas( GetPortalByInsidePoint( centerPoint ) );
            Portals.getInstance().OtherPortals.Remove( centerPoint );
        }

        public static Rectangle CreatePortal( Point centerPoint ) {
            Rectangle rect = new Rectangle {
                Width = WIDTH,
                Height = HEIGHT,
                Fill = Brushes.Purple
            };
            Canvas.SetLeft( rect, centerPoint.X - WIDTH / 2 );
            Canvas.SetTop( rect, centerPoint.Y - HEIGHT / 2 );

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
                return new Point( Canvas.GetLeft( Portals.getInstance().FirstPortal ) + WIDTH / 2, Canvas.GetTop( Portals.getInstance().FirstPortal ) + HEIGHT / 2 );
            } else if(!Player.getInstance().FirstClick && Portals.getInstance().FirstPortal != null && Portals.getInstance().SecondPortal != null) {
                return new Point( Canvas.GetLeft( Portals.getInstance().SecondPortal ) + WIDTH / 2, Canvas.GetTop( Portals.getInstance().SecondPortal ) + HEIGHT / 2 ); ;
            } else {
                return INVALID_POINT;
            }
        }

        public static Rectangle GetPortalByInsidePoint( Point point ) {
            return (Rectangle)Portals.getInstance().OtherPortals[point];
        }

    }
}
