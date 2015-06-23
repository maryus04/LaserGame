using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Collections;
using Client.CanvasComponents;

namespace Client.CanvasBehavior {
    static class CanvasBlocks {

        public static List<Block> list = new List<Block>();
        public static List<MovableBlock> listMovable = new List<MovableBlock>();

        public static void Change() {
            foreach(MovableBlock block in listMovable.ToList()) {
                block.ChangePosition();
            }
            GameWindow.GetInstance().CanvasChanged();
            GameWindow.GetInstance().ConstructLaser();
            GameWindow.GetInstance().blocksUpdated = true;
            LaserBehavior.GetLaserStarCount();
        }

    }
}
