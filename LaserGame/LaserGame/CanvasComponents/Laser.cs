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

namespace Client.LaserComponents {
    class Laser {

        private static Laser instance;

        private ObservableCollection<Line> _laserLines;

        public Laser( Canvas gameCanvas, Canvas debugCanvas ) {
            instance = this;
            _laserLines = new ObservableCollection<Line>(); // TODO: use this to autoupdate the laser after a intersection occured
            _laserLines.CollectionChanged += ListChanged;
        }

        public Line GetLastLine() {
            return _laserLines[_laserLines.Count - 1];
        }

        public List<Line> GetAllLines() {
            return _laserLines.ToList();
        }

        public void buildLaserLine( Point p1, Point p2 ) {
            Line myLine = new Line();
            myLine.Stroke = System.Windows.Media.Brushes.Red;
            myLine.X1 = p1.X;
            myLine.Y1 = p1.Y;

            myLine.X2 = p2.X;
            myLine.Y2 = p2.Y;

            myLine.StrokeThickness = 1;

            _laserLines.Add( myLine );
            Point firstIntersectionPoint;
            if((firstIntersectionPoint = LaserBehavior.GetInterLastLineAllBlocks( myLine )) != new Point( -1, -1 )) {
                myLine.X2 = firstIntersectionPoint.X;
                myLine.Y2 = firstIntersectionPoint.Y;
            }
            GameWindow.getInstance().gameCanvas.Children.Add( myLine );
        }

        private void ListChanged( object sender, EventArgs e ) {
            LaserBehavior.GetInterLastLineAllBlocks( GetLastLine() );
        }

        public static Laser getInstance() {
            return instance;
        }
    }
}
