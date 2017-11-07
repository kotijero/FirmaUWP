using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirmaDAL
{
    interface IDalProvider<T>
    {
        int ItemsCount();
        List<T> FetchAll();
        T Fetch(int Id);
        T FetchByPKs(params object[] keyValues);
        T FetchAtPosition<TKey>(int position);
        void SaveChanges(IEnumerable<T> changedItems, IEnumerable<T> newItems, IEnumerable<T> deletedItems);
    }
}
