using ISafetyVer2.Models;
using ISafetyVer2.Services;

namespace ISafetyVer2.Views;

public partial class CategoryPage : ContentPage
{
	public List<DBUser> authorities {  get; set; }

	public CategoryPage()
	{
		InitializeComponent();
		LoadAuthoritiesIntoPicker();
    }

	private async void LoadAuthoritiesIntoPicker()
	{
		authorities = await new FirebaseHelper().GetDBUserByRole("Admin");
		CatAuthoPicker.ItemsSource = authorities;
		CatAuthoPicker.ItemDisplayBinding = new Binding("UserName");
	}

	private async void AddCategoryBtnOnClick(object sender, EventArgs e)
	{
		DBUser selectedAuthority = (DBUser)CatAuthoPicker.SelectedItem;

		// Validate inputs:
		if (string.IsNullOrWhiteSpace(CategoryNameEntry.Text))
		{
			await DisplayAlert("Alert", "Category Name can't be empty!", "OK");
			return;
		}

		if (selectedAuthority == null)
		{
			await DisplayAlert("Alert", "Authority can't be empty!", "OK");
			return;
		}

		await new FirebaseHelper().AddCategory(new Category
		{
			CategoryName = CategoryNameEntry.Text,
			AuthorityID = selectedAuthority.UserID
		});

		// Inform insertion completed:
		await DisplayAlert("Completed", $"Category: \"{CategoryNameEntry.Text}\" added!", "Back");
		await Navigation.PopAsync();
	}

    private void BackCat(object obj, EventArgs e)
    {
        Navigation.PopAsync();   // Safetytips1 in original.
    }
}