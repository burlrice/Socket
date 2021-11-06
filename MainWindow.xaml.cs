using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;

namespace Socket
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    { 
        public MainWindow()
        {
            var split = System.Reflection.Assembly.GetEntryAssembly().Location.Split('\\')
                .TakeWhile(s => string.Compare(s, "bin") != 0).ToList()
                .Union(new List<string>() { "7819996.xml" });

            InitializeComponent();
            File.Text = string.Join('\\', split);
            LoadFile(File.Text);
        }

        void LoadFile(string file)
        {
            try
            {
                Data.Text = System.IO.File.ReadAllText(file);
            }
            catch (Exception e)
            {
                ButtonAutomationPeer peer = new ButtonAutomationPeer(Browse);
                IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                invokeProv.Invoke();
            }
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var addr = IPAddress.Parse(IpAddress.Text);
                byte[] bytes = new byte[1024];
                using (var socket = new System.Net.Sockets.Socket(SocketType.Stream, ProtocolType.Tcp))
                {
                    socket.Connect(new IPEndPoint(addr, 2202));
                    socket.Send(Encoding.ASCII.GetBytes(Data.Text));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception");
            }
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();

            openFileDialog.InitialDirectory = string.Join('\\', System.Reflection.Assembly.GetEntryAssembly().Location.Split('\\')
                .TakeWhile(s => string.Compare(s, "bin") != 0));

            openFileDialog.Filter = "All files (*.*)|*.*|XML files (*.xml)|*.xml";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == true) 
            {
                LoadFile(openFileDialog.FileName);
            }
        }
    }
}
