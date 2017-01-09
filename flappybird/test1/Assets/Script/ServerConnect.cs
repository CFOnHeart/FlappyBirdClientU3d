using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace Assets.Script
{
    public class ServerConnect
    {
        public static ServerConnect instance = null;
        public int server_port = 8787;
        public string host = "192.168.123.128";
        public int buf_size = 1024;
        public Socket socket;
        public void connect()
        {
            IPAddress ip = IPAddress.Parse(host);
            IPEndPoint ipe = new IPEndPoint(ip, server_port);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(ipe);
            Debug.Log("connecting");
        }

        public void sendMessage(string message)
        {
            byte[] bs = Encoding.ASCII.GetBytes(message);
            socket.Send(bs, bs.Length, 0);
        }

        public string receiveMessage()
        {
            string recvStr = "";
            byte[] recvBytes = new byte[buf_size];
            int bytes = socket.Receive(recvBytes, buf_size, 0);
            recvStr += Encoding.ASCII.GetString(recvBytes , 0 , bytes);
            Debug.Log("receive message is : " + recvStr);
            return recvStr;
        }

        public void close()
        {
            socket.Close();
            instance = null;
        }
    }
}
