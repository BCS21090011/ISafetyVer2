using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISafetyVer2.Models;
using ISafetyVer2.Services;

namespace ISafetyVer2.ViewModels
{
    internal class RegisterViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public INavigation _navigation;

        public Command RegisterUserBtn { get; }

        private string email;
        private string password;
        private string name;
        private string phonenumber;

        public string Name
        {
            get => name;
            set
            {
                if (name != value)
                {
                    name = value;
                    RaisePropertyChanged(nameof(Name));
                }
            }
        }
        public string PhoneNumber
        {
            get => phonenumber;
            set
            {
                if (phonenumber != value)
                {
                    phonenumber = value;
                    RaisePropertyChanged(nameof(PhoneNumber));
                }
            }
        }
        public string Email
        {
            get => email;
            set
            {
                if (email != value)
                {
                    email = value;
                    RaisePropertyChanged(nameof(Email));
                }
            }
        }

        public string Password
        {
            get => password;
            set
            {
                if (password != value)
                {
                    password = value;
                    RaisePropertyChanged(nameof(Password));
                }
            }
        }

        public RegisterViewModel(INavigation navigation)
        {
            this._navigation = navigation;

            RegisterUserBtn = new Command(RegisterUserBtnOnClick);
        }

        private async void RegisterUserBtnOnClick(object obj)
        {
            // IMPORTANT!: Data validation before create new user in Authentication.
            // Else it might cause problems where user data in database is invalid / unable to create User in Users table in database but the email is already registered in Authentication.

            try
            {
                FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(Settings.FireBaseAuthAPIKey));
                FirebaseAuthLink auth = await authProvider.CreateUserWithEmailAndPasswordAsync(Email, Password);

                // Get UserID from authentication:
                User user = await authProvider.GetUserAsync(auth);
                string userID = user.LocalId;

                // These values should be binded:
                string userName = name;
                string userPhoneNumber = phonenumber;
                string userEmail = email;

                DBUser userDB = new DBUser {
                    UserID = userID,
                    UserName = userName,
                    UserPhoneNumber = userPhoneNumber,
                    UserEmail = userEmail,
                    Role = "User"
                };

                string userDBID = await new FirebaseHelper().AddUser(userDB);

                string token = auth.FirebaseToken;

                if (token != null)
                {
                    await App.Current.MainPage.DisplayAlert("Success!", "User Registered Successfully", "Login");
                }

                await this._navigation.PopAsync();
            }
            catch (Exception ex)
            {
                string reason = ex.Message;

                // Get only the reason:
                int found = reason.IndexOf("Reason: ");
                if (found != -1)    // If Reason is in ex.Message.
                {
                    reason = reason.Substring(found);
                }

                await App.Current.MainPage.DisplayAlert("Alert", reason, "OK");
            }
        }

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
