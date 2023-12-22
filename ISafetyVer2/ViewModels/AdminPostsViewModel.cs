using ISafetyVer2.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ISafetyVer2.Models;

namespace ISafetyVer2.ViewModels
{
    public class AdminPostsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private INavigation _navigation;
        private string _postTitle;
        public string PostTitle
        {
            get => _postTitle;
            set
            {
                _postTitle = value;
                RaisePropertyChanged(nameof(PostTitle));
            }
        }

        private string _postDescription;
        public string PostDescription
        {
            get => _postDescription;
            set
            {
                _postDescription = value;
                RaisePropertyChanged(nameof(PostDescription));
            }
        }

        private string _mediaURL;
        public string MediaURL
        {
            get => _mediaURL;
            set
            {
                _mediaURL = value;
                RaisePropertyChanged(nameof(MediaURL));
            }
        }

        private string _reportDateTime;
        public string ReportDateTime
        {
            get => _reportDateTime;
            set
            {
                _reportDateTime = value;
                RaisePropertyChanged(nameof(ReportDateTime));
            }
        }

        private string _location;
        public string Location
        {
            get => _location;
            set
            {
                _location = value;
                RaisePropertyChanged(nameof(Location));
            }
        }

        public ICommand SubmitCommand { get; private set; }
        public ICommand UploadPhotoCommand { get; private set; }

        public AdminPostsViewModel(INavigation navigation)
        {
            _navigation = navigation;
            SubmitCommand = new Command(async () => await SubmitPost());
            UploadPhotoCommand = new Command(async () => await UploadPhoto());
        }
        private async Task UploadPhoto()
        {
            try
            {
                var photo = await MediaPicker.PickPhotoAsync();
                if (photo != null)
                {
                    var mediaUrl = await new FirebaseHelper().UploadMediaToFirebase(photo.FullPath);
                    MediaURL = mediaUrl;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., user cancels the photo picker)
            }
        }
        private async Task SubmitPost()
        {
            AdminPost newPost = new AdminPost
            {
                PostTitle = this.PostTitle,
                Location = this.Location,
                PostDescription = this.PostDescription,
                MediaURL = this.MediaURL,
                ReportDateTime = this.ReportDateTime
            };

            var postId = await new FirebaseHelper().AddAdminPost(newPost);
            await App.Current.MainPage.DisplayAlert("Success", $"Post: \"{postId}\" added!", "OK");

            // Pop back to the previous page in the navigation stack
            if (_navigation != null)
            {
                await _navigation.PopAsync();
            }
            // Handle post submission result
        }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
