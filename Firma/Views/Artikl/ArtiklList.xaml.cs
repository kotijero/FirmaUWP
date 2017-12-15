using FirmaDAL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
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

namespace Firma.Views.Artikl
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ArtiklList : Page, INotifyPropertyChanged
    {
        FirmaDAL.Artikl persistedItem;
        ObservableCollection<FirmaDAL.Artikl> Artikli { get; } = new ObservableCollection<FirmaDAL.Artikl>();

        List<Misc.OrderBy> OrderByList = new List<Misc.OrderBy>();
        private Misc.OrderBy _currentOrderBy;

        public event PropertyChangedEventHandler PropertyChanged;

        public Misc.OrderBy CurrentOrderBy
        {
            get
            {
                return _currentOrderBy;
            }
            set
            {
                if (_currentOrderBy != value)
                {
                    _currentOrderBy = value;
                    RaisePropertyChanged("OrderBy");
                }
            }
        }
        public ArtiklList()
        {
            this.InitializeComponent();

            OrderByList.Add(new Misc.OrderBy(1, "Šifra artikla"));
            OrderByList.Add(new Misc.OrderBy(2, "Naziv artikla"));
            OrderByList.Add(new Misc.OrderBy(3, "Jedinica mjere"));
            OrderByList.Add(new Misc.OrderBy(4, "Cijena artikla"));
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Loading:
            Artikli.Clear();
            ArtiklDalProvider dalProvider = new ArtiklDalProvider();
            LoadingIndicator.IsActive = true;
            List<FirmaDAL.Artikl> result = await Task.Run(() => dalProvider.FetchAll());
            LoadingIndicator.IsActive = false;
            foreach (var item in result)
            {
                Artikli.Add(item);
            }

            // Navigation logic
            Frame rootFrame = Window.Current.Content as Frame;
            if(rootFrame.CanGoBack)
            {
                Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Windows.UI.Core.AppViewBackButtonVisibility.Visible;
            }
            else
            {
                Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Windows.UI.Core.AppViewBackButtonVisibility.Collapsed;
            }
        }

        private void ArtiklListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            persistedItem = e.ClickedItem as FirmaDAL.Artikl;
            this.Frame.Navigate(typeof(ArtiklDetails), e.ClickedItem);
        }

        private void OrderByComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            
            var result = comboBox.SelectedItem.ToString();
            //OrderBySelectionTextBlock.Text = result;
        }

        void RaisePropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
