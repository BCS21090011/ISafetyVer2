using ISafetyVer2.ViewModels;

namespace ISafetyVer2.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
		BindingContext = new LoginViewModel(Navigation);
	}
}