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
        private string _descriptionText;

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

            return true;
        }

        private string _tempImagePath; // Temporary storage for the image path

        public void SetImageTempPath(string path)
        {
            _tempImagePath = path;
        }

        public async Task<string> InsertQRToDB()
        {
            // Upload the image to Firebase Storage
            var imageUrl = await UploadImageToFirebase(_tempImagePath);

            // Create a QuickReport object with all details including the image URL
            var quickReport = new QuickReport
            {
                UserID = Preferences.Get("UserID", "NoUserID"),
                SubCatID = SelectedSubCat?.SubCatID,
                ReportDateTime = DateTime.Now,
                QRDescription = DescriptionText,
                Latitude = 2.3407990m,
                Longitude = 111.8456972m,
                ImageUrl = imageUrl // Set the image URL here
            };

            // Save the QuickReport object to Firebase Realtime Database
            string qrID = await new FirebaseHelper().AddQuickReport(quickReport);
            return qrID;
        }

        private async Task<string> UploadImageToFirebase(string imagePath)
        {
            var stream = File.OpenRead(imagePath); // Open a stream to the image file
            var fileName = Path.GetFileName(imagePath); // Get the file name
            var storageImage = await new FirebaseStorage("isafety20231117.appspot.com")
                                     .Child("images")
                                     .Child(fileName)
                                     .PutAsync(stream);

            stream.Dispose(); // Dispose the stream
            return storageImage; // The URL of the image in Firebase Storage
        }

        private string GetCurrentUserId()
        {
            // Example: Retrieving user ID from application preferences or settings
            return Preferences.Get("UserID", "default_user_id");
        }

        public QuickReportViewModel(INavigation navigation)
        {
            _navigation = navigation;

            // Realtime:
            IDisposable observer1 = new FirebaseHelper().firebase
                .Child("Categories")
                .AsObservable<Category>()
                .Subscribe(cat => InitializeCategoriesDataAsync());

            IDisposable observer2 = new FirebaseHelper().firebase
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
