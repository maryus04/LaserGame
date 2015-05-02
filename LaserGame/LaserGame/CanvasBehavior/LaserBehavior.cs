using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows;
using System.Collections.ObjectModel;
using Client.CanvasBehavior;
using Client.LaserComponents;

namespace Client.CanvasBehavior {
    static class LaserBehavior {

        public static void LaserIntersectionAllCanvasRects( List<Line> laser ) {
            var child = GameWindow.getInstance().gameCanvas.Children;
            var rectlist = child.OfType<Rectangle>();

            foreach(Line currentLine in laser) {
                foreach(Rectangle rect in rectlist) {
                    Mechanic.GetIntersectionPointLineRect( currentLine, rect );
                }
            }
            DebugManager.DebugLaser( GameWindow.getInstance().debugCanvas );
        }

        public static Point IntersectionLineAllCanvasRects( Line lastLine ) {
            foreach(Block block in CanvasBlocks.list) {
                Point temp;
                if((temp = Mechanic.GetIntersectionPointLineRect( lastLine, block.BlockItem )) != new Point( -1, -1 )) {
                    DebugManager.DebugLaser( GameWindow.getInstance().debugCanvas );
                    return temp;
                }
            }
            return new Point( -1, -1 );
        }

        public static string IntersectionLinePortals( Line lastLine ) {
            if(Mechanic.GetIntersectionPointLineRect( lastLine, Portals.getInstance().FirstPortal ) != new Point( -1, -1 )) {
                DebugManager.DebugLaser( GameWindow.getInstance().debugCanvas );
                return "first";
            } else if(Mechanic.GetIntersectionPointLineRect( lastLine, Portals.getInstance().SecondPortal ) != new Point( -1, -1 )) {
                DebugManager.DebugLaser( GameWindow.getInstance().debugCanvas );
                return "second";
            } else
                return "none";
        }

        public static void buildLaserLine( Point p1, Point p2 ) {
            Line myLine = new Line();
            myLine.Stroke = System.Windows.Media.Brushes.Red;
            myLine.X1 = p1.X;
            myLine.Y1 = p1.Y;

            myLine.X2 = p1.X;
            myLine.Y2 = p1.Y;

            myLine.StrokeThickness = 1;

            Laser.getInstance().AddLine( myLine );
            Point temp;
            while((temp = LaserBehavior.IntersectionLineAllCanvasRects( myLine )) == new Point( -1, -1 )) {
                if(myLine.X2 <= p2.X) {
                    myLine.X2 += 3;
                }
                if(myLine.Y2 <= p2.Y) {
                    myLine.Y2 += 3;
                }
            }
            GameWindow.getInstance().gameCanvas.Children.Add( myLine );
        }
    }
}
