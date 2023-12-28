using ISafetyVer2.ViewModels;
namespace ISafetyVer2.Views;

public partial class ReportedCase : ContentPage
{
    bool frameIsExpanded = false;
    public ReportedCase()
	{
		InitializeComponent();
        BindingContext = new ReportedCaseViewModel();
    }



    private void BackReportedCaseClick(object obj, EventArgs e)
    {
        Navigation.PopAsync();   // Safetytips1 in original.
    }

    private void OnFrameTapped(object sender, EventArgs e)
    {
        if (sender is Frame frame)
        {
            if (frameIsExpanded == true)
            {
                // Currently expanded.
                frame.HeightRequest = 200;  // Collapse it.
                frameIsExpanded = false;
            }
            else
            {
                // Currently collapsed.
                frame.HeightRequest = -1;  // Expand it.
                frameIsExpanded = true;
            }
        }
    }
}


