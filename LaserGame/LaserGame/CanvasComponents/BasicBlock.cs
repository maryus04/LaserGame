using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;

namespace Client.CanvasComponents {
    class BasicBlock {

        public bool CanPass { get; set; }
        public Rectangle Rectangle { get; set; }

        public BasicBlock( int width, int height, int leftPossition, int topPossition, Brush color ) {
            Rectangle = new Rectangle {
                Width = width,
                Height = height,
                Stroke = color,
                StrokeThickness = 10
            };
            Canvas.SetLeft( Rectangle, leftPossition );
            Canvas.SetTop( Rectangle, topPossition );
        }
    }
}
