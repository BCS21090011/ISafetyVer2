using ISafetyVer2.ViewModels;

namespace ISafetyVer2.Views;

public partial class Community : ContentPage
{
    bool frameIsExpanded = false;   // Will be collapsed initially.

    public Community()
    {
        InitializeComponent();
        BindingContext = new CommunityViewModel();
    }

    private void OnFrameTapped(object sender, EventArgs e)
    {
        if (sender is Frame frame)
        {
            if (frameIsExpanded == true)
            {
                // Currently expanded.
                frame.HeightRequest = 100;  // Collapse it.
                frameIsExpanded = false;
            }
            else
            {
                // Currently collapsed.
                frame.HeightRequest = 500;  // Expand it.
                frameIsExpanded = true;
            }
        }
    }
}
