using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows;

namespace Client.CanvasBehavior {
    public class Mechanic {

        public static Point GetIntersectionPointLineRect( Line line, Rectangle rect ) {
            double rectX = Canvas.GetLeft( rect );
            double rectY = Canvas.GetTop( rect );

            Point firstLinePoint = new Point( line.X1, line.Y1 );
            Point secondLinePoint = new Point( line.X2, line.Y2 );
            Point temp;
            if((temp = GetIntersectionPointTwoLines( firstLinePoint, secondLinePoint, new Point( rectX, rectY ), new Point( rectX + rect.Width, rectY ) )) != new Point( -1, -1 ))
                return temp;
            if((temp = GetIntersectionPointTwoLines( firstLinePoint, secondLinePoint, new Point( rectX + rect.Width, rectY ), new Point( rectX + rect.Width, rectY + rect.Height ) ))!= new Point( -1, -1 ))
                return temp;
            if((temp = GetIntersectionPointTwoLines( firstLinePoint, secondLinePoint, new Point( rectX + rect.Width, rectY + rect.Height ), new Point( rectX, rectY + rect.Height ) ))!= new Point( -1, -1 ))
                return temp;
            if((temp = GetIntersectionPointTwoLines( firstLinePoint, secondLinePoint, new Point( rectX, rectY + rect.Height ), new Point( rectX, rectY ) )) != new Point( -1, -1 ))
                return temp;

            return new Point(-1,-1);
        }

        public static string GetLaserPath( Line line) {
            Point firstLinePoint = new Point( line.X1, line.Y1 );
            Point secondLinePoint = new Point( line.X2, line.Y2 );
            foreach(Block rect in CanvasBlocks.list){
                double rectX = Canvas.GetLeft( rect.BlockItem );
                double rectY = Canvas.GetTop( rect.BlockItem );

                if(GetIntersectionPointTwoLines( firstLinePoint, secondLinePoint, new Point( rectX, rectY ), new Point( rectX + rect.BlockItem.Width, rectY ) ) != new Point( -1, -1 ))
                    return "UP";
                if(GetIntersectionPointTwoLines( firstLinePoint, secondLinePoint, new Point( rectX + rect.BlockItem.Width, rectY ), new Point( rectX + rect.BlockItem.Width, rectY + rect.BlockItem.Height ) ) != new Point( -1, -1 ))
                    return "RIGHT";
                if(GetIntersectionPointTwoLines( firstLinePoint, secondLinePoint, new Point( rectX + rect.BlockItem.Width, rectY + rect.BlockItem.Height ), new Point( rectX, rectY + rect.BlockItem.Height ) ) != new Point( -1, -1 ))
                    return "DOWN";
                if(GetIntersectionPointTwoLines( firstLinePoint, secondLinePoint, new Point( rectX, rectY + rect.BlockItem.Height ), new Point( rectX, rectY ) ) != new Point( -1, -1 ))
                    return "LEFT";
            }

            return "NONE";
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
                return new Point( -1, -1 );
            }

            Point intersection = new Point( (B2 * C1 - B1 * C2) / delta, (A1 * C2 - A2 * C1) / delta );

            if(!(IsOnLine( lp1, lp2, intersection ) && IsOnLine( rp1, rp2, intersection ))) {
                return new Point( -1, -1 );
            }

            DebugManager.PointsToBeDrawn.Add( intersection );

            return intersection;
        }

        public static bool IsOnLine( Point lp1, Point lp2, Point pt ) {
            return ((pt.X >= lp1.X && pt.X <= lp2.X) && (pt.Y >= lp1.Y && pt.Y <= lp2.Y)) ||
                ((pt.X >= lp1.X && pt.X <= lp2.X) && (pt.Y <= lp1.Y && pt.Y >= lp2.Y)) ||
                ((pt.X <= lp1.X && pt.X >= lp2.X) && (pt.Y >= lp1.Y && pt.Y <= lp2.Y)) ||
                ((pt.X <= lp1.X && pt.X >= lp2.X) && (pt.Y <= lp1.Y && pt.Y >= lp2.Y));
        }

    }
}
