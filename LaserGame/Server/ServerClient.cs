﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Drawing;

namespace Server {
    public class ServerClient {
        private string nickName = "NotYetSet";

        public TcpClient TcpClient { get; set; }

        public StreamReader Reader { get; set; }
        public StreamWriter Writer { get; set; }

        public bool FirstClick { get; set; }

        public Rectangle FirstPortal { get; set; }
        public Rectangle SecondPortal { get; set; }

        public bool Ready { get; set; }

        public string NickName {
            get { return nickName; }
            set { nickName = value; }
        }

        public bool isConnected;

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
            try {
                return Reader.ReadLine();
            } catch(Exception) {
                Reader.Close();
                Writer.Close();
                TcpClient.Close();
            }
            return "";
        }

        public void Flush() {
            Writer.Flush();
        }

        public void Dispose() {
            isConnected = false;
            Reader.Close();
            Writer.Close();
            TcpClient.Close();
        }

        public int GetPort() {
            return ((IPEndPoint)TcpClient.Client.RemoteEndPoint).Port;
        }

        public string GetIp() {
            return ((IPEndPoint)TcpClient.Client.RemoteEndPoint).Address.ToString();
        }
    }
}
