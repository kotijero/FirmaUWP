using FirmaBLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirmaBLL
{
    class MjestoBllProvider
    {
        public List<LookupModel> FetchLookup()
        {
            FirmaDAL.MjestoDalProvider dalProvider = new FirmaDAL.MjestoDalProvider();
            var res = dalProvider.FetchLookup();
            List<LookupModel> lookupList = new List<LookupModel>();
            foreach(KeyValuePair<int, string> kv in res)
            {
                lookupList.Add(new LookupModel { ID = kv.Key, Value = kv.Value });
            }
            return lookupList;
        }
    }
}
