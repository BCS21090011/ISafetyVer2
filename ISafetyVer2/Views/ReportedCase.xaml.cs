namespace ISafetyVer2.Views;

public partial class ReportedCase : ContentPage
{
	public ReportedCase()
	{
		InitializeComponent();
	}



    private void BackReportedCaseClick(object obj, EventArgs e)
    {
        Navigation.PopAsync();   // Safetytips1 in original.
    }
}


