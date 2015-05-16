using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.CanvasBehavior;
using System.Windows.Shapes;

namespace Client.CanvasComponents {
    class StarBlock {

        public static void Add( Rectangle block ) {
            CanvasBlocks.list.Add( new Block( "StarBlock", block, false ) );
            GameWindow.getInstance().AddToGameCanvas( block );
        }

    }
}
