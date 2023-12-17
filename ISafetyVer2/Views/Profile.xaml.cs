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

    private void AccountInfoOnClick(object obj, EventArgs e)
    {
        Navigation.PushAsync(new AcountInformation());    // Quickreport in original.
    }

    private void ReportHistoryOnClick(object obj, EventArgs e)
    {
        Navigation.PushAsync(new QuickReportHistory());    // Quickreport in original.
    }

}