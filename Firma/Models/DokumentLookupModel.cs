using FirmaBLL.Models;
using FirmaDAL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Firma.Models
{
    public class DokumentLookupModel : INotifyPropertyChanged
    {
        #region Constructors
        public DokumentLookupModel(List<LookupModel> partnerLookupList, List<LookupModel> dokumentLookupList, List<LookupModel> artiklLookupList, List<Artikl> artiklList)
        {
            this.PartnerLookupList = partnerLookupList;
            this.DokumentLookupList = dokumentLookupList;
            this.ArtiklLookupList = artiklLookupList;

            this.artiklList = artiklList;

            this.Dokument1 = new HashSet<DokumentLookupModel>();
            this.Stavke = new ObservableCollection<StavkaLookupModel>();
        }

        public DokumentLookupModel(Dokument dokument, List<LookupModel> partnerLookupList, List<LookupModel> dokumentLookupList, List<LookupModel> artiklLookupList, List<Artikl> artiklList)
        {
            this.PartnerLookupList = partnerLookupList;
            this.DokumentLookupList = dokumentLookupList;
            this.ArtiklLookupList = artiklLookupList;

            this.artiklList = artiklList;

            this.Dokument1 = new HashSet<DokumentLookupModel>();
            this.Stavke = new ObservableCollection<StavkaLookupModel>();
            CopyFromDTO(dokument);
        }

        #endregion

        #region NotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Private variables

        private int idDokumenta;
        private string vrDokumenta;
        private int brDokumenta;
        private DateTime datDokumenta;
        private int idPartnera;
        private Nullable<int> idPrethDokumenta;
        private decimal postoPorez;
        private decimal iznosDokumenta;
        private ObservableCollection<StavkaLookupModel> stavke;

        private LookupModel partnerLookup;
        private LookupModel dokumentLookup;

        private List<Artikl> artiklList;

        #endregion

        #region Public properties

        public int IdDokumenta
        {
            get { return idDokumenta; }
            set
            {
                idDokumenta = value;
                OnPropertyChanged();
            }
        }
        public string VrDokumenta
        {
            get { return vrDokumenta; }
            set
            {
                vrDokumenta = value;
                OnPropertyChanged();
            }
        }
        public int BrDokumenta
        {
            get { return brDokumenta; }
            set
            {
                brDokumenta = value;
                OnPropertyChanged();
            }
        }
        public DateTime DatDokumenta
        {
            get { return datDokumenta; }
            set
            {
                datDokumenta = value;
                OnPropertyChanged();
            }
        }
        public int IdPartnera
        {
            get { return idPartnera; }
            set
            {
                idPartnera = value;
                OnPropertyChanged();
            }
        }
        public Nullable<int> IdPrethDokumenta
        {
            get { return idPrethDokumenta; }
            set
            {
                idPrethDokumenta = value;
                OnPropertyChanged();
            }
        }
        public decimal PostoPorez
        {
            get { return postoPorez; }
            set
            {
                postoPorez = value;
                OnPropertyChanged();
            }
        }
        public decimal IznosDokumenta
        {
            get { return iznosDokumenta; }
            set
            {
                iznosDokumenta = value;
                OnPropertyChanged();
            }
        }

        public LookupModel PartnerLookup
        {
            get { return partnerLookup; }
            set
            {
                partnerLookup = value;
                //OnPropertyChanged();
            }
        }

        public LookupModel DokumentLookup
        {
            get { return dokumentLookup; }
            set
            {
                partnerLookup = value;
                //OnPropertyChanged();
            }
        }

        public virtual ICollection<DokumentLookupModel> Dokument1 { get; set; }
        public virtual DokumentLookupModel PrethodniDokument { get; set; }
        public virtual FirmaDAL.Partner Partner { get; set; }
        public virtual ObservableCollection<StavkaLookupModel> Stavke
        {
            get { return stavke; }
            set
            {
                stavke = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Lookup


        public List<LookupModel> PartnerLookupList { get; set; }
        public List<LookupModel> DokumentLookupList { get; set; }
        public List<LookupModel> ArtiklLookupList { get; set; }


        #endregion


        public void CopyFromDTO(Dokument dokument)
        {
            IdDokumenta = dokument.IdDokumenta;
            VrDokumenta = dokument.VrDokumenta;
            BrDokumenta = dokument.BrDokumenta;
            DatDokumenta = dokument.DatDokumenta;
            IdPartnera = dokument.IdPartnera;
            IdPrethDokumenta = dokument.IdPrethDokumenta;
            PostoPorez = dokument.PostoPorez;
            IznosDokumenta = dokument.IznosDokumenta;
            //Dokument1 = dokument.Dokument1;
            Partner = dokument.Partner;

            Stavke.Clear();
            foreach (Stavka stavka in dokument.Stavke)
            {
                Stavke.Add(new StavkaLookupModel(stavka, ArtiklLookupList, artiklList));
            }
            foreach (var stLookup in Stavke)
            {
                stLookup.SetInitMode(false);
            }

            LookupModel newPartnerLookup = PartnerLookupList.Where(t => t.ID == idPartnera).Single();
            partnerLookup = PartnerLookupList.Where(t => t.ID == idPartnera).Single();
            OnPropertyChanged("PartnerLookup");

            if (dokument.PrethodniDokument != null)
            {
                dokumentLookup = DokumentLookupList.Where(t => t.ID == dokument.PrethodniDokument.IdDokumenta).Single();
                OnPropertyChanged("DokumentLookup");
            } else
            {
                dokumentLookup = DokumentLookupList.Where(t => t.ID == -1).Single();
                OnPropertyChanged("DokumentLookup");
            }
        }

        public Dokument ToDTO()
        {
            Dokument dokument = new Dokument
            {
                IdDokumenta = idDokumenta,
                BrDokumenta = brDokumenta,
                VrDokumenta = vrDokumenta,
                DatDokumenta = datDokumenta,
                IdPartnera = partnerLookup.ID,
                IdPrethDokumenta = DokumentLookup.ID,
                PostoPorez = postoPorez,
                IznosDokumenta = iznosDokumenta,
                Partner = Partner
            };
            return dokument;
        }

        public void CreateEmpty()
        {
            IdDokumenta = 0;
            BrDokumenta = 0;
            VrDokumenta = string.Empty;

            partnerLookup = PartnerLookupList.Where(t => t.ID == -1).Single();
            dokumentLookup = DokumentLookupList.Where(t => t.ID == -1).Single();

            OnPropertyChanged("PartnerLookup");
            OnPropertyChanged("DokumentLookup");
            DatDokumenta = DateTime.Now;

            Stavke.Clear();
            Stavke.Add(new Models.StavkaLookupModel(ArtiklLookupList));
        }
    }
}
