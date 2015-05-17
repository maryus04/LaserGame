﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Shapes;

namespace Client.MessageControl {
    static class MessageParser {

        public static Tuple<string, string> getMethodMessage( string message ) {
            string method = message.Substring( 0, message.IndexOf( ":" ) + 1 );
            message = message.Substring( message.IndexOf( ":" ) + 1, message.Length - message.IndexOf( ":" ) + 1 );
            return Tuple.Create( method, message );
        }

        public static Point GetPoint( string message ) {
            message = message.Substring( message.IndexOf( "COORD:" ) + 6, (message.IndexOf( "ENDCOORD" )) - (message.IndexOf( "COORD:" ) + 6) );
            string[] points = message.Split( ',' );

            return new Point( Double.Parse( points[0] ), Double.Parse( points[1] ) );
        }

        public static Line GetLine( string message ) {
            message = message.Substring( message.IndexOf( "COORD2:" ) + 7, (message.IndexOf( "ENDCOORD2" )) - (message.IndexOf( "COORD2:" ) + 7) );
            string[] points = message.Split( ',' );

            return new Line() { X1 = Double.Parse( points[0] ), Y1 = Double.Parse( points[1] ), X2 = Double.Parse( points[2] ), Y2 = Double.Parse( points[3] ) };
        }

        public static string GetNick( string message ) {
            string playerName = message.Substring( message.IndexOf( "NICK:" ) + 5, (message.IndexOf( "ENDNICK" )) - (message.IndexOf( "NICK:" ) + 5) );
            return playerName + " : ";
        }

        public static string GetName( string message ) {
            return message.Substring( message.IndexOf( "NICK:" ) + 5, (message.IndexOf( "ENDNICK" )) - (message.IndexOf( "NICK:" ) + 5) );
        }

        public static string GetValue( string message ) {
            return message.Substring( message.IndexOf( "VALUE:" ) + 6, (message.IndexOf( "ENDVALUE" )) - (message.IndexOf( "VALUE:" ) + 6) );
        }

        public static string RemoveNickFrom( string message ) {
            string playerName = message.Substring( message.IndexOf( "NICK:" ) + 5, (message.IndexOf( "ENDNICK" )) - (message.IndexOf( "NICK:" ) + 5) );
            message = message.Replace( "NICK:" + playerName + "ENDNICK", "" );
            return message;
        }

    }
}
