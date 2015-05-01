using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;

namespace Client.CanvasBehavior {
    class HardBlock {

        public static void Add(string blockName , Rectangle block ) {
            CanvasBlocks.list.Add( new Block(blockName , block) );
            GameWindow.getInstance().AddToGameCanvas( block );
        }

    }
}
