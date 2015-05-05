using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.MessageControl {
    static class MessageParser {

        public static Tuple<string, string> getMethodMessage( string message ) {
            string method = message.Substring( 0, message.IndexOf( ":" ) + 1 );
            message = message.Substring( message.IndexOf( ":" ) + 1, message.Length - message.IndexOf( ":" ) + 1 );
            return Tuple.Create( method, message );
        }

        public static Tuple<double, double, double, double> GetLine( string message ) {
            message = message.Substring( message.IndexOf( "COORD2:" ) + 7, (message.IndexOf( "ENDCOORD2" )) - (message.IndexOf( "COORD2:" ) + 7) );
            string[] points = message.Split(',');

            return Tuple.Create( Double.Parse( points[0]),Double.Parse( points[1] ),Double.Parse( points[2] ),Double.Parse( points[3] ) );
        }

        public static Tuple<int, int> GetPointsFromMessage( string message ) {
            message = message.Substring( message.IndexOf( "COORD:" ) + 6, (message.IndexOf( "ENDCOORD" )) - (message.IndexOf( "COORD:" ) + 6) );
            string[] points = message.Split( ',' );

            return Tuple.Create( Int32.Parse( points[0] ), Int32.Parse( points[1] ) );
        }

    }
}
