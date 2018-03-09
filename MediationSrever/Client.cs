﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using DetailsLibrary;
using Newtonsoft.Json;
using DBLibrary;


namespace MediationSrever
{
    class Client
    {
        //class attributes
        private Socket _socket;
        private DetailsFromUser _userDetails;
        private string _userName;
        private bool _initialMessage = true;


        //DBHandler handler = new DBHandler();
        public string UserName { get; set; }
        public string Id { get; private set; }
        public IPEndPoint EndPoint { get; private set; }
        public Socket Socket { get { return _socket; } }

        //delegates and handlers
        public delegate void ClientRecievedHandler(Client sender, string message);
        public event ClientRecievedHandler ClientRecievedEvent;
        public delegate void ClientDisconnectedHandlers(String userName);
        public event ClientDisconnectedHandlers ClientDisconnectedEvent;

        public Client(Socket accepted, int functions)
        {
            _socket = accepted;
            Id = Guid.NewGuid().ToString();
            EndPoint = (IPEndPoint)_socket.RemoteEndPoint;
            if (functions == Server.FILE_REQUEST)
            {
                _socket.BeginReceive(new byte[] { 0 }, 0, 0, 0, FileCallBack, null);
            }
            else
                _socket.BeginReceive(new byte[] { 0 }, 0, 0, 0, ConnectionCallBack, null);
        }

        //method for recieving a file
        void FileCallBack(IAsyncResult ar)
        {
            try
            {
                _socket.EndReceive(ar);
                byte[] buffer;
                int recieveBuffer;
               // DetailsFromUser userDetails;

                if (_initialMessage)
                {
                    _initialMessage = false;
                    buffer = new byte[8192];
                    recieveBuffer = _socket.Receive(buffer, buffer.Length, 0);
                    if (recieveBuffer < buffer.Length) { Array.Resize<byte>(ref buffer, recieveBuffer); }
                    //check if there is a a message and answer it
                    _userDetails = JsonConvert.DeserializeObject<DetailsFromUser>(Encoding.Default.GetString(buffer));
                    _userName = _userDetails.UserName;

                    try
                    {
                        ClientsTable.Instance.IsClientValid(_userName, _userDetails.Password);
                        _socket.Send(Encoding.Default.GetBytes("OK"));
                    }
                    catch
                    {
                        _socket.Send(Encoding.Default.GetBytes("ERROR"));
                    }
                }
                buffer = new byte[8192];
                recieveBuffer = _socket.Receive(buffer, buffer.Length, 0);
                if (recieveBuffer < buffer.Length) { Array.Resize<byte>(ref buffer, recieveBuffer); }
                //check if there is a a message and answer it
                _userDetails = JsonConvert.DeserializeObject<DetailsFromUser>(Encoding.Default.GetString(buffer));
                if (_userDetails.UserName == null)
                {
                    ClientsTable.Instance.DeactivateUser(_userName);
                    Close();
                }

                if (ClientRecievedEvent != null) { ClientRecievedEvent(this, _userDetails.ToString()); }
                buffer = new byte[8192];
                recieveBuffer = _socket.Receive(buffer, buffer.Length, 0);
                if (recieveBuffer < buffer.Length) { Array.Resize<byte>(ref buffer, recieveBuffer); }
                String fileName = (Encoding.Default.GetString(buffer));
                if (ClientRecievedEvent != null) { ClientRecievedEvent(this, "search: " + fileName); }

                try
                {
                    if (ClientsTable.Instance.IsClientValid(_userDetails.UserName, _userDetails.Password))
                    {
                        List<MySearchResult> res = FilesTable.Instance.FindFile(fileName);
                        if (ClientRecievedEvent != null) { ClientRecievedEvent(this, "res: " + res); }
                        _socket.Send(Encoding.Default.GetBytes(JsonConvert.SerializeObject(res)));
                    }
                }
                catch
                {
                    _socket.Send(Encoding.Default.GetBytes(JsonConvert.SerializeObject(null)));
                }
                _socket.BeginReceive(new byte[] { 0 }, 0, 0, 0, FileCallBack, null);
            }
            catch { Close(); }

        }// end of function FileCallBack


        //method for recieving a connection
        void ConnectionCallBack(IAsyncResult ar)
        {
            try
            {
                _socket.EndReceive(ar);
                byte[] buf = new byte[8192];
                int recieveBuffer = _socket.Receive(buf, buf.Length, 0);
                if (recieveBuffer < buf.Length) { Array.Resize<byte>(ref buf, recieveBuffer); }
                //check if there is a a message and answer it
                var jsonObject = JsonConvert.DeserializeObject<DetailsFromUser>(Encoding.Default.GetString(buf));
                if (ClientRecievedEvent != null)
                {
                    ClientRecievedEvent(this, jsonObject.ToString());
                }
                try
                {
                    if (ClientsTable.Instance.IsClientValid(jsonObject.UserName, jsonObject.Password))
                    {
                        ClientsTable.Instance.ActivateUser(jsonObject.UserName, jsonObject.IpLocalHost, jsonObject.Files);
                        _socket.Send(Encoding.Default.GetBytes("OK"));
                    }
                }
                catch (Exception ex)
                {
                    _socket.Send(Encoding.Default.GetBytes(ex.Message.ToString()));
                }
                _socket.BeginReceive(new byte[] { 0 }, 0, 0, 0, ConnectionCallBack, null);

            }
            catch { Close(); }

        }// end of function ConnectionCallBack


        public void Close()
        {
            if (ClientDisconnectedEvent != null)
                ClientDisconnectedEvent(Id);
            _socket.Close();
            _socket.Dispose();
        }

    }//end of class Client
}//end of namespace 