using ISafetyVer2.Models;
using ISafetyVer2.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using System.ComponentModel;
using System.Diagnostics;

namespace ISafetyVer2.ViewModels
{
    public class ReportedCaseViewModel : BindableObject
    {
        private ObservableCollection<QRDetailed> _quickReports = new ObservableCollection<QRDetailed>();
        public ObservableCollection<QRDetailed> QuickReports
        {
            get => _quickReports;
            set
            {
                if (_quickReports != value)
                {
                    _quickReports = value;
                    OnPropertyChanged(nameof(QuickReports));
                }
            }
        }

        public ReportedCaseViewModel()
        {
            FetchReports();
        }

        private async Task FetchReports()
        {
            try
            {
                var firebaseHelper = new FirebaseHelper();
                var categories = await firebaseHelper.GetCategoryByAuthorityID(Preferences.Get("UserID", "NoUser"));

                // List<Task> allTasks = new List<Task>();
                //App.Current.MainPage.DisplayAlert("howard", $"{categories.Count}", "OK");
                foreach (var category in categories)
                {
                    /*QuickReports.Add(new QRCatDetailed
                    {
                        QRCat = category,
                        QRSubCat = await ProcessCategory(firebaseHelper, category)
                    });*/

                    // allTasks.Add(ProcessCategory(firebaseHelper, category));
                    await ProcessCategory(firebaseHelper, category);
                }

                // await Task.WhenAll(allTasks);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading reports: {ex.Message}");
            }
        }

        private async Task ProcessCategory(FirebaseHelper firebaseHelper, Category category)
        {
            var subCategories = await firebaseHelper.GetSubCategoriesByCategoryID(category.CategoryID);

            // List<QRSubCatDetailed> subCatDetaileds = new List<QRSubCatDetailed>();

            foreach (var subCategory in subCategories)
            {
                /*subCatDetaileds.Add(new QRSubCatDetailed
                {
                    QRSubCat = subCategory,
                    QR = await firebaseHelper.GetAllQuickReportBySubCatID(subCategory.SubCatID)
                });*/

                /*
                var reports = await firebaseHelper.GetAllQuickReportBySubCatID(subCategory.SubCatID);

                foreach (var report in reports)
                {
                    QuickReports.Add(report);
                }
                */

                var reports = await firebaseHelper.GetAllQuickReportBySubCatID(subCategory.SubCatID);

                foreach (var report in reports)
                {
                    QuickReports.Add(new QRDetailed
                    {
                        QR = report,
                        QRCat = category,
                        QRSubCat = subCategory
                    });
                }
            }

            // return subCatDetaileds;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
