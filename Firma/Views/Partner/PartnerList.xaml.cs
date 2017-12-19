using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Firma.Views.Partner
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PartnerList : Page
    {
        ObservableCollection<FirmaDAL.Partner> Partneri { get; } = new ObservableCollection<FirmaDAL.Partner>();
        public PartnerList()
        {
            this.InitializeComponent();
            PartnerListView.ItemsSource = Partneri;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            Partneri.Clear();
            FirmaDAL.PartnerDalProvider dalProvider = new FirmaDAL.PartnerDalProvider();
            LoadingIndicator.IsActive = true;
            List<FirmaDAL.Partner> result = await Task.Run(() => dalProvider.FetchAll());
            LoadingIndicator.IsActive = false;
            foreach (var item in result)
            {
                Partneri.Add(item);
            }

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
        }
    }
}
