using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using Client.CanvasBehavior;
using System.Windows.Controls;

namespace Client.CanvasComponents {
    class MovableBlock {
        private int blockWidth;
        private int blockHeight;
        private int x;
        private int y;
        private System.Windows.Media.SolidColorBrush color;

        public Block movableBlock;
        Rectangle rect;

        bool first;

        public MovableBlock( int blockWidth, int blockHeight, int x, int y, System.Windows.Media.SolidColorBrush solidColorBrush ) {
            this.blockWidth = blockWidth;
            this.blockHeight = blockHeight;
            this.x = x;
            this.y = y;
            this.color = solidColorBrush;

            first = false;
            Add();
        }

        public void ChangePosition() {
            if(first) {
                Remove();
                first = false;
                Add();
            } else {
                Remove();
                first = true;
                Add();
            }
        }

        public void GetDimension( out int width, out int height, out int x, out int y ) {
            width = blockWidth / 2;
            height = blockHeight / 2;
            x = this.x;
            y = this.y;

            if(!first) {
                x = this.x + blockWidth / 2;
                y = this.y + blockHeight / 2;
            }
            if(blockWidth / 2 <= Map.MapParser._multiplierX) {
                width = Map.MapParser._multiplierX;
                if(!first) {
                    x = this.x;
                }
            }
            if(blockHeight / 2 <= Map.MapParser._multiplierY) {
                height = Map.MapParser._multiplierY;
                if(!first) {
                    y = this.y;
                }

            }
        }

        public void Add() {
            int width, height, x, y;
            GetDimension( out width, out height, out x, out y );

            rect = Block.Create( width, height, x, y, color );
            movableBlock = new Block( "MovableBlock", rect, true );
            CanvasBlocks.listMovable.Add( this );
            GameWindow.GetInstance().AddToGameCanvas( movableBlock.BlockItem );
        }

        public void Remove() {
            CanvasBlocks.listMovable.Remove( this );
            GameWindow.GetInstance().RemoveFromGameCanvas( movableBlock.BlockItem );
        }
    }
}
