using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Client.MessageControl {
    static class MessageParser {

        public static Tuple<string, string> getMethodMessage( string message ) {
            string method = message.Substring( 0, message.IndexOf( ":" ) + 1 );
            message = message.Substring( message.IndexOf( ":" ) + 1, message.Length - message.IndexOf( ":" ) + 1 );
            return Tuple.Create( method, message );
        }

        public static Point GetPoint( string message ) {
            message = message.Substring( message.IndexOf( "COORD:" ) + 6, (message.IndexOf( "ENDCOORD" )) - (message.IndexOf( "COORD:" ) + 6) );
            string x = message.Substring( 0, (message.IndexOf( "," )) );
            message = message.Replace( x + ",", "" );
            string y = message.Substring( 0, message.Length );

            return new Point( Int32.Parse( x ), Int32.Parse( y ) );
        }

    }
}
