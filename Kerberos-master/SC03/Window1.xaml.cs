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
using SC03;

namespace Kerberos
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }
        Socket c;
        MainWindow w = new MainWindow();
        data d2 = new data();
        private static string key_c_v;
        private static string tkt_v;
        private static string TS5;
        private void button_click1(object sender, RoutedEventArgs e)
        {
            //           MainWindow w = new MainWindow();
            textbox1.Text = w.get_key_c_tgs();
            int port = int.Parse(textbox3.Text);
            string host = textbox2.Text;
            IPAddress ip = IPAddress.Parse(host);
            IPEndPoint ipe = new IPEndPoint(ip, port);//把ip和端口转化为IPEndPoint实例
            c = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建一个Socket
            try
            {
                c.Connect(ipe);//连接到服务器
                System.Windows.MessageBox.Show("连接TGS服务器成功！");
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("连接超时！");
            }
        }

        private void button_click2(object sender, RoutedEventArgs e)//生成Aut
        {
            TS5 = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            System.Windows.MessageBox.Show(w.get_IDc());
            string str = w.get_IDc() +"####"+ GetLocalIP()+"####" + TS5+"   ";
            textbox4.Text = des.EncryptString(str, w.get_key_c_tgs());
            System.Windows.MessageBox.Show(GetLocalIP());
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
                        string ip = "";
                        ip = IpEntry.AddressList[i].ToString();
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

        private void button_click3(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show(textbox1.Text);
            if (send_msg(message.Enc_msg3("SER", w.get_tkt_tgs(), textbox4.Text)) == 1)
            {
                System.Windows.MessageBox.Show("发送消息成功！");
            }
            rec_msg();
        }
        private int send_msg(string str)
        {
            try
            {
                byte[] buffer = Encoding.ASCII.GetBytes(str);
                textbox5.Text = str;
                c.Send(buffer, buffer.Length, 0);
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
                int len = c.Receive(buffer);
                if (len == 0)
                {
                    textbox6.AppendText("未收到消息！");
                }
                string str = Encoding.ASCII.GetString(buffer, 0, len);
                textbox6.AppendText(str);
                
                d2 = message.Dec_msg(message.s_to_m(str),textbox1.Text);
                key_c_v = d2.data4_key_c_v;
                tkt_v = d2.data4_tkt_v;
                textbox6.AppendText("\r\n");
                textbox6.AppendText(d2.data4_key_c_v);
                textbox6.AppendText("\r\n");
                textbox6.AppendText(d2.data4_IDv);
                textbox6.AppendText("\r\n");
                textbox6.AppendText(d2.data4_TS4);
                textbox6.AppendText("\r\n");
                textbox6.AppendText(d2.data4_tkt_v);
                textbox6.AppendText("\r\n");
            }catch(Exception e)
            {
                textbox6.AppendText(e.ToString());
            }
            if (d2.data4_IDv == "SER")
            {
                textbox6.AppendText("验证成功！");
                Window f2 = new Window2();
                f2.Show();
            }
            else
                textbox6.AppendText("验证失败！");
        }
        private void button_click4(object sender, RoutedEventArgs e)
        {
            c.Close();
            System.Windows.MessageBox.Show("已断开连接！");
            textbox6.Text = "";
        }
        public string get_key_c_v()
        {
            return key_c_v;
        }
        public string get_tkt_v()
        {
            return tkt_v;
        }
        public string get_TS5()
        {
            return TS5;
        }
    }
}
