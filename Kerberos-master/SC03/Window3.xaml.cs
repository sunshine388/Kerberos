using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Window3.xaml 的交互逻辑
    /// </summary>
    public partial class Window3 : Window
    {
        public Window3()
        {
            InitializeComponent();
        }
        MainWindow w = new MainWindow();
        data d3 = new data();
        data d4 = new data();
        data d5 = new data();
        Window1 w1 = new Window1();
        RSA R = new RSA();
        private void button_click1(object sender, RoutedEventArgs e)
        {
            if (send_msg(message.Enc_msg7(w.get_IDc(), DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),"sea", get_book_name(), get_H_IDc(),w1.get_key_c_v())) == 1)
            {
                System.Windows.MessageBox.Show(textbox1.Text);
                System.Windows.MessageBox.Show("发送消息成功！");
            }
            rec_msg1();
        }
        private  string get_H_IDc()
        {
            string privatekey= @"<RSAKeyValue><Modulus>uhRRNLgDnQT4BqDRxzNDRnBmO9s3AeKgJQCY87di0LYiLjDe4kJqknne+Glw4RBSwqZjbS8DZywXG4jPR1ul+QzgGNPw+fdgAJ
yZRvp5UdIGwblqKT4e1gdu/gNCrzq0ua/3CsUdhNh/5lg0H2vbnWLxtz1UGddqOp16vOXI7jk=</Modulus><Exponent>AQAB</Exponent><P>1iJ0WEQB
hCEGK7qpsUbGkeWuc0RDXFnSPnTHULmfdn60INWOwPbdtPHpdr0BqSoi5gPQnJNV6EM8SFFwRm9ttw==</P><Q>3nWvOLoO0doSXnsLBaVTCcbeetBHtSOqh
M9VTG68dEwbgCnDJdRPbFf8bm4RkP03cRYRksx/eYlp5PthusqDjw==</Q><DP>ruSvL9RDhL5QQvEPXepXjpwQcrajac8256oLjMQ/GNH9nv+tu68lf39B1
qgirh7rKACfpvKzbKNVnh9d2zlxWw==</DP><DQ>W4wesb+Pvbh6ers0C/y/MXTPtcbk25xz3ffc6HlhwJJZduK41maK0NFcpq1ROVObb0RfswPJKeDs3Ti+
PTp97Q==</DQ><InverseQ>Xuu1p+Lrq30STgGE2vLU/rii68fmt03ndsfpqxpUya797KY1cOj6zLK8+lgKf5Ljq1PIM1SxdDGx4Ts05JkiNA==</Inverse
Q><D>SWJ13CquEiDBiCuoT70hzQfK6dQ8T1wG+FDSAVvTdqg9tqXDYMv8p6O0kZdT0w7S9L0V/RtLZkb3Hf7IcEXuoCuKxWkPz5+84eFOr4JAkWiuDqAaNfU
39LJzjszrmOutPoeEnr3fD4TLoUW8zbhUdRSYnAeSy+TurnnHQhm9Ynk=</D></RSAKeyValue>
";
            return  R.Sign(RSA.MD5Hash(w.get_IDc()),privatekey);
        }
        private string get_book_name()
        {
            return textbox1.Text;
        }
        private int send_msg(string str)
        {
            try
            {
                byte[] buffer = Encoding.Unicode.GetBytes(str);
                textbox3.Text = str;
                Window2.lib.c.Send(buffer, buffer.Length, 0);
                return 1;
            }
            catch
            {
                System.Windows.MessageBox.Show("发送失败！");
                return 0;
            }
        }
        private void rec_msg1()
        {
            try
            {
                byte[] buffer = new byte[1024 * 1024 * 3];
                //实际接收到的有效字节数
                int len = Window2.lib.c.Receive(buffer);
                if (len == 0)
                {
                    textbox3.AppendText("未收到消息！");
                }
                string str = Encoding.Unicode.GetString(buffer, 0, len);
                textbox3.AppendText(str);
                
                d3 = message.Dec_msg(message.s_to_m(str), w1.get_key_c_v());
           
            }
            catch (Exception e)
            {
                textbox3.AppendText(e.ToString());
            }
            if (R.SignCheck(d3.data5_Aut_c, d3.data5_tkt_v, @"<RSAKeyValue><Modulus>sIfR0O4uR80lTeY4pjE43OijKhdWKlXYs80bX8iAIsE/spkrxufhFL0D04IgquzeUz7CAvkRE62vs9WnUqLSjPPT7mEw6bRY3F
Xc1lxaDQA8kX2VPFzuEDjMDYvBLQbx2yNEfy16KiwhosXBDLl5X7SrUMV9IjyVOVJkFf4Egmk=</Modulus><Exponent>AQAB</Exponent></RSAKeyVal
ue>
"))
            {
                textbox3.AppendText("签名验证成功！");
                if(d3.data5_d5_tag=="1")
                {
                    textbox2.Text = "可浏览   "+textbox1.Text;
                }
                else if (d3.data5_d5_tag == "0")
                {
                    System.Windows.MessageBox.Show("没有此书籍");
                }
            }
            else
                textbox3.AppendText("此消息为假的！");
        }
        private void rec_msg2()
        {
            try
            {
                byte[] buffer = new byte[1024 * 1024 * 3];
                //实际接收到的有效字节数
                int len = Window2.lib.c.Receive(buffer);
                if (len == 0)
                {
                    textbox3.AppendText("未收到消息！");
                }
                string str = Encoding.Unicode.GetString(buffer, 0, len);
                textbox3.AppendText(str);
               
                d4 = message.Dec_msg(message.s_to_m(str), w1.get_key_c_v());

            }
            catch (Exception e)
            {
                textbox3.AppendText(e.ToString());
            }
            if (R.SignCheck(d4.data2_IDtgs, d4.data2_tkt_tgs, @"<RSAKeyValue><Modulus>sIfR0O4uR80lTeY4pjE43OijKhdWKlXYs80bX8iAIsE/spkrxufhFL0D04IgquzeUz7CAvkRE62vs9WnUqLSjPPT7mEw6bRY3F
Xc1lxaDQA8kX2VPFzuEDjMDYvBLQbx2yNEfy16KiwhosXBDLl5X7SrUMV9IjyVOVJkFf4Egmk=</Modulus><Exponent>AQAB</Exponent></RSAKeyVal
ue>
"))
            { 
                textbox3.AppendText("签名验证成功！");
                System.Windows.MessageBox.Show(d4.data2_TS2);
                System.Windows.MessageBox.Show(d4.data2_Lifetime1);
                System.Windows.MessageBox.Show(d4.data2_key_c_tgs);
                /*
                item1.Content = d4.data2_TS2;
                item2.Content = d4.data2_Lifetime1;
                item3.Content = d4.data2_key_c_tgs;
                */
                
            }
            else
                textbox3.AppendText("这是假消息！");
        }
        private void button_click3(object sender, RoutedEventArgs e)
        {
            /*
            if (combobox1.Text.Equals(d4.data2_TS2))
            {
                if (send_msg(message.Enc_msg7(w.get_IDc(), DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), "rea", d4.data2_TS2, get_H_IDc(), w1.get_key_c_v())) == 1)
                {
                    System.Windows.MessageBox.Show("发送消息成功！");
                    textbox2.Text = "";
                    rec_msg3();
                }              
            }
            else if (combobox1.Text.Equals(d4.data2_Lifetime1))
            {
                if (send_msg(message.Enc_msg7(w.get_IDc(), DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), "rea", d4.data2_Lifetime1, get_H_IDc(), w1.get_key_c_v())) == 1)
                {
                    System.Windows.MessageBox.Show("发送消息成功！");
                    textbox2.Text = "";
                    rec_msg3();
                }
                
            }
            else if (combobox1.Text.Equals(d4.data2_key_c_tgs))
            {
                if (send_msg(message.Enc_msg7(w.get_IDc(), DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), "rea", d4.data2_key_c_tgs, get_H_IDc(), w1.get_key_c_v())) == 1)
                {
                    System.Windows.MessageBox.Show("发送消息成功！");
                    textbox2.Text = "";
                    rec_msg3();
                }           
            }*/
             if (get_book_name() != "")
            {
               if (send_msg(message.Enc_msg7(w.get_IDc(), DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), "rea", get_book_name(), get_H_IDc(), w1.get_key_c_v())) == 1)
                {
                    System.Windows.MessageBox.Show("发送消息成功！");
                    textbox2.Text = "";
                    rec_msg3();
                }
            }     
        }
        private void rec_msg3()
        {
            try
            {
                byte[] buffer = new byte[1024 * 1024 * 3];
                //实际接收到的有效字节数
                int len = Window2.lib.c.Receive(buffer);
                if (len == 0)
                {
                    textbox3.AppendText("未收到消息！");
                }
                string str = Encoding.Unicode.GetString(buffer, 0, len);
                textbox3.AppendText(str);

                d5 = message.Dec_msg(message.s_to_m(str), w1.get_key_c_v());
                textbox2.AppendText(d5.data5_Aut_c);
                textbox2.AppendText(d5.data5_tkt_v);
                textbox2.AppendText(d5.data5_d5_tag);
                textbox2.AppendText(UnicodeToString(d5.data5_d5_tag));
            }
            catch (Exception e)
            {
                textbox3.AppendText(e.ToString());
            }
            if (R.SignCheck(d5.data5_Aut_c, d5.data5_tkt_v, @"<RSAKeyValue><Modulus>sIfR0O4uR80lTeY4pjE43OijKhdWKlXYs80bX8iAIsE/spkrxufhFL0D04IgquzeUz7CAvkRE62vs9WnUqLSjPPT7mEw6bRY3F
Xc1lxaDQA8kX2VPFzuEDjMDYvBLQbx2yNEfy16KiwhosXBDLl5X7SrUMV9IjyVOVJkFf4Egmk=</Modulus><Exponent>AQAB</Exponent></RSAKeyVal
ue>
"))
            { 
                textbox3.AppendText("签名验证成功！");
                textbox2.AppendText( d5.data5_d5_tag);              
            }
            else
                textbox3.AppendText("此消息为假的！");
        }
        
        /// <summary>
        /// 字符串转Unicode码
        /// </summary>
        /// <returns>The to unicode.</returns>
        /// <param name="value">Value.</param>
        private string StringToUnicode(string value)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(value);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i += 2)
            {
                // 取两个字符，每个字符都是右对齐。
                stringBuilder.AppendFormat("u{0}{1}", bytes[i + 1].ToString("x").PadLeft(2, '0'), bytes[i].ToString("x").PadLeft(2, '0'));
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Unicode转字符串
        /// </summary>
        /// <returns>The to string.</returns>
        /// <param name="unicode">Unicode.</param>
        private string UnicodeToString(string unicode)
        {
            string resultStr = "";
            string[] strList = unicode.Split('u');
            for (int i = 1; i < strList.Length; i++)
            {
                resultStr += (char)int.Parse(strList[i], System.Globalization.NumberStyles.HexNumber);
            }
            return resultStr;
        }
    }
}
