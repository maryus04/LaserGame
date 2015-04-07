using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;

namespace Server {
    public class ServerClient {
        public TcpClient TcpClient { set; get; }

        public StreamReader Reader { set; get; }
        public StreamWriter Writer { set; get; }

        public string NickName { set; get; }

        public bool IsConnected { set; get; }

        public NetworkStream GetStream() {
            return TcpClient.GetStream();
        }

        public void WriteLine( string message ) {
            Writer.WriteLine( message );
            Writer.Flush();
        }

        public bool IsTcpConnected() {
            return TcpClient.Connected;
        }

        public String ReadLine() {
            return Reader.ReadLine();
        }

        public void Flush() {
            Writer.Flush();
        }

        public void Dispose() {
            IsConnected = false;
            Reader.Close();
            Writer.Close();
            TcpClient.Close();
        }
    }
}
