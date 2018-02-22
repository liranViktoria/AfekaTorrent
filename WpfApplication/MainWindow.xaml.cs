
using System.Net.Sockets;
using System.Windows;
namespace WpfApplication
{

  
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        private Socket s;
        private DetailsFromUser details;

        public MainWindow()
        {
            
            InitializeComponent();
            details.UserName = user_name_texbox.Text;
            details.Password = password_box.Password;
            s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s.Connect("127.0.0.1", 8080);
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
    }
}

