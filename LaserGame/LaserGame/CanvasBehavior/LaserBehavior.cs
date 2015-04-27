using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows;
using System.Collections.ObjectModel;

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

        public static void IntersectionLineAllCanvasRects( Line lastLine ) {
            var child = GameWindow.getInstance().gameCanvas.Children;
            var rectlist = child.OfType<Rectangle>();

            foreach(Rectangle rect in rectlist) {
                Mechanic.GetIntersectionPointLineRect( lastLine, rect );
            }
            DebugManager.DebugLaser( GameWindow.getInstance().debugCanvas );
        }

    }
}
