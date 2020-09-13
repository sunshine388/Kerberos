using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vserver1
{
    class Dic
    {
        public static Dictionary<string, string> myDictionary = new Dictionary<string, string>();

        public void addDic(string key,string value)
        {
            Dic.myDictionary.Add(key, value);

        }
    }
}
