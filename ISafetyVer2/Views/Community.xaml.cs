namespace ISafetyVer2.Views;

public partial class Community : ContentPage
{
	public Community()
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

	private void QuickReportBtnOnClick(object sender, EventArgs e)
	{
		Navigation.PushAsync(new QuickReportPage());	// Quickreport in original.
	}
}