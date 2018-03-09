using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetailsLibrary
{
    public class FileDetails
    {
        public string NameFile { set; get; }
        public long NumberOfBytesOfFiles { set; get; }
        public FileDetails(string name, long numberBytes)
        {
            NameFile = name;
            NumberOfBytesOfFiles = numberBytes;
        }
       
    }
}
