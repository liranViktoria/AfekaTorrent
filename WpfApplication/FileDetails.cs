
namespace WpfApplication
{
    class FileDetails
    {
        public FileDetails(string name, long numberBytes)
        {
            NameFile = name;
            NumberOfBytesOfFiles = numberBytes;
        }
        public string NameFile { set; get; }
        public long NumberOfBytesOfFiles { set; get; }
    }
}
