using Firebase.Auth;
using ISafetyVer2.Services;
using ISafetyVer2.ViewModels;

namespace ISafetyVer2.Views;

public partial class AcountInformation : ContentPage
{
    private AccountInformationViewModel viewModel;

    public AcountInformation(string userId)
    {
        InitializeComponent();
        viewModel = new AccountInformationViewModel(userId);
        BindingContext = viewModel;
    }

    private void AIClick(object obj, EventArgs e)
    {
        Navigation.PopAsync();   // Safetytips1 in original.
    }
    




}