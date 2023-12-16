namespace ISafetyVer2.Views;

public partial class QuickReportHistory : ContentPage
{
	public QuickReportHistory()
	{
		InitializeComponent();
	}

    private void QRHClick(object obj, EventArgs e)
    {
        Navigation.PopAsync();   // Safetytips1 in original.
    }
}