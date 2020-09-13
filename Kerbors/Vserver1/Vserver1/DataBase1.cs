using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;
using System.IO;
using System.Windows;

namespace Vserver1
{
    public class DataBase1
    {
        OleDbConnection oleDb = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Administrator\Desktop\Kerberos\Kerbors\Vserver1\Vserver1\AS.accdb");
        byte[] byData = new byte[1000];
        char[] charData = new char[2000];
        public DataBase1() //构造函数
        {
            oleDb.Open();
        }

        public int compare1(string id)
        {

            int num = 0;
            string sql = "select * from YH ";
            //获取用户表中用户名为b的内容

            OleDbDataAdapter dbDataAdapter = new OleDbDataAdapter(sql, oleDb); //创建适配对象
            DataSet dt = new DataSet();
            dbDataAdapter.Fill(dt); //用适配对象填充表对象
            for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
            {
                if (id == dt.Tables[0].Rows[i][0].ToString())
                {
                    num = num + 1;
                }
            }
            dt.Dispose();
            return num;

        }
        public int compare2(string bookname)
        {

            int num = 0;
            string sql = "select * from BOOK ";
            //获取用户表中用户名为b的内容

            OleDbDataAdapter dbDataAdapter = new OleDbDataAdapter(sql, oleDb); //创建适配对象
            DataSet dt = new DataSet();
            dbDataAdapter.Fill(dt); //用适配对象填充表对象
            for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
            {
                if (bookname == dt.Tables[0].Rows[i][0].ToString())
                {
                    num = num + 1;
                }
            }
            dt.Dispose();
            return num;

        }

        public string gettxt(string bookname)
        {
            string a;
            string b;
          //  MessageBox.Show(bookname);
            string sql = "select * from BOOK WHERE 书名= '" + bookname + "'";
            OleDbDataAdapter dbDataAdapter = new OleDbDataAdapter(sql, oleDb); //创建适配对象
            DataSet dt = new DataSet();
            dbDataAdapter.Fill(dt); //用适配对象填充表对象
            int num = 0;
            for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
            {
                if (bookname == dt.Tables[0].Rows[i][0].ToString())
                {
                    num = num + 1;
                }
            }
            if (num == 0)
            {
                b="没有此书籍";
                dt.Dispose();
            }
            else
            {
                a = dt.Tables[0].Rows[0][1].ToString();
                dt.Dispose();
                b = Read(a);
            }

            return b;
        }
        public int getviewer(string bookname)
        {
            int a;
            string sql = "select * from BOOK WHERE 书名= '" + bookname + "'";
            OleDbDataAdapter dbDataAdapter = new OleDbDataAdapter(sql, oleDb); //创建适配对象
            DataSet dt = new DataSet();
            dbDataAdapter.Fill(dt); //用适配对象填充表对象
            a = Convert.ToInt32(dt.Tables[0].Rows[0][2].ToString());
            dt.Dispose();
            return a;
        }
        public void addviewer(string bookname)
        {
            string a;
            int c;
            string sql = "select * from BOOK WHERE 书名= '" + bookname + "'";
            OleDbDataAdapter dbDataAdapter = new OleDbDataAdapter(sql, oleDb); //创建适配对象
            DataSet dt = new DataSet();
            dbDataAdapter.Fill(dt); //用适配对象填充表对象
            c = Convert.ToInt32(dt.Tables[0].Rows[0][2]) + 1;
            a = c.ToString();
            dt.Dispose();
            // string myupdate = string.Format(@"UPDATE person SET 浏览次数='｛0｝' ,a");
            string sql1 = "update BOOK set 浏览次数='" + a + "' where 书名='" + bookname + "'";
            // oleDb.Parameters.AddWithValue(“@a1”, a);
            //将表BOOK中浏览次数加1
            OleDbCommand oleDbCommand = new OleDbCommand(sql1, oleDb);
            oleDbCommand.ExecuteNonQuery();
        }
        public void addfinder(string bookname)
        {
            int a;
            string sql = "select * from BOOK WHERE 书名= '" + bookname + "'";
            OleDbDataAdapter dbDataAdapter = new OleDbDataAdapter(sql, oleDb); //创建适配对象
            DataSet dt = new DataSet();
            dbDataAdapter.Fill(dt); //用适配对象填充表对象
            a = Convert.ToInt32(dt.Tables[0].Rows[0][3]) + 1;
            dt.Dispose();
            string sql1 = "update BOOK set 查询次数='" + a + "'where 书名='" + bookname + "'";
            //将表BOOK中浏览次数加1
            OleDbCommand oleDbCommand = new OleDbCommand(sql1, oleDb);
            oleDbCommand.ExecuteNonQuery();

        }
        public int getfinder(string bookname)
        {
            int a;
            string sql = "select * from BOOK WHERE 书名= '" + bookname + "'";
            OleDbDataAdapter dbDataAdapter = new OleDbDataAdapter(sql, oleDb); //创建适配对象
            DataSet dt = new DataSet();
            dbDataAdapter.Fill(dt); //用适配对象填充表对象
            a = Convert.ToInt32(dt.Tables[0].Rows[0][3].ToString());
            dt.Dispose();
            return a;
        }
        public string Read(string str)
        {
           
            string str2 = File.ReadAllText(str, Encoding.Unicode);
           // Console.WriteLine("文本:{0}", str2);

            return str2;

            // StreamReader sR2 = new StreamReader(@"F:\\学习C++\\Vserver1\\斗破苍穹.txt", Encoding.UTF8);

        }
        public string refresh()
        {
            string a;
            string sql = "select * from BOOK ";
            int d = 0;
            int b = 0;
            int c = 0;
            OleDbDataAdapter dbDataAdapter = new OleDbDataAdapter(sql, oleDb); //创建适配对象
            DataSet dt = new DataSet();
            dbDataAdapter.Fill(dt);
            List<int> scoreList = new List<int>();
            for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
            {
                scoreList.Add(Convert.ToInt32(dt.Tables[0].Rows[i][2]));
            }
            scoreList.Sort((x, y) => -x.CompareTo(y));//默认按升序排列，相当于：scoreList.Sort((x,y)=>x.CompareTo(y))
                                                      //Console.WriteLine(scoreList[0]);
            for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
            {
                if (Convert.ToInt32(dt.Tables[0].Rows[i][2]) == scoreList[0])
                {
                    b = i;
                    continue;
                }
                if (Convert.ToInt32(dt.Tables[0].Rows[i][2]) == scoreList[1])
                {
                    c = i;
                    continue;
                }
                if (Convert.ToInt32(dt.Tables[0].Rows[i][2]) == scoreList[2])
                {
                    d = i;
                    continue;
                }

            }
            a = string.Concat(dt.Tables[0].Rows[b][0].ToString(), "####", dt.Tables[0].Rows[c][0].ToString(), "####", dt.Tables[0].Rows[d][0].ToString());
            // }
            //if (i > 0)
            //{
            // a = string.Concat(a, string.Concat(dt.Tables[0].Rows[0][i].ToString(), dt.Tables[0].Rows[2][i].ToString(), dt.Tables[0].Rows[3][i].ToString()));





            dt.Dispose();
            return a;
        }

    }
}

    

