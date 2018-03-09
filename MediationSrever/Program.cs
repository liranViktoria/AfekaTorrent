

using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Net;
using System.Net.Sockets;
using MediationSrever;
using DBLibrary;


namespace MediationSrever
{

    class Server
    {
        static SocketHandler _fileRequestListener;//listener for file requests
        static SocketHandler _connectionListener;//listener for connection 
        static List<Client> _clients;
        public static readonly int FILE_REQUEST = 1;
        public static readonly int CONNECTION = 0;

        static void Main(string[] args)
        {
            _fileRequestListener = new SocketHandler(8080);
            _connectionListener = new SocketHandler(8085);
            _connectionListener.SocketAcceptedConnectionEvent += new SocketHandler.SocketAcceptedConnectionHandler(l_socketAcceptedConnection);
            _fileRequestListener.SocketAcceptedFileRequestEvent += new SocketHandler.SocketAcceptedFileRequestHandler(l_socketAcceptedFileRequest);
            _clients = new List<Client>();
            Console.WriteLine("\n\n************************************* VIKA and LIRAN Afeka Mini Torrent Server***************************************\n");
            Console.WriteLine("Regisered accounts:");
            ClientsTable.Instance.RegisterUser("momo", "123123");
            ClientsTable.Instance.RegisterUser("Liran", "111111");
            ClientsTable.Instance.RegisterUser("Victoria", "222222");
            ClientsTable.Instance.RegisterUser("Maxim", "333333");
            ClientsTable.Instance.RegisterUser("Zaychik", "777777");
            Console.WriteLine(ClientsTable.Instance.PrintAccounts());
            _fileRequestListener.Start();
            _connectionListener.Start();
            Console.WriteLine("SERVER STARTED LISTENING...");
            Console.Read();

        }//end of main

        private static void l_socketAcceptedConnection(Socket s)
        {
            Client client = new Client(s, CONNECTION);
            client.ClientRecievedEvent += new Client.ClientRecievedHandler(client_Received);
            client.ClientDisconnectedEvent += (client_Disconnected);
            Console.WriteLine("Port 8085 Socket accepted:\nEndPoint: {0}, client id: {1} ...\n", client.EndPoint.ToString(), client.Id);
        }

        private static void l_socketAcceptedFileRequest(Socket s)
        {
            Client client = new Client(s, FILE_REQUEST);
            client.ClientRecievedEvent += new Client.ClientRecievedHandler(client_Received);
            client.ClientDisconnectedEvent += (client_Disconnected);
            Console.WriteLine("Port 8080 Socket accepted:\nEndPoint: {0}, client id: {1} ...\n", client.EndPoint.ToString(), client.Id);
        }

        private static void client_Disconnected(String id)
        {
            Console.WriteLine("Client {0} has disconnected from server\n", id);
        }

        private static void client_Received(Client sender, string msg)
        {
            Console.WriteLine("Client received {2}...\nmsg: {0}, client id: {1}\n", msg, sender.Id, DateTime.Now.ToString());
        }

    }//end of class

}//end of namespace
