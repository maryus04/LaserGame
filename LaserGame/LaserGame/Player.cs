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

        public static string name;
        public static bool firstClick;
        public static Rectangle firstPortal;
        public static Rectangle secondPortal;

        public static TcpClient tcpClient;
        public static bool connected;

        public static StreamWriter writer;
        public static StreamReader reader;

        public static int width;
        public static int height;

        public static void WriteLine( string message ) {
            writer.WriteLine( message );
            writer.Flush();
        }

        public static string ReadLine() {
            try {
                return reader.ReadLine();
            } catch(Exception) {
                Player.reader.Close();
                Player.writer.Close();
                Player.tcpClient.Close();
            }
            return "";
        }

    }
}
