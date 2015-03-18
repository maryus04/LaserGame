using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Controls;

namespace Client {
    static class MapParser {


        //
        // Summary:
        //     Gets WIDTH and HEIGTH from the map definition
        //
        // Parameters:
        //
        // Returns:
        //     
        //
        public static void ParseMapDimensions(Label width, Label height) {
            StreamReader reader = File.OpenText( "Resources/Maps/initial.map1" );
            string line;
            if ( ( line = reader.ReadLine() ) != null ) {
                string[] items = line.Split( ',' );

                width.Content= items[0];
                height.Content= items[1];
            }
            
        }

    }
}
