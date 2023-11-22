using ISafetyVer2.ViewModels;

namespace ISafetyVer2.Views;

public partial class SafetyTipsCategoriesPage : ContentPage
{
	public SafetyTipsCategoriesPage()
	{
		InitializeComponent();

		BindingContext = new SafetyTipsCategoriesViewModel(Navigation);
	}
}