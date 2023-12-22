using System.ComponentModel;
using ISafetyVer2.Models;
using ISafetyVer2.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Diagnostics;

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

        public ICommand UpdateUserCommand { get; private set; }

        public AccountInformationViewModel(string userId)
        {
            LoadUserData(userId);
            UpdateUserCommand = new Command(async () => await UpdateUserAsync());

        }

        private async Task UpdateUserAsync()
        {
            bool result = await new FirebaseHelper().UpdateUser(CurrentUser);
            if (result)
            {
                // Notify the user of a successful update
                await App.Current.MainPage.DisplayAlert("Success", "Account information updated successfully.", "OK");
                
            }
            else
            {
                // Handle update failure
                await App.Current.MainPage.DisplayAlert("Error", "Failed to update account information. Please try again.", "OK");
            }
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
