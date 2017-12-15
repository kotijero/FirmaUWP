using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firma.Misc
{
    public class OrderBy
    {
        public int OrderByValue { get; set; }
        public string OrderByName { get; set; }
        public OrderBy(int orderByValue, string orderByName)
        {
            OrderByValue = orderByValue;
            OrderByName = orderByName;
        }

        public override string ToString()
        {
            return OrderByName;
        }
    }
}
