using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using System.Xml;
using ClientApplication;
using DetailsLibrary;

namespace WpfApplication
{


    public partial class MainWindowFiles 
    {
        private Socket s;
        private DetailsFromUser details = new DetailsFromUser();
        private string downloadPath;
        private List<MySearchResult> searchResults = new List<MySearchResult>();
        private List<MyUpload> uploads = new List<MyUpload>();
        private List<MyDownload> downloads = new List<MyDownload>();


        public MainWindowFiles()
        {
            InitializeComponent();

         
            GetDetailsFromXmlFile();
            s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s.Connect("10.0.0.32", 8080);
            Thread.Sleep(1000);
            try
            {
                s.Send(Encoding.Default.GetBytes(JsonConvert.SerializeObject(details)));

                byte[] buf = new byte[1024];

                int rec = s.Receive(buf, buf.Length, 0);
                if (rec < buf.Length)
                {
                    Array.Resize<byte>(ref buf, rec);
                }

                if (Encoding.Default.GetString(buf) == "ERROR")
                {
                    MessageBox.Show("Something wrong here...");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            // Binding listviews to their data
            listViewUpload.ItemsSource = uploads;
            listViewSearchResult.ItemsSource = searchResults;
            listViewDownload.ItemsSource = downloads;



            // Start listening to upload requests from other clients
            Task.Factory.StartNew(() =>
            {
                UploadFolder u = new UploadFolder();
                u.UploadStartedEvent += NewUploadStarted;
                u.UploadFinishedEvent += UploadFinished;
                u.TCPListen();
            });
        }

        private void NewUploadStarted(MyUpload myUpload)
        {
            Dispatcher.BeginInvoke(new Action(delegate ()
            {
                uploads.Add(myUpload);
                listViewUpload.Items.Refresh();
            }));

        }

        private void UploadFinished(MyUpload myUpload)
        {
            Dispatcher.BeginInvoke(new Action(delegate ()
            {
                myUpload.Status = "Finished";
                listViewUpload.Items.Refresh();
            }));

        }

        private void NewDownloadStarted(MyDownload myDownload)
        {
            Dispatcher.BeginInvoke(new Action(delegate ()
            {
                myDownload.StartTime = DateTime.Now;
                downloads.Add(myDownload);
                listViewDownload.Items.Refresh();
            }));
        }

        private void DownloadFinished(MyDownload myDownload)
        {
            Dispatcher.BeginInvoke(new Action(delegate ()
            {
                myDownload.TimeTaken = DateTime.Now - myDownload.StartTime;
                myDownload.Kbps = (int)((myDownload.Size / 1000) / myDownload.TimeTaken.TotalSeconds);
                myDownload.Status = "Finished";
                listViewDownload.Items.Refresh();
            }));

        }

        private void DownloadFile(List<string> list, string filename, int size)
        {
            Task.Factory.StartNew(() =>
            {
                DownloadFolder d = new DownloadFolder(list, filename, size);
                d.DownloadFinishedEvent += DownloadFinished;
                d.DownloadStartedEvent += NewDownloadStarted;
                d.Start();
            });
        }

        private void GetDetailsFromXmlFile()
        {
            XmlTextReader xmlReader = new XmlTextReader(MainWindow.FILE_NAME);

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
                        case "DownloadFolder":
                            xmlReader.Read();
                            downloadPath = xmlReader.Value;
                            xmlReader.Close();
                            return;

                        default:
                            break;
                    }
                }
            }
            xmlReader.Close();

        }


        private void log_out_button_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(MainWindow.FILE_NAME))
            {
                File.Delete(MainWindow.FILE_NAME);
            }
            new MainWindow();
            Close();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String nameFileSearchText = textBox_search.Text;
                if (nameFileSearchText == "")
                {
                    MessageBox.Show("You must enter name!");
                    return;
                }
                GetDetailsFromXmlFile();


                s.Send(Encoding.Default.GetBytes(JsonConvert.SerializeObject(details)));
                Thread.Sleep(1000);
                s.Send(Encoding.Default.GetBytes(nameFileSearchText));

                byte[] buf = new byte[1024];

                int rec = s.Receive(buf, buf.Length, 0);
                if (rec < buf.Length)
                {
                    Array.Resize<byte>(ref buf, rec);
                }
                try
                {
                    List<MySearchResult> res = JsonConvert.DeserializeObject<List<MySearchResult>>(Encoding.Default.GetString(buf));
                    searchResults.Clear();
                    foreach (MySearchResult msr in res)
                    {
                        searchResults.Add(msr);
                        //MessageBox.Show(msr.ToString());
                    }
                    listViewSearchResult.Items.Refresh();
                }
                catch
                {
                    MessageBox.Show("Not found!");
                    return;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }    try
            {
                String fileName = textBox_search.Text;
                if (fileName == "")
                {
                    MessageBox.Show("Enter a file name!");
                    return;
                }
                GetDetailsFromXmlFile();


                s.Send(Encoding.Default.GetBytes(JsonConvert.SerializeObject(details)));
                Thread.Sleep(1000);
                s.Send(Encoding.Default.GetBytes(fileName));

                byte[] buf = new byte[1024];

                int rec = s.Receive(buf, buf.Length, 0);
                if (rec < buf.Length)
                {
                    Array.Resize<byte>(ref buf, rec);
                }
                try
                {
                    List<MySearchResult> res = JsonConvert.DeserializeObject<List<MySearchResult>>(Encoding.Default.GetString(buf));
                    searchResults.Clear();
                    foreach (MySearchResult msr in res)
                    { 
                        searchResults.Add(msr);
                        //MessageBox.Show(msr.ToString());
                    }
                    listViewSearchResult.Items.Refresh();
                }
                catch
                {
                    MessageBox.Show(fileName + "does not exiest");
                    return;
                }

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void listViewSearchResult_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Call downloadFile with selected row data
            MySearchResult msr = (MySearchResult)listViewSearchResult.SelectedItem;
            DownloadFile(msr.Ips, msr.FileName, (int)msr.Size);
        }

        private void listViewUpload_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

   

        private void listViewDownload_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MyDownload md = (MyDownload)listViewDownload.SelectedItem;
            // Check if finished
            if (!md.Status.Equals("Finished")) return;
            // Check if dll
            if (!md.FileName.EndsWith(".dll")) return;

            // Load dll from download folder 

            Assembly a = Assembly.LoadFrom(downloadPath + "\\" + md.FileName);
            Type[] allTypes = a.GetTypes();
            foreach (Type t in allTypes)
            {
                AuthorAttribute author = (AuthorAttribute)Attribute.GetCustomAttribute(t, typeof(AuthorAttribute));

                if (author == null)
                {
                    MessageBox.Show("The attribute in this .dll file is not supported. Must be of type AuthorAttribute", "Unknown attribute");
                }
                else
                {
                    if (author.Name.Equals("Yael"))
                    {
                        MethodInfo m = t.GetMethod("Panic");
                        object obj = Activator.CreateInstance(t);

                        object[] objects = { 42 };
                        m.Invoke(obj, objects);
                    }
                    else if (author.Name.Equals("Daniel"))
                    {
                        MethodInfo m = t.GetMethod("GoOnVacation");
                        object obj = Activator.CreateInstance(t);

                        object[] objects = { "Colorado" };
                        m.Invoke(obj, objects);
                    }
                    else if (author.Name.Equals("Cookie"))
                    {
                        MethodInfo m = t.GetMethod("DigInTheYard");
                        object obj = Activator.CreateInstance(t);
                        m.Invoke(obj, null);
                    }
                    else
                    {
                        MessageBox.Show("Class written by unknown author and will not be executed", "Unknown Author");
                    }

                }
            }
        }

        private void listViewUpload_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void listViewDownload_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void listView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void textBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}
