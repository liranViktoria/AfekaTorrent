using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetailsLibrary
{
    public class MySearches
    {
        private List<String> ipAddresses;

        public MySearches()
        {
            ipAddresses = new List<string>();
        }
        public MySearches(String fileName, long size, List<String> ips)
        { 
            FileName = fileName;
            Size = size;
            this.ipAddresses = ips;
        }

        public string FileName { get; set; }
        public long Size { get; set; }
        public List<String> Ips { get { return ipAddresses; } }
        public void AddIp(String ip)
        {
            ipAddresses.Add(ip);
        }
        public void ClearIps()
        {
            ipAddresses.Clear();
        }
        public override string ToString()
        {
            string s = "file name " + FileName + " size " + Size + "\nips:\n";
            foreach (string ip in ipAddresses)
                s += ip + "\n";
            return s;


        }

    }
}
