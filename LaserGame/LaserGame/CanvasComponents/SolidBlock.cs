using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Client.CanvasComponents {
    class SolidBlock : BasicBlock {

        public SolidBlock( int width, int height, int leftPossition, int topPossition, Brush color )
            : base( width, height, leftPossition, topPossition, color ) {
            base.CanPass = true;
        }

    }
}
