using System.Windows.Input;
using Microsoft.Maui.Controls;
using Firebase.Auth;
using ISafetyVer2.Views;
using System.ComponentModel;
// Ensure you have Xamarin.Essentials referenced

namespace ISafetyVer2.ViewModels
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        public ICommand LogoutCommand { get; }
        
        public ProfileViewModel()
        {
            LogoutCommand = new Command(async () => await Logout());
        }

        public async Task Logout()
        {
            // Clear the stored Firebase token/session state
            Preferences.Remove("FreshFirebaseToken"); // Replace with your actual preference key

            // Navigate to Login Page
            await Shell.Current.GoToAsync("//LoginPage"); // Update this navigation path as per your app's structure
        }

        // ... Other properties and methods ...

        // Implement INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
