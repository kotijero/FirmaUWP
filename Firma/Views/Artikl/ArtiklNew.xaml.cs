using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class ArtiklNew : Page
    {
        FirmaDAL.Artikl artiklModel;
        public ArtiklNew()
        {
            artiklModel = new FirmaDAL.Artikl();
            this.InitializeComponent();
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void BackRequested(object sender, System.ComponentModel.HandledEventArgs e)
        {
            ContentDialog saveChangesDialog = new ContentDialog
            {
                Title = "Save artikl",
                Content = "Do you want to save this item?",
                PrimaryButtonText = "Save",
                SecondaryButtonText = "Don't save",
                CloseButtonText = "Cancel"
            };

            ContentDialogResult result = await saveChangesDialog.ShowAsync();

            bool backConfirmed = false;

            if (result == ContentDialogResult.Primary)
            {
                FirmaDAL.ArtiklDalProvider dalProvider = artiklModel.DalProvider;
                dalProvider.AddItem(artiklModel);

                backConfirmed = true;
            } else if (result == ContentDialogResult.Primary)
            {
                backConfirmed = true;
            }

            if (backConfirmed)
            {
                Frame rootFrame = Window.Current.Content as Frame;
                if (rootFrame.CanGoBack) rootFrame.GoBack();
            }
        }
    }
}
