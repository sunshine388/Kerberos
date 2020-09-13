using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;

namespace SC03
{
    /// <summary>
    /// TGS.xaml 的交互逻辑
    /// </summary>
    public partial class TGS : Window
    {
        private Socket connection;
        private TcpListener listener;
        private IPAddress ip;
        private Int32 port;
        public string Time = DateTime.Now.ToString("yyyy/MM/dd HH：mm：ss");
        private static byte[] result = new byte[1000];
        public string str_privatekey = @"<RSAKeyValue><Modulus>t3YIHdHMuBrhFIVhv1iuwMkY5SdzHWpmSVo6l5w3KBxF0x2dmGBI8Rg5JNZyDA1e2a5gk45tK2YTaPqFQmgsu0lOTs8/86bozm
LEUYp0+PW61P0NLbPeti2SHv9tFy6mUwItc/1hfal8PGdMqEomBpuwKZFo+RHbdlCZrxPXofk=</Modulus><Exponent>AQAB</Exponent><P>6wGMAsdE
v97Ibtfa/n4XmaoSCuJGtj9YdrWAp3jwhdbvK6UNQbLa3lk+Obxn4ha76NzTOb03FSDNptFZxDK39w==</P><Q>x9muQl7Am7kt6wOMs7uRkG+jKaVIY+WFa
b4DF1qZvjBOEPXFhHq5Lxj62lMNHZheT6VI+s+fKUq93Dh/u2JZjw==</Q><DP>ObbcisYjdTeiY85xQvT2mM9ytOeTVCjEcgrrOLXz4/rALCjR06O4KskiB
g3mOVk59ttcqJ52i6LNvr1FXXDTSw==</DP><DQ>H+pRe+Pp2nSOMrPUaneP3YMWiXYNw3ZKAgMIN1iOqOLJ9MdrDacTn8Pbe7Y7p/hHR2zb5MH3Zv66p0qW
i3Q/XQ==</DQ><InverseQ>NKKMdh0ZWOgFIR0F9Se5VkVhFBsbhXW7Fs7SkSJeLKIpgGQVuq8KzJLXUgRuXfhYlN8Hv1EJbM8p5+Ez4WSOYQ==</Inverse
Q><D>fvhcY67ovV40+ovZe6F3yIslBKuJ2wjhY9DjjjwaXFM87ACqKmQw91MpcVpNMhQq2gL6agT/fzK1Kkl/4tbQZV0/pi5yBd7wQ6Pe9sClQ/ONPr774yl
HWvD8xrwtyT4z/dBwS801y28n6uc4CwB1T+LQY4UqAZxN77Z23n0umGU=</D></RSAKeyValue>";
        RSAHelper Y = new RSAHelper();
        public TGS()
        {
            string host = GetLocalIP();
            IPAddress ip = IPAddress.Parse(host);
            InitializeComponent();
            this.ip = ip;
            this.port = 9001;
        }
        //侦听客户连接请求
        public void runAs()
        {

            while (true)
            {

                this.Dispatcher.Invoke(new Action(() => { TextBox1.AppendText("Waiting for Connection\r\n"); }));
                connection = listener.AcceptSocket();
                //在新线程中启动新的socket连接，每个socket等待，并保持连接

                IPEndPoint iprm = (IPEndPoint)connection.RemoteEndPoint;
                this.Dispatcher.Invoke(new Action(() => { TextBox1.AppendText("远程主机:" + iprm.Address.ToString() + ":" + iprm.Port.ToString() + "连接上本机\r\n"); }));
                Thread thread = new Thread(new ThreadStart(dealClient));
                Thread myThread = new Thread(dealClient);
                thread.Start();

            }
        }

        //和客户端对话
        private void dealClient()
        {
            Socket connection = this.connection;
            IPEndPoint iprm = (IPEndPoint)connection.RemoteEndPoint;
            this.Dispatcher.Invoke(new Action(() => { TextBox3.AppendText("准备接受消息！\n"); }));
            Thread receiveThread = new Thread(ReceiveMessage);
            receiveThread.Start(connection);
        }

        //处理客户端发过来的消息。
        public Message DealMsg1(string msg1)
        {
            Message a = new Message();
            int y = Time.Length;
            string type;
            string pwd;
            string tag;

            string idv;
            string tgt;
            string aut;
            
            type = msg1.Substring(0, 2);
            tag = msg1.Substring(6, 2);
            pwd = msg1.Substring(2, 4);
            if (type == "03")
            {
                var index = msg1.IndexOf("#");
                idv = msg1.Substring(8, 3);
                tgt = msg1.Substring(11, index-11);
                aut = msg1.Substring(index+4, msg1.Length-index-4);
                int t = 3;
                a = new Message(idv, tgt, aut, t);
            }
            return a;

        }
        public void ReceiveMessage(object ClientSocket)
        {
            Socket myClientSocket = (Socket)ClientSocket;
            IPEndPoint iprm = (IPEndPoint)connection.RemoteEndPoint;
            while (true)
            {
                try
                {
                    //通过clientsocket接收数据
                    int num = myClientSocket.Receive(result);
                    //System.Windows.MessageBox.Show(Encoding.ASCII.GetString(result, 0, num));
                    //TB_recv_1.Text = "sss";
                    this.Dispatcher.Invoke(new Action(() => { TextBox3.AppendText(Encoding.ASCII.GetString(result, 0, num)); }));
                    Thread sendThread = new Thread(SendMessage);
                    sendThread.Start(myClientSocket);
                    // Message c = DealMsg1(Encoding.ASCII.GetString(result));
                    //c.msg2_tkt_ADc = iprm.Address.ToString();
                    //this.Dispatcher.Invoke(new Action(() => { TextBox2.AppendText(c.MMessage(c)); }));
                    //string ssmg;
                    // ssmg=c.ssMessage(c);
                    //this.Dispatcher.Invoke(new Action(() => { TextBox4.AppendText(ssmg); }));
                    // Byte[] ssmg1 = new byte[1024];
                    // ssmg1=Encoding.ASCII.GetBytes(ssmg);
                    // int num1= myClientSocket.Send(ssmg1);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    //this.Dispatcher.Invoke(new Action(() => { TextBox3.AppendText("已断开连接"); }));
                    //myClientSocket.Shutdown(SocketShutdown.Both);
                    // myClientSocket.Close();
                    break;
                }
            }
        }
        public void SendMessage(object clientSocket)
        {

            Socket myClientSocket = (Socket)clientSocket;
            IPEndPoint iprm = (IPEndPoint)connection.RemoteEndPoint;
            Message c = DealMsg1(Encoding.ASCII.GetString(result));
            //Message b = new Message();
            if (c.type == "03")
            {
                // this.Dispatcher.Invoke(new Action(() => { TextBox2.AppendText(c.msg3_tgt); }));
                c.dealMsg(c);
                c.msg4_key = Y.GetRandomString(8);
                c.msg4_tkt_ADc = iprm.Address.ToString();
                c.msg4_ts4 = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                this.Dispatcher.Invoke(new Action(() => { TextBox2.AppendText(c.MMessage4(c)); }));
                string ssmg;
                ssmg = c.ssMessage4(c);
                this.Dispatcher.Invoke(new Action(() => { TextBox4.AppendText(ssmg); }));
                Byte[] ssmg3 = new byte[1024];
                ssmg3 = Encoding.ASCII.GetBytes(ssmg);
                myClientSocket.Send(ssmg3, ssmg3.Length, 0);
                // myClientSocket.Close();
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            listener = getListener(ip, port);
            Thread thread = new Thread(new ThreadStart(runAs));
            thread.Start();

        }
        public static TcpListener getListener(IPAddress address, Int32 port)
        {
            try
            {
                TcpListener listener = new TcpListener(address, port);
                listener.Start();
                return listener;
            }
            catch (Exception error)
            {
                MessageBox.Show(error.ToString());
            }
            return null;
        }
        public static string GetLocalIP()
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
    }
}
