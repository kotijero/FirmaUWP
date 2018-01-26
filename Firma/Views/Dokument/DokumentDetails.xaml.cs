using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Firma.Views.Dokument
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DokumentDetails : Page, INotifyPropertyChanged
    {
        #region NotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Constructors
        public DokumentDetails()
        {
            inEditMode = false;
            this.InitializeComponent();
            dokumentList = new ObservableCollection<FirmaDAL.Dokument>();
        }

        #endregion

        #region Private Members

        private ObservableCollection<FirmaDAL.Dokument> dokumentList;
        //private ObservableCollection<Models.StavkaLookupModel> currentStavkeList;
        private Models.DokumentLookupModel currentDokument;
        private int currentId;
        private int displayedId;

        private bool inEditMode;
        private bool inSaveMode;

        private List<FirmaBLL.Models.LookupModel> partnerLookupList;
        private List<FirmaBLL.Models.LookupModel> dokumentLookupList;
        private List<FirmaBLL.Models.LookupModel> artiklLookupList;

        #endregion

        #region Public Members
        public int CurrentId
        {
            get { return currentId + 1; }
            set
            {
                this.currentId = value - 1;
                this.OnPropertyChanged();
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

        public bool InSaveMode
        {
            get { return inSaveMode; }
            set
            {
                inSaveMode = value;
                OnPropertyChanged();
            }
        }

        //public ObservableCollection<Models.StavkaLookupModel> CurrentStavkeList
        //{
        //    get { return currentStavkeList; }
        //    set
        //    {
        //        currentStavkeList.Clear();
        //        foreach(var stavka in value)
        //        {
        //            currentStavkeList.Add(stavka);
        //        }
        //        OnPropertyChanged();
        //    }
        //}

        #endregion

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Navigation logic
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame.CanGoBack)
            {
                Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Windows.UI.Core.AppViewBackButtonVisibility.Visible;
            }
            else
            {
                Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Windows.UI.Core.AppViewBackButtonVisibility.Collapsed;
            }

            LoadingIndicator.IsActive = true;
            DetailsGrid.Visibility = Visibility.Collapsed;
            StavkeListView.Visibility = Visibility.Collapsed;
            InitializeData();
            LoadingIndicator.IsActive = false;
            DetailsGrid.Visibility = Visibility.Visible;
            StavkeListView.Visibility = Visibility.Visible;
        }

        private void InitializeData()
        {
            // initial data fetch:
            FirmaDAL.DokumentDalProvider dokumentDalProvider = new FirmaDAL.DokumentDalProvider();
            List<FirmaDAL.Dokument> fetchedDokumentList = dokumentDalProvider.FetchAll();

            dokumentLookupList = new List<FirmaBLL.Models.LookupModel>();
            //currentStavkeList = new ObservableCollection<Models.StavkaLookupModel>();

            // dokuments & lookup
            foreach (FirmaDAL.Dokument dokument in fetchedDokumentList)
            {
                dokumentList.Add(dokument);
                dokumentLookupList.Add(new FirmaBLL.Models.LookupModel { ID = dokument.IdDokumenta, Value = dokument.LookupData });
            }
            dokumentLookupList.Add(new FirmaBLL.Models.LookupModel { ID = -1, Value = "- Nije odabrano -" });

            // partner lookup:
            FirmaDAL.PartnerDalProvider partnerDalProvider = new FirmaDAL.PartnerDalProvider();
            partnerLookupList = new List<FirmaBLL.Models.LookupModel>();
            foreach (KeyValuePair<int, string> kv in partnerDalProvider.FetchLookup())
            {
                partnerLookupList.Add(new FirmaBLL.Models.LookupModel { ID = kv.Key, Value = kv.Value });
            }
            partnerLookupList.Add(new FirmaBLL.Models.LookupModel { ID = -1, Value = "-Odaberi partnera-" });

            // artikl lookup:
            FirmaDAL.ArtiklDalProvider artiklDalProvider = new FirmaDAL.ArtiklDalProvider();
            //var artiklDict = artiklDalProvider.FetchLookup();
            var artiklList = artiklDalProvider.FetchAll();
            artiklLookupList = new List<FirmaBLL.Models.LookupModel>();
            foreach (var art in artiklList)
                artiklLookupList.Add(new FirmaBLL.Models.LookupModel { ID = art.SifArtikla, Value = art.NazArtikla });
            artiklLookupList.Add(new FirmaBLL.Models.LookupModel { ID = -1, Value = "-Odaberi artikl" });

            // counter:
            TotalCountTextBlock.Text = fetchedDokumentList.Count.ToString();
            currentId = 0;
            displayedId = 0;

            currentDokument = new Models.DokumentLookupModel(dokumentList[displayedId], partnerLookupList, dokumentLookupList, artiklLookupList, artiklList);
            UpdateDocumentData();

            SaveDokumentButton.Visibility = Visibility.Collapsed;
            CancelDokumentButton.Visibility = Visibility.Collapsed;

            UpdateNavigation(null, null);
        }
        

        private void UpdateNavigation(Control sender, FocusDisengagedEventArgs args)
        {
            TotalCountTextBlock.Text = dokumentList.Count.ToString();

            if(currentId < 1)
            {
                PreviousButton.IsEnabled = false;
                NextButton.IsEnabled = true;
            } else if (currentId == dokumentList.Count - 1)
            {
                PreviousButton.IsEnabled = true;
                NextButton.IsEnabled = false;
            } else
            {
                PreviousButton.IsEnabled = true;
                NextButton.IsEnabled = true;
            }

            if (displayedId != currentId)
            {
                UpdateDocumentData();
            }
        }

        private void UpdateDocumentData()
        {
            displayedId = currentId;
            currentDokument.CopyFromDTO(dokumentList[displayedId]);
            //currentStavkeList.Clear();
            //foreach (var stavka in currentDokument.Stavke)
            //    currentStavkeList.Add(stavka);
        }

        #region Navigation Control

        private async void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (inEditMode)
            {
                InEditMode = await CheckAndSaveChanges();
                //foreach (var item in currentStavkeList)
                //{
                //    item.InEditMode = inEditMode;
                //}
                if (inEditMode) return;
            }
            CurrentId -= 1;
            UpdateNavigation(null, null);
        }

        private async void NextButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (inEditMode)
            {
                InEditMode = await CheckAndSaveChanges();
                //foreach (var item in currentStavkeList)
                //{
                //    item.InEditMode = inEditMode;
                //}
                if (inEditMode) return;
            }
            CurrentId += 1;
            UpdateNavigation(null, null);
        }

        private void PositionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (inEditMode)
            {
                EditDokumentButton_Click(null, null);
                if (inEditMode)
                {
                    return;
                }
            }
            int value = 0;
            if (!Int32.TryParse(PositionTextBox.Text, out value))
            {
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Input error",
                    Content = "Entered value is not a whole number.",
                    CloseButtonText = "OK"
                };
                PositionTextBox.Text = displayedId.ToString();
                return;
            }
            if (value >= dokumentList.Count)
            {
                CurrentId = dokumentList.Count;
            }
            else if (value < 1)
            {
                CurrentId = 1;
            }
            else
            {
                CurrentId = value;
            }
            UpdateNavigation(null, null);
        }

        #endregion

        

        #region EditMode

        private async void EditDokumentButton_Click(object sender, RoutedEventArgs e)
        {
            if (inEditMode)
            {
                
                InEditMode = await CheckAndSaveChanges();
                if (!inEditMode)
                {
                    NewStavkaButton.Visibility = Visibility.Collapsed;
                }
            } else
            {
                InEditMode = true;
                NewStavkaButton.Visibility = Visibility.Visible;
            }
            foreach (var item in currentDokument.Stavke)
            {
                item.InEditMode = inEditMode;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>InEditMode value</returns>
        private async Task<bool> CheckAndSaveChanges()
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Pohranjivanje promjena",
                Content = "Pohraniti promjene?",
                PrimaryButtonText = "Da",
                SecondaryButtonText = "Ne",
                CloseButtonText = "Odustani"
            };
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                FirmaDAL.DokumentDalProvider dalProvider = new FirmaDAL.DokumentDalProvider();
                dalProvider.UpdateItem(currentDokument.ToDTO());
                return false;
            }
            else if (result == ContentDialogResult.Secondary)
            {
                // revert changes...
                UpdateDocumentData();
                return false;
            } else
            {
                return true;
            }
        }

        #endregion

        private async void  DeleteDokumentButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Brisanje zapisa",
                Content = "Jeste li sigurni da želite obrisati zapis?",
                PrimaryButtonText = "Da",
                CloseButtonText = "Odustani"
            };

            ContentDialogResult result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                FirmaDAL.DokumentDalProvider dalProvider = new FirmaDAL.DokumentDalProvider();

                dalProvider.DeleteItem(currentDokument.ToDTO());
                dokumentList.RemoveAt(currentId);

                if (currentId >= dokumentList.Count )
                {
                    if (currentId == 0)
                    {
                        throw new NotImplementedException("One day...");
                    } else
                    {
                        currentId -= 1;
                    }
                }

                // This will trigger display refreshing:
                CurrentId = currentId + 1;
            }
        }

        private void NewDokumentButton_Click(object sender, RoutedEventArgs e)
        {
            SetSaveState(!inSaveMode);
        }

        private async void SetSaveState(bool newSaveState)
        {
            if (!newSaveState)
            {
                bool switchState = false;
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Spremanje",
                    Content = "Spremiti novi zapis dokumenta?",
                    PrimaryButtonText = "Da",
                    SecondaryButtonText = "Ne",
                    CloseButtonText = "Natrag na uređivanje"
                };
                var result = await dialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    switchState = true;

                    FirmaDAL.DokumentDalProvider dalProvider = new FirmaDAL.DokumentDalProvider();

                    dalProvider.AddItem(currentDokument.ToDTO());
                    dokumentList.Add(currentDokument.ToDTO());

                    CurrentId = dokumentList.Count;
                }
                else if (result == ContentDialogResult.Secondary)
                {
                    switchState = true;
                    CurrentId = currentId + 1;
                }
                else
                {
                    return;
                }
                
                if (switchState)
                {
                    NavigationStackPanel.Visibility = Visibility.Visible;
                    EditDokumentButton.IsEnabled = true;
                    DeleteDokumentButton.IsEnabled = true;

                    SaveDokumentButton.Visibility = Visibility.Collapsed;
                    CancelDokumentButton.Visibility = Visibility.Collapsed;
                    NewStavkaButton.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                currentDokument.CreateEmpty();
                //currentStavkeList.Clear();
                //foreach(var stavka in currentDokument.Stavke)
                //{
                //    stavka.InEditMode = true;
                //    currentStavkeList.Add(stavka);
                //}

                NavigationStackPanel.Visibility = Visibility.Collapsed;
                EditDokumentButton.IsEnabled = false;
                DeleteDokumentButton.IsEnabled = false;

                SaveDokumentButton.Visibility = Visibility.Visible;
                CancelDokumentButton.Visibility = Visibility.Visible;
                NewStavkaButton.Visibility = Visibility.Visible;

                InEditMode = true;
            }
        }

        private void NewStavkaButton_Click(object sender, RoutedEventArgs e)
        {
            currentDokument.Stavke.Add(new Models.StavkaLookupModel(artiklLookupList) { InEditMode = true });
        }

        private void DeleteStavkaButton_Click(object sender, RoutedEventArgs e)
        {
            var current = ((sender as Button).Tag as Models.StavkaLookupModel);
        }
    }
}
