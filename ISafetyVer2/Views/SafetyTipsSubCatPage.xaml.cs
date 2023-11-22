namespace ISafetyVer2.Views;

public partial class SafetyTipsSubCatPage : ContentPage
{
	public SafetyTipsSubCatPage(string categoryID)
	{
		InitializeComponent();
		Say(categoryID);
	}

	private async void Say(string id)
	{
		await DisplayAlert("Got ID", $"ID: {id}", "OK");
	}
}