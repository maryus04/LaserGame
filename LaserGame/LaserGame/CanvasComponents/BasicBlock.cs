using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Media;

namespace Client.CanvasComponents {
    class BasicBlock {

        private bool CanPass { get; set; }
        private Rectangle Rectangle { get; set; }

        public BasicBlock( int width, int height, Brush color ) {
            Rectangle = new Rectangle {
                Width = width,
                Height = height,
                Stroke = color,
                StrokeThickness = 10
            };
        }
    }
}
