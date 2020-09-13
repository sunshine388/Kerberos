using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;

namespace SC03
{
    public class DataBase1
    {
        OleDbConnection oleDb = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Administrator\Desktop\Kerberos\Kerbors\AS\SC03\AS.accdb");

        public DataBase1() //构造函数
        {
            oleDb.Open();
        }
        public string Getkey(string id)
        {
            string a;
            string sql = "select * from YH WHERE 用户名='" + id + "'";
            //获取用户表中用户名为b的内容

            OleDbDataAdapter dbDataAdapter = new OleDbDataAdapter(sql, oleDb); //创建适配对象
            DataSet dt = new DataSet();
            dbDataAdapter.Fill(dt); //用适配对象填充表对象

            // foreach (DataRow item in dt.Rows)
            // {
            // Console.WriteLine(item[0] + "  " + item[1]+" "+item[2]);


            // }
            a = dt.Tables[0].Rows[0][1].ToString();
            dt.Dispose();
            return a;

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
        public bool Add(string IDC,string key)
        {
            
            string sql = "insert into YH (用户名,密码) values ('"+IDC+"','"+key+"')";
            //往表1添加一条记录，昵称是LanQ，账号是2545493686
            OleDbCommand oleDbCommand = new OleDbCommand(sql, oleDb);
            int i = oleDbCommand.ExecuteNonQuery(); //返回被修改的数目
            return i > 0;
        }
        

    }
}
    

