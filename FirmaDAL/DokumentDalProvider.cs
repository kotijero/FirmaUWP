using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirmaDAL
{
    public class DokumentDalProvider : IDalProvider<Dokument>
    {
        public Dokument Fetch(int Id)
        {
            string query = @"SELECT * FROM Dokument WHERE IdDokumenta = " + Id;
            DataTable result = QueryExecutor.ExecuteQuery(query);
            if (result.Rows.Count < 1)
            {
                return null;
            }
            else
            {
                // Primary data:
                DataRow row = result.Rows[0];
                Dokument dokument = new Dokument
                {
                    IdDokumenta = (int)row["IdDokumenta"],
                    VrDokumenta = (string)row["VrDokumenta"],
                    BrDokumenta = (int)row["BrDokumenta"],
                    DatDokumenta = (DateTime)row["DatDokumenta"],
                    IdPartnera = (int)row["IdPartnera"],
                    IdPrethDokumenta = (int)row["IdPrethDokumenta"],
                    PostoPorez = (decimal)row["PostoPorez"],
                    IznosDokumenta = (decimal)row["IznosDokumenta"],
                    Stavke = new List<Stavka>()
                };

                // Partner:
                PartnerDalProvider partnerDalProvider = new PartnerDalProvider();
                dokument.Partner = partnerDalProvider.Fetch(dokument.IdPartnera);

                // PrethDokument:
                if (dokument.IdPrethDokumenta != null)
                {
                    query = "SELECT * FROM Dokument WHERE IdDokumenta = " + dokument.IdPrethDokumenta.Value;
                    result = QueryExecutor.ExecuteQuery(query);
                    row = result.Rows[0];
                    dokument.PrethodniDokument = new Dokument
                    {
                        IdDokumenta = (int)row["IdDokumenta"],
                        VrDokumenta = (string)row["VrDokumenta"],
                        BrDokumenta = (int)row["BrDokumenta"],
                        DatDokumenta = (DateTime)row["DatDokumenta"],
                        IdPartnera = (int)row["IdPartnera"],
                        IdPrethDokumenta = (int)row["IdPrethDokumenta"],
                        PostoPorez = (decimal)row["PostoPorez"],
                        IznosDokumenta = (decimal)row["IznosDokumenta"]
                    };
                }

                // Stavke:
                query = "SELECT IdStavke, IdDokumenta, Stavka.SifArtikla, KolArtikla, JedCijArtikla, PostoRabat, NazArtikla, JedMjere FROM Stavka JOIN Artikl ON Stavka.SifArtikla = Artikl.SifArtikla WHERE IdDokumenta = " + Id;
                result = QueryExecutor.ExecuteQuery(query);
                foreach(DataRow stRow in result.Rows)
                {
                    Stavka stavka = new Stavka
                    {
                        IdStvke = (int)row["IdStavke"],
                        IdDokumenta = (int)row["IdDokumenta"],
                        SifArtikla = (int)row["SifArtikla"],
                        KolArtikla = (decimal)row["KolArtikla"],
                        JedCijArtikla = (decimal)row["JedCijArtikla"],
                        PostoRabat = (decimal)row["PostoRabat"],
                        Artikl = new Artikl
                        {
                            SifArtikla = (int)row["SifArtikla"],
                            NazArtikla = (string)row["NazArtikla"],
                            JedMjere = (string)row["JedMjere"]
                        }
                    };
                    dokument.Stavke.Add(stavka);
                }
                return dokument;
            }
        }

        public List<Dokument> FetchAll()
        {
            string query = "SELECT * FROM Dokument";
            DataTable documentResult = QueryExecutor.ExecuteQuery(query);
            if (documentResult.Rows.Count < 1)
            {
                return null;
            }
            else
            {
                // Priprema - Stavke:
                query = "SELECT IdStavke, IdDokumenta, Stavka.SifArtikla, KolArtikla, JedCijArtikla, PostoRabat, NazArtikla, JedMjere FROM Stavka JOIN Artikl ON Stavka.SifArtikla = Artikl.SifArtikla";
                DataTable StavkeResult = QueryExecutor.ExecuteQuery(query);
                List<Stavka> stavkaList = new List<Stavka>();
                foreach(DataRow row in StavkeResult.Rows)
                {
                    Stavka stavka = new Stavka
                    {
                        IdStvke = (int)row["IdStavke"],
                        IdDokumenta = (int)row["IdDokumenta"],
                        SifArtikla = (int)row["SifArtikla"],
                        KolArtikla = (decimal)row["KolArtikla"],
                        JedCijArtikla = (decimal)row["JedCijArtikla"],
                        PostoRabat = (decimal)row["PostoRabat"],
                        Artikl = new Artikl
                        {
                            SifArtikla = (int)row["SifArtikla"],
                            NazArtikla = (string)row["NazArtikla"],
                            JedMjere = (string)row["JedMjere"]
                        }
                    };
                    stavkaList.Add(stavka);
                }

                // Priprema - Partneri:
                PartnerDalProvider partnerDalProvider = new PartnerDalProvider();
                List<Partner> partnerList = partnerDalProvider.FetchAll();

                // Dokument:
                List<Dokument> dokumentList = new List<Dokument>();
                foreach (DataRow row in documentResult.Rows)
                {
                    Dokument dokument = new Dokument
                    {
                        IdDokumenta = (int)row["IdDokumenta"],
                        VrDokumenta = (string)row["VrDokumenta"],
                        BrDokumenta = (int)row["BrDokumenta"],
                        DatDokumenta = (DateTime)row["DatDokumenta"],
                        IdPartnera = (int)row["IdPartnera"],
                        //IdPrethDokumenta = (int)(row["IdPrethDokumenta"] == DBNull.Value ? null : row["IdPrethDokumenta"]),
                        PostoPorez = (decimal)row["PostoPorez"],
                        IznosDokumenta = (decimal)row["IznosDokumenta"]
                        //Stavke = stavkaList.Where(t => t.IdDokumenta == (int)row["IdDokumenta"]).ToList(),
                        //Partner = partnerList.Where(t => t.IdPartnera == (int)row["IdPartnera"]).FirstOrDefault()
                    };
                    dokument.Stavke = stavkaList.Where(t => t.IdDokumenta == dokument.IdDokumenta).ToList();
                    dokument.Partner = partnerList.Where(t => t.IdPartnera == dokument.IdPartnera).FirstOrDefault();
                    dokumentList.Add(dokument);
                }

                // PrethDokument:
                foreach(Dokument dokument in dokumentList)
                {
                    if (dokument.IdPrethDokumenta != null)
                    {
                        dokument.PrethodniDokument = dokumentList.Where(t => t.IdDokumenta == dokument.IdPrethDokumenta).FirstOrDefault();
                    }
                }
                return dokumentList;
            }
        }

        public Dokument FetchAtPosition<TKey>(int position)
        {
            throw new NotImplementedException();
        }

        public Dokument FetchByPKs(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public int ItemsCount()
        {
            return (int)QueryExecutor.ExecuteQuery("SELECT COUNT(*) FROM Dokument").Rows[0].ItemArray[0];
        }

        public void SaveChanges(IEnumerable<Artikl> changedItems, IEnumerable<Artikl> newItems, IEnumerable<Artikl> deletedItems)
        {
            throw new NotImplementedException();
        }

        public void AddItem(Dokument item)
        {
            string query = String.Format(@"INSERT INTO Dokument (VrDokumenta, BrDokumenta, DatDokumenta, IdPartnera, IdPrethDokumenta, PostoPorez, IznosDokumenta)
                                            OUTPUT inserted.IdDokumenta VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6})",
                                            item.VrDokumenta,
                                            item.BrDokumenta,
                                            item.DatDokumenta,
                                            item.IdPartnera,
                                            item.IdPrethDokumenta,
                                            item.PostoPorez,
                                            item.IznosDokumenta);
            int result = (int)QueryExecutor.ExecuteQuery(query).Rows[0].ItemArray[0];
            foreach (Stavka stavka in item.Stavke)
            {
                query = String.Format(@"INSERT INTO Stavka (IdDokumenta, SifArtikla, KolArtikla, JedCijArtikla, PostoRabat)
                                                    VALUES ({0}, {1}, {2}, {3}, {4})",
                                                    result, stavka.SifArtikla, stavka.KolArtikla, stavka.JedCijArtikla, stavka.PostoRabat);
                QueryExecutor.ExecuteNonQuery(query);
            }
        }

        public void UpdateItem(Dokument item)
        {
            string query;
            if (item.IdPrethDokumenta != null)
            {
                query = String.Format(@"UPDATE Dokument
                                              SET VrDokumenta = '{0}',
                                                  BrDokumenta = {1},
                                                  DatDokumenta = '{2}',
                                                  IdPartnera = {3},
                                                  IdPrethDokumenta = {4},
                                                  PostoPorez = {5},
                                                  IznosDokumenta = {6}
                                            WHERE IdDokumenta = {7}",
                                            item.VrDokumenta,
                                            item.BrDokumenta,
                                            item.DatDokumenta.ToString(),
                                            item.IdPartnera,
                                            item.IdPrethDokumenta,
                                            item.PostoPorez,
                                            item.IznosDokumenta,
                                            item.IdDokumenta);
                QueryExecutor.ExecuteNonQuery(query);
            } else
            {
                query = String.Format(@"UPDATE Dokument
                                              SET VrDokumenta = '{0}',
                                                  BrDokumenta = {1},
                                                  DatDokumenta = '{2}',
                                                  IdPartnera = {3},
                                                  PostoPorez = {4},
                                                  IznosDokumenta = {5}
                                            WHERE IdDokumenta = {6}",
                                            item.VrDokumenta,
                                            item.BrDokumenta,
                                            item.DatDokumenta.ToString(),
                                            item.IdPartnera,
                                            item.PostoPorez,
                                            item.IznosDokumenta,
                                            item.IdDokumenta);
                QueryExecutor.ExecuteNonQuery(query);
            }
            foreach (Stavka stavka in item.Stavke)
            {
                query = String.Format(@"UPDATE Stavka
                                           SET IdDokumenta = {0},
                                               SifArtikla = {1},
                                               KolArtikla = {2},
                                               JedCijArtikla = {3},
                                               PostoRabat = {4}
                                         WHERE IdStavke = {5}",
                                         stavka.IdDokumenta,
                                         stavka.SifArtikla,
                                         stavka.KolArtikla,
                                         stavka.JedCijArtikla,
                                         stavka.PostoRabat,
                                         stavka.IdStvke);
                QueryExecutor.ExecuteNonQuery(query);
            }
        }

        public void DeleteItem(Dokument dokument)
        {
            string query = String.Format("DELETE FROM Stavka WHERE IdDokumenta = {0}", dokument.IdDokumenta);
            QueryExecutor.ExecuteNonQuery(query);
            query = String.Format("DELETE FROM Dokument WHERE IdDokumenta = {0}", dokument.IdDokumenta);
            QueryExecutor.ExecuteNonQuery(query);
        }

        public void SaveChanges(IEnumerable<Dokument> changedItems, IEnumerable<Dokument> newItems, IEnumerable<Dokument> deletedItems)
        {
            throw new NotImplementedException();
        }

        public Dictionary<int, string> FetchLookup()
        {
            string query = "SELECT IdDokumenta, BrDokumenta, IdPartnera";
            throw new NotImplementedException();
        }
    }
}
