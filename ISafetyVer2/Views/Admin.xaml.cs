namespace ISafetyVer2.Views;

public partial class Admin : ContentPage
{
	public Admin()
	{
		InitializeComponent();
	}
    private void SubCatBtnOnClick(object sender, EventArgs e)
    {
        Navigation.PushAsync(new SubCategoryPage());
    }

    private void CategoryBtnOnClick(object sender, EventArgs e)
    {
        Navigation.PushAsync(new CategoryPage());
    }

    private void ReportedCase(object obj, EventArgs e)
    {
        Navigation.PushAsync(new ReportedCase());    // Quickreport in original.
    }

}