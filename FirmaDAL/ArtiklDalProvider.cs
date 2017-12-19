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
            DataTable result = QueryExecutor.ExecuteQuery(query);
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
                    TekstArtikla = (string)row["TekstArtikla"],
                    DalProvider = this
                };
                return artikl;
            }
        }

        public List<Artikl> FetchAll()
        {
            string query = "SELECT * FROM Artikl";
            DataTable result = QueryExecutor.ExecuteQuery(query);
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
                        TekstArtikla = row["TekstArtikla"].GetType() == typeof(DBNull) ? string.Empty : (string)row["TekstArtikla"],
                        DalProvider = this
                    };
                    artiklList.Add(artikl);
                }
                
                return artiklList.OrderBy(t => t.NazArtikla).ToList();
            }
        }

        public Artikl FetchAtPosition<TKey>(int position)
        {
            string query = "SELECT * FROM Artikl ORDER BY SifArtikla";
            DataTable result = QueryExecutor.ExecuteQuery(query);
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
                    TekstArtikla = (string)row["TekstArtikla"],
                    DalProvider = this
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
            DataTable result = QueryExecutor.ExecuteQuery(query);
            return (int)result.Rows[0].ItemArray[0];
        }

        public void SaveChanges(IEnumerable<Artikl> changedItems, IEnumerable<Artikl> newItems, IEnumerable<Artikl> deletedItems)
        {
            if (changedItems != null)
            {
                foreach (var item in changedItems)
                {
                    UpdateItem(item);
                }
            }
            if (newItems != null)
            {
                foreach (var item in newItems)
                {
                    AddItem(item);
                }
            }
            if (deletedItems != null)
            {
                foreach (var item in deletedItems)
                {
                    DeleteItem(item);
                }
            }
        }
        public void AddItem(Artikl item)
        {
            string query = String.Format(@"INSERT INTO Artikl (NazArtikla, JedMjere, CijArtikla, ZastUsluga, TekstArtikla)
                                                VALUES ({0}, {1}, {2}, {3}, {4})",
                                                     item.NazArtikla,
                                                     item.JedMjere,
                                                     item.CijArtkila,
                                                     item.ZastUsluga ? 1 : 0,
                                                     item.TekstArtikla);
            QueryExecutor.ExecuteNonQuery(query);
        }

        public void UpdateItem(Artikl item)
        {
            string query = String.Format(@"UPDATE Artikl
                                              SET NazArtikla = '{0}',
                                                  JedMjere = '{1}',
                                                  CijArtikla = {2},
                                                  ZastUsluga = {3},
                                                  TekstArtikla = '{4}'
                                            WHERE SifArtikla = {5}",
                                                     item.NazArtikla,
                                                     item.JedMjere,
                                                     item.CijArtkila,
                                                     item.ZastUsluga ? 1 : 0,
                                                     item.TekstArtikla,
                                                     item.SifArtikla);
            QueryExecutor.ExecuteNonQuery(query);
        }

        

        public void DeleteItem(Artikl item)
        {
            string query = String.Format(@"DELETE FROM Artikl WHERE SifArtikla = {0}", item.SifArtikla);
            QueryExecutor.ExecuteNonQuery(query);
        }
    }
}
