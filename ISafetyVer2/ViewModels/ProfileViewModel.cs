using System.Windows.Input;
using Microsoft.Maui.Controls;
using Firebase.Auth;
using ISafetyVer2.Views;
using System.ComponentModel;
using ISafetyVer2.Models;
using ISafetyVer2.Services;


namespace ISafetyVer2.ViewModels
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        public ICommand LogoutCommand { get; }


        private DBUser _currentUser;
        public DBUser CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
            }
        }

        public ProfileViewModel()
        {
            LogoutCommand = new Command(async () => await Logout());
            LoadUserData();
        }

        private async void LoadUserData()
        {
            string userId = Preferences.Get("UserID", string.Empty);
            if (!string.IsNullOrEmpty(userId))
            {
                CurrentUser = await new FirebaseHelper().GetDBUserByUserID(userId);
            }
        }

        public async Task Logout()
        {
            // Clear the stored Firebase token/session state
            Preferences.Remove("FreshFirebaseToken");
            Preferences.Remove("UserID");
            Preferences.Remove("UserDBID");
            Preferences.Remove("UserRole");

            // Navigate to Login Page
            await Shell.Current.GoToAsync("//LoginPage");
        }

        // Implement INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
