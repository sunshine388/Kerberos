using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kerberos
{
    class data
    {
        public string  d_tag;//数据标识符
        public data()
        {
            d_tag = "00";
        }
        //data(1)
        public string data1_IDc;
        public string data1_IDtgs;
        public string data1_TS1;
        public data(string IDc,string IDtgs, DateTime TS1)
        {
            this.d_tag = "01";
            this.data1_IDc = IDc;
            this.data1_IDtgs = IDtgs;
            this.data1_TS1 = TS1.ToString("yyyy/MM/dd HH:mm:ss");
        }
        //data(2)
        public string data2_key_c_tgs;
        public string data2_IDtgs;
        public string data2_TS2;
        public string data2_Lifetime1;
        //ticket
        public string data2_tkt_tgs;
        public data(string key_c_tgs, string IDtgs, string TS2, string Lifetime1,string tkt)
        {
            this.d_tag = "02";
            this.data2_key_c_tgs = key_c_tgs;
            this.data2_IDtgs = IDtgs;
            this.data2_TS2 = TS2;
            this.data2_Lifetime1 = Lifetime1;
            this.data2_tkt_tgs = tkt;
        }
        //data3
        public string data3_IDv;
        public string data3_tkt_tgs;
        public string data3_d3_tag;
        public string data3_Aut_c;
        public data(string IDv,string tkt_tgs,string d3_tag,string Aut_c)
        {
            this.d_tag = "03";
            this.data3_IDv =IDv;
            this.data3_tkt_tgs = tkt_tgs;
            this.data3_d3_tag = d3_tag;
            this.data3_Aut_c = Aut_c;
        }
        //data4
        public string data4_key_c_v;
        public string data4_IDv;
        public string data4_TS4;
        //ticket
        public string data4_tkt_v;
        public data(string key_c_v, string IDv, DateTime TS4, string tkt_v)
        {
            this.d_tag = "04";
            this.data4_key_c_v = key_c_v;
            this.data4_IDv = IDv;
            this.data4_tkt_v = tkt_v;
            this.data4_TS4= TS4.ToString("yyyy/MM/dd HH:mm:ss");
        }
        //data5
        public string data5_tkt_v;
        public string data5_d5_tag;
        public string data5_Aut_c;
        public data(string tkt_v, string Aut_c, string d5_tag)
        {
            this.d_tag = "05";
            this.data5_tkt_v = tkt_v;
            this.data5_d5_tag = d5_tag;
            this.data5_Aut_c = Aut_c;
        }
        //data6
        public string data6_TS6;
        public data(string TS6)
        {
            this.d_tag = "06";
            this.data6_TS6= TS6;
        }
        //data7   请求操作报文
        public string data7_IDc;
        public string data7_TS7;
        public string data7_oper;
        public string data7_d7_tag2;
        public string data7_book_name;
        public string data7_d7_tag;
        public string data7_H_IDc;
        public data(string TS7,string IDc,string oper,string d7_tag2,string book_name,string d7_tag,string H_IDc)
        {
            this.d_tag = "07";
            this.data7_IDc = IDc;
            this.data7_TS7 = TS7;
            this.data7_oper = oper;
            this.data7_d7_tag2 = d7_tag2;
            this.data7_book_name = book_name;
            this.data7_d7_tag = d7_tag;
            this.data7_H_IDc = H_IDc;
        }
        //data9
        public string data11_IDc;
        public string data11_H_IDc;
        public data(string IDc,string H_IDc)
        {
         //   this.d_tag = 9;
            this.data11_IDc = IDc;
            this.data11_H_IDc = H_IDc;
        }
    }
}
