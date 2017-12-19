using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
    public sealed partial class ArtiklDetails : Page
    {
        FirmaDAL.Artikl artiklModel;
        private FirmaDAL.Artikl originalArtikl;
        private bool changed;
        public bool inEditMode = false;

        public ArtiklDetails()
        {
            this.InitializeComponent();
            changed = false;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            artiklModel = e.Parameter as FirmaDAL.Artikl;
            inEditMode = false;
            changed = false;
        }
        


        private async void ToggleEditState()
        {
            if (inEditMode)
            {
                // Save changes?
                if (changed)
                {
                    bool response = await ConfirmSaveChanges();

                    if (response)
                    {
                        RemoveChangesNotifySettings();
                        inEditMode = false;
                    }
                }
                else
                {
                    RemoveChangesNotifySettings();
                    inEditMode = false;
                }
            }
            else
            {
                originalArtikl = new FirmaDAL.Artikl
                {
                    SifArtikla = artiklModel.SifArtikla,
                    NazArtikla = artiklModel.NazArtikla,
                    JedMjere = artiklModel.JedMjere,
                    CijArtkila = artiklModel.CijArtkila,
                    ZastUsluga = artiklModel.ZastUsluga,
                    SlikaArtikla = artiklModel.SlikaArtikla,
                    TekstArtikla = artiklModel.TekstArtikla
                };
                inEditMode = true;
                changed = false;
                SetChangesNotifySettings();
            }
            UpdateTextBoxStates();
        }

        private async System.Threading.Tasks.Task<bool> ConfirmSaveChanges()
        {
            ContentDialog saveChangesDialog = new ContentDialog
            {
                Title = "Changes detected",
                Content = "Do you want to save changes?",
                PrimaryButtonText = "Yes",
                SecondaryButtonText = "No",
                CloseButtonText = "Cancel"
            };

            ContentDialogResult result = await saveChangesDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                RemoveChangesNotifySettings();
                FirmaDAL.ArtiklDalProvider dalProvider = artiklModel.DalProvider;

                dalProvider.UpdateItem(artiklModel);
                inEditMode = false;
                UpdateTextBoxStates();

                return true;
            }
            else if (result == ContentDialogResult.Secondary)
            {
                RemoveChangesNotifySettings();
                artiklModel.NazArtikla = originalArtikl.NazArtikla;
                artiklModel.JedMjere = originalArtikl.JedMjere;
                artiklModel.CijArtkila = originalArtikl.CijArtkila;
                artiklModel.ZastUsluga = originalArtikl.ZastUsluga;
                artiklModel.SlikaArtikla = originalArtikl.SlikaArtikla;
                artiklModel.TekstArtikla = originalArtikl.TekstArtikla;

                inEditMode = false;
                UpdateTextBoxStates();

                return true;
            }
            else
            {
                return false;
            }
        }

        private void UpdateTextBoxStates()
        {
            NazivArtiklaTextBox.IsEnabled = inEditMode;
            CijenaArtiklaTextBox.IsEnabled = inEditMode;
            JedMjereTextBox.IsEnabled = inEditMode;
            ZastUslugaCheckBox.IsEnabled = inEditMode;
            TekstArtiklaTextBox.IsEnabled = inEditMode;
            SaveChangesButton.Visibility = inEditMode ? Visibility.Visible : Visibility.Collapsed;
            NewButton.Visibility = inEditMode ? Visibility.Collapsed : Visibility.Visible;

        }

        private void NotifyChanged(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            changed = true;
        }

        private void SetChangesNotifySettings()
        {
            NazivArtiklaTextBox.BeforeTextChanging += NotifyChanged;
            JedMjereTextBox.BeforeTextChanging += NotifyChanged;
            CijenaArtiklaTextBox.BeforeTextChanging += NotifyChanged;
            ZastUslugaCheckBox.Checked += ZastUslugaCheckBox_Checked;
            ZastUslugaCheckBox.Unchecked += ZastUslugaCheckBox_Checked;
            TekstArtiklaTextBox.BeforeTextChanging += NotifyChanged;
        }

        private void RemoveChangesNotifySettings()
        {
            NazivArtiklaTextBox.BeforeTextChanging -= NotifyChanged;
            JedMjereTextBox.BeforeTextChanging -= NotifyChanged;
            CijenaArtiklaTextBox.BeforeTextChanging -= NotifyChanged;
            ZastUslugaCheckBox.Checked -= ZastUslugaCheckBox_Checked;
            ZastUslugaCheckBox.Unchecked -= ZastUslugaCheckBox_Checked;
            TekstArtiklaTextBox.BeforeTextChanging -= NotifyChanged;
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog deleteDialog = new ContentDialog
            {
                Title = "Delete artikl",
                Content = "This function is not implemented yet",
                PrimaryButtonText = "Oh, okay"
            };
            await deleteDialog.ShowAsync();
        }

        private void ZastUslugaCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            NotifyChanged(null, null);
        }

        private async void BackRequested(object sender, System.ComponentModel.HandledEventArgs e)
        {
            e.Handled = true;
            bool response = await ConfirmSaveChanges();

            Frame rootFrame = Window.Current.Content as Frame;
            if (response && rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
            }
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ArtiklNew));
        }
    }
}
