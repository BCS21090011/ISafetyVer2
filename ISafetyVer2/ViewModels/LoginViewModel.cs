using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth;
using ISafetyVer2.Views;
using ISafetyVer2.Models;
using Newtonsoft.Json;
using ISafetyVer2.Services;

namespace ISafetyVer2.ViewModels
{
    internal class LoginViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private INavigation _navigation;

        public Command LoginBtn { get; }
        public Command RegisterBtn { get; }

        private string userName;
        private string userPassword;

        public string UserName
        {
            get => userName;
            set
            {
                if (userName != value)
                {
                    userName = value;
                    RaisePropertyChanged(nameof(UserName));
                }
            }
        }

        public string UserPassword
        {
            get => userPassword;
            set
            {
                if (userPassword != value)
                {
                    userPassword = value;
                    RaisePropertyChanged(nameof(UserPassword));
                }
            }
        }

        public LoginViewModel(INavigation navigation)
        {
            this._navigation = navigation;

            LoginBtn = new Command(LoginBtnOnClick);
            RegisterBtn = new Command(RegisterBtnOnClick);
        }

        private async void LoginBtnOnClick(object obj)
        {
            FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(Settings.FireBaseAuthAPIKey));

            try
            {
                FirebaseAuthLink auth = await authProvider.SignInWithEmailAndPasswordAsync(UserName, UserPassword);
                FirebaseAuthLink content = await auth.GetFreshAuthAsync();

                // Get UserID:
                User user = await authProvider.GetUserAsync(auth);
                string userID = user.LocalId;
                DBUser userDB = await new FirebaseHelper().GetDBUserByUserID(userID);

                string serializedContent = JsonConvert.SerializeObject(content);
                Preferences.Set("FreshFirebaseToken", serializedContent);
                Preferences.Set("UserID", userID);
                Preferences.Set("UserDBID", userDB.UserID);
                Preferences.Set("UserRole", userDB.Role);
                Application.Current.MainPage = new AppShell();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
            }
        }

        private async void RegisterBtnOnClick(object obj)
        {
            await this._navigation.PushAsync(new RegisterPage());
        }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
