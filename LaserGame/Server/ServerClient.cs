using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Net;

namespace Server {
    public class ServerClient {
        public TcpClient tcpClient;

        public StreamReader reader;
        public StreamWriter writer;

        public string nickName = "NotYetSet";

        public bool isConnected;

        public NetworkStream GetStream() {
            return tcpClient.GetStream();
        }

        public void WriteLine( string message ) {
            writer.WriteLine( message );
            writer.Flush();
        }

        public bool IsTcpConnected() {
            return tcpClient.Connected;
        }

        public String ReadLine() {
            return reader.ReadLine();
        }

        public void Flush() {
            writer.Flush();
        }

        public void Dispose() {
            isConnected = false;
            reader.Close();
            writer.Close();
            tcpClient.Close();
        }

        public int GetPort(){
            return ((IPEndPoint)tcpClient.Client.RemoteEndPoint).Port;
        }

        public long GetIp() {
            return ((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address.Address;
        }
    }
}
