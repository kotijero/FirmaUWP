using FirmaBLL.Models;
using FirmaDAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Firma.Models
{
    public class StavkaLookupModel : INotifyPropertyChanged
    {
        #region Private Variables

        private int idStavke;
        private int idDokumenta;
        private int sifArtikla;
        private decimal kolArtikla;
        private decimal jedCijArtikla;
        private decimal postoRabat;

        private Artikl artikl;
        private DokumentLookupModel dokument;

        private bool inEditMode;
        private bool inInitMode;

        #endregion

        #region Constructors

        public StavkaLookupModel(Stavka stavka, List<LookupModel> artiklLookupList, List<Artikl> artiklList)
        {
            ArtiklLookupList = artiklLookupList;

            idStavke = stavka.IdStvke;
            idDokumenta = stavka.IdDokumenta;
            sifArtikla = stavka.SifArtikla;
            kolArtikla = stavka.KolArtikla;
            jedCijArtikla = stavka.JedCijArtikla;
            postoRabat = stavka.PostoRabat;
            artikl = stavka.Artikl;

            inEditMode = false;
            inInitMode = true;

            currentArtiklLookup = artiklLookupList.Where(t => t.ID == sifArtikla).Single();
            this.artiklList = artiklList;
        }

        public StavkaLookupModel(List<LookupModel> artiklLookupList)
        {
            ArtiklLookupList = artiklLookupList;

            inEditMode = false;

            sifArtikla = -1;
            currentArtiklLookup = artiklLookupList.Where(t => t.ID == -1).Single();
        }

        #endregion

        #region NotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this?.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// This method is used to prevent StackOverflow Exception during data initialization. When object is in initialization
        /// mode, the lookup mode will not trigger the OnPropertyChanged event.
        /// </summary>
        /// <param name="newInitMode"></param>
        public void SetInitMode(bool newInitMode)
        {
            inInitMode = newInitMode;
        }

        #endregion


        #region Public Properties

        public int IdStavke
        {
            get { return idStavke; }
            set
            {
                idStavke = value;
                OnPropertyChanged();
            }
        }

        public int IdDokumenta
        {
            get { return idDokumenta; }
            set
            {
                idDokumenta = value;
                OnPropertyChanged();
            }
        }

        public int SifArtikla
        {
            get { return sifArtikla; }
            set
            {
                sifArtikla = value;
                OnPropertyChanged();
            }
        }

        public decimal KolArtikla
        {
            get { return kolArtikla; }
            set
            {
                kolArtikla = value;
                OnPropertyChanged();
            }
        }

        public decimal JedCijArtikla
        {
            get { return jedCijArtikla; }
            set
            {
                jedCijArtikla = value;
                OnPropertyChanged();
            }
        }

        public decimal PostoRabat
        {
            get { return postoRabat; }
            set
            {
                postoRabat = value;
                OnPropertyChanged();
            }
        }

        public Artikl Artikl
        {
            get { return artikl; }
            set
            {
                artikl = value;
                OnPropertyChanged();
            }
        }

        public bool InEditMode
        {
            get { return inEditMode; }
            set
            {
                inEditMode = value;
                OnPropertyChanged();
            }
        }

        #region Forwarded Properties

        public string NazArtikla
        {
            get
            {
                return artikl == null ? string.Empty : artikl.NazArtikla;
            }
        }

        public string JedMjere
        {
            get
            {
                return artikl == null ? string.Empty : artikl.JedMjere;
            }
        }

        public decimal Ukupno
        {
            get
            {
                return jedCijArtikla * kolArtikla;
            }
        }

        

        #endregion

        #endregion

        #region Navigation Lookup

        private LookupModel currentArtiklLookup;

        public LookupModel CurrentArtiklLookup
        {
            get
            {
                return currentArtiklLookup;
            }
            set
            {
                if (!currentArtiklLookup.Equals(value))
                {
                    currentArtiklLookup = value;
                    if (artiklList.Where(t => t.SifArtikla == currentArtiklLookup.ID).Count() > 0)
                    Artikl = artiklList.Where(t => t.SifArtikla == currentArtiklLookup.ID).Single();
                    JedCijArtikla = artikl.CijArtkila;
                    if (!inInitMode)
                    {
                        OnPropertyChanged("JedMjere");
                        OnPropertyChanged("Ukupno");
                        OnPropertyChanged();
                    }
                }
            }
        }

        public List<LookupModel> ArtiklLookupList;

        private List<Artikl> artiklList;

        #endregion
    }
}
