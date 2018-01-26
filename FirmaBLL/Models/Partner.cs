using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Interfaces.Validation;
using Template10.Validation;

namespace FirmaBLL.Models
{
    public enum TipPartnera
    {
        Osoba,
        Tvrtka,
        Nedefinirano
    }
    public class Partner : ValidatableModelBase
    {
        public Partner()
        {

        }

        public Partner(FirmaDAL.Partner partner, List<LookupModel> mjestoLookupList, Action<IValidatableModel> validate)
        {
            this.IdPartnera = partner.IdPartnera;
            this.TipPartneraString = partner.TipPartnera;
            this.IdMjestaPartnera = partner.IdMjestaPartnera == null ? -1 : partner.IdMjestaPartnera;
            this.AdrPartnera = partner.AdrPartnera;
            this.IdMjestaIsporuke = partner.IdMjestaIsporuke == null ? -1 : partner.IdMjestaPartnera;
            this.AdrIsporuke = partner.AdrIsporuke;
            this.OIB = partner.OIB;
            if (partner.GetType().Equals(typeof(FirmaDAL.Osoba)))
            {
                FirmaDAL.Osoba osoba = partner as FirmaDAL.Osoba;
                this.ImeOsobe = osoba.ImeOsobe;
                this.PrezimeOsobe = osoba.PrezimeOsobe;
                this.NazivTvrtke = string.Empty;
                this.MatBrTvrtke = string.Empty;
            }
            if (partner.GetType().Equals(typeof(FirmaDAL.Tvrtka)))
            {
                FirmaDAL.Tvrtka tvrtka = partner as FirmaDAL.Tvrtka;
                this.ImeOsobe = string.Empty;
                this.PrezimeOsobe = string.Empty;
                this.NazivTvrtke = tvrtka.NazivTvrtke;
                this.MatBrTvrtke = tvrtka.MatBrTvrtke;
            }
            this.MjestoLookupList = mjestoLookupList;
            MjSjedistaLookup = mjestoLookupList.Where(t => t.ID == IdMjestaPartnera.Value).Single();
            MjIsporukeLookup = mjestoLookupList.Where(t => t.ID == IdMjestaIsporuke.Value).Single();

            this.Validator = e => validate.Invoke(e);
        }

        public Partner(Partner partner)
        {
            this.IdPartnera = partner.IdPartnera;
            this.TipPartneraString = partner.TipPartneraString;
            this.IdMjestaPartnera = partner.IdMjestaPartnera;
            this.AdrPartnera = partner.AdrPartnera;
            this.IdMjestaIsporuke = partner.IdMjestaPartnera;
            this.AdrIsporuke = partner.AdrIsporuke;
            this.OIB = partner.OIB;

            this.ImeOsobe = partner.ImeOsobe;
            this.PrezimeOsobe = partner.PrezimeOsobe;
            this.NazivTvrtke = partner.NazivTvrtke;
            this.MatBrTvrtke = partner.MatBrTvrtke;

            this.MjestoLookupList = partner.MjestoLookupList;

            MjSjedistaLookup = mjestoLookupList.Where(t => t.ID == IdMjestaPartnera.Value).Single();
            MjIsporukeLookup = mjestoLookupList.Where(t => t.ID == IdMjestaIsporuke.Value).Single();
        }
        #region Private Members

        private int idPartnera;
        private List<LookupModel> mjestoLookupList;

        #endregion

        #region Public Members

        public int IdPartnera
        {
            get { return idPartnera; }
            set
            {
                if (idPartnera != value)
                {
                    idPartnera = value;
                    // notify property changed?
                }
            }
        }

        public string TipPartneraString
        {
            get
            {
                if (TipPartnera == TipPartnera.Osoba) return "O";
                if (TipPartnera == TipPartnera.Tvrtka) return "T";
                return string.Empty;
            }
            set
            {
                if (value.Equals("O")) TipPartnera = TipPartnera.Osoba;
                if (value.Equals("T")) TipPartnera = TipPartnera.Tvrtka;
            }
        }

        public TipPartnera TipPartnera { get { return Read<TipPartnera>(); } set { Write(value); } }
        public int? IdMjestaPartnera { get { return Read<int?>(); } set { Write(value); } }
        public string AdrPartnera { get { return Read<string>(); } set { Write(value); } }
        public int? IdMjestaIsporuke { get { return Read<int?>(); } set { Write(value); } }
        public string AdrIsporuke { get { return Read<string>(); } set { Write(value); } }
        public string OIB { get { return Read<string>(); } set { Write(value); } }

        // Osoba:
        public string ImeOsobe { get { return Read<string>(); } set { Write(value); } }
        public string PrezimeOsobe { get { return Read<string>(); } set { Write(value); } }

        // Tvrtka:
        public string NazivTvrtke { get { return Read<string>(); } set { Write(value); } }
        public string MatBrTvrtke { get { return Read<string>(); } set { Write(value); } }

        // Lookup:
        public List<LookupModel> MjestoLookupList
        {
            get { return mjestoLookupList; }
            set
            {
                mjestoLookupList = value;
                RaisePropertyChanged();
            }
        }

        public LookupModel MjSjedistaLookup { get { return Read<LookupModel>(); } set { Write(value); } }
        public LookupModel MjIsporukeLookup { get { return Read<LookupModel>(); } set { Write(value); } }

        #endregion

        public FirmaDAL.Partner ToDTO()
        {
            if (TipPartnera == TipPartnera.Osoba)
            {
                return new FirmaDAL.Osoba
                {
                    IdPartnera = idPartnera,
                    TipPartnera = TipPartneraString,
                    IdMjestaPartnera = IdMjestaPartnera,
                    AdrPartnera = AdrPartnera,
                    IdMjestaIsporuke = IdMjestaIsporuke,
                    AdrIsporuke = AdrIsporuke,
                    OIB = OIB,
                    ImeOsobe = ImeOsobe,
                    PrezimeOsobe = PrezimeOsobe
                };
            }
            if (TipPartnera == TipPartnera.Tvrtka)
            {
                return new FirmaDAL.Tvrtka
                {
                    IdPartnera = idPartnera,
                    TipPartnera = TipPartneraString,
                    IdMjestaPartnera = IdMjestaPartnera,
                    AdrPartnera = AdrPartnera,
                    IdMjestaIsporuke = IdMjestaIsporuke,
                    AdrIsporuke = AdrIsporuke,
                    OIB = OIB,
                    NazivTvrtke = NazivTvrtke,
                    MatBrTvrtke = MatBrTvrtke
                };
            }

            return null;
        }

        public void CopyInto(Partner partner)
        {
            IdPartnera = partner.IdPartnera;
            TipPartneraString = partner.TipPartneraString;
            IdMjestaPartnera = partner.IdMjestaPartnera;
            AdrPartnera = partner.AdrPartnera;
            IdMjestaIsporuke = partner.IdMjestaIsporuke;
            AdrIsporuke = partner.AdrIsporuke;
            OIB = partner.OIB;
            NazivTvrtke = partner.NazivTvrtke;
            MatBrTvrtke = partner.MatBrTvrtke;

            MjSjedistaLookup = mjestoLookupList.Where(t => t.ID == IdMjestaPartnera).Single();
            MjIsporukeLookup = mjestoLookupList.Where(t => t.ID == IdMjestaIsporuke).Single();
        }

        
    }
}
