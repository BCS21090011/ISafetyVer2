using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Storage;
using ISafetyVer2.Models;
using ISafetyVer2.Services;
using ISafetyVer2.Views;

namespace ISafetyVer2.ViewModels
{
    public class QuickReportViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private INavigation _navigation;

        public Command SubmitBtnOnClick { get; }

        // Properties to be binded:
        private Category _selectedCategory;
        private ObservableCollection<Category> _categories;
        private SubCategory _selectedSubCat;
        private ObservableCollection<SubCategory> _subCategories;
        private Location _incidentLocation = null;
        private string _descriptionText;

        public string MediaPath { get; set; } = null;

        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (_selectedCategory != value)
                {
                    _selectedCategory = value;
                    RaisePropertyChanged(nameof(SelectedCategory));
                    InitializeSubCatDataAsync();
                }
            }
        }

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

        public SubCategory SelectedSubCat
        {
            get => _selectedSubCat;
            set
            {
                if (_selectedSubCat != value)
                {
                    _selectedSubCat = value;
                    RaisePropertyChanged(nameof(SelectedSubCat));
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

        public Location IncidentLocation
        {
            get => _incidentLocation;
            set
            {
                if (_incidentLocation != value)
                {
                    _incidentLocation = value;
                    RaisePropertyChanged(nameof(IncidentLocation));
                }
            }
        }

        public string DescriptionText
        {
            get => _descriptionText;
            set
            {
                if (_descriptionText != value)
                {
                    _descriptionText = value;
                    RaisePropertyChanged(nameof(DescriptionText));
                }
            }
        }

        public async Task<bool> ValidateInput()
        {
            if (SelectedCategory == null)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Category can't be empty!", "OK");
                return false;
            }
            if (SelectedSubCat == null)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "SubCategory can't be empty!", "OK");
                return false;
            }
            if (string.IsNullOrWhiteSpace(DescriptionText))
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Description can't be empty!", "OK");
                return false;
            }
            if (IncidentLocation == null)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Location can't be empty!", "OK");
                return false;
            }

            return true;
        }

        public async Task<string> InsertQRToDB()
        {
            string mediaURL = "NoMedia";    // The MediaURL will be NoMedia if there is none.

            if (MediaPath != null)
            {
                mediaURL = await new FirebaseHelper().UploadMediaToFirebase(MediaPath);
            }

            string qrID = await new FirebaseHelper().AddQuickReport(new QuickReport
            {
                UserID = Preferences.Get("UserID", "NoUserID"),
                SubCatID = SelectedSubCat.SubCatID,
                ReportDateTime = DateTime.Now,  // Get current DateTime.
                QRDescription = DescriptionText,
                Latitude = IncidentLocation.Latitude,
                Longitude = IncidentLocation.Longitude,
                MediaURL = mediaURL,
                Status = "Pending"
            });

            return qrID;
        }

        public QuickReportViewModel(INavigation navigation)
        {
            _navigation = navigation;

            // Realtime:
            IDisposable observer1 = new FirebaseHelper().firebaseClient
                .Child("Categories")
                .AsObservable<Category>()
                .Subscribe(cat => InitializeCategoriesDataAsync());

            IDisposable observer2 = new FirebaseHelper().firebaseClient
                .Child("SubCategories")
                .AsObservable<SubCategory>()
                .Subscribe(subcat => InitializeSubCatDataAsync());

            SubmitBtnOnClick = new Command(SubmitQuickReport);
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
            List<SubCategory> subCategories = new List<SubCategory>();

            if (SelectedCategory != null)
            {
                subCategories = await new FirebaseHelper().GetSubCategoriesByCategoryID(SelectedCategory.CategoryID);
            }

            SubCategories = new ObservableCollection<SubCategory>();
            foreach (SubCategory subCategory in subCategories)
            {
                SubCategories.Add(subCategory);
            }
        }

        private async void SubmitQuickReport(object obj)
        {
            if (await ValidateInput() == true)
            {
                string qrID = await InsertQRToDB();
                await App.Current.MainPage.DisplayAlert("Completed", $"Sub Category: \"{qrID}\" added!", "Back");
                await _navigation.PopAsync();
            }
        }

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
