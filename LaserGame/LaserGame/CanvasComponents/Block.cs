using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;

namespace Client.CanvasComponents {
    static class Block {

        public static Rectangle Create( int width, int height, int leftPossition, int topPossition, Brush color ) {
            Rectangle rect = new Rectangle {
                Width = width,
                Height = height,
                Stroke = color,
                StrokeThickness = 10
            };
            Canvas.SetLeft( rect, leftPossition );
            Canvas.SetTop( rect, topPossition );

            return rect;
        }
    }
}
