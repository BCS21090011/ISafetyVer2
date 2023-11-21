using ISafetyVer2.ViewModels;

namespace ISafetyVer2.Views;

public partial class RegisterPage : ContentPage
{
	public RegisterPage()
	{
		InitializeComponent();
		BindingContext = new RegisterViewModel(Navigation);
	}
}