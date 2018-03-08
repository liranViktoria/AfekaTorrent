using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DetailsLibrary;

namespace DBLibrary
{
    public class ClientsTable
    {
        private static readonly ClientsTable instance = new ClientsTable();
        public  static          ClientsTable Instance { get { return instance; } }
        private ClientsTable() {}

        //register (add) a new user 
        public void RegisterUser(string newUserName, string password)
        {
            if (IsRegistered(newUserName))
            {
                using(AfekaMiniTorrent_DataBaseServerDataContext dataBase = new AfekaMiniTorrent_DataBaseServerDataContext())
                { 
                Client newClient = new Client();
                newClient.UserName = newUserName;
                newClient.Password = password;
                newClient.IsOn = 0;
                dataBase.Clients.InsertOnSubmit(newClient);
                dataBase.SubmitChanges();
                }
    }
            else
            {
                throw new Exception("User Name already exists!");
            }
        }

        //check if a certain user is already registered in system 
        public bool IsRegistered(string userName)
        {
            AfekaMiniTorrent_DataBaseServerDataContext dataBase = new AfekaMiniTorrent_DataBaseServerDataContext();
            int count = 0;
            foreach (var client in dataBase.Clients) { if (client.UserName.Equals(userName)) { count++; } }
            return (count == 0);
        }

        public bool IsClientValid(String userName, string password)
        {
            using (AfekaMiniTorrent_DataBaseServerDataContext dataBase = new AfekaMiniTorrent_DataBaseServerDataContext())
            { 
                if (IsRegistered(userName))
                {
                    foreach (var client in dataBase.Clients)
                    {
                        if (client.UserName == userName)
                        {
                            if (client.Password == password)
                            {
                                return true;
                            }
                            else
                            {
                                throw new Exception("Wrong password!");
                            }
                        }
                            
                    }
                }
                else { throw new Exception("There is no such user name!");}
            }
        return false;
    }

        //print all peers
        public String PrintAccounts()
        {
            String all = "";
            int count = 0;
            using (AfekaMiniTorrent_DataBaseServerDataContext dataBase = new AfekaMiniTorrent_DataBaseServerDataContext())
            {
                foreach (var client in dataBase.Clients)
                {
                    count++;
                    String isActive = "Active Now";
                    if (client.IsOn == 0) { isActive = "Not Active Now"; }
                    all += "#" + count + ":: User Name: " + client.UserName + ", Password: " + client.Password + ", " + isActive +"\n";
                }
            }
            return all;
        }

        //return a list of all clients
        public List<Client> GetAllClients()
        {
            List<Client> clientsList = new List<Client>();
            using(AfekaMiniTorrent_DataBaseServerDataContext dataBase = new AfekaMiniTorrent_DataBaseServerDataContext())
            {
                foreach (var client in dataBase.Clients)
                {
                    clientsList.Add(client);
                }
            }
            return clientsList;
        }

        //return a list of all active clients
        public List<Client> GetActiveClients()
        {
            List<Client> activeClientsList = new List<Client>();
            using (AfekaMiniTorrent_DataBaseServerDataContext dataBase = new AfekaMiniTorrent_DataBaseServerDataContext())
            {
                foreach (var client in dataBase.Clients)
                {
                    if(client.IsOn == 1) { activeClientsList.Add(client);  }
                }
            }
            return activeClientsList;
        }

        public int GetNumOfAllClients() { return GetAllClients().Count(); }
        public int GetNumOfActiveClients() { return GetActiveClients().Count(); }

        public void ActivateUser(string userName, String ip, List<FileDetails> userFiles)
        {
            using (AfekaMiniTorrent_DataBaseServerDataContext dataBase = new AfekaMiniTorrent_DataBaseServerDataContext())
            {
                dataBase.Clients.Single(c => c.UserName == userName).IsOn = 1;
                dataBase.SubmitChanges();
            }
            FilesTable.Instance.AddFiles(ip, userName, userFiles);
        }

        public void DeactivateUser(string userName)
        {
            using (AfekaMiniTorrent_DataBaseServerDataContext dataBase = new AfekaMiniTorrent_DataBaseServerDataContext())
            {
                dataBase.Clients.Single(c => c.UserName == userName).IsOn = 0;
                dataBase.SubmitChanges();
            }
            FilesTable.Instance.DeleteFiles(userName);
        }






    }//end of class ClientsTable

}//end of namespace 
