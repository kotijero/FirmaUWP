using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirmaDAL
{
    public class ArtiklDalProvider : IDalProvider<Artikl>
    {
        public Artikl Fetch(int Id)
        {
            string query = "SELECT * FROM Artikl WHERE Id = " + Id;
            DataTable result = QueryExecutor.ExecuteReaderQuery(query);
            if (result.Rows.Count < 1)
            {
                return null;
            }
            else
            {
                DataRow row = result.Rows[0];
                Artikl artikl = new Artikl
                {
                    SifArtikla = (int)row["SifArtikla"],
                    NazArtikla = (string)row["NazArtikla"],
                    JedMjere = (string)row["JedMjere"],
                    CijArtkila = (decimal)row["CijArtikla"],
                    ZastUsluga = (bool)row["ZastUsluga"],
                    SlikaArtikla = (byte[])row["SlikaArtikla"],
                    TekstArtikla = (string)row["TekstArtikla"]
                };
                return artikl;
            }
        }

        public List<Artikl> FetchAll()
        {
            string query = "SELECT * FROM Artikl";
            DataTable result = QueryExecutor.ExecuteReaderQuery(query);
            if (result.Rows.Count < 1)
            {
                return null;
            }
            else
            {
                List<Artikl> artiklList = new List<Artikl>();

                foreach(DataRow row in result.Rows)
                {
                    Artikl artikl = new Artikl
                    {
                        SifArtikla = row["SifArtikla"].GetType() == typeof(DBNull) ? 0 : (int)row["SifArtikla"],
                        NazArtikla = row["NazArtikla"].GetType() == typeof(DBNull) ? string.Empty : (string)row["NazArtikla"],
                        JedMjere = row["JedMjere"].GetType() == typeof(DBNull) ? string.Empty : (string)row["JedMjere"],
                        CijArtkila = row["CijArtikla"].GetType() == typeof(DBNull) ? (decimal)0.0 :(decimal)row["CijArtikla"],
                        ZastUsluga = row["ZastUsluga"].GetType() == typeof(DBNull) ? false : (bool)row["ZastUsluga"],
                        //SlikaArtikla = (byte[])row["SlikaArtikla"],
                        TekstArtikla = row["TekstArtikla"].GetType() == typeof(DBNull) ? string.Empty : (string)row["TekstArtikla"]
                    };
                    artiklList.Add(artikl);
                }
                
                return artiklList;
            }
        }

        public Artikl FetchAtPosition<TKey>(int position)
        {
            string query = "SELECT * FROM Artikl ORDER BY SifArtikla";
            DataTable result = QueryExecutor.ExecuteReaderQuery(query);
            if (result.Rows.Count < position)
            {
                return null;
            }
            else
            {
                DataRow row = result.Rows[position];
                Artikl artikl = new Artikl
                {
                    SifArtikla = (int)row["SifArtikla"],
                    NazArtikla = (string)row["NazArtikla"],
                    JedMjere = (string)row["JedMjere"],
                    CijArtkila = (decimal)row["CijArtikla"],
                    ZastUsluga = (bool)row["ZastUsluga"],
                    SlikaArtikla = (byte[])row["SlikaArtikla"],
                    TekstArtikla = (string)row["TekstArtikla"]
                };
                return artikl;
            }
        }

        public Artikl FetchByPKs(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public int ItemsCount()
        {
            string query = @"SELECT COUNT(*) FROM Artikl";
            DataTable result = QueryExecutor.ExecuteReaderQuery(query);
            return (int)result.Rows[0].ItemArray[0];
        }

        public void SaveChanges(IEnumerable<Artikl> changedItems, IEnumerable<Artikl> newItems, IEnumerable<Artikl> deletedItems)
        {
            throw new NotImplementedException();
        }
    }
}
