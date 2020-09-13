using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net.Security;
using System.IO;
using System.Security.Cryptography;
using System.Windows;

namespace Vserver1
{
    /**
    * 消息类，客户端和服务端所有的信息都是格式化为Message 来传递的。
    */
    [Serializable()]

    public class Message
    {
        public string type;
        public string tag;
        public string pwd;
        private static string cvkey1;
        byte[] byData = new byte[1000];
        char[] charData = new char[2000];
        public string private_key = @"<RSAKeyValue><Modulus>sIfR0O4uR80lTeY4pjE43OijKhdWKlXYs80bX8iAIsE/spkrxufhFL0D04IgquzeUz7CAvkRE62vs9WnUqLSjPPT7mEw6bRY3F
Xc1lxaDQA8kX2VPFzuEDjMDYvBLQbx2yNEfy16KiwhosXBDLl5X7SrUMV9IjyVOVJkFf4Egmk=</Modulus><Exponent>AQAB</Exponent><P>x4+1du7o
4K41urYdjzu957+o2M/MNzdhnPjKZlAYPJzOuHupY0j7N4ZzBYPHgC4DS8jX1H3o53UWhayHc8475w==</P><Q>4nSsiXVwjQ0+tNGD3qHBmqxWx3yIwa7KJ
DAeQuA2BgdeQPUMZsYJF9wij+RATWdzd4ghXj80t2C+kxvMjKQFLw==</Q><DP>qPGWlJtpd1zEi87Fc48GFH4DOZmhr1UpRDSzVK6V9ipiL3gxTKZqVvVxx
sdrS66oh63+WhxF4j0T1hLbkUSVHQ==</DP><DQ>hVN+FbwQFOucZKcKBbSkoOUgfniip626E45E8sjA5dHGu2XK9GNaDTMtIhyXgzsKL3D1fGCoL+MEsOXo
C1GyPw==</DQ><InverseQ>h3QNBsdHXARJAQhxus/BBmqjSEl0Mo8sCmZwanRgQAQo7E9AxNtZlNMaMEbd14iatTGIncKxi+4PaqhyjxkD2w==</Inverse
Q><D>ngn6HUIezMksDIF/VcnbTmo3KQebbGtOhimpyCaIEJVRoWLffkM5jodpVYG6HUvR/lTA/EhesW9dTXKJScHbHDgIR3ShrsiJQutCfRDFEScCApRJIF6
9chxpQixT5ht4L+PiZvlKg6sMn5IZH/7PG0x3xM7seTWAkwnAF/wbY8E=</D></RSAKeyValue>
";
        public string publickey = @"<RSAKeyValue><Modulus>uhRRNLgDnQT4BqDRxzNDRnBmO9s3AeKgJQCY87di0LYiLjDe4kJqknne+Glw4RBSwqZjbS8DZywXG4jPR1ul+QzgGNPw+fdgAJ
yZRvp5UdIGwblqKT4e1gdu/gNCrzq0ua/3CsUdhNh/5lg0H2vbnWLxtz1UGddqOp16vOXI7jk=</Modulus><Exponent>AQAB</Exponent></RSAKeyVal
ue>
";
        RSAHelper y = new RSAHelper();
        public Message()
        {
            type = "00";
            tag = "00";
            pwd = "0000";


        }
        public string msg9_IDC;
        public string msg9_operation;
        public string msg9_rsa;
        public Message(string IDC, string opreation, string rsa)//message9
        {
            this.pwd = "0000";
            this.type = "09";
            this.tag = "09";
            this.msg9_IDC = IDC;
            this.msg9_operation = opreation;
            this.msg9_rsa = rsa;
        }
        //Meessage5
        public string msg5_tkt_TS4;
        public string msg5_tkt_Lifetime4;
        public string msg5_tkt_IDc;
        public string msg5_tkt_ADc;
        public string msg5_tkt_IDv;
        public string msg5_Au_TS5;
        public string msg5_Au_IDc;
        public string msg_Au_ADc;
        public string cvkey;

        public Message(string TS4, string Lifetime4, string IDc, string ADc, string IDv, string TS5, string IDC1, string ADC1, string cvkey)
        {
            this.pwd = "0000";
            this.type = "05";
            this.tag = "05";
            this.msg5_Au_TS5 = TS5;
            this.msg5_tkt_Lifetime4 = Lifetime4;
            this.msg5_tkt_IDc = IDc;
            this.msg5_tkt_ADc = ADc;
            this.msg5_tkt_TS4 = TS4;
            this.msg_Au_ADc = ADC1;
            this.msg5_Au_IDc = IDC1;
            this.cvkey = cvkey;
        }
        //Message6和
        public string msg6_TS6;
        public Message(string TS6)
        {
            this.pwd = "0000";
            this.type = "06";
            this.tag = "06";
            this.msg6_TS6 = TS6;
        }
        //message8
        public string msg8_IDV = "SER";
        public string msg8_msg;
        public Message(string IDV, string msg)
        {
            this.pwd = "0000";
            this.type = "08";
            this.tag = "08";
            this.msg8_IDV = IDV;
            this.msg8_msg = msg;
        }
        //Message7
        public string msg7_operation;
        public string msg7_bookname;
        public string msg7_IDC;
        public string msg_TS7;
        public string msg7_rsa;
        public Message( string operation, string bookname, string IDC, DateTime ts, string rsa)
        {
            this.pwd = "0000";
            this.type = "07";
            this.tag = "07";
            this.msg7_operation = operation;
            this.msg7_bookname = bookname;
            this.msg7_IDC = IDC;
            this.msg_TS7 = ts.ToString("yyyy/MM/dd HH：mm：ss");
            this.msg7_rsa = rsa;
        }
        /*public Message dealmsg7(Message msg7)//处理message7
        {
            DataBase1 b = new DataBase1();
            Message a = new Message();
            if (msg7.type == "07" && y.SignCheck(MD5Hash(msg7.msg7_IDC), msg7.msg7_rsa, publickey))
            {
                if (msg7.msg7_operation == "sea")
                {
                    int judge;
                    judge = b.compare2(msg7.msg7_bookname);
                    b.addfinder(msg7.msg7_bookname);
                    a = new Message(msg8_IDV, judge.ToString());
                    a.type = "08";
                    a.tag = "08";

                }
                if (msg7.msg7_operation == "rea")
                {
                    string txt = b.gettxt(msg7_bookname);
                    b.addviewer(msg7.msg7_bookname);
                    a = new Message(msg8_IDV, txt);
                    a.type = "10";
                    a.tag = "10";
                  // MessageBox.Show(b.Read());
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
       /* public string ssMessage8(Message msg7)//发送message8,Message10, Message12
        {
            Message a = new Message();
            a = dealmsg7(msg7);
            string ssmg;
           // MessageBox.Show(cvkey1);
            ssmg = string.Concat(a.type,a.pwd,Encrypt1(string.Concat(a.tag,a.msg8_IDV,a.msg8_msg,"####", y.Sign(a.msg8_IDV, private_key)),cvkey1));
           // MessageBox.Show(a.msg8_msg);
            return ssmg;
        }*/
        public string MMessage5(Message msg5)//显示明文信息
        {
            string ticketv;
            string AU;
            string mmsg;
            ticketv = getkey2() + " " + msg5.msg5_tkt_IDc + " " + msg5.msg5_tkt_ADc + " " + msg5.msg5_tkt_IDv + " " + msg5.msg5_tkt_TS4 + " " + msg5.msg5_tkt_Lifetime4 + " \n";
            AU = msg5_Au_IDc + " " + msg5.msg_Au_ADc + " " + msg5.msg5_Au_TS5;
            mmsg = "ticketv:" + ticketv + "\n" + " Authenticatorc:" + AU + "\n";
            return mmsg;
        }
        /*
        public Message dealMsg(Message msg5)// 处理message5
        {
           // DataBase1 a = new DataBase1();
            //UTF8Encoding enc = new UTF8Encoding();
            string type = msg5.type;
            cvkey1 = msg5.cvkey;
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
                    if(DateTime.Compare(dt2, d3) > 0)
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
        public string ssMessage(Message msg5)//发送message6
        {
            Message a = new Message();
            a = dealMsg(msg5);
            string ssmg;
            string ssmg1;
            ssmg = Encrypt1(string.Concat(a.tag,a.msg6_TS6),cvkey1);
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
        }*/

        public string Encrypt3(string message2, string key)
        {
            byte[] keyBytes = Encoding.ASCII.GetBytes(key.Substring(0, 8));
            byte[] keyIV = keyBytes;
            byte[] inputByteArray = Encoding.ASCII.GetBytes(message2);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, provider.CreateEncryptor(keyBytes, keyIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Encoding.Unicode.GetString(mStream.ToArray());

        }


        public string Encrypt2(string message2)
        {
            byte[] keyBytes = Encoding.ASCII.GetBytes(gettgskey().Substring(0, 8));
            byte[] keyIV = keyBytes;
            byte[] inputByteArray = Encoding.ASCII.GetBytes(message2);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, provider.CreateEncryptor(keyBytes, keyIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());

        }
        public string DesDecrypt(string decryptString, string ID)
        {
            byte[] keyBytes = Encoding.ASCII.GetBytes(getDESKey(ID).Substring(0, 8));
            byte[] keyIV = keyBytes;
            byte[] inputByteArray = Convert.FromBase64String(decryptString);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, provider.CreateDecryptor(keyBytes, keyIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Encoding.UTF8.GetString(mStream.ToArray());
        }
        public string DesDecrypt1(string decryptString, string ID)
        {
            byte[] keyBytes = Encoding.Unicode.GetBytes(getDESKey(ID).Substring(0, 8));
            byte[] keyIV = keyBytes;
            byte[] inputByteArray = Convert.FromBase64String(decryptString);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, provider.CreateDecryptor(keyBytes, keyIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Encoding.Unicode.GetString(mStream.ToArray());
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


        //获取客户端DES密钥
        public static string getDESKey(string IDC)
        {
            string b;
            //DataBase1 a = new DataBase1();
            b = "12345678";
            return b;
        }
        //获取客户端session key 的副本
        public string getkey2()
        {
            string IDC = "12345678";
            return IDC;
        }
        public string gettgskey()
        {
            string key = "12345678";
            return key;
        }

    }
}
