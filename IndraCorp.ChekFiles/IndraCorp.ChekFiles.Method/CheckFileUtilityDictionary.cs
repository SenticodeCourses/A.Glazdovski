using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndraCorp.ChekFiles.Method
{
    [Serializable]
    public class CheckFileUtilityDictionary
    {
        public Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
    }
}
