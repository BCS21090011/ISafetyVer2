using System.ComponentModel;
using ISafetyVer2.Models;
using ISafetyVer2.Services;
using System.Collections.ObjectModel;

namespace ISafetyVer2.ViewModels
{
    public class AccountInformationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private DBUser _currentUser;
        public DBUser CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                RaisePropertyChanged(nameof(CurrentUser));
            }
        }

        public AccountInformationViewModel(string userId)
        {
            LoadUserData(userId);
        }

        private async void LoadUserData(string userId)
        {
            var user = await new FirebaseHelper().GetDBUserByUserID(userId);
            if (user != null)
            {
                CurrentUser = user;
            }
        }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
