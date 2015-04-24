using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows;
using System.Collections.ObjectModel;

namespace Client.CanvasBehavior {
    static class Behavior {

        private static Canvas _gameCanvas;
        private static Canvas _debugCanvas;

        public static Canvas GameCanvas {
            set {
                _gameCanvas = value;
            }
            get {
                return _gameCanvas;
            }
        }

        public static Canvas DebugCanvas {
            set {
                _debugCanvas = value;
            }
            get {
                return _debugCanvas;
            }
        }

        public static void LaserIntersectionAllCanvasRects( List<Line> laser ) {
            var child = _gameCanvas.Children;
            var rectlist = child.OfType<Rectangle>();

            foreach(Line currentLine in laser) {
                foreach(Rectangle rect in rectlist) {
                    Mechanic.GetIntersectionPointLineRect( currentLine, rect );
                }
            }
            DebugManager.DebugLaser( _debugCanvas );
        }

        public static void IntersectionLineAllCanvasRects(Line lastLine) {
            var child = _gameCanvas.Children;
            var rectlist = child.OfType<Rectangle>();

            foreach(Rectangle rect in rectlist) {
                Mechanic.GetIntersectionPointLineRect( lastLine, rect );
            }
            DebugManager.DebugLaser( _debugCanvas );
        }

    }
}
