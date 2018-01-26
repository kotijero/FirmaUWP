using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirmaDAL
{
    public class MjestoDalProvider : IDalProvider<Mjesto>
    {
        public Mjesto Fetch(int Id)
        {
            string query = "SELECT * FROM Mjesto WHERE Id = " + Id;
            DataTable result = QueryExecutor.ExecuteQuery(query);
            if (result.Rows.Count < 1)
            {
                return null;
            }
            else
            {
                DataRow row = result.Rows[0];
                Mjesto mjesto = new Mjesto
                {
                    IdMjesta = (int)row["IdMjesta"],
                    OznDrzave = (string)row["OznDrzave"],
                    NazMjesta = (string)row["NazMjesta"],
                    PostBrMjesta = (int)row["PostBrMjesta"],
                    PostNazMjesta = (string)row["PostNazMjesta"]
                };
                return mjesto;
            }
        }

        public List<Mjesto> FetchAll()
        {
            string query = "SELECT * FROM Mjesto";
            DataTable result = QueryExecutor.ExecuteQuery(query);
            if (result.Rows.Count < 1)
            {
                return new List<Mjesto>();
            }
            else
            {
                List<Mjesto> mjestoList = new List<Mjesto>();
                foreach(DataRow row in result.Rows)
                {
                    mjestoList.Add(new Mjesto
                    {
                        IdMjesta = (int)row["IdMjesta"],
                        OznDrzave = (string)row["OznDrzave"],
                        NazMjesta = (string)row["NazMjesta"],
                        PostBrMjesta = (int)row["PostBrMjesta"],
                        PostNazMjesta = (string)row["PostNazMjesta"]
                    });
                }
                return mjestoList;
            }
        }

        public Mjesto FetchAtPosition<TKey>(int position)
        {
            throw new NotImplementedException();
        }

        public Mjesto FetchByPKs(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public int ItemsCount()
        {
            string query = "SELECT COUNT(*) FROM Mjesto";
            DataTable result = QueryExecutor.ExecuteQuery(query);
            return (int)result.Rows[0].ItemArray[0];
        }

        public void SaveChanges(IEnumerable<Mjesto> changedItems, IEnumerable<Mjesto> newItems, IEnumerable<Mjesto> deletedItems)
        {
            throw new NotImplementedException();
        }

        public Dictionary<int, string> FetchLookup()
        {
            string query = "SELECT IdMjesta, NazMjesta FROM Mjesto";
            DataTable result = QueryExecutor.ExecuteQuery(query);

            Dictionary<int, string> res = new Dictionary<int, string>();
            foreach(DataRow row in result.Rows)
            {
                res.Add((int)row["IdMjesta"], (string)row["NazMjesta"]);
            }
            res.Add(-1, "-Nije odabrano-");
            return res;
        }
    }
}
