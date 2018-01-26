using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace Firma.Views.Partner
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PartnerDetails : Page, INotifyPropertyChanged
    {
        #region NotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion


        public PartnerDetails()
        {
            this.InitializeComponent();
            inEditMode = true;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            FirmaBLL.PartnerBllProvider bllProvider = new FirmaBLL.PartnerBllProvider();
            partnerList = bllProvider.FetchAll();
            partnerModel = new FirmaBLL.Models.Partner(partnerList[0]);

            UpdatePartnerData();
            SetEditMode(false);
            UpdateNavigationLook();

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

        private void UpdatePartnerData(bool changeCurrentPartner = false)
        {
            if (currentId < 0)
            {
                CurrentId = 1;
                
                UpdateNavigationLook();
            }
            if (currentId >= partnerList.Count)
            {
                CurrentId = partnerList.Count;
                UpdateNavigationLook();
            }
            if (changeCurrentPartner)
            {
                partnerModel.CopyInto(partnerList[currentId]);
            }
            if (partnerModel.TipPartnera == FirmaBLL.Models.TipPartnera.Osoba)
            {
                OsobaGrid.Visibility = Visibility.Visible;
                TvrtkaGrid.Visibility = Visibility.Collapsed;

                OsobaRadioButton.IsChecked = true;
                TvrtkaRadioButton.IsChecked = false;
            }
            if (partnerModel.TipPartnera == FirmaBLL.Models.TipPartnera.Tvrtka)
            {
                OsobaGrid.Visibility = Visibility.Collapsed;
                TvrtkaGrid.Visibility = Visibility.Visible;

                OsobaRadioButton.IsChecked = false;
                TvrtkaRadioButton.IsChecked = true;
            }
            
        }

        //private FirmaDAL.Partner partnerModel;
        private FirmaBLL.Models.Partner partnerModel;
        private List<FirmaBLL.Models.Partner> partnerList;
        private int currentId;
        private bool inEditMode;

        #region Binding Properties
        public int CurrentId
        {
            get { return currentId + 1; }
            set
            {
                if (!currentId.Equals(value - 1))
                {
                    currentId = value - 1;
                    UpdatePartnerData(true);
                    OnPropertyChanged();
                }
            }
        }

        public bool InEditMode
        {
            get { return inEditMode; }
            set
            {
                if(!inEditMode.Equals(value))
                {
                    inEditMode = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Event handlers

        #region Navigation Event handlers
        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentId -= 1;
            UpdateNavigationLook();
        }

        private void PositionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateNavigationLook();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentId += 1;
            UpdateNavigationLook();
        }

        private void UpdateNavigationLook()
        {
            if (currentId < 1)
            {
                PreviousButton.IsEnabled = false;
                NextButton.IsEnabled = true;
            }
            else if (currentId >= partnerList.Count - 1)
            {
                PreviousButton.IsEnabled = true;
                NextButton.IsEnabled = false;
            }
            else
            {
                PreviousButton.IsEnabled = true;
                NextButton.IsEnabled = true;
            }
            TotalCountTextBlock.Text = partnerList.Count.ToString();
        }

        #endregion

        #region CRUD event handlers

        private void EditPartnerButton_Click(object sender, RoutedEventArgs e)
        {
            SetEditMode(true);
        }

        private void DeletePartnerButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void SavePartnerButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Spremanje",
                Content = "Spremiti zapis partnera?",
                PrimaryButtonText = "Da",
                SecondaryButtonText = "Ne",
                CloseButtonText = "Natrag na uređivanje"
            };
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                // save changes
                SetEditMode(false);
                UpdatePartnerData(true);
            }
            else if (result == ContentDialogResult.Secondary)
            {
                SetEditMode(false);
                UpdatePartnerData(true);
            }

        }

        private void CancelPartnerButton_Click(object sender, RoutedEventArgs e)
        {
            SetEditMode(false);
            UpdatePartnerData(true);
        }

        private void NewPartnerButton_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        #region Tip Partnera 
        private void TvrtkaRadioButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OsobaRadioButton_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #endregion

        #region Edit mode

        private void SetEditMode(bool newEditMode)
        {
            if (inEditMode == newEditMode) return;
            if (inEditMode) // to non-edit mode
            {
                // save changes?
                InEditMode = false;

                // navigation
                PositionTextBox.IsEnabled = true;
                UpdateNavigationLook();

                // appbar
                SavePartnerButton.Visibility = Visibility.Collapsed;
                CancelPartnerButton.Visibility = Visibility.Collapsed;

                EditPartnerButton.Visibility = Visibility.Visible;
            }
            else // to edit mode
            {
                InEditMode = true;

                // navigation
                PreviousButton.IsEnabled = false;
                PositionTextBox.IsEnabled = false;
                NextButton.IsEnabled = false;

                // appbar
                SavePartnerButton.Visibility = Visibility.Visible;
                CancelPartnerButton.Visibility = Visibility.Visible;

                EditPartnerButton.Visibility = Visibility.Collapsed;
            }
        }

        #endregion

        
    }
}
