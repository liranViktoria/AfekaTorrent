
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Xml.Serialization;
using Newtonsoft.Json;
 
namespace WpfApplication
{

  
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        public static String fileName = "MyConfig.xml";
        private DetailsFromUser details = new DetailsFromUser();
        private XmlSerializer serializerObj;
        private Socket s ;
       

        public MainWindow()
        {
            InitializeComponent();

            s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
           
            s.Connect("127.0.0.1", 8005);
        }

        private void SaveToFile()
        {
            using(var srFileStream = new FileStream(fileName, FileMode.Create))
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


    }
}

