using SC03;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Kerberos
{
    /// <summary>
    /// Window2.xaml 的交互逻辑
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
        }
        public class lib
        {
            public static Socket c;
        }
        data d2 = new data();
 //       Socket c;
        Window1 w = new Window1();
        MainWindow w1 = new MainWindow();
        private void button_click1(object sender, RoutedEventArgs e)
        {
            int port = int.Parse(textbox2.Text);
            string host = textbox1.Text;
            IPAddress ip = IPAddress.Parse(host);
            IPEndPoint ipe = new IPEndPoint(ip, port);//把ip和端口转化为IPEndPoint实例
            lib.c = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建一个Socket
            try
            {
                lib.c.Connect(ipe);//连接到服务器
                System.Windows.MessageBox.Show("连接应用服务器成功！");
                textbox3.Text = w.get_key_c_v();
                textbox5.Text = w.get_tkt_v();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("连接超时！");
            }

        }
        private void button_click3(object sender, RoutedEventArgs e)//生成Aut
        {
            string str = w1.get_IDc() +"####"+ GetLocalIP() + "####"+DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            textbox6.Text = des.EncryptString(str, textbox3.Text);
        }
        private string GetLocalIP()
        {
            try
            {
                string HostName = Dns.GetHostName(); //得到主机名
                IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                for (int i = 0; i < IpEntry.AddressList.Length; i++)
                {
                    //从IP地址列表中筛选出IPv4类型的IP地址
                    //AddressFamily.InterNetwork表示此IP为IPv4,
                    //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                    if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        return IpEntry.AddressList[i].ToString();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        private void button_click2(object sender, RoutedEventArgs e)
        {
            if (send_msg(message.Enc_msg5( textbox5.Text, textbox6.Text)) == 1)
            {
                System.Windows.MessageBox.Show("发送消息成功！");
            }
            rec_msg();
        }
        private int send_msg(string str)
        {
            try
            {
                byte[] buffer = Encoding.Unicode.GetBytes(str);
                textbox4.Text = str;
                lib.c.Send(buffer, buffer.Length, 0);
                return 1;
            }
            catch
            {
                System.Windows.MessageBox.Show("发送失败！");
                return 0;
            }
        }
        private void rec_msg()
        {
            try
            {
                byte[] buffer = new byte[1024 * 1024 * 3];
                //实际接收到的有效字节数
                int len = lib.c.Receive(buffer);
                if (len > 0)
                {
                    textbox7.AppendText("验证成功！");
                    Window f2 = new Window3();
                    f2.ShowDialog();
                }  
                else
                    textbox7.AppendText("验证失败！");
                  
            }
            catch
            {
                textbox7.AppendText("验证失败！");
            }
            //    System.Windows.MessageBox.Show(w.get_TS5());
            //    System.Windows.MessageBox.Show(DateTime.Parse(w.get_TS5()).AddSeconds(1).ToString("yyyy/MM/dd HH:mm:ss"));
   
        }
    }
}
