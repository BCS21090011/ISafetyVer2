using ISafetyVer2.Services;

namespace ISafetyVer2.Views;

public partial class SafetyTipsSubCatPage : ContentPage
{
	public SafetyTipsSubCatPage(string categoryID)
	{
		InitializeComponent();
		Say(categoryID);
        InitializeSubCategoriesDataAsync(categoryID);

    }

	private async void Say(string id)
	{
		await DisplayAlert("Got ID", $"ID: {id}", "OK");
	}
    private void Backclicksub(object obj, EventArgs e)
    {
        Navigation.PopAsync();   // Safetytips1 in original.
    }


    private async void InitializeSubCategoriesDataAsync(string categoryID)
    {
        var subCategoriesWithDescriptions = await new FirebaseHelper().GetSubCategoriesByCategoryID(categoryID);
        // Assume you have a ListView or CollectionView named SubCategoriesListView
        SubCategoriesListView.ItemsSource = subCategoriesWithDescriptions;
    }

}