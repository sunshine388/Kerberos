using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SC03;

namespace Kerberos
{
     
    class message
    {
        public string type;//消息类型 
        public string m_pwd;//报文密码
        public string m_data;//报文数据段
        public message()
        {
            type = "00";
            m_pwd = "0000";
            this.m_data = " ";
        }
        
        public static data Dec_msg(message msg,string pwd)
        {
            string type = msg.type;
            data result = new data();
            if(type=="02")
            {
                // StreamReader sr = new StreamReader(@"C:\Users\Administrator\Desktop\Kerberos-master\编程记录\Kerberos-master\密钥.txt");
                //   string nextLine = sr.ReadLine();
                MainWindow w44 = new MainWindow();
                string nextLine = w44.get_pwd_c();
                string str=des.DecryptString(msg.m_data,nextLine);
                result.d_tag = str.Substring(0,2);
                result.data2_key_c_tgs = str.Substring(2,8);
                result.data2_IDtgs = str.Substring(10,3);
                result.data2_TS2=str.Substring (13,19);
                result.data2_Lifetime1=str.Substring(32,4);
                result.data2_tkt_tgs=str.Substring(36,str.Length-36);
            }
            else if(type=="04")
            {
                string str = des.DecryptString(msg.m_data, pwd);
             //   char[] data1 = str.ToCharArray();
             //   byte[] ss = System.Text.Encoding.ASCII.GetBytes(str);
                result.d_tag = str.Substring(0,2);
                result.data4_key_c_v=str.Substring(2,8);
                result.data4_IDv=str.Substring(10,3);
                result.data4_TS4=str.Substring(13,19);
                result.data4_tkt_v=str.Substring(32,str.Length-32);
            }
            else if(type=="06")
            {
                string str = des.DecryptString(msg.m_data, pwd);
                //  char[] data1 = str.ToCharArray();
                // byte[] ss = System.Text.Encoding.ASCII.GetBytes(str);
                // result.d_tag = Convert.ToInt32(data1[0]);
                result.d_tag = str.Substring(0, 2);
                result.data6_TS6=str.Substring(2,19);
            }
            else if(type=="08")
            {
                string str = des.DecryptString(msg.m_data, pwd);
                result.d_tag= str.Substring(0, 2);
                result.data5_Aut_c = str.Substring(2, 3);//      IDv
                result.data5_d5_tag = str.Substring(5, 1);//   msg
                result.data5_tkt_v = str.Substring(10, str.Length - 10);
            }
            else if(type=="10")
            {
                string str = des.DecryptString(msg.m_data, pwd);
                result.d_tag = str.Substring(0, 2);
                result.data5_Aut_c = str.Substring(2, 3);      //IDv
                string kk = str.Substring(5, str.Length - 5);
                string[] a = Regex.Split(kk, "####", RegexOptions.IgnoreCase);
 //               int index = a[1].IndexOf('\0');
  //              a[1] = a[1].Remove(index, a[0].Length - index);
                result.data5_d5_tag = a[0];   //text
                result.data5_tkt_v = a[1];      //
            }
            else if(type =="12")
            {
                string str = des.DecryptString(msg.m_data, pwd);
                result.d_tag = str.Substring(0, 2);
                result.data2_IDtgs= str.Substring(2, 3);     //IDv
                string kk = str.Substring(5, str.Length - 5);
                string[] a = Regex.Split(kk, "####", RegexOptions.IgnoreCase);
                int index = a[3].IndexOf('\0');
                result.data2_TS2 = a[0];//boook1
                result.data2_Lifetime1 = a[1];//book2
                result.data2_key_c_tgs = a[2];//book3
                result.data2_tkt_tgs = a[3];
            }
            return result;
        }
        public static string Enc_msg1(string IDc,string IDtgs,string TS1)//设置发送数据包的数据段
        {
            data result = new data();
            result.d_tag = "01";
            result.data1_IDc= IDc;
            result.data1_IDtgs= IDtgs;
            result.data1_TS1= TS1;
            message result1 = new message();
            result1.type = "01";
            //           result1.m_data = result.ToString();
            string str1 = result.d_tag + result.data1_IDc + result.data1_IDtgs + result.data1_TS1;
            string str = result1.type + result1.m_pwd+str1;
            return str;
        }
        public static string Enc_msg3(string IDv,string tkt_tgs,string Aut_c)
        {
            data result = new data();
            result.d_tag = "03";
            result.data3_IDv = IDv;
            result.data3_tkt_tgs = tkt_tgs;
            result.data3_d3_tag = "####";
            result.data3_Aut_c = Aut_c;
            message result1 = new message();
            result1.type = "03";
            result1.m_data = result.d_tag+result.data3_IDv+ result.data3_tkt_tgs+result.data3_d3_tag+result.data3_Aut_c;
            string str = result1.type  + result1.m_pwd +result1.m_data;
            return str;
        }
        public static string Enc_msg5( string tkt_v, string Aut_c)
        {
            data result = new data();
            result.d_tag = "05";
            result.data5_tkt_v = tkt_v;
            result.data5_d5_tag = "####";
            result.data5_Aut_c = Aut_c;
            message result1 = new message();
            result1.type = "05";
            result1.m_data = result.d_tag+result.data5_tkt_v+result.data5_d5_tag+result.data5_Aut_c+"####";
            string str = result1.type + result1.m_pwd + result1.m_data;
            return str;
        }
        public static string Enc_msg7( string IDc, string TS7, string oper, string book_name,  string H_IDc,string key)
        {
            data result = new data();
            result.d_tag = "07";
            result.data7_IDc = IDc;
            result.data7_TS7 = TS7;
            result.data7_oper = oper;
            result.data7_d7_tag2 = "####";
            result.data7_book_name = book_name;
            result.data7_d7_tag = "####";
            result.data7_H_IDc = H_IDc;
            message result1 = new message();
            result1.type = "07";
            result1.m_data = result.d_tag+result.data7_TS7+result.data7_oper+result.data7_d7_tag2+ result.data7_book_name+result.data7_d7_tag+result.data7_H_IDc+"####";
            string str = result1.type + result1.m_pwd + result.data7_IDc + "####"+ des.EncryptString(result1.m_data,key)+"####";
            return str;
        }
        public static  string Enc_msg9(string IDc,string H_IDc,string key)
        {
            data result = new data();
            result.d_tag = "09";
            result.data11_IDc = IDc;
            result.data11_H_IDc = H_IDc;
            message result1 = new message();
            result1.type = "09";
            result1.m_data = result.d_tag +"ref"+"####"+ result.data11_H_IDc+"####";
            string str = result1.type + result1.m_pwd + result.data11_IDc + des.EncryptString(result1.m_data,key)+"####";
            return str;
        }
        public static string Enc_msg13(string IDc,string pwd)
        {
            data result = new data();
            result.d_tag = "13";
            result.data11_IDc = IDc;
            result.data11_H_IDc = pwd;
            message result1 = new message();
            result1.type = "13";
            RSA R = new RSA();
            string str = result1.type + result1.m_pwd + result.d_tag + result.data11_IDc +result.data11_H_IDc+ R.EncryptString(result.data11_H_IDc, @"<RSAKeyValue><Modulus>t3YIHdHMuBrhFIVhv1iuwMkY5SdzHWpmSVo6l5w3KBxF0x2dmGBI8Rg5JNZyDA1e2a5gk45tK2YTaPqFQmgsu0lOTs8/86bozm
LEUYp0+PW61P0NLbPeti2SHv9tFy6mUwItc/1hfal8PGdMqEomBpuwKZFo+RHbdlCZrxPXofk=</Modulus><Exponent>AQAB</Exponent></RSAKeyVal
ue>
")+"####";
            return str;
        }
        public static message s_to_m(string str1)
        {
            message m = new message();
            m.type = str1.Substring(0,2);
            m.m_pwd = str1.Substring(2,4);
            m.m_data= str1.Substring(6,str1.Length-6);
            return m;
        }
  /*      public static int byteToInt2(byte[] b)
        {

            int mask = 0xff;
            int temp = 0;
            int n = 0;
            for (int i = 0; i < b.Length; i++)
            {
                n <<= 8;
                temp = b[i] & mask;
                n |= temp;
            }
            return n;
        }*/
    }
}
       
