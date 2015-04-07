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

        public static string Name { set; get; }
        public static bool firstClick;
        public static Rectangle FirstPortal { get; set; }
        public static Rectangle SecondPortal { get; set; }

        public static TcpClient TcpClient { set; get; }
        public static bool Connected { set; get; }

        public static StreamWriter Writer { set; get; }
        public static StreamReader Reader { set; get; }

        public static int Width { set; get; }
        public static int Height { set; get; }

        

        public static void WriteLine( string message ) {
            Writer.WriteLine( message );
            Writer.Flush();
        }

        public static string ReadLine() {
            return Reader.ReadLine();
        }

    }
}
