
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;
using DetailsLibrary;
using Newtonsoft.Json;
using WpfApplication;

namespace ClientApplication
{

  
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        public static String FILE_NAME = "MyConfig.xml";
        private String uploadPath = null;
        private String downloadPath = null;
        private DetailsFromUser details = new DetailsFromUser();
        private XmlSerializer serializerObj;
        private Socket s ;
       

        public MainWindow()
        {//
            try
            {
            InitializeComponent();
            Hide();
            s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s.Connect("10.0.0.32", 8085);

                if (File.Exists(FILE_NAME))
                {
                    GetDetailsFromXmlFile();
                    details.IpLocalHost = GetIPAddress().ToString();


                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    if (!Directory.Exists(downloadPath))
                    {
                        Directory.CreateDirectory(downloadPath);
                    }
                    GetFiles();
                    s.Send(Encoding.Default.GetBytes(JsonConvert.SerializeObject(details)));

                    byte[] buf = new byte[1024];

                    int rec = s.Receive(buf, buf.Length, 0);
                    if (rec < buf.Length)
                    {
                        Array.Resize<byte>(ref buf, rec);
                    }
                    String msg = Encoding.Default.GetString(buf);
                  
                    CreateConfigFille();
                    new MainWindowFiles().Show();
                     Close();
                    
      
                }
                else
                    Show();
            }
            catch (Exception ex)
            {
                Show();
                MessageBox.Show(ex.Message.ToString());
            }




        }//


        private void CreateConfigFille()
        {
            XmlWriterSettings xmlSetings = new XmlWriterSettings();
            xmlSetings.Indent = true;
            xmlSetings.IndentChars = "\t";
            using (XmlWriter xmlWriter = XmlWriter.Create(@FILE_NAME, xmlSetings))
            {

                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("Client");

                xmlWriter.WriteStartElement("UserName");
                xmlWriter.WriteString(details.UserName);
                xmlWriter.WriteEndElement();//UserName

                xmlWriter.WriteStartElement("Password");
                xmlWriter.WriteString(details.Password);
                xmlWriter.WriteEndElement();//Password

                xmlWriter.WriteStartElement("Ip");
                xmlWriter.WriteString(details.IpLocalHost);
                xmlWriter.WriteEndElement();//Ip

                xmlWriter.WriteStartElement("UploadFolder");
                xmlWriter.WriteString(uploadPath);
                xmlWriter.WriteEndElement();//UploadFolder

                xmlWriter.WriteStartElement("DownloadFolder");
                xmlWriter.WriteString(downloadPath);
                xmlWriter.WriteEndElement();//DownloadFolder

                xmlWriter.WriteStartElement("PortIn");
                xmlWriter.WriteString("8005");
                xmlWriter.WriteEndElement();//PortIn

                xmlWriter.WriteStartElement("PortOut");
                xmlWriter.WriteString("8006");
                xmlWriter.WriteEndElement();//PortOut

                xmlWriter.WriteEndDocument();//Doc
                xmlWriter.Close();
            }
        }



        private void GetFiles()
        {
            var myFiles = new DirectoryInfo(uploadPath).GetFiles();

           // details.ClearFiles();
            for (int i = 0; i < myFiles.Length; i++)
            {
                details.AddFile((myFiles[i].Name), (myFiles[i].Length));
              
            }

        }


        public static IPAddress GetIPAddress()
        {
            IPAddress[] hostAddresses = Dns.GetHostAddresses("");

            foreach (IPAddress hostAddress in hostAddresses)
            {
                if (hostAddress.AddressFamily == AddressFamily.InterNetwork &&
                    !IPAddress.IsLoopback(hostAddress) &&  // ignore loopback addresses
                    !hostAddress.ToString().StartsWith("169.254."))  // ignore link-local addresses
                    return hostAddress;
            }
            return null;
        }

        private void SaveToFile()
        {
            using(var srFileStream = new FileStream(FILE_NAME, FileMode.Create))
            {
                serializerObj = new XmlSerializer(typeof(DetailsFromUser));
                serializerObj.Serialize(srFileStream, details);

            }
        }

        private void button_path_upload_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult unused = dialog.ShowDialog();
                upload_path_texbox.Text = dialog.SelectedPath;
                success_upload.Visibility = Visibility.Visible;
            }
        }

        private void button_path_download_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.ShowDialog();
                download_path_texbox.Text = dialog.SelectedPath;
                success_download.Visibility = Visibility.Visible;
            }
        }

        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            details.UserName = user_name_texbox.Text;
            details.Password = password_box.Password;
            details.PathDownload = download_path_texbox.Text;
            details.PathUpload = upload_path_texbox.Text;
            details.PortIn = 8005;
            try
            {
                InputChacker(details.UserName, details.Password,details.PathDownload,details.PathUpload);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Confirmation", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            details.IpLocalHost = GetIpAddress().ToString();
            GetFilesFromPath();
            SaveToFile();




        }

        private void InputChacker(string userNameCheck, string passwordCheck, string pathDownload, string pathUpload)
        {
            if(userNameCheck == "" && passwordCheck == "")
                throw new Exception("You must enter username and password before uploading...");
            else if (userNameCheck == "")
                throw new Exception("You must enter username before uploading...");
            else if( passwordCheck == "")
                throw  new Exception("You must enter password before uploading...");
            else if(pathDownload == "" && pathUpload == "")
                throw new Exception("You must choose paths...");
            else if(pathDownload == "")
                throw new Exception("You must choose download path...");
            else if(pathUpload == "")
                throw new Exception("You must choose upload path...");
        }

        public static IPAddress GetIpAddress()
        {
            IPAddress[] hostAddresses = Dns.GetHostAddresses("");
            foreach (IPAddress hostAddress in hostAddresses)
            {
                if (hostAddress.AddressFamily == AddressFamily.InterNetwork &&
                    !IPAddress.IsLoopback(hostAddress) &&  !hostAddress.ToString().StartsWith("169.254."))  
                    return hostAddress;
            }
            return null;
        }

        public void GetFilesFromPath()
        {
            var files = new DirectoryInfo(details.PathUpload).GetFiles();
            for (int i = 0; i < files.Length; i++)
            {
              details.AddFile(files[i].Name,files[i].Length);
            }



        }

        private void GetDetailsFromXmlFile()
        {
            XmlTextReader xmlReader = new XmlTextReader(FILE_NAME);

            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    switch (xmlReader.Name)
                    {
                        case "UserName":
                            xmlReader.Read();
                            details.UserName = xmlReader.Value;
                            break;
                        case "Password":
                            xmlReader.Read();
                            details.Password = xmlReader.Value;
                            break;
                        case "Ip":
                            xmlReader.Read();
                            details.IpLocalHost = xmlReader.Value;
                            break;
                        case "UploadFolder":
                            xmlReader.Read();
                            uploadPath = xmlReader.Value;
                            break;
                        case "DownloadFolder":
                            xmlReader.Read();
                            downloadPath = xmlReader.Value;
                            break;
                        case "PortIn":
                            xmlReader.Read();
                            details.PortIn = int.Parse(xmlReader.Value);
                            break;
                        case "PortOut":
                            xmlReader.Read();
                            details.PortOut = int.Parse(xmlReader.Value);
                            break;
                        default:
                            break;
                    }
                }
            }
            xmlReader.Close();

        }


    }
}

