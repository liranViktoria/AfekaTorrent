

using System;
using System.ServiceModel;
using System.Net;
using System.Net.Sockets;




namespace AfekaTorrentServer
{
   
    public class Program
    {

       

     

        static void Main(string[] args)
        {
            Socket s;
            s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s.Bind(new IPEndPoint(0, 8005));
            s.Listen(0);
            Console.WriteLine("Server started listening...");

            Console.Read();

            // s.BeginAccept(callback, null);
        }

        //void callback(IAsyncResult ar)
        //{
        //    try
        //    {
        //        Socket s = this.s.EndAccept(ar);
        //        if (SocketAcceptedFilesReq != null)
        //            SocketAcceptedFilesReq(s);

        //        if (SocketAcceptedConnection != null)
        //            SocketAcceptedConnection(s);

        //        this.s.BeginAccept(callback, null);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}


    }



}
