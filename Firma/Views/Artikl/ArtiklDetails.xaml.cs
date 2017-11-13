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
                    var dialog = new MessageDialog("Do you want to save changes?");
                    dialog.Commands.Add(new UICommand("Yes", new UICommandInvokedHandler(AnswerSaveChanges)));
                    dialog.Commands.Add(new UICommand("No", new UICommandInvokedHandler(AnswerSaveChanges)));
                    dialog.Commands.Add(new UICommand("Cancel", new UICommandInvokedHandler(AnswerSaveChanges)));
                    await dialog.ShowAsync();
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

        private void AnswerSaveChanges(IUICommand command)
        {
            if (command.Label.Equals("Yes"))
            {
                RemoveChangesNotifySettings();
                FirmaDAL.ArtiklDalProvider dalProvider = artiklModel.DalProvider;
                dalProvider.UpdateItem(artiklModel);
                inEditMode = false;
                UpdateTextBoxStates();
            }
            else if (command.Label.Equals("No"))
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
            }
        }

        private void UpdateTextBoxStates()
        {
            NazivArtiklaTextBox.IsEnabled = inEditMode;
            CijenaArtiklaTextBox.IsEnabled = inEditMode;
            JedMjereTextBox.IsEnabled = inEditMode;
            ZastUslugaCheckBox.IsEnabled = inEditMode;
            TekstArtiklaTextBox.IsEnabled = inEditMode;
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

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ZastUslugaCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            NotifyChanged(null, null);
        }
    }
}
