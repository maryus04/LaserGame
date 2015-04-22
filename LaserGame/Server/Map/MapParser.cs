using System;
using System.Linq;
using System.IO;

namespace Server {
    static class MapParser {

        private static StreamReader _reader;
        private static string _fileName;
        private static string _line;
        private static int _width, _height;

        public static void SetMapName( string map ) {
            _fileName = "Resources/Maps/" + map;
        }

        public static Tuple<int, int> ParseMapDimensions() {
            _reader = new StreamReader( _fileName );
            if((_line = _reader.ReadLine()) != null) {
                char[] itemsOnRow = _line.ToCharArray();
                _width = itemsOnRow.Count();

                string[] lines = _reader.ReadToEnd().Split( new string[] { "\r\n" }, StringSplitOptions.None );
                _height = lines.Count() + 1;
            }
            if(CheckRowsIntegrity( _width )) {
                return Tuple.Create( _width, _height );
            }
            return Tuple.Create( 0, 0 );
        }

        private static bool CheckRowsIntegrity( int mapWidth ) {
            _reader = new StreamReader( _fileName );
            string fullMap = _reader.ReadToEnd();
            int mapLine = 0;

            string[] lines = fullMap.Replace( " ", "" ).Split( new string[] { "\r\n" }, StringSplitOptions.None );
            foreach(string line in lines) {
                mapLine++;
                if(line.ToCharArray().Count() != mapWidth) {
                    throw new System.InvalidOperationException( "ERROR : Diferent rows measure in the map. On map line " + mapLine );
                }
            }
            return true;
        }

    }
}
