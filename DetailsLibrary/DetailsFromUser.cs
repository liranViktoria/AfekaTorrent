using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace DetailsLibrary
{
    
     public  class DetailsFromUser 
    {
        public List<FileDetails> files;
        public List<FileDetails> Files { get { return files; } }
        public string UserName { set; get; }
        public string Password { set; get; }
        public string PathDownload { set; get; }
        public string PathUpload { set; get; }
        public string IpLocalHost { set; get; }
        public int PortIn { set; get; }
        public int PortOut { set; get; }

        public DetailsFromUser()
        {
            files = new List<FileDetails>();  
        }
        public void AddFile(string name, long numOfBytes)
        {
            files.Add(new FileDetails(name, numOfBytes));
        }

     

    }



    public class MySearchResult
    {
        private List<String> ips;

        public MySearchResult()
        {
            ips = new List<string>();
        }
        public MySearchResult(String fileName, long size, List<String> ips)
        {
            FileName = fileName;
            Size = size;
            this.ips = ips;
        }

        public string FileName { get; set; }
        public long Size { get; set; }
        public List<String> Ips { get { return ips; } }
        public void AddIp(String ip)
        {
            ips.Add(ip);
        }
        public void ClearIps()
        {
            ips.Clear();
        }
        public override string ToString()
        {
            string s = "file name " + FileName + " size " + Size + "\nips:\n";
            foreach (string ip in ips)
                s += ip + "\n";
            return s;


        }

    }
}
