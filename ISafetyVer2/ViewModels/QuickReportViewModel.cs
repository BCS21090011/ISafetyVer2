using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISafetyVer2.Models;
using ISafetyVer2.Services;

namespace ISafetyVer2.ViewModels
{
    public class QuickReportViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private INavigation _navigation;

        // Properties to be binded:
        private ObservableCollection<Category> _categories;
        private ObservableCollection<SubCategory> _subCategories;

        public ObservableCollection<Category> Categories
        {
            get => _categories;
            set
            {
                if (_categories != value)
                {
                    _categories = value;
                    RaisePropertyChanged(nameof(Categories));
                }
            }
        }

        public ObservableCollection<SubCategory> SubCategories
        {
            get => _subCategories;
            set
            {
                if (_subCategories != value)
                {
                    _subCategories = value;
                    RaisePropertyChanged(nameof(SubCategories));
                }
            }
        }

        // User inputs:
        public string subCatID { get; set; }
        public string qRDescription { get; set; }
        public decimal latitude { get; set; }
        public decimal longitude { get; set; }

        public async void ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(qRDescription))
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Description can't be empty!", "OK");
                return;
            }
        }

        public async void InsertQRToDB()
        {
            await new FirebaseHelper().AddQuickReport(new QuickReport
            {
                UserID = Preferences.Get("UserID", "NoUserID"),
                SubCatID = subCatID,
                ReportDateTime = DateTime.Now,  // Get current DateTime.
                QRDescription = qRDescription,
                Latitude = latitude,
                Longitude = longitude
            });
        }

        public QuickReportViewModel(INavigation navigation)
        {
            _navigation = navigation;

            // Realtime:
            IDisposable observer = new FirebaseHelper().firebase
                .Child("Categories")
                .AsObservable<Category>()
                .Subscribe(cat => InitializeCategoriesDataAsync());

            IDisposable observer2 = new FirebaseHelper().firebase
                .Child("SubCategories")
                .AsObservable<SubCategory>()
                .Subscribe(subcat => InitializeSubCatDataAsync());
        }

        private async Task InitializeCategoriesDataAsync()
        {
            List<Category> categories = await new FirebaseHelper().GetAllCategories();
            Categories = new ObservableCollection<Category>();
            foreach (Category category in categories)
            {
                Categories.Add(category);
            }
        }

        private async Task InitializeSubCatDataAsync()
        {
            List<SubCategory> subCategories = await new FirebaseHelper().GetAllSubCategories();
            SubCategories = new ObservableCollection<SubCategory>();
            foreach (SubCategory subCategory in subCategories)
            {
                SubCategories.Add(subCategory);
            }
        }

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
