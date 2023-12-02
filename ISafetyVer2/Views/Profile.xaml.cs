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

   

}