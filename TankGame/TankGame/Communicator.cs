using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace TankGame
{
    class Communicator
    {
        TcpListener listener;
        TcpClient client;
        StreamReader ls; 
        NetworkStream ns;
        string serverip; int serverport;

        public Communicator(string svrip,int svrport,int listnerport)
        {
            serverip=svrip;
            serverport = svrport;
            listener = new TcpListener(listnerport); 
            listener.Start();
        }

        public void send(string msg)
        {
            try
            {
                client = new TcpClient(serverip, serverport);
                Stream s = client.GetStream();
                StreamWriter sw = new StreamWriter(s);
                sw.AutoFlush = true;
                sw.Write(msg);
            }
            finally
            {
                client.Close();
            }
        }

        public string listen()
        {
            string msg = "";
            if (listener.Pending())
            {
                ns = new NetworkStream(listener.AcceptSocket());
                ls = new StreamReader(ns);
                if (ns.DataAvailable)
                {
                    msg=ls.ReadLine();
                }
            }
            return msg;
        }
    }
}
