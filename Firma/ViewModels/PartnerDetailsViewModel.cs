using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Utils;
using Windows.UI.Xaml.Navigation;

namespace Firma.ViewModels
{
    public class PartnerDetailsViewModel : ViewModelBase
    {
        FirmaBLL.PartnerBllProvider _partnerBllProvider;

        bool loading;
        public PartnerDetailsViewModel()
        {
            currentPosition = 0;
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                // runtime
            }
            else
            {
                _partnerBllProvider = new FirmaBLL.PartnerBllProvider();
                Partners.AddRange(_partnerBllProvider.FetchAll());
                loading = false;
                //Load();
                //Task.Run(() => Partners.AddRange(_partnerBllProvider.FetchAll()));
            }
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            currentPosition = 0;
            Partners.AddRange(_partnerBllProvider.FetchAll());
            CurrentPartner.CopyInto(Partners[currentPosition]);
            await Task.CompletedTask;
            // return base.OnNavigatedToAsync(parameter, mode, state);
        }

        public async void Load()
        {
            loading = true;
            await Task.Run(() =>
            {
                Partners.AddRange(_partnerBllProvider.FetchAll());
                
            });
            RaisePropertyChanged("CurrentPartner");
            loading = false;
        }

        public ObservableCollection<FirmaBLL.Models.Partner> Partners { get; } = new ObservableCollection<FirmaBLL.Models.Partner>();

        FirmaBLL.Models.Partner _currentPartner = default(FirmaBLL.Models.Partner);
        public FirmaBLL.Models.Partner CurrentPartner
        {
            get
            {
                if (loading) return default(FirmaBLL.Models.Partner);
                return Partners[currentPosition];
            }
        }

        public void SaveCurrentPartner()
        {

        }

        #region Edit Mode

        private bool inEditMode;
        
        public bool InEditMode
        {
            get { return inEditMode; }
            set
            {
                if (inEditMode != value)
                {
                    inEditMode = value;
                    RaisePropertyChanged();
                    RaisePropertyChanged("NotInEditMode");
                }
            }
        }

        public bool NotInEditMode
        {
            get { return !inEditMode; }
        }

        private void SetEditMode(bool newEditMode)
        {

        }

        public void EditAction()
        {
            SetEditMode(true);
        }

        public void SaveAction()
        {

        }

        public void CancelAction()
        {

        }

        public void NewAction()
        {

        }

        public void DeleteAction()
        {

        }

        #endregion

        #region Navigation

        private int currentPosition;
        public int CurrentPosition
        {
            get { return currentPosition + 1; }
            set
            {
                if (currentPosition + 1 != value)
                {
                    if (value < 1)
                    {
                        currentPosition = 0;
                    } else if (value > Partners.Count) {
                        currentPosition = Partners.Count - 1;
                    } else
                    {
                        currentPosition = value - 1;
                    }
                    //UpdateCurrentPartner();
                    //RaisePropertyChanged("IsNotFirst");
                    //RaisePropertyChanged("IsNotLast");
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsNotFirst { get { return !(currentPosition <= 0); } }
        public bool IsNotLast { get { return !(currentPosition >= Partners.Count - 1); } }

        public void NextUser()
        {
            CurrentPosition += 1;
        }

        public void PreviousUser()
        {
            CurrentPosition -= 1;
        }

        private void UpdateCurrentPartner()
        {
            if (currentPosition <= 0)
            {
                CurrentPosition = 1;
            } else if (currentPosition >= Partners.Count)
            {
                CurrentPosition = Partners.Count;
            }
            // CurrentPartner.CopyInto(Partners[currentPosition]);
        }

        #endregion
    }
}
