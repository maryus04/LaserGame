using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows;
using Client.CanvasComponents;

namespace Client.CanvasBehavior {
    public class Mechanic {

        private static readonly Point INVALID_POINT = new Point( -1, -1 );

        public static Point GetIntersectionPointLineRect( Line laser, Rectangle block ) {

            double blockX;
            double blockY;

            Point blockLeftTop;
            Point blockRightTop;
            Point blockRightBot;
            Point blockLeftBot;

            Point firstLlaserPoint;
            Point secondLlaserPoint;

            GetBlockXY( block, out blockX, out blockY );
            GetBlockPoints( block, blockX, blockY, out blockLeftTop, out blockRightTop, out blockRightBot, out blockLeftBot );
            ExctractLaserPoints( laser, out firstLlaserPoint, out secondLlaserPoint );

            return GetFirstColissionPoint( ref blockLeftTop, ref blockRightTop, ref blockRightBot, ref blockLeftBot, ref firstLlaserPoint, ref secondLlaserPoint );
        }

        private static Point GetFirstColissionPoint( ref Point blockLeftTop, ref Point blockRightTop, ref Point blockRightBot, ref Point blockLeftBot, ref Point firstLaserPoint, ref Point secondLaserPoint ) {
            List<Point> blockIntersectionPoints = new List<Point>();

            blockIntersectionPoints.Add( GetIntersectionPointTwoLines( firstLaserPoint, secondLaserPoint, blockLeftTop, blockRightTop ) );
            blockIntersectionPoints.Add( GetIntersectionPointTwoLines( firstLaserPoint, secondLaserPoint, blockRightTop, blockRightBot ) );
            blockIntersectionPoints.Add( GetIntersectionPointTwoLines( firstLaserPoint, secondLaserPoint, blockRightBot, blockLeftBot ) );
            blockIntersectionPoints.Add( GetIntersectionPointTwoLines( firstLaserPoint, secondLaserPoint, blockLeftBot, blockLeftTop ) );

            return GetClosestPoint( firstLaserPoint, blockIntersectionPoints );
        }

        public static Tuple<string, Point> GetLaserPathAndBlockCollision( Line line ) {
            Point firstLaserPoint = new Point( line.X1, line.Y1 );
            Point secondLaserPoint = new Point( line.X2, line.Y2 );

            foreach(Block block in CanvasBlocks.list) {
                if(!block.Collide) {
                    continue;
                }

                double blockX;
                double blockY;
                Point blockLeftTop;
                Point blockRightTop;
                Point blockRightBot;
                Point blockLeftBot;

                GetBlockXY( block.BlockItem, out blockX, out blockY );
                GetBlockPoints( block.BlockItem, blockX, blockY, out blockLeftTop, out blockRightTop, out blockRightBot, out blockLeftBot );

                Point intersectionPoint;
                if((intersectionPoint = GetIntersectionPointTwoLines( firstLaserPoint, secondLaserPoint, blockLeftTop, blockRightTop )) != INVALID_POINT)
                    return Tuple.Create( "UP", intersectionPoint );
                if((intersectionPoint = GetIntersectionPointTwoLines( firstLaserPoint, secondLaserPoint, blockRightTop, blockRightBot )) != INVALID_POINT)
                    return Tuple.Create( "RIGHT", intersectionPoint );
                if((intersectionPoint = GetIntersectionPointTwoLines( firstLaserPoint, secondLaserPoint, blockRightBot, blockLeftBot )) != INVALID_POINT)
                    return Tuple.Create( "DOWN", intersectionPoint );
                if((intersectionPoint = GetIntersectionPointTwoLines( firstLaserPoint, secondLaserPoint, blockLeftBot, blockLeftTop )) != INVALID_POINT)
                    return Tuple.Create( "LEFT", intersectionPoint );
            }

            return Tuple.Create( "NONE", INVALID_POINT );
        }

        public static Point GetIntersectionPointTwoLines( Point firstLaserPoint, Point secondLaserPoint, Point firstBlockPoint, Point secondBlockPoint ) {
            double A1 = secondLaserPoint.Y - firstLaserPoint.Y;
            double B1 = firstLaserPoint.X - secondLaserPoint.X;
            double C1 = A1 * firstLaserPoint.X + B1 * firstLaserPoint.Y;

            double A2 = secondBlockPoint.Y - firstBlockPoint.Y;
            double B2 = firstBlockPoint.X - secondBlockPoint.X;
            double C2 = A2 * firstBlockPoint.X + B2 * firstBlockPoint.Y;

            double delta = A1 * B2 - A2 * B1;

            if(delta == 0) {
                return INVALID_POINT;
            }

            Point intersection = new Point( (B2 * C1 - B1 * C2) / delta, (A1 * C2 - A2 * C1) / delta );

            if(!(IsOnLine( firstLaserPoint, secondLaserPoint, intersection ) && IsOnLine( firstBlockPoint, secondBlockPoint, intersection ))) {
                return INVALID_POINT;
            }
            return intersection;
        }

        public static bool IsOnLine( Point firstLaserPoint, Point secondLaserPoint, Point collisionPoint ) {
            return ((collisionPoint.X >= firstLaserPoint.X && collisionPoint.X <= secondLaserPoint.X) && (collisionPoint.Y >= firstLaserPoint.Y && collisionPoint.Y <= secondLaserPoint.Y)) ||
                ((collisionPoint.X >= firstLaserPoint.X && collisionPoint.X <= secondLaserPoint.X) && (collisionPoint.Y <= firstLaserPoint.Y && collisionPoint.Y >= secondLaserPoint.Y)) ||
                ((collisionPoint.X <= firstLaserPoint.X && collisionPoint.X >= secondLaserPoint.X) && (collisionPoint.Y >= firstLaserPoint.Y && collisionPoint.Y <= secondLaserPoint.Y)) ||
                ((collisionPoint.X <= firstLaserPoint.X && collisionPoint.X >= secondLaserPoint.X) && (collisionPoint.Y <= firstLaserPoint.Y && collisionPoint.Y >= secondLaserPoint.Y));
        }

        public static Point GetClosestPoint( Point origin, List<Point> pointList ) {
            var closestPoints = pointList.Where( point => point != origin && point != INVALID_POINT )
                                         .OrderBy( point => SmartDistanceCalcTwoPoints( origin, point ) )
                                         .Take( 1 );
            if(closestPoints.Count() > 0) {
                return closestPoints.First();
            } else {
                return INVALID_POINT;
            }
        }

        private static void GetBlockXY( Rectangle rect, out double rectX, out double rectY ) {
            rectX = Canvas.GetLeft( rect );
            rectY = Canvas.GetTop( rect );
        }

        private static void ExctractLaserPoints( Line line, out Point firstLinePoint, out Point secondLinePoint ) {
            firstLinePoint = new Point( line.X1, line.Y1 );
            secondLinePoint = new Point( line.X2, line.Y2 );
        }

        private static void GetBlockPoints( Rectangle rect, double rectX, double rectY, out Point rectLeftTop, out Point rectRightTop, out Point rectRightBot, out Point rectLeftBot ) {
            rectLeftTop = new Point( rectX, rectY );
            rectRightTop = new Point( rectX + rect.Width, rectY );
            rectRightBot = new Point( rectX + rect.Width, rectY + rect.Height );
            rectLeftBot = new Point( rectX, rectY + rect.Height );
        }

        private static double SmartDistanceCalcTwoPoints( Point source, Point target ) {
            return Math.Pow( target.X - source.X, 2 ) + Math.Pow( target.Y - source.Y, 2 );
        }

    }
}
