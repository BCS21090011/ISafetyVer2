using ISafetyVer2.Models;
using ISafetyVer2.Services;

namespace ISafetyVer2.Views;

public partial class CategoryPage : ContentPage
{
	public CategoryPage()
	{
		InitializeComponent();
	}

	private async void AddCategoryBtnOnClick(object sender, EventArgs e)
	{
		// Validate inputs:
		if (string.IsNullOrWhiteSpace(CategoryNameEntry.Text))
		{
			await DisplayAlert("Alert", "Category Name can't be empty!", "OK");
			return;
		}

		await new FirebaseHelper().AddCategory(new Category
		{
			CategoryName = CategoryNameEntry.Text,
		});

		// Inform insertion completed:
		await DisplayAlert("Completed", $"Category: \"{CategoryNameEntry.Text}\" added!", "Back");
		await Navigation.PopAsync();
	}
}