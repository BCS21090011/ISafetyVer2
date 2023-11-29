using ISafetyVer2.ViewModels;

namespace ISafetyVer2.Views;

public partial class SafetyTipsCategoriesPage : ContentPage
{
	public SafetyTipsCategoriesPage()
	{
		InitializeComponent();

		BindingContext = new SafetyTipsCategoriesViewModel(Navigation);
	}

    private void Backclickcatpage(object obj, EventArgs e)
    {
        Navigation.PopAsync();   // Safetytips1 in original.
    }
}