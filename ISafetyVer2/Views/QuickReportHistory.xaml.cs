using ISafetyVer2.ViewModels;

namespace ISafetyVer2.Views;

public partial class QuickReportHistory : ContentPage
{
	public QuickReportHistory()
	{
		InitializeComponent();
        BindingContext = new CommunityViewModel();
    }

    private void QRHClick(object obj, EventArgs e)
    {
        Navigation.PopAsync();   // Safetytips1 in original.
    }
}