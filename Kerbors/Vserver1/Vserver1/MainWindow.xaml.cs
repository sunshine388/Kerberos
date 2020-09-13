
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;

namespace Vserver1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public string msg8_IDV = "SER";
        Dic dic = new Dic();
        public string publickey = @"<RSAKeyValue><Modulus>uhRRNLgDnQT4BqDRxzNDRnBmO9s3AeKgJQCY87di0LYiLjDe4kJqknne+Glw4RBSwqZjbS8DZywXG4jPR1ul+QzgGNPw+fdgAJ
yZRvp5UdIGwblqKT4e1gdu/gNCrzq0ua/3CsUdhNh/5lg0H2vbnWLxtz1UGddqOp16vOXI7jk=</Modulus><Exponent>AQAB</Exponent></RSAKeyVal
ue>
";
        public string private_key = @"<RSAKeyValue><Modulus>sIfR0O4uR80lTeY4pjE43OijKhdWKlXYs80bX8iAIsE/spkrxufhFL0D04IgquzeUz7CAvkRE62vs9WnUqLSjPPT7mEw6bRY3F
Xc1lxaDQA8kX2VPFzuEDjMDYvBLQbx2yNEfy16KiwhosXBDLl5X7SrUMV9IjyVOVJkFf4Egmk=</Modulus><Exponent>AQAB</Exponent><P>x4+1du7o
4K41urYdjzu957+o2M/MNzdhnPjKZlAYPJzOuHupY0j7N4ZzBYPHgC4DS8jX1H3o53UWhayHc8475w==</P><Q>4nSsiXVwjQ0+tNGD3qHBmqxWx3yIwa7KJ
DAeQuA2BgdeQPUMZsYJF9wij+RATWdzd4ghXj80t2C+kxvMjKQFLw==</Q><DP>qPGWlJtpd1zEi87Fc48GFH4DOZmhr1UpRDSzVK6V9ipiL3gxTKZqVvVxx
sdrS66oh63+WhxF4j0T1hLbkUSVHQ==</DP><DQ>hVN+FbwQFOucZKcKBbSkoOUgfniip626E45E8sjA5dHGu2XK9GNaDTMtIhyXgzsKL3D1fGCoL+MEsOXo
C1GyPw==</DQ><InverseQ>h3QNBsdHXARJAQhxus/BBmqjSEl0Mo8sCmZwanRgQAQo7E9AxNtZlNMaMEbd14iatTGIncKxi+4PaqhyjxkD2w==</Inverse
Q><D>ngn6HUIezMksDIF/VcnbTmo3KQebbGtOhimpyCaIEJVRoWLffkM5jodpVYG6HUvR/lTA/EhesW9dTXKJScHbHDgIR3ShrsiJQutCfRDFEScCApRJIF6
9chxpQixT5ht4L+PiZvlKg6sMn5IZH/7PG0x3xM7seTWAkwnAF/wbY8E=</D></RSAKeyValue>
";
        private Socket connection;
        private TcpListener listener;
        private IPAddress ip;
        private Int32 port;
        RSAHelper y = new RSAHelper();
        public string Time = DateTime.Now.ToString("yyyy/MM/dd HH：mm：ss");
        private static byte[] result = new byte[1000];
        public string kcvkey;
        public MainWindow()
        {
            string host = GetLocalIP();
            IPAddress ip = IPAddress.Parse(host);
            InitializeComponent();
            this.ip = ip;
            this.port = 8000;
        }
        //侦听客户连接请求
        public void runAs()
        {

            while (true)
            {

                this.Dispatcher.Invoke(new Action(() => { T1.AppendText("Waiting for Connection\r\n"); }));
                connection = listener.AcceptSocket();
                //在新线程中启动新的socket连接，每个socket等待，并保持连接

                IPEndPoint iprm = (IPEndPoint)connection.RemoteEndPoint;
                this.Dispatcher.Invoke(new Action(() => { T1.AppendText("远程主机:" + iprm.Address.ToString() + ":" + iprm.Port.ToString() + "连接上本机" + DateTime.Now.ToString() + "\n"); }));
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
            this.Dispatcher.Invoke(new Action(() => { T2.AppendText("准备接受消息！\n"); }));

            Thread receiveThread = new Thread(ReceiveMessage);
            receiveThread.Start(connection);
        }
        public Message DealMsg7(string msg7)//处理信息7
        {
            Message a = new Message();
            string type;
            string pwd;
            string tag;
            string IDC;
            string TS;
            string operation;
            string bookname;
            type = msg7.Substring(0, 2);
            pwd = msg7.Substring(2, 4);
            if (type == "05")
            {
                string ticket;
                string Authenticaorc;
                string Mticket;
                string MAuthenticaorc;
                string ADC;
                string IDv;
                string TS4;
                string Lifetime4;
                string TS5;
                string IDC1;
                string ADC1;
                type = msg7.Substring(0, 2);
                pwd = msg7.Substring(2, 4);
                tag = msg7.Substring(6, 2);
                string tkA;
                tkA = msg7.Substring(8, msg7.Length - 8);
                //Console.WriteLine(tkA);
                string[] sArray = Regex.Split(tkA, "####", RegexOptions.IgnoreCase);
                Mticket = sArray[0];
                MAuthenticaorc = sArray[1];
                //MessageBox.Show(getvkey());
                ticket = DecryptString(Mticket, getvkey());
                string[] sArray1 = Regex.Split(ticket, "####", RegexOptions.IgnoreCase);
                kcvkey = sArray1[0].Substring(0, 8);
                //MessageBox.Show(kcvkey);
                IDC = sArray1[0].Substring(8, 3);
                ADC = sArray1[1];
                IDv = sArray1[2].Substring(0, 3);
                TS4 = sArray1[2].Substring(3, Time.Length);
                Lifetime4 = sArray1[2].Substring(3 + Time.Length, 4);
                Authenticaorc = DecryptString(MAuthenticaorc, kcvkey);
                string[] sArray2 = Regex.Split(Authenticaorc, "####", RegexOptions.IgnoreCase);
                IDC1 = sArray2[0];
                ADC1 = sArray2[1];
                //int index2 = sArray2[2].IndexOf('\0');
                //int len2 = sArray2[2].Length;
                //sArray2[2].Remove(index2, len2 - index2);
                TS5 = sArray2[2];
                dic.addDic(IDC, kcvkey);
                a = new Message(TS4, Lifetime4, IDC, ADC, IDv, TS5, IDC1, ADC1, kcvkey);
            }
            if (type == "07")
            {
                string MM;
                IDC = msg7.Substring(6, 3);
                // MessageBox.Show(mark);
                string[] sArray1 = Regex.Split(msg7, "####", RegexOptions.IgnoreCase);
                MM = sArray1[1];
                string CC;
                string rsa;
                string key;
                //Dic.myDictionary.TryGetValue(mark, out key);
                // MessageBox.Show(key);
                //MessageBox.Show(Dic.myDictionary.ContainsKey(mark).ToString());
                //if (Dic.myDictionary.ContainsKey(mark))
                //{
                Dic.myDictionary.TryGetValue(IDC, out key);
                // MessageBox.Show(key);
                CC = DecryptString(MM, key);
                string[] sArray = Regex.Split(CC, "####", RegexOptions.IgnoreCase);
                tag = sArray[0].Substring(0, 2);
                TS = sArray[0].Substring(2, Time.Length);
                operation = sArray[0].Substring(2 + Time.Length, 3);
                bookname = sArray[1];
                rsa = sArray[2];
                //MessageBox.Show(bookname);
                //Console.WriteLine(bookname);
                a = new Message( operation, bookname, IDC, DateTime.Parse(TS), rsa);
                // }
            }
            if (type == "09")
            {
                string MM;
                IDC = msg7.Substring(6, 3);
                //MessageBox.Show(mark);
                string[] sArray1 = Regex.Split(msg7, "####", RegexOptions.IgnoreCase);
                MM = sArray1[1];
                string CC;
                string rsa;
                string key;
                // Dic.myDictionary.TryGetValue(mark, out key);
                // MessageBox.Show(key);
                //if (Dic.myDictionary.ContainsKey(mark))
                //{
                Dic.myDictionary.TryGetValue(IDC, out key);
                CC = DecryptString(MM, key);
                string[] sArray = Regex.Split(CC, "####", RegexOptions.IgnoreCase);
                // tag = Int32.Parse(CC.Substring(0, 
                tag = sArray[0].Substring(0, 2);
                operation = sArray[0].Substring(2, 3);
                rsa = sArray[1];
                // MessageBox.Show(operation);
                // MessageBox.Show(IDC);
                // MessageBox.Show(rsa);
                // MessageBox.Show(a.type);
                a = new Message( IDC, operation, rsa);
                // MessageBox.Show(a.tag);
                // }

            }
            return a;
        }
        public Message dealmsg7(Message msg7)//处理message7并且返回一个信息8
        {
            DataBase1 b = new DataBase1();
            Message a = new Message();
            if (msg7.type == "07" && y.SignCheck(MD5Hash(msg7.msg7_IDC), msg7.msg7_rsa, publickey))
            {
                if (msg7.msg7_operation == "sea")
                {
                    int judge;
                    judge = b.compare2(msg7.msg7_bookname);
                    if (judge == 0)
                    {
                        a = new Message(msg8_IDV, judge.ToString());
                        a.type = "08";
                        a.tag = "08";
                    }
                    else
                    {
                        b.addfinder(msg7.msg7_bookname);
                        a = new Message(msg8_IDV, judge.ToString());
                        a.type = "08";
                        a.tag = "08";
                    }

                }
                if (msg7.msg7_operation == "rea")
                {
                    int judge;
                    judge = b.compare2(msg7.msg7_bookname);
                    if (judge == 0)
                    {
                        //MessageBox.Show(msg7.msg7_bookname);
                        string txt = b.gettxt(msg7.msg7_bookname);
                        a = new Message(msg8_IDV, txt);
                        a.type = "10";
                        a.tag = "10";
                        // MessageBox.Show(b.Read());
                    }
                    else
                    {
                        //MessageBox.Show(msg7.msg7_bookname);
                        string txt = b.gettxt(msg7.msg7_bookname);
                        b.addviewer(msg7.msg7_bookname);
                        a = new Message(msg8_IDV, txt);
                        a.type = "10";
                        a.tag = "10";
                        // MessageBox.Show(b.Read());
                    }
                }
            }
            // MessageBox.Show(a.msg1_IDc);
            if (msg7.type == "09" && y.SignCheck(MD5Hash(msg7.msg9_IDC), msg7.msg9_rsa, publickey))
            {
                // MessageBox.Show(a.msg1_IDc);
                if (msg7.msg9_operation == "ref")
                {
                    string refresh = b.refresh();
                    //MessageBox.Show(a.msg1_IDc);
                    a = new Message(msg8_IDV, refresh);
                    a.type = "12";
                    a.tag = "12";
                }
            }

            return a;
        }
        public Message dealMsg(Message msg5)// 处理message5并·返回一个信息6
        {
            // DataBase1 a = new DataBase1();
            //UTF8Encoding enc = new UTF8Encoding()
            string type = msg5.type;
            Message result = new Message();
            DateTime dt2 = Convert.ToDateTime(msg5.msg5_Au_TS5);
            DateTime d3 = DateTime.Parse(msg5.msg5_tkt_TS4).AddSeconds(Convert.ToInt32(msg5.msg5_tkt_Lifetime4));
            DateTime ssmg = DateTime.Parse(msg5.msg5_Au_TS5).AddSeconds(1);
            if (type == "05")//MESSAGE
            {
                if (msg5.msg5_tkt_IDc == msg5.msg5_Au_IDc)
                {
                    if (DateTime.Compare(dt2, d3) <= 0)
                    {
                        result = new Message(ssmg.ToString());
                    }
                    if (DateTime.Compare(dt2, d3) > 0)
                    {
                        MessageBox.Show("ticket已过期！");
                    }
                }
            }
            else
            {
                MessageBox.Show("发送数据有误！");
            }
            return result;

        }
        public string ssMessage8(Message msg7)//发送message8,Message10, Message12
        {
            string key;
            //  MessageBox.Show(msg7.msg7_mark);
            Dic.myDictionary.TryGetValue(msg7.msg7_IDC, out key);
            Message a = new Message();
            a = dealmsg7(msg7);
            string ssmg;
            // MessageBox.Show(cvkey1);
            ssmg = string.Concat(a.type, a.pwd, Encrypt1(string.Concat(a.tag, a.msg8_IDV, a.msg8_msg, "####", y.Sign(a.msg8_IDV, private_key)), key));
            // MessageBox.Show(a.msg8_msg);
            return ssmg;
        }
        public string ssMessage12(Message msg9)//发送message8,Message10, Message12
        {
            string key;
            //  MessageBox.Show(msg7.msg7_mark);
            Dic.myDictionary.TryGetValue(msg9.msg9_IDC, out key);
            Message a = new Message();
            a = dealmsg7(msg9);
            string ssmg;
            // MessageBox.Show(cvkey1);
            // MessageBox.Show(a.tag+a.msg8_IDV+a.msg8_msg);
            ssmg = string.Concat(a.type, a.pwd, Encrypt1(string.Concat(a.tag, a.msg8_IDV, a.msg8_msg, "####", y.Sign(a.msg8_IDV, private_key)), key));
            // MessageBox.Show(a.msg8_msg);
            return ssmg;
        }
        public string ssMessage(Message msg5)//发送message6
        {
            string key;
            // MessageBox.Show(msg5.cvkey);
            Dic.myDictionary.TryGetValue(msg5.msg5_Au_IDc, out key);
            // MessageBox.Show(key);
            Message a = new Message();
            a = dealMsg(msg5);
            string ssmg;
            string ssmg1;
            ssmg = Encrypt1(string.Concat(a.tag, a.msg6_TS6), key);
            ssmg1 = string.Concat(a.type, a.pwd, ssmg);
            return ssmg1;
        }
        public static string Encrypt1(string str, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.Default.GetBytes(str);
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);// 密匙
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);// 初始化向量
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            var retB = Convert.ToBase64String(ms.ToArray());
            return retB;
        }
        public static string DecryptString(string pToDecrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Convert.FromBase64String(pToDecrypt);
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            // 如果两次密匙不一样，这一步可能会引发异常
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
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
                    this.Dispatcher.Invoke(new Action(() => { T2.AppendText(Encoding.Unicode.GetString(result, 0, num)); }));

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
                    // break;
                }
            }
        }
        public void SendMessage(object clientSocket)
        {

            // while (true)
            // {
            Socket myClientSocket = (Socket)clientSocket;
            Message c = DealMsg7(Encoding.Unicode.GetString(result));
            //MessageBox.Show(c.type);
            if (c.type == "05")
            {
                //Message b = new Message();
                c.msg5_Au_TS5 = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                //this.Dispatcher.Invoke(new Action(() => { TextBox1.AppendText(c.msg2_TS2); }));
                this.Dispatcher.Invoke(new Action(() => { T3.AppendText(c.MMessage5(c)); }));
                DateTime ssmg = DateTime.Parse(c.msg5_Au_TS5).AddSeconds(1);
                this.Dispatcher.Invoke(new Action(() => { T5.AppendText(ssmg.ToString("yyyy/MM/dd HH:mm:ss")); }));
                // this.Dispatcher.Invoke(new Action(() => { TextBox1.AppendText(ssmg.Length.ToString()); }));
                string Cssmg;
                Cssmg = ssMessage(c);
                this.Dispatcher.Invoke(new Action(() => { T4.AppendText(Cssmg); }));
                Byte[] ssmg1 = new byte[1024];
                ssmg1 = Encoding.ASCII.GetBytes(Cssmg);
                //int num1 = myClientSocket.Send(ssmg1);
                myClientSocket.Send(ssmg1, ssmg1.Length, 0);
                // Thread sendThread = new Thread(SendMessage);
                // sendThread.Start(myClientSocket);
                // myClientSocket.Close();
            }
            if (c.type == "09")
            {
                // MessageBox.Show(c.msg1_IDc);
                string ssmg;
                ssmg = ssMessage12(c);
                //MessageBox.Show(ssmg);
                Byte[] ssmg1 = new byte[1024];
                ssmg1 = Encoding.Unicode.GetBytes(ssmg);
                myClientSocket.Send(ssmg1, ssmg1.Length, 0);
                // myClientSocket.Close();
            }
            if (c.type == "07")
            {
                string ssmg;
                ssmg = ssMessage8(c);
                //MessageBox.Show(ssmg);
                Byte[] ssmg1 = new byte[2048];
                ssmg1 = Encoding.Unicode.GetBytes(ssmg);
                myClientSocket.Send(ssmg1, ssmg1.Length, 0);
                //myClientSocket.Close();
            }
            // myClientSocket.Close();
            //}
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
        public static string MD5Hash(string stringToHash)
        {
            var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] emailBytes = Encoding.UTF8.GetBytes(stringToHash.ToLower());
            byte[] hashedEmailBytes = md5.ComputeHash(emailBytes);
            StringBuilder sb = new StringBuilder();
            foreach (var b in hashedEmailBytes)
            {
                sb.Append(b.ToString("x2").ToLower());

            }
            return sb.ToString();
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
        public string getvkey()
        {
            return "TGStoSER";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            listener = getListener(ip, port);
            Thread thread = new Thread(new ThreadStart(runAs));
            thread.Start();
        }
    }

}

