using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Net.Sockets;
using System.IO;

namespace Client {
    public static class Player {

        public static string Name { get; set; }
        public static bool FirstClick { get; set; }
        public static Rectangle FirstPortal { get; set; }
        public static Rectangle SecondPortal { get; set; }

        public static TcpClient TcpClient { get; set; }
        public static bool Connected { get; set; }

        public static StreamWriter Writer { get; set; }
        public static StreamReader Reader { get; set; }
        
        public static void WriteLine( string message ) {
            try {
                Writer.WriteLine( message );
                Writer.Flush();
            } catch(Exception) {
                if(TcpClient.Connected) {
                    Reader.Close();
                    Writer.Close();
                    TcpClient.Close();
                }
            }
        }

        public static string ReadLine() {
            try {
                return Reader.ReadLine();
            } catch(Exception) {
                if(TcpClient.Connected) {
                    Reader.Close();
                    Writer.Close();
                    TcpClient.Close();
                }
            }
            return "";
        }

        public static void CloseConnection() {
            if(TcpClient != null && TcpClient.Connected) {
                Connected = false;
                WriteLine( "CloseConnection:" );
                Reader.Close();
                Writer.Close();
                TcpClient.Close();
            }
        }

        public static void InitializeStream() {
            Writer = new StreamWriter( TcpClient.GetStream() );
            Reader = new StreamReader( TcpClient.GetStream() );
        }

    }
}
