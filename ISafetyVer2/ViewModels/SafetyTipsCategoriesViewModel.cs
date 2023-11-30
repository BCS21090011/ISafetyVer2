using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ISafetyVer2.Models;
using ISafetyVer2.Services;
using ISafetyVer2.Views;
// using System.Timers;

namespace ISafetyVer2.ViewModels
{
    public class SafetyTipsCategoriesViewModel : INotifyPropertyChanged
    {
        // private System.Timers.Timer _refreshTimer;

        public event PropertyChangedEventHandler PropertyChanged;
        private INavigation _navigation;

        public ICommand CategoryOnClicked { get; private set; }

        private ObservableCollection<Category> _categories;

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

        public SafetyTipsCategoriesViewModel(INavigation navigation)
        {
            CategoryOnClicked = new Command<string>(OnCategoryClicked);
            _navigation = navigation;

            // Realtime:
            IDisposable obserable = new FirebaseHelper().firebase
                .Child("Categories")
                .AsObservable<Category>()
                .Subscribe(cat => InitializeCategoriesDataAsync());

            /*
            // Update Categories at given time interval, not a good way but it can keep it sort of up to date.
            _refreshTimer = new System.Timers.Timer(60000);
            _refreshTimer.Elapsed += OnTimedEvent;
            _refreshTimer.AutoReset = true;
            _refreshTimer.Enabled = true;
            */
        }

        /*
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Categories = new ObservableCollection<Category>();
            InitializeCategoriesDataAsync();
        }
        */

        private async Task InitializeCategoriesDataAsync()
        {
            List<Category> categories = await new FirebaseHelper().GetAllCategories();
            Categories = new ObservableCollection<Category>();
            foreach (Category category in categories)
            {
                Categories.Add(category);
            }
        }

        private void OnCategoryClicked(string categoryID)
        {
            _navigation.PushAsync(new SafetyTipsSubCatPage(categoryID));
        }

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
    }
}
