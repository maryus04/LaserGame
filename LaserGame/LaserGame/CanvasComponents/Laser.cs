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
        }

        public Line GetLastLine() {
            return _laserLines[_laserLines.Count - 1];
        }

        public void RemoveAll() {
            for(int i = 1; i < _laserLines.Count; i++) {
                _laserLines.RemoveAt(i);
            }
        }

        public List<Line> GetAllLines() {
            return _laserLines.ToList();
        }

        public void AddLine( Line line ) {
            _laserLines.Add( line );
        }

        public void buildPortalLine( Rectangle portalStart ) {
            double x = Canvas.GetLeft( portalStart );
            double y = Canvas.GetTop( portalStart );
            switch(Mechanic.GetLaserPath( new Line(){X1=x , Y1=y , X2=x+10,Y2=y+10})) {
                case "UP":
                    buildLaserLine( new Point( x + 5, y ), new Point( x + 5, 0 ) );
                    break;
                case "DOWN":
                    buildLaserLine( new Point( x + 5, y + 10 ), new Point( x + 5, 900 ) );
                    break;
                case "LEFT":
                    buildLaserLine( new Point( x, y + 5 ), new Point( 0, y + 5 ) );
                    break;
                case "RIGHT":
                    buildLaserLine( new Point( x + 10, y + 5 ), new Point( 900, y + 5 ) );
                    break;
            }
        }

        public void buildLaserLine( Point p1, Point p2 ) {
            Line myLine = new Line();
            myLine.Stroke = System.Windows.Media.Brushes.Red;
            myLine.X1 = p1.X;
            myLine.Y1 = p1.Y;

            myLine.X2 = p1.X;
            myLine.Y2 = p1.Y;

            myLine.StrokeThickness = 1;

            _laserLines.Add( myLine );
            Point temp;
            while((temp = LaserBehavior.IntersectionLineAllCanvasRects( myLine )) == new Point( -1, -1 )) {
                if(p1.X <= p2.X) {
                    if(myLine.X2 <= p2.X) {
                        myLine.X2 += 3;
                    }
                    if(myLine.Y2 <= p2.Y) {
                        myLine.Y2 += 3;
                    }
                } else {
                    if(myLine.X2 >= p2.X) {
                        myLine.X2 -= 3;
                    }
                    if(myLine.Y2 >= p2.Y) {
                        myLine.Y2 -= 3;
                    }
                }
            }
            GameWindow.getInstance().gameCanvas.Children.Add( myLine );
        }

        public static Laser getInstance() {
            return instance;
        }
    }
}
