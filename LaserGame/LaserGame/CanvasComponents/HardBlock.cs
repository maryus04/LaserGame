using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using Client.CanvasBehavior;

namespace Client.CanvasComponents {
    class HardBlock {

        public static void Add( string blockName, Rectangle block ) {
            CanvasBlocks.list.Add( new Block( blockName, block, true ) );
            GameWindow.getInstance().AddToGameCanvas( block );
        }

    }
}
