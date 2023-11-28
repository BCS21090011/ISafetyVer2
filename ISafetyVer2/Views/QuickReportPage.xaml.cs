using ISafetyVer2.ViewModels;

namespace ISafetyVer2.Views;

public partial class QuickReportPage : ContentPage
{
	public QuickReportPage()
	{
		InitializeComponent();

		BindingContext = new QuickReportViewModel(Navigation);
	}

	private async void UploadImgBtnOnClick(Object sender, EventArgs e)
	{
		await DisplayAlert("Alert", "UploadImgBtn Clicked", "OK");
	}

    private async void SubmitBtnOnClick(Object sender, EventArgs e)
	{
		await DisplayAlert("Alert", "SubmitBtn Clicked", "OK");
	}

    private void Backclick(object obj, EventArgs e)
    {
       Navigation.PopAsync();   // Safetytips1 in original.
    }
}