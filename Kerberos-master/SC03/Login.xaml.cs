using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;
using Kerberos;
using System.IO;

namespace SC03
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        data d1 = new data();
        data d2 = new data();
        Socket c;
        private static string tkt_tgs;
        private static string key_c_tgs;
        public static string IDc;
        public static string pwd_c;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void button_click1(object sender, RoutedEventArgs e)
        {
            int port = int.Parse(textbox2.Text);
            string host = textbox1.Text;
            IPAddress ip = IPAddress.Parse(host);
            IPEndPoint ipe = new IPEndPoint(ip, port);//把ip和端口转化为IPEndPoint实例
            c = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建一个Socket
            try
            {
                c.Connect(ipe);//连接到服务器
                System.Windows.MessageBox.Show("连接AS服务器成功！");
                
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("连接超时！");
            }
        }
        private void button_click2(object sender, RoutedEventArgs e)// 
        {
            textbox5.Text = "正在向AS服务器发送消息...";
            //TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);            //获取时间
            //           StreamReader sr = new StreamReader(@"F:\编程记录\Kerberos-master\密钥.txt");
           //           string key = sr.ReadLine();
            if (send_msg(message.Enc_msg1(textbox3.Text, "TGS", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"))) == 1)
            {
                textbox5.AppendText("发送消息成功！");
            }
            IDc = textbox3.Text;
            pwd_c = textbox4.Password;
            rec_msg();
        }
        private void button_click4(object sender, RoutedEventArgs e)// 
        {
            System.Windows.MessageBox.Show(textbox3.Text);
            System.Windows.MessageBox.Show(textbox4.Password);
            if (send_msg(message.Enc_msg13(textbox3.Text,textbox4.Password)) == 1)
            {
                textbox5.AppendText("发送注册请求！");
            }
            rec_msg1();
        }
        private void rec_msg1()
        {
            try
            {
                byte[] buffer = new byte[1024 * 1024 * 3];
                //实际接收到的有效字节数
                int len = c.Receive(buffer);
                if (len != 0)
                {
                    textbox5.AppendText("注册成功！");
                } 
                else
                    textbox5.AppendText("注册失败！");
            }
            catch (Exception e)
            {
                textbox5.AppendText(e.ToString());
            }
        }
        private int send_msg(string str)
        {
            try
            {
                byte[] buffer = Encoding.ASCII.GetBytes(str);
                c.Send(buffer, buffer.Length,0);
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
                    textbox5.AppendText("未收到消息！");
                }
                string str2 = Encoding.ASCII.GetString(buffer, 0, len);
                textbox5.AppendText(str2);
             //   textbox5.AppendText(str2.Length.ToString());
                message g = message.s_to_m(str2);
           //     textbox5.AppendText(g.type.ToString());
      //          textbox5.AppendText(g.m_data);
                d1 = message.Dec_msg(g, "");
                key_c_tgs = d1.data2_key_c_tgs;
                tkt_tgs = d1.data2_tkt_tgs;
                textbox5.AppendText("\r\n");
                textbox5.AppendText(d1.data2_key_c_tgs);
                textbox5.AppendText("\r\n");
                textbox5.AppendText(d1.data2_IDtgs);
                textbox5.AppendText("\r\n");
                textbox5.AppendText(d1.data2_TS2);
                textbox5.AppendText("\r\n");
                textbox5.AppendText(d1.data2_Lifetime1);
                textbox5.AppendText("\r\n");
                textbox5.AppendText(d1.data2_tkt_tgs);
                textbox5.AppendText("\r\n");
                // this.Dispatcher.Invoke(new Action()=>{ textbox5.AppendText(d1.data2_IDtgs); });
                if (d1.data2_IDtgs == "TGS"&&(DateTime.Compare(DateTime.Parse(d1.data2_TS2).AddSeconds(Convert.ToInt32(d1.data2_Lifetime1)),DateTime.Now)>0))
                {
                    textbox5.AppendText("登陆成功！");
                    Window f1 = new Window1();
                    f1.Show();
                    //f1.ShowDialog();
                }
                else
                    textbox5.AppendText("登陆失败！");
            }
            catch(Exception e)
            {
                textbox5.AppendText(e.ToString());
            }
        }
        public string get_tkt_tgs()
        {
            return tkt_tgs;
        }
        public string get_key_c_tgs()
        {
            return key_c_tgs;
        }
        public  string get_IDc()
        {
            return IDc;
        }
        public  string get_pwd_c()
        {
            return pwd_c;
        }
        private void button_click3(object sender, RoutedEventArgs e)
        {
            c.Close();
            System.Windows.MessageBox.Show("已断开连接！");
            textbox5.Text = "";
        }
        
    }
}
