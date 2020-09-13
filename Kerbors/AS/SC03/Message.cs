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

namespace SC03
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
        RSAHelper Y = new RSAHelper();
        public string private_key = "<RSAKeyValue><Modulus>pFyBuMG3Thz4LIfuDZ2fPSsuCerhmcJ+ym8lh239tULjtLAy0edbx3c31FHWQZnfjCVgUhrPUcLBVkhLK/G0UDs+SaQVsN+BoKJD6rRmF/ErF4wj+x+PJyzTZM6IpG7gxUiqiBZXp+OWRltqWhmSmSgkA2D51h5NU2NA0Arhtak=</Modulus><Exponent>AQAB</Exponent><P>0fGa+38U/fpNCwIUxcP7uRTyv5SuYugEvjFlaZAEcewVAFlWl98SPjJZU6J6ZsArYJGQw6v6v7GO/CSkNcY3rw==</P><Q>yGsAzXYWVcC1UyVk9YXQgGaIds6rtZpYArA5z3iglP1POm4Bt1/O+vVOPeJFonDXroDgMTofIKWluc35uNjmJw==</Q><DP>Qw+P1+HiDLaVQXzqsblGgPpGCBgwE/vU+APDRxLvuIwWsUPYOy7QHvnqOqLdwlCECpa0zSv0LqNC7xutMFgelQ==</DP><DQ>cAFs92ZxYQdLzuXtIFHijn++8DbcO6fAW/BEvrA5fkp6xrrH9sVylUoWqfsU042g3ANmR9ylnMc2cTqtvyJorQ==</DQ><InverseQ>TfQXW6S7x5ipvw4Wp8xEoaIqdy94oq+XeKCGuEs4+SUadpYgmyTxflL5pulK/UWh89DaNZ4++BCQ8l9nQ3vt1A==</InverseQ><D>h/gfOIDI6837YJtyy8DBoxC9nWa6C0FjMoSQP2E2qWUUir3YzNzgiDqP7Q1meg6ZaI7jCAk1ySWcW4yi40+pIgLlr7W/SbtALF2cSblLNzhWBrbKDGyQoER8VkA/C0Mu2xNqBnRmgrMroCDV2IYEtO84IjFWgjugB/DG552JWaE=</D></RSAKeyValue>";
        public Message()
        {
            type = "00";
            tag = "00";
            pwd = "0000";


        }
        public string msg1_IDc;
        public string msg1_TS1;
        public string IDtgs;
        public Message(string IDc, string tgs, string TS1)//Message(1)
        {
            this.pwd = "0000";
            this.type = "01";
            this.tag = "01";
            this.msg1_IDc = IDc;
            this.msg1_TS1 = TS1;
            this.IDtgs = tgs;
        }
        //Message(2)
        public string msg2_IDtgs;
        public string msg2_TS2;
        public string msg2_Lifetime2;
        //Ticket
        public string msg2_tkt_IDc;
        public string msg2_tkt_ADc;
        //public byte[] IDtgs;
        public Message(string IDtgs, string TS2, string Lifetime2, string IDc, string ADc)
        {
            this.pwd = "0000";
            this.type = "02";
            this.tag = "02";
            this.msg2_IDtgs = IDtgs;
            this.msg2_TS2 = TS2;
            this.msg2_Lifetime2 = Lifetime2;
            this.msg2_tkt_IDc = IDc;
            this.msg2_tkt_ADc = ADc;
        }
        public string msg3_ser;
        public string msg3_tgt;
        public string msg3_aut;
        public Message(string ser, string tgt, string aut, int m)
        {
            this.pwd = "0000";
            this.type = "03";
            this.tag = "03";
            this.msg3_ser = ser;
            this.msg3_tgt = tgt;
            this.msg3_aut = aut;
        }

        public string msg4_key;
        public string msg4_idv;
        public string msg4_ts4;
        public string msg4_tkt;
        public string msg4_Lifetime4;
        public string msg4_tkt_ADc;
        public Message(string key, string idv, string ts4, string Lifetime4, string tkt,string ADc)
        {
            this.type = "04";
            this.tag = "04";
            this.pwd = "0000";
            this.msg4_key = key;
            this.msg4_idv = idv;
            this.msg4_ts4 = ts4;
            this.msg4_tkt = tkt;
            this.msg4_Lifetime4 = Lifetime4;
            this.msg4_tkt_ADc = ADc;
        }

        //Message 13
        public string msg13_IDC;
        public string msg13_pwd;
        public Message(string IDc, string pwd)
        {
            this.pwd = "0000";
            this.type = "13";
            this.tag = "13";
            this.msg13_IDC = IDc;
            this.msg13_pwd = pwd;
        }
        //Message 14
        public string msg14_TS;
        public Message(string TS14)
        {
            this.pwd = "0000";
            this.type = "14";
            this.tag = "14";
            this.msg14_TS = TS14;

        }
        public string SSMessage14(Message msg13)//发送Message14
        {
            Message msg14 = dealMsg(msg13);
            string ssmg;
            ssmg = string.Concat(msg14.type, msg14.pwd, msg14.tag, msg14.msg14_TS);
            return ssmg;
        }
        public string MMessage14(Message msg14)//显示发送信息14
        {
            string ssmg;
            ssmg = "注册成功时间：" + msg14.msg14_TS + "\n";
            return ssmg;
        }
        public string MMessage(Message msg1)//显示发送信息2
        {
            Message msg2 = dealMsg(msg1);
            string tickets;
            tickets = String.Concat(getkey2(), msg2.msg2_tkt_IDc, msg2_tkt_ADc, msg2.msg2_IDtgs, msg2.msg2_TS2, msg2.msg2_Lifetime2);
            string M;
            M = getkey2() + " " + msg2.msg2_IDtgs + " " + msg2.msg2_TS2 + " " + msg2.msg2_Lifetime2 + "\n" + "   票据是：" + tickets;
            return M;
        }
        public string MMessage4(Message msg1)//显示发送信息4
        {
            string ssmg = "";
            return ssmg;
        }
        public string ssMessage4(Message msg1)
        {
            string tgt = msg1.msg3_tgt;
            string tkt = Decrypt2(tgt);
            string tkt_key = tkt.Substring(0, 8);
            string tkt_IDc = tkt.Substring(8, 3);
            Message msg2 = dealMsg(msg1);
            string tickets;
            tickets = Encrypt3(String.Concat( msg2.msg4_key, tkt_IDc, "####", msg4_tkt_ADc, "####", msg2.msg4_idv,  msg2.msg4_ts4,msg2.msg4_Lifetime4));
            string ssmg1;
            ssmg1 = EncryptString(String.Concat(msg2.tag, msg2.msg4_key, msg2.msg4_idv, msg2.msg4_ts4, tickets), tkt_key);
            string ssmg;
            ssmg = String.Concat(msg2.type,msg2.pwd, ssmg1);
            return ssmg;

        }
        public string ssMessage(Message msg1)//加密信息2并发送
        {
            Message msg2 = dealMsg(msg1);
            string tickets;
            tickets = Encrypt2(String.Concat(getkey2(), msg2.msg2_tkt_IDc,"####", msg2_tkt_ADc,"####", msg2.msg2_IDtgs, msg2.msg2_TS2, msg2.msg2_Lifetime2));
            string ssmg1;
            ssmg1 = Encrypt1(String.Concat(msg2.tag, getkey2(), msg2.msg2_IDtgs, msg2.msg2_TS2, msg2.msg2_Lifetime2, tickets), msg1_IDc);
            string ssmg;
            ssmg = String.Concat(msg2.type, msg2.pwd, ssmg1);
            return ssmg;

        }

        public string Encrypt1(string message2,string id)
        {
            byte[] keyBytes = Encoding.ASCII.GetBytes(getDESKey(id).Substring(0, 8));
            byte[] keyIV = keyBytes;
            byte[] inputByteArray = Encoding.ASCII.GetBytes(message2);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, provider.CreateEncryptor(keyBytes, keyIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());

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
        public string Encrypt3(string message2)
        {
            byte[] keyBytes = Encoding.ASCII.GetBytes(getvkey().Substring(0, 8));
            byte[] keyIV = keyBytes;
            byte[] inputByteArray = Encoding.ASCII.GetBytes(message2);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, provider.CreateEncryptor(keyBytes, keyIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());

        }

        public string Decrypt2(string message2)
        {
            byte[] keyBytes = Encoding.ASCII.GetBytes(gettgskey().Substring(0, 8));
            byte[] keyIV = keyBytes;
            byte[] inputByteArray = Convert.FromBase64String(message2);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, provider.CreateDecryptor(keyBytes, keyIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Encoding.UTF8.GetString(mStream.ToArray());

        }
        public static string EncryptString(string str, string sKey)
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
        public  string DesDecrypt(string decryptString,string ID)
        {
            byte[] keyBytes = Encoding.ASCII.GetBytes(getDESKey(ID).Substring(0, 8));
            byte[] keyIV = keyBytes;
            byte[] inputByteArray = Convert.FromBase64String(decryptString);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, provider.CreateDecryptor(keyBytes, keyIV), CryptoStreamMode.Write);
            cStream.Read(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Encoding.UTF8.GetString(mStream.ToArray());
        }

        public  Message dealMsg(Message msg)//处理信息1和信息13
        {
            int b = 1000;
            DataBase1 a = new DataBase1();
            UTF8Encoding enc = new UTF8Encoding();
            string type = msg.type;
            Message result = new Message();
            if (type == "01")//MESSAGE1
            {
                if (a.compare1(msg.msg1_IDc)==1)
                {
                    result = new Message(msg.IDtgs,DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), b.ToString(), msg.msg1_IDc, msg.msg2_tkt_ADc);
                }
            }
            if (type == "03")
            {
                result = new Message(msg.msg4_key, msg.msg3_ser, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), b.ToString(), msg.msg4_tkt, msg.msg4_tkt_ADc);
            }
             if(type=="13")
            {
               // Y.DecryptString(msg.msg13_pwd, private_key);
                a.Add(msg.msg13_IDC, msg.msg13_pwd);
                 result = new Message();
                
            }
           // else 
            //{
              //  MessageBox.Show("发送数据有误！");
            //}
            return result;

        }
        //获取客户端DES密钥
        public static string getDESKey(string IDC)
        {
            string b;
            DataBase1 a = new DataBase1();
            b=a.Getkey(IDC);
            return b;  
        }
        //获取客户端session key 的副本
        public string getkey2()
        {
           string kctgs = Y.GetRandomString(8);
            return kctgs;
        }
        public string gettgskey()
        {
            string key = "ASandTGS";
            return key;
        }
        public string getvkey()
        {
            return "TGStoSER";
        }

    }
}
