using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;

namespace Client.CanvasComponents {
    class Block {

        public string BlockType { get; set; }
        public Rectangle BlockItem { get; set; }
        public bool Collide { get; set; }

        public Block( string blockType, Rectangle block, bool collide ) {
            BlockType = blockType;
            BlockItem = block;
            Collide = collide;
        }

        public static Rectangle Create( int width, int height, int leftPossition, int topPossition, Brush color ) {
            Rectangle rect = new Rectangle {
                Width = width,
                Height = height,
                Fill = color
            };
            Canvas.SetLeft( rect, leftPossition );
            Canvas.SetTop( rect, topPossition );

            return rect;
        }
    }
}
