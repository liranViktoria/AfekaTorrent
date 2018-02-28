using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Documents;

namespace WpfApplication
{
    
     public  class DetailsFromUser : IDataErrorInfo
    {
        private List<FileDetails> files;
        private List<FileDetails> Files { get { return files; } }
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
        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string this[string columnName]
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
