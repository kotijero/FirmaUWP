using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirmaDAL
{
    public class PartnerDalProvider : IDalProvider<Partner>
    {
        public Partner Fetch(int Id)
        {
            string query = "SELECT * FROM Partner WHERE Id = " + Id;
            DataTable result = QueryExecutor.ExecuteQuery(query);
            if (result.Rows.Count < 1)
            {
                return null;
            }
            else
            {
                DataRow row = result.Rows[0];
                if (row["TipPartnera"].Equals("T"))
                {
                    DataRow tvrtkaRow = QueryExecutor.ExecuteQuery("SELECT * FROM Tvrtka WHERE IdTvrtke = " + Id).Rows[0];
                    Tvrtka tvrtka = new Tvrtka
                    {
                        NazivTvrtke = (string)tvrtkaRow["NazivTvrtke"],
                        MatBrTvrtke = (string)tvrtkaRow["MatBrTvrtke"],
                        IdPartnera = (int)row["IdPartnera"],
                        TipPartnera = (string)row["TipPartnera"],
                        OIB = (string)row["OIB"],
                        IdMjestaPartnera = (int)row["IdMjestaPartnera"],
                        AdrPartnera = (string)row["AdrPartnera"],
                        IdMjestaIsporuke = (int)row["IdMjestaIsporuke"],
                        AdrIsporuke = (string)row["AdrIsporuke"]
                    };
                    return tvrtka;
                }
                else
                {
                    DataRow osobaRow = QueryExecutor.ExecuteQuery("SELECT * FROM Osoba WHERE IdOsobe = " + Id).Rows[0];
                    Osoba osoba = new Osoba
                    {
                        ImeOsobe = (string)osobaRow["ImeOsobe"],
                        PrezimeOsobe = (string)osobaRow["PrezimeOsobe"],
                        IdPartnera = (int)row["IdPartnera"],
                        TipPartnera = (string)row["TipPartnera"],
                        OIB = (string)row["OIB"],
                        IdMjestaPartnera = (int)row["IdMjestaPartnera"],
                        AdrPartnera = (string)row["AdrPartnera"],
                        IdMjestaIsporuke = (int)row["IdMjestaIsporuke"],
                        AdrIsporuke = (string)row["AdrIsporuke"]
                    };
                    return osoba;
                }
            }
        }

        public List<Partner> FetchAll()
        {
            string osobaQuery = @"SELECT PrezimeOsobe, 
		                                 ImeOsobe, 
	                                     IdPartnera, 
	                                     TipPartnera, 
	                                     OIB, 
	                                     IdMjestaPartnera, 
	                                     AdrPartnera, 
	                                     IdMjestaIsporuke, 
	                                     AdrIsporuke, 
	                                     MjIsporuke.OznDrzave AS OznDrzaveIsporuke,
	                                     MjIsporuke.NazMjesta AS NazMjestaIsporuke,
	                                     MjIsporuke.PostBrMjesta AS PostBrMjestaIsporuke,
	                                     MjIsporuke.PostNazMjesta AS PostNazMjestaIsporuke,
	                                     MjSjedista.OznDrzave AS OznDrzaveSjedista,
	                                     MjSjedista.NazMjesta AS NazMjestaSjedista,
	                                     MjSjedista.PostBrMjesta AS PostBrMjestaSjedista,
	                                     MjSjedista.PostNazMjesta AS PostNazMjestaSjedista
		                        FROM Osoba JOIN Partner ON Osoba.IdOsobe = Partner.IdPartnera 
                                LEFT JOIN Mjesto AS MjIsporuke ON  Partner.IdMjestaIsporuke = MjIsporuke.IdMjesta
                                LEFT JOIN Mjesto AS MjSjedista ON Partner.IdMjestaPartnera = MjSjedista.IdMjesta";
            DataTable osobaResult = QueryExecutor.ExecuteQuery(osobaQuery);

            string tvrtkaQuery = @"SELECT NazivTvrtke,
	                                      MatBrTvrtke,
	                                      IdPartnera, 
	                                      TipPartnera, 
	                                      OIB, 
	                                      IdMjestaPartnera, 
	                                      AdrPartnera, 
	                                      IdMjestaIsporuke, 
	                                      AdrIsporuke, 
	                                      MjIsporuke.OznDrzave AS OznDrzaveIsporuke,
	                                      MjIsporuke.NazMjesta AS NazMjestaIsporuke,
	                                      MjIsporuke.PostBrMjesta AS PostBrMjestaIsporuke,
	                                      MjIsporuke.PostNazMjesta AS PostNazMjestaIsporuke,
	                                      MjSjedista.OznDrzave AS OznDrzaveSjedista,
	                                      MjSjedista.NazMjesta AS NazMjestaSjedista,
	                                      MjSjedista.PostBrMjesta AS PostBrMjestaSjedista,
	                                      MjSjedista.PostNazMjesta AS PostNazMjestaSjedista
	                                FROM Tvrtka JOIN Partner ON Tvrtka.IdTvrtke = Partner.IdPartnera
	                                LEFT JOIN Mjesto AS MjIsporuke ON Partner.IdMjestaIsporuke = MjIsporuke.IdMjesta
	                                LEFT JOIN Mjesto AS MjSjedista On Partner.IdMjestaPartnera = MjSjedista.IdMjesta;";
            DataTable tvrtkaResult = QueryExecutor.ExecuteQuery(tvrtkaQuery);
            

            if (osobaResult.Rows.Count < 1 && tvrtkaResult.Rows.Count < 1)
            {
                return null;
            }
            else
            {
                List<Partner> partnerList = new List<Partner>();

                foreach (DataRow row in tvrtkaResult.Rows)
                {
                    Tvrtka tvrtka = new Tvrtka
                    {
                        IdPartnera = (int)row["IdPartnera"],
                        NazivTvrtke = (string)row["NazivTvrtke"],
                        MatBrTvrtke = (string)row["MatBrTvrtke"],
                        TipPartnera = (string)row["TipPartnera"],
                        OIB = (string)row["OIB"],
                        IdMjestaPartnera = (int?)(row["IdMjestaPartnera"] == DBNull.Value ? null : row["IdMjestaPartnera"]),
                        AdrPartnera = (string)(row["AdrPartnera"] == DBNull.Value ? string.Empty : row["AdrPartnera"]),
                        IdMjestaIsporuke = (int?)(row["IdMjestaIsporuke"] == DBNull.Value ? null : row["IdMjestaIsporuke"]),
                        AdrIsporuke = (string)(row["AdrIsporuke"] == DBNull.Value ? string.Empty : row["AdrIsporuke"])
                    };
                    if (tvrtka.IdMjestaIsporuke != null)
                    {
                        tvrtka.MjestoIsporuke = new Mjesto
                        {
                            IdMjesta = (int)row["IdMjestaIsporuke"],
                            NazMjesta = (string)row["NazMjestaIsporuke"],
                            OznDrzave = (string)row["OznDrzaveIsporuke"],
                            PostBrMjesta = (int)row["PostBrMjestaIsporuke"],
                            PostNazMjesta = (string)row["PostNazMjestaIsporuke"]
                        };
                    }
                    if (tvrtka.IdMjestaPartnera != null)
                    {
                        tvrtka.MjestoSjedista = new Mjesto
                        {
                            IdMjesta = (int)row["IdMjestaPartnera"],
                            NazMjesta = (string)row["NazMjestaSjedista"],
                            OznDrzave = (string)row["OznDrzaveSjedista"],
                            PostBrMjesta = (int)row["PostBrMjestaSjedista"],
                            PostNazMjesta = (string)row["PostNazMjestaSjedista"]
                        };
                    }
                    partnerList.Add(tvrtka);
                }
                foreach (DataRow row in osobaResult.Rows)
                {
                    Osoba osoba = new Osoba
                    {
                        
                        ImeOsobe = (string)row["ImeOsobe"],
                        PrezimeOsobe = (string)row["PrezimeOsobe"],
                        IdPartnera = (int)row["IdPartnera"],
                        TipPartnera = (string)row["TipPartnera"],
                        OIB = (string)row["OIB"],
                        IdMjestaPartnera = (int?)(row["IdMjestaPartnera"] == DBNull.Value ? null : row["IdMjestaPartnera"]),
                        AdrPartnera = (string)(row["AdrPartnera"] == DBNull.Value ? string.Empty : row["AdrPartnera"]),
                        IdMjestaIsporuke = (int?)(row["IdMjestaIsporuke"] == DBNull.Value ? null : row["IdMjestaIsporuke"]),
                        AdrIsporuke = (string)(row["AdrIsporuke"] == DBNull.Value ? string.Empty : row["AdrIsporuke"])
                    };
                    partnerList.Add(osoba);
                }
                return partnerList;
            }
        }

        public Partner FetchAtPosition<TKey>(int position)
        {
            throw new NotImplementedException();
        }

        public Partner FetchByPKs(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public int ItemsCount()
        {
            string query = @"SELECT COUNT(*) FROM Partner";
            DataTable result = QueryExecutor.ExecuteQuery(query);
            return (int)result.Rows[0].ItemArray[0];
        }

        public void SaveChanges(IEnumerable<Partner> changedItems, IEnumerable<Partner> newItems, IEnumerable<Partner> deletedItems)
        {
            throw new NotImplementedException();
        }

        public void AddItem(Partner item)
        {
            string query = String.Format(@"INSERT INTO Partner (IdPartnera, TipPartnera, OIB, IdMjestaPartnera, AdrPartnera, IdMjestaIsporuke, AdrIsporuke)
                                                VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6})",
                                                item.IdPartnera, 
                                                item.TipPartnera, 
                                                item.OIB, 
                                                item.IdMjestaIsporuke, 
                                                item.AdrPartnera, 
                                                item.IdMjestaIsporuke, 
                                                item.AdrIsporuke);
            QueryExecutor.ExecuteNonQuery(query);
        }

        public void UpdateItem(Partner item)
        {
            string query = String.Format(@"UPDATE Partner 
                                              SET TipPartnera = {1},
                                                  OIB = {2},
                                                  IdMjestaPartnera = {3},
                                                  AdrPartnera = {4},
                                                  IdMjestaIsporuke = {5}, 
                                                  AdrIsporuke = {6})
                                                WHERE IdPartnera = {0}",
                                                item.IdPartnera,
                                                item.TipPartnera,
                                                item.OIB,
                                                item.IdMjestaIsporuke,
                                                item.AdrPartnera,
                                                item.IdMjestaIsporuke,
                                                item.AdrIsporuke);
            QueryExecutor.ExecuteNonQuery(query);
        }

        public void DeleteItem(Partner item)
        {
            string query = String.Format(@"DELETE FROM Partner WHERE IdPartnera = {0}", item.IdPartnera);
            QueryExecutor.ExecuteNonQuery(query);
        }

        public Dictionary<int, string> FetchLookup()
        {
            string query = String.Format(@"SELECT IdPartnera, ImeOsobe, PrezimeOsobe, NazivTvrtke, TipPartnera FROM Partner LEFT JOIN Osoba ON Partner.IdPartnera = Osoba.IdOsobe
                                                                                                               LEFT JOIN Tvrtka ON Partner.IdPartnera = Tvrtka.IdTvrtke");
            DataTable result = QueryExecutor.ExecuteQuery(query);
            Dictionary<int, string> res = new Dictionary<int, string>();
            foreach(DataRow row in result.Rows)
            {
                if (row["TipPartnera"].Equals("T"))
                {
                    res.Add((int)row["IdPartnera"], (string)row["NazivTvrtke"]);
                }
                else
                {
                    res.Add((int)row["IdPartnera"], (string)row["ImeOsobe"] + " " + (string)row["PrezimeOsobe"]);
                }
            }
            return res;
        }

        public static Partner GenerateEmpty()
        {
            Partner partner = new Osoba
            {
                IdPartnera = 0,
                ImeOsobe = string.Empty,
                PrezimeOsobe = string.Empty,
                IdMjestaIsporuke = -1,
                AdrIsporuke = string.Empty,
                IdMjestaPartnera = -1,
                TipPartnera = "O",
                AdrPartnera = string.Empty,
                OIB = string.Empty
            };
            return partner;
        }
    }
}
