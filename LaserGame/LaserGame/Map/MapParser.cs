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
        private static int _width, _height = 0;
        private static List<string> _parsedPart = new List<string>();
        private static int _curParsingRow = 0;
        private static int _curParsingColumn = 0;

        public static readonly int MULTIPLY = 13;

        public static void ParseMap( string[] map ) {
            _width = Int32.Parse( map[0] );
            _height = map.Length - 2;
            for(int i = 1; i < map.Length; i++) {
                _map.Add( map[i].ToUpper() );
            }

            StartDrawing();
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

        private static void StartDrawing() {
            string currentChar = "" + _map[0][0];

            while(_curParsingRow <= _height && _curParsingColumn <= _width) {

                if(!_parsedPart.Contains( "" + _curParsingRow + "." + _curParsingColumn )) {
                    SetCurrentChar();

                    Tuple<int, int, int, int> diagonal = GetBlockPoints( currentChar );

                    int x = (diagonal.Item3 + 1) * MULTIPLY - diagonal.Item1 * MULTIPLY;
                    int y = (diagonal.Item4 + 1) * MULTIPLY - diagonal.Item2 * MULTIPLY;
                    int blockWidth = diagonal.Item1 * MULTIPLY;
                    int blockHeight = diagonal.Item2 * MULTIPLY;

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

        private static void SetParsedLine( List<string> parsedParts ) {
            foreach(string parse in parsedParts) {
                _parsedPart.Add( parse );
            }
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

        private static void SetCurrentChar() {
            _parsedPart.Add( "" + _curParsingRow + "." + _curParsingColumn );
        }

    }
}
