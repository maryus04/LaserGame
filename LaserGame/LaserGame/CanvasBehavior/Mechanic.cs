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

        public static Point GetIntersectionPointLineRect( Line line, Rectangle rect ) {
            double rectX = Canvas.GetLeft( rect );
            double rectY = Canvas.GetTop( rect );

            Point rectLeftTop;
            Point rectRightTop;
            Point rectRightBot;
            Point rectLeftBot;
            GetRectanglePoints( rect, rectX, rectY, out rectLeftTop, out rectRightTop, out rectRightBot, out rectLeftBot );

            Point firstLinePoint = new Point( line.X1, line.Y1 );
            Point secondLinePoint = new Point( line.X2, line.Y2 );

            return GetFirstColissionPoint( ref rectLeftTop, ref rectRightTop, ref rectRightBot, ref rectLeftBot, ref firstLinePoint, ref secondLinePoint );
        }

        private static Point GetFirstColissionPoint( ref Point rectLeftTop, ref Point rectRightTop, ref Point rectRightBot, ref Point rectLeftBot, ref Point firstLinePoint, ref Point secondLinePoint ) {
            List<Point> fourIntersectionPoints = new List<Point>();

            fourIntersectionPoints.Add( GetIntersectionPointTwoLines( firstLinePoint, secondLinePoint, rectLeftTop, rectRightTop ) );
            fourIntersectionPoints.Add( GetIntersectionPointTwoLines( firstLinePoint, secondLinePoint, rectRightTop, rectRightBot ) );
            fourIntersectionPoints.Add( GetIntersectionPointTwoLines( firstLinePoint, secondLinePoint, rectRightBot, rectLeftBot ) );
            fourIntersectionPoints.Add( GetIntersectionPointTwoLines( firstLinePoint, secondLinePoint, rectLeftBot, rectLeftTop ) );

            return GetClosestPoint( firstLinePoint, fourIntersectionPoints );
        }

        public static Tuple<string, Point> GetLaserPathAndBlockCollision( Line line ) {
            Point firstLinePoint = new Point( line.X1, line.Y1 );
            Point secondLinePoint = new Point( line.X2, line.Y2 );

            foreach(Block rect in CanvasBlocks.list) {
                double rectX = Canvas.GetLeft( rect.BlockItem );
                double rectY = Canvas.GetTop( rect.BlockItem );

                Point rectLeftTop;
                Point rectRightTop;
                Point rectRightBot;
                Point rectLeftBot;
                GetRectanglePoints( rect.BlockItem, rectX, rectY, out rectLeftTop, out rectRightTop, out rectRightBot, out rectLeftBot );

                Point intersectionPoint;
                if((intersectionPoint = GetIntersectionPointTwoLines( firstLinePoint, secondLinePoint, rectLeftTop, rectRightTop )) != INVALID_POINT)
                    return Tuple.Create( "UP", intersectionPoint );
                if((intersectionPoint = GetIntersectionPointTwoLines( firstLinePoint, secondLinePoint, rectRightTop, rectRightBot )) != INVALID_POINT)
                    return Tuple.Create( "RIGHT", intersectionPoint );
                if((intersectionPoint = GetIntersectionPointTwoLines( firstLinePoint, secondLinePoint, rectRightBot, rectLeftBot )) != INVALID_POINT)
                    return Tuple.Create( "DOWN", intersectionPoint );
                if((intersectionPoint = GetIntersectionPointTwoLines( firstLinePoint, secondLinePoint, rectLeftBot, rectLeftTop )) != INVALID_POINT)
                    return Tuple.Create( "LEFT", intersectionPoint );
            }

            return Tuple.Create( "NONE", INVALID_POINT );
        }

        public static Point GetIntersectionPointTwoLines( Point lp1, Point lp2, Point rp1, Point rp2 ) {
            double A1 = lp2.Y - lp1.Y;
            double B1 = lp1.X - lp2.X;
            double C1 = A1 * lp1.X + B1 * lp1.Y;

            double A2 = rp2.Y - rp1.Y;
            double B2 = rp1.X - rp2.X;
            double C2 = A2 * rp1.X + B2 * rp1.Y;

            double delta = A1 * B2 - A2 * B1;

            if(delta == 0) {
                return INVALID_POINT;
            }

            Point intersection = new Point( (B2 * C1 - B1 * C2) / delta, (A1 * C2 - A2 * C1) / delta );

            if(!(IsOnLine( lp1, lp2, intersection ) && IsOnLine( rp1, rp2, intersection ))) {
                return INVALID_POINT;
            }
            return intersection;
        }

        public static bool IsOnLine( Point lp1, Point lp2, Point pt ) {
            return ((pt.X >= lp1.X && pt.X <= lp2.X) && (pt.Y >= lp1.Y && pt.Y <= lp2.Y)) ||
                ((pt.X >= lp1.X && pt.X <= lp2.X) && (pt.Y <= lp1.Y && pt.Y >= lp2.Y)) ||
                ((pt.X <= lp1.X && pt.X >= lp2.X) && (pt.Y >= lp1.Y && pt.Y <= lp2.Y)) ||
                ((pt.X <= lp1.X && pt.X >= lp2.X) && (pt.Y <= lp1.Y && pt.Y >= lp2.Y));
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

        private static void GetRectanglePoints( Rectangle rect, double rectX, double rectY, out Point rectLeftTop, out Point rectRightTop, out Point rectRightBot, out Point rectLeftBot ) {
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
