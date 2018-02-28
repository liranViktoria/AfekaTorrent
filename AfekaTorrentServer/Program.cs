

using System;
using System.ServiceModel;
using System.Net;
using System.Net.Sockets;




namespace AfekaTorrentServer
{

    class Program
    {
        private static ServiceHost server;

        static void Main(string[] args)
        {
            IPEndPoint meetingPoint = new IPEndPoint(IPAddress.Loopback, 8005);
            TcpListener listener = new TcpListener(meetingPoint.Port);
            listener.Start();
            Console.WriteLine("server started!");
            Console.Read();
        }


    }
}
