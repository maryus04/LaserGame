using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Windows;
using Client.CanvasBehavior;
using System.Windows.Shapes;
using System.Windows.Media;
using Client.CanvasComponents;

namespace Client.Map {
    static class MapParser {

        private static List<string> _map = new List<string>();
        private static List<string> _parsedPart = new List<string>();
        private static int _width, _height = 0;
        private static int _curParsingRow = 0;
        private static int _curParsingColumn = 0;

        private static int _multiplierX = 1;
        private static int _multiplierY = 1;

        public static void ParseMap( string[] map ) {
            _width = Int32.Parse( map[0] ) + 2;

            CreateMap( map );

            _height = _map.Count - 1;

            var screen = System.Windows.Forms.Screen.PrimaryScreen.Bounds;

            MapParser.SetMultiplier( screen.Width / _width, screen.Height / _height );
            PortalBehavior.SetDimensions( _multiplierX, _multiplierY );
            GameWindow.getInstance().SetGridLayout( (_width + 1) * _multiplierX, (_height + 1) * _multiplierY );

            StartDrawing();
        }

        private static void StartDrawing() {
            string currentChar = "" + _map[0][0];

            while(_curParsingRow <= _height && _curParsingColumn <= _width) {

                if(!_parsedPart.Contains( "" + _curParsingRow + "." + _curParsingColumn )) {
                    SetCurrentChar();

                    int x;
                    int y;
                    int blockWidth;
                    int blockHeight;
                    ExtractBlockDimensionAndMultiply( currentChar, out x, out y, out blockWidth, out blockHeight );

                    switch(currentChar) {
                        case "H":
                            HardBlock.Add( "HardBlock", Block.Create( x, y, blockWidth, blockHeight, Brushes.Green ) );
                            break;
                        case "S":
                            StarBlock.Add( "StarBlock", Block.Create( x, y, blockWidth, blockHeight, Brushes.Gold ) );
                            break;
                        default:
                            break;
                    }
                }

                currentChar = GetNextChar();
            }
            GameWindow.getInstance().ShowGame();
        }

        private static void ExtractBlockDimensionAndMultiply( string currentChar, out int x, out int y, out int blockWidth, out int blockHeight ) {
            Tuple<int, int, int, int> diagonal = GetBlockPoints( currentChar );

            x = (diagonal.Item3 + 1) * _multiplierX - diagonal.Item1 * _multiplierX;
            y = (diagonal.Item4 + 1) * _multiplierY - diagonal.Item2 * _multiplierY;
            blockWidth = diagonal.Item1 * _multiplierX;
            blockHeight = diagonal.Item2 * _multiplierY;
        }

        private static Tuple<int, int, int, int> GetBlockPoints( string character ) {
            //First Point
            int x1 = _curParsingColumn;
            int y1 = _curParsingRow;

            int x2 = _curParsingColumn;
            while(PeekNextChar() == character && !_parsedPart.Contains( PeekPosition() )) {
                GetNextChar();
                x2 = _curParsingColumn;
                SetCurrentChar();
            }

            //Second point
            int y2 = ParseBlock( character, y1, x1, x2 );

            return Tuple.Create( x1, y1, x2, y2 );
        }

        private static int ParseBlock( string character, int rowNumber, int lineStart, int lineStop ) {
            int curRow = rowNumber + 1;
            int curColumn = lineStart;
            List<string> parsedLinePart = new List<string>();

            while(PeekCharAt( curRow, curColumn ) == character) {
                parsedLinePart.Add( "" + curRow + "." + curColumn );

                if(curColumn == lineStop) { //NextLine
                    curRow++;
                    curColumn = lineStart;
                    SetParsedLine( parsedLinePart );
                    parsedLinePart.Clear();
                } else { //NextChar
                    curColumn++;
                }
            }

            return curRow - 1;
        }

        private static string ConstructLine( int width ) {
            string line = "";
            for(int i = 0; i < width + 2; i++) {
                line += "H";
            }
            return line;
        }

        private static string GetNextChar() {
            _curParsingColumn++;

            if(_curParsingColumn > _width && _curParsingRow + 1 <= _height) {
                _curParsingColumn = 0;
                _curParsingRow++;
                return "" + _map[_curParsingRow][_curParsingColumn];
            } else if(_curParsingColumn <= _width && _curParsingRow <= _height) {
                return "" + _map[_curParsingRow][_curParsingColumn];
            }
            return "";
        }

        private static string PeekCharAt( int row, int column ) {
            if(column > _width && row + 1 <= _height) {
                column = 0;
                row++;
                return "" + _map[row][column];
            } else if(column <= _width && row <= _height) {
                return "" + _map[row][column];
            }
            return "";
        }

        private static string PeekNextChar() {
            int peekColumn = _curParsingColumn + 1;
            int peekRow = _curParsingRow;

            if(_curParsingColumn > _width && _curParsingRow + 1 <= _height) {
                peekColumn = 0;
                peekRow++;
                return "" + _map[peekRow][peekColumn];
            } else if(peekColumn <= _width && peekRow <= _height) {
                return "" + _map[peekRow][peekColumn];
            }
            return "";
        }

        private static string PeekPosition() {
            int peekColumn = _curParsingColumn + 1;
            int peekRow = _curParsingRow;

            if(_curParsingColumn > _width && _curParsingRow + 1 <= _height) {
                peekColumn = 0;
                peekRow++;
                return "" + peekRow + '.' + peekColumn;
            } else if(peekColumn <= _width && peekRow <= _height) {
                return "" + peekRow + '.' + peekColumn;
            }
            return "";
        }

        private static void SetParsedLine( List<string> parsedParts ) {
            foreach(string parse in parsedParts) {
                _parsedPart.Add( parse );
            }
        }

        private static void CreateMap( string[] map ) {
            _map.Add( ConstructLine( map[1].Length ) );
            for(int i = 1; i < map.Length; i++) {
                _map.Add( "H" + map[i].ToUpper() + "H" );
            }
            _map.Add( ConstructLine( _width ) );
        }

        private static void SetMultiplier( int x, int y ) {
            _multiplierX = x;
            _multiplierY = y;
        }

        private static void SetCurrentChar() {
            _parsedPart.Add( "" + _curParsingRow + "." + _curParsingColumn );
        }

    }
}
