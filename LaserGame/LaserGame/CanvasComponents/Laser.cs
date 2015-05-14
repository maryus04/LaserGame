using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using Drawing = System.Drawing;
using System.Windows.Shapes;
using Client.CanvasBehavior;
using System.Collections;
using System.Collections.Specialized;

namespace Client.CanvasComponents {
    class Laser {

        private static readonly Point INVALID_POINT = new Point( -1, -1 );

        private static Laser instance;

        private SortedDictionary<string, Line> _laserLines = new SortedDictionary<string, Line>();

        private Line _myLaser;

        public Laser() {
            instance = this;
        }

        public bool BuildLaser() {
            if(_myLaser != null) {
                return false;
            }
            return true;
        }

        private void ListChanged( object sender, EventArgs e ) {
        }

        public Line GetLine( string lineCoord ) {
            Line foundLaser = null;
            _laserLines.TryGetValue( lineCoord, out foundLaser );
            return foundLaser;
        }

        public void RemoveLine( string lineCoord ) {
            _laserLines.Remove( lineCoord );
        }

        public Line GetLastLine() {
            return (Line)_laserLines.Values.Last();
        }

        public void RemoveAll() {
            RemoveMyLaser();
            for(int i = 1; i < _laserLines.Count; i++) {
                GameWindow.getInstance().RemoveFromGameCanvas( _laserLines.Values.ElementAt( i ) );
                _laserLines.Remove( _laserLines.Keys.ElementAt( i ) );
            }
        }

        public void RemoveMyLaser() {
            if(_myLaser == null) {
                return;
            }
            Player.getInstance().WriteLine( "LaserRemoved:COORD2:" + _myLaser.X1 + "," + _myLaser.Y1 + "," + _myLaser.X2 + "," + _myLaser.Y2 + "ENDCOORD2" );
            GameWindow.getInstance().RemoveFromGameCanvas( _myLaser );
            _myLaser = null;
        }

        public List<Line> GetAllLines() {
            return _laserLines.Values.ToList();
        }

        public void AddLine( Line line ) {
            _laserLines.Add( "" + line.X1 + line.Y1 + line.X2 + line.Y2, line );
        }

        public void CreateLaserFromOpositePortal( string buildingDirection, Point startPoint, int radian ) {
            RotateTransform rotation = new RotateTransform( radian );
            Point p1 = rotation.Transform( new Point( GetLastLine().X1, GetLastLine().Y1 ) );
            Point p2 = rotation.Transform( new Point( GetLastLine().X2, GetLastLine().Y2 ) );

            BuildMyLaserLine( buildingDirection, startPoint, MoveLine( p1, p2, startPoint ) );
        }

        // Origin1 is used to calculate distance between the first points of the two lines(since we have accesonly to the first point of second line)
        public Point MoveLine( Point origin1, Point origin2, Point moveTo ) {
            double x = origin1.X - moveTo.X;
            double y = origin1.Y - moveTo.Y;

            return new Point( origin2.X - x, origin2.Y - y );
        }


        public Line BuildMyLaserLine( string buildingDirection, Point p1, Point p2 ) {
            _myLaser = BuildIntersectedLaserLine( buildingDirection, p1, p2 );

            GameWindow.getInstance().AddToGameCanvas( _myLaser );
            Player.getInstance().WriteLine( "LaserCreated:VALUE:" + buildingDirection + "ENDVALUE" + "COORD2:" + _myLaser.X1 + "," + _myLaser.Y1 + "," + _myLaser.X2 + "," + _myLaser.Y2 + "ENDCOORD2" );
            return _myLaser;
        }

        public Line BuildLaserLine( string buildingDirection, Point p1, Point p2 ) {
            Line myLine = BuildIntersectedLaserLine( buildingDirection, p1, p2 );

            AddLine( myLine );
            GameWindow.getInstance().AddToGameCanvas( myLine );
            return myLine;
        }

        private static Line BuildIntersectedLaserLine( string buildingDirection, Point p1, Point p2 ) {
            Line myLine = new Line();
            myLine.Stroke = System.Windows.Media.Brushes.Red;
            myLine.X1 = p1.X;
            myLine.Y1 = p1.Y;

            myLine.X2 = p2.X;
            myLine.Y2 = p2.Y;

            myLine.StrokeThickness = 1;

            Point firstIntersectionPoint;

            if((firstIntersectionPoint = LaserBehavior.GetInterLastLineAllBlocks( buildingDirection, myLine )) != INVALID_POINT) {
                myLine.X2 = firstIntersectionPoint.X;
                myLine.Y2 = firstIntersectionPoint.Y;
            }
            return myLine;
        }

        public static Laser getInstance() {
            return instance;
        }
    }
}
