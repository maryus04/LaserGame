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

        private static ObservableCollection<Line> _laserLines;
        private static Canvas _gameCanvas;

        public Laser( Canvas gameCanvas ,Canvas debugCanvas) {
            Behavior.DebugCanvas = debugCanvas;
            Behavior.GameCanvas = gameCanvas;
            _gameCanvas = gameCanvas;
            _laserLines = new ObservableCollection<Line>(); // TODO: use this to autoupdate the laser after a intersection is occured
            _laserLines.CollectionChanged += ListChanged;
        }

        public Line GetLastLine() {
            return _laserLines[_laserLines.Count - 1];
        }

        public List<Line> GetAllLines() {
            return _laserLines.ToList();
        }

        public void buildFirstLine() {
            Line myLine = new Line();
            myLine.Stroke = System.Windows.Media.Brushes.Red;
            myLine.X1 = 0;
            myLine.Y1 = 150;

            myLine.X2 = 500;
            myLine.Y2 = 150;

            myLine.StrokeThickness = 1;

            _laserLines.Add( myLine );
            _gameCanvas.Children.Add( myLine );
        }

        private void ListChanged( object sender, EventArgs e ) {
            Behavior.IntersectionLineAllCanvasRects( GetLastLine() );
        }

        


    }
}
