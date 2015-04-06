using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;

namespace Server {
    public class ServerClient {
        public TcpClient TcpClient {set; get;}

        public StreamReader Reader { set; get; }
        public StreamWriter Writer { set; get; }

        public string NickName {set; get;}

        public NetworkStream GetStream() {
            return TcpClient.GetStream();
        }

        public bool Connected() {
            return TcpClient.Connected;
        }

        public String ReadLine() {
            return Reader.ReadLine();
        }

        public void Flush() {
            Writer.Flush();
        }
    }
}
