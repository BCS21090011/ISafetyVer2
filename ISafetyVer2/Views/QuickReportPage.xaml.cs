using Firebase.Storage;
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

    private async void OnUploadImageTapped(object sender, EventArgs e)
    {
        try
        {
            var file = await MediaPicker.PickPhotoAsync();
            if (file == null) return;

            var filePath = file.FullPath;
             ((QuickReportViewModel)BindingContext).SetImageTempPath(filePath);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

}

