using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApplication
{


    public delegate void UploadHandler(MyUpload myUpload);
    public delegate void DownloadHandler(MyDownload myDownload);

    [AttributeUsage(AttributeTargets.Class |
                    AttributeTargets.Struct)]
    public class AuthorAttribute : Attribute
    {
        private string name;
        public double version;
        public string Name { get { return name; } }

        public AuthorAttribute(string name)
        {
            this.name = name;
            version = 1.0;
        }
    }
    public class MyDownload
    {
        public List<String> Ips { get; set; }
        public string FileName { get; set; }
        public int Size { get; set; }
        public string Status { get; set; }
        public int Kbps { get; set; }
        public TimeSpan TimeTaken { get; set; }
        public DateTime StartTime { get; set; }
    }

    public class MyUpload
    {
        public string IP { get; set; }
        public string FileName { get; set; }
        public int Size { get; set; }
        public string Status { get; set; }
    }





    class Stuff
    {

  
    }
}
