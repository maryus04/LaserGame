using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows;
using System.Collections.ObjectModel;
using Client.CanvasBehavior;
using Client.CanvasComponents;

namespace Client.CanvasBehavior {
    static class LaserBehavior {

        private static readonly Point INVALID_POINT = new Point( -1, -1 );

        public static void GetAllIntersectionPointsForDebug( List<Line> laser ) { //TODO: this is not really nice ...find another way to debug laser
            var child = GameWindow.getInstance().gameCanvas.Children;
            var rectlist = child.OfType<Rectangle>();

            foreach(Line currentLine in laser) {
                foreach(Block block in CanvasBlocks.list) {
                    Mechanic.GetIntersectionPointLineRect( currentLine, block.BlockItem );
                }
            }
            DebugManager.DebugLaser( GameWindow.getInstance().debugCanvas );
        }

        public static Point GetInterLastLineAllBlocks( Line lastLine ) {
            foreach(Block block in CanvasBlocks.list) {
                Point temp;
                if((temp = Mechanic.GetIntersectionPointLineRect( lastLine, block.BlockItem )) != INVALID_POINT) {
                    return temp;
                }
            }
            return INVALID_POINT;
        }

        public static string IntersectionLinePortals( Line lastLine ) {
            if(Mechanic.GetIntersectionPointLineRect( lastLine, Portals.getInstance().FirstPortal ) != INVALID_POINT) {
                return "first";
            } else if(Mechanic.GetIntersectionPointLineRect( lastLine, Portals.getInstance().SecondPortal ) != INVALID_POINT) {
                return "second";
            } else
                return "none";
        }

        public static void BuildPortalLine( Rectangle portalStart ) {
            double x = Canvas.GetLeft( portalStart );
            double y = Canvas.GetTop( portalStart );
            Tuple<string, Point> result = Mechanic.GetLaserPathAndBlockCollision( new Line() { X1 = x, Y1 = y, X2 = x + PortalBehavior.WIDTH, Y2 = y + PortalBehavior.HEIGHT } );
            Line lastLine = Laser.getInstance().GetLastLine();
            switch(result.Item1) {
                case "UP"://180
                    if(lastLine.Y1 > lastLine.Y2) {//up
                        Laser.getInstance().CreateLaserFromOpositePortal( result.Item2, 0 );
                    } else if(lastLine.X1 < lastLine.X2) {//right
                        Laser.getInstance().CreateLaserFromOpositePortal( result.Item2, 270 );
                    } else if(lastLine.Y1 < lastLine.Y2) {// down
                        Laser.getInstance().CreateLaserFromOpositePortal( result.Item2, 180 );
                    } else if(lastLine.X1 > lastLine.X2) {//left
                        Laser.getInstance().CreateLaserFromOpositePortal( result.Item2, 90 );
                    }
                    break;
                case "DOWN"://0
                    if(lastLine.Y1 > lastLine.Y2) {//up
                        Laser.getInstance().CreateLaserFromOpositePortal( result.Item2, 180 );
                    } else if(lastLine.X1 < lastLine.X2) {//right
                        Laser.getInstance().CreateLaserFromOpositePortal( result.Item2, 90 );
                    } else if(lastLine.Y1 < lastLine.Y2) {// down
                        Laser.getInstance().CreateLaserFromOpositePortal( result.Item2, 0 );
                    } else if(lastLine.X1 > lastLine.X2) {//left
                        Laser.getInstance().CreateLaserFromOpositePortal( result.Item2, 270 );
                    }
                    break;
                case "LEFT"://90
                    if(lastLine.Y1 > lastLine.Y2) {//up
                        Laser.getInstance().CreateLaserFromOpositePortal( result.Item2, 270 );
                    } else if(lastLine.X1 < lastLine.X2) {//right
                        Laser.getInstance().CreateLaserFromOpositePortal( result.Item2, 180 );
                    } else if(lastLine.Y1 < lastLine.Y2) {// down
                        Laser.getInstance().CreateLaserFromOpositePortal( result.Item2, 90 );
                    } else if(lastLine.X1 > lastLine.X2) {//left
                        Laser.getInstance().CreateLaserFromOpositePortal( result.Item2, 0 );
                    }
                    break;
                case "RIGHT"://270
                    if(lastLine.Y1 > lastLine.Y2) {//up
                        Laser.getInstance().CreateLaserFromOpositePortal( result.Item2, 90 );
                    } else if(lastLine.X1 < lastLine.X2) {//right
                        Laser.getInstance().CreateLaserFromOpositePortal( result.Item2, 0 );
                    } else if(lastLine.Y1 < lastLine.Y2) {// down
                        Laser.getInstance().CreateLaserFromOpositePortal( result.Item2, 270 );
                    } else if(lastLine.X1 > lastLine.X2) {//left
                        Laser.getInstance().CreateLaserFromOpositePortal( result.Item2, 180 );
                    }
                    break;
            }
        }
    }
}
