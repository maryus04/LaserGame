using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows;
using System.Collections.ObjectModel;
using Client.CanvasBehavior;

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

        public static Point GetInterLastLineAllBlocks( Line lastLine ) {
            foreach(Block block in CanvasBlocks.list) {
                Point temp;
                if((temp = Mechanic.GetIntersectionPointLineRect( lastLine, block.BlockItem )) != new Point( -1, -1 )) {
                    return temp;
                }
            }
            return new Point( -1, -1 );
        }

    }
}
