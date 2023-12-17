using Firebase.Auth;
using ISafetyVer2.ViewModels;

namespace ISafetyVer2.Views;


public partial class Profile : ContentPage
{
    private ProfileViewModel _viewModel;

    public Profile()
    {
        InitializeComponent();
        _viewModel = new ProfileViewModel();
        BindingContext = _viewModel;
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        await _viewModel.Logout();
    }


    private async void AccountInfoOnClick(object sender, EventArgs e)
    {
        try
        {
            // Fetch the user ID from Preferences
            string userId = Preferences.Get("UserID", string.Empty);
            if (!string.IsNullOrEmpty(userId))
            {
                await Navigation.PushAsync(new AcountInformation(userId));
            }
            else
            {
                // Handle the case where the user ID is not found
                await DisplayAlert("Error", "User not logged in.", "OK");
            }
        }
        catch (Exception ex)
        {
            // Handle any other exceptions
            await DisplayAlert("Error", "An error occurred: " + ex.Message, "OK");
        }
    }
    private void ReportHistoryOnClick(object obj, EventArgs e)
    {
        Navigation.PushAsync(new QuickReportHistory());    // Quickreport in original.
    }

}