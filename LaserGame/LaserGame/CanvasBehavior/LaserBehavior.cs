using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows;
using System.Collections.ObjectModel;
using Client.CanvasBehavior;
using Client.CanvasComponents;
using Client.Map;

namespace Client.CanvasBehavior {
    static class LaserBehavior {

        private static List<Block> laserHitBlocks = new List<Block>();

        private static readonly Point INVALID_POINT = new Point( -1, -1 );

        public static Point GetInterLastLineAllBlocks( string buildingDirection, Line lastLaser ) {
            var movblockList = CanvasBlocks.listMovable;
            if("UP".Equals( buildingDirection ) || "LEFT".Equals( buildingDirection )) {
                movblockList = CanvasBlocks.listMovable.AsEnumerable().Reverse<MovableBlock>().ToList();
            }
            if("RIGHT".Equals( buildingDirection ) || "LEFT".Equals( buildingDirection )) {
                List<Point> points = new List<Point>();
                foreach(MovableBlock block in movblockList) {
                    Point temp;
                    if((temp = Mechanic.GetIntersectionPointLineRect( lastLaser, block.movableBlock.BlockItem )) != INVALID_POINT) {
                        if(block.movableBlock.Collide == true) {
                            points.Add( temp );
                        }
                    }
                }
                if(points.Count > 0) {
                    return Mechanic.GetClosestPoint( new Point( lastLaser.X1, lastLaser.Y1 ), points );
                }
            }
            foreach(MovableBlock block in movblockList) {
                Point temp;
                if((temp = Mechanic.GetIntersectionPointLineRect( lastLaser, block.movableBlock.BlockItem )) != INVALID_POINT) {
                    if(block.movableBlock.Collide == true) {
                        return temp;
                    }
                }
            }
            var blockList = CanvasBlocks.list;
            if("UP".Equals( buildingDirection ) || "LEFT".Equals( buildingDirection )) {
                blockList = CanvasBlocks.list.AsEnumerable().Reverse<Block>().ToList();
            }
            if("RIGHT".Equals( buildingDirection ) || "LEFT".Equals( buildingDirection )) {
                List<Point> points = new List<Point>();
                foreach(Block block in blockList) {
                    Point temp;
                    if((temp = Mechanic.GetIntersectionPointLineRect( lastLaser, block.BlockItem )) != INVALID_POINT) {
                        if(block.Collide == true) {
                            points.Add( temp );
                        }
                    }
                }
                return Mechanic.GetClosestPoint( new Point( lastLaser.X1, lastLaser.Y1 ), points );
            }
            foreach(Block block in blockList) {
                Point temp;
                if((temp = Mechanic.GetIntersectionPointLineRect( lastLaser, block.BlockItem )) != INVALID_POINT) {
                    if(block.Collide == true) {
                        return temp;
                    }
                }
            }
            
            return INVALID_POINT;
        }

        public static void ResetStarBlockList() {
            laserHitBlocks.Clear();
        }

        public static void GetLaserStarCount() {
            var blockList = CanvasBlocks.list;
            foreach(Block block in blockList) {
                Line myLaser = Laser.GetInstance().GetMyLaser();
                if(myLaser == null) {
                    return;
                }
                if(Mechanic.GetIntersectionPointLineRect( Laser.GetInstance().GetMyLaser(), block.BlockItem ) != INVALID_POINT) {
                    if("StarBlock".Equals( block.BlockType ) && !laserHitBlocks.Contains( block )) {
                        GameWindow.GetInstance().StarHit();
                        laserHitBlocks.Add( block );
                    }
                }
            }
            foreach(Line laser in Laser.GetInstance().GetAllLines()) {
                foreach(Block block in blockList) {
                    if(Mechanic.GetIntersectionPointLineRect( laser, block.BlockItem ) != INVALID_POINT) {
                        if("StarBlock".Equals( block.BlockType ) && !laserHitBlocks.Contains( block )) {
                            GameWindow.GetInstance().StarHit();
                            laserHitBlocks.Add( block );
                        }
                    }
                }
            }
        }

        public static string IntersectionLinePortals( Line lastLaser ) {
            if(Mechanic.GetIntersectionPointLineRect( lastLaser, Portals.getInstance().FirstPortal ) != INVALID_POINT) {
                return "first";
            } else if(Mechanic.GetIntersectionPointLineRect( lastLaser, Portals.getInstance().SecondPortal ) != INVALID_POINT) {
                return "second";
            } else
                return "none";
        }

        public static void BuildPortalLine( Rectangle portalStart ) {
            double x = Canvas.GetLeft( portalStart );
            double y = Canvas.GetTop( portalStart );
            Tuple<string, Point> result = Mechanic.GetLaserPathAndBlockCollision( new Line() {
                X1 = x,
                Y1 = y,
                X2 = x + PortalBehavior.WIDTH,
                Y2 = y + PortalBehavior.HEIGHT
            } );
            Line lastLaser = Laser.GetInstance().GetLastLine();
            switch(result.Item1) {
                case "UP"://180
                    if(lastLaser.Y1 > lastLaser.Y2) {//up
                        Laser.GetInstance().CreateLaserFromOpositePortal( result.Item1, new Point( x + PortalBehavior.WIDTH / 2, result.Item2.Y ), 0 );
                    } else if(lastLaser.X1 < lastLaser.X2) {//right
                        Laser.GetInstance().CreateLaserFromOpositePortal( result.Item1, new Point( x + PortalBehavior.WIDTH / 2, result.Item2.Y ), 270 );
                    } else if(lastLaser.Y1 < lastLaser.Y2) {// down
                        Laser.GetInstance().CreateLaserFromOpositePortal( result.Item1, new Point( x + PortalBehavior.WIDTH / 2, result.Item2.Y ), 180 );
                    } else if(lastLaser.X1 > lastLaser.X2) {//left
                        Laser.GetInstance().CreateLaserFromOpositePortal( result.Item1, new Point( x + PortalBehavior.WIDTH / 2, result.Item2.Y ), 90 );
                    }
                    break;
                case "DOWN"://0
                    if(lastLaser.Y1 > lastLaser.Y2) {//up
                        Laser.GetInstance().CreateLaserFromOpositePortal( result.Item1, new Point( x + PortalBehavior.WIDTH / 2, result.Item2.Y ), 180 );
                    } else if(lastLaser.X1 < lastLaser.X2) {//right
                        Laser.GetInstance().CreateLaserFromOpositePortal( result.Item1, new Point( x + PortalBehavior.WIDTH / 2, result.Item2.Y ), 90 );
                    } else if(lastLaser.Y1 < lastLaser.Y2) {// down
                        Laser.GetInstance().CreateLaserFromOpositePortal( result.Item1, new Point( x + PortalBehavior.WIDTH / 2, result.Item2.Y ), 0 );
                    } else if(lastLaser.X1 > lastLaser.X2) {//left
                        Laser.GetInstance().CreateLaserFromOpositePortal( result.Item1, new Point( x + PortalBehavior.WIDTH / 2, result.Item2.Y ), 270 );
                    }
                    break;
                case "LEFT"://90
                    if(lastLaser.Y1 > lastLaser.Y2) {//up
                        Laser.GetInstance().CreateLaserFromOpositePortal( result.Item1, new Point( result.Item2.X, y + PortalBehavior.HEIGHT / 2 ), 270 );
                    } else if(lastLaser.X1 < lastLaser.X2) {//right
                        Laser.GetInstance().CreateLaserFromOpositePortal( result.Item1, new Point( result.Item2.X, y + PortalBehavior.HEIGHT / 2 ), 180 );
                    } else if(lastLaser.Y1 < lastLaser.Y2) {// down
                        Laser.GetInstance().CreateLaserFromOpositePortal( result.Item1, new Point( result.Item2.X, y + PortalBehavior.HEIGHT / 2 ), 90 );
                    } else if(lastLaser.X1 > lastLaser.X2) {//left
                        Laser.GetInstance().CreateLaserFromOpositePortal( result.Item1, new Point( result.Item2.X, y + PortalBehavior.HEIGHT / 2 ), 0 );
                    }
                    break;
                case "RIGHT"://270
                    if(lastLaser.Y1 > lastLaser.Y2) {//up
                        Laser.GetInstance().CreateLaserFromOpositePortal( result.Item1, new Point( result.Item2.X, y + PortalBehavior.HEIGHT / 2 ), 90 );
                    } else if(lastLaser.X1 <= lastLaser.X2) {//right
                        Laser.GetInstance().CreateLaserFromOpositePortal( result.Item1, new Point( result.Item2.X, y + PortalBehavior.HEIGHT / 2 ), 0 );
                    } else if(lastLaser.Y1 < lastLaser.Y2) {// down
                        Laser.GetInstance().CreateLaserFromOpositePortal( result.Item1, new Point( result.Item2.X, y + PortalBehavior.HEIGHT / 2 ), 270 );
                    } else if(lastLaser.X1 >= lastLaser.X2) {//left
                        Laser.GetInstance().CreateLaserFromOpositePortal( result.Item1, new Point( result.Item2.X, y + PortalBehavior.HEIGHT / 2 ), 180 );
                    }
                    break;
            }
        }
    }
}
