using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace Client.Map.Sprite {
    class Sheet {
        public static Sheet mapSheet = new Sheet( "test.png" );

        private readonly string PATH = @"Resources/SpriteSheets/";
        private readonly Uri sourceUrl;
        private BitmapImage image;

        public Sheet( string file ) {
            sourceUrl = new Uri( PATH + file, UriKind.Relative );
            LoadImage();
        }

        private void LoadImage() {
            try {
                image = new BitmapImage( sourceUrl );
            } catch(Exception e) {
                Console.WriteLine( e.StackTrace );
            }
        }
    }
}
