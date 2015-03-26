using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Client {
    public partial class Second : Window {
        public Second() {
            InitializeComponent();
        }

        public Second(int width,int height) {
            InitializeComponent();
            GlobalVariable.debugMode = true;
            Client.LaserComponents.Laser laser = new Client.LaserComponents.Laser( Canvas );

            laser.buildFirstLine();


            //Image im = new Image();

            //var uriSource = new Uri( @"Resources/SpriteMaps/test.png", UriKind.Relative );

            //im.Source = new BitmapImage(uriSource);
            //Canvas.Children.Add( im );
            this.Show();

        }
    }
}
