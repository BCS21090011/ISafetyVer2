using ISafetyVer2.Models;
using ISafetyVer2.Services;

namespace ISafetyVer2.Views;

public partial class SubCategoryPage : ContentPage
{
	private List<Category> categories;

	public SubCategoryPage()
	{
		InitializeComponent();
		LoadCategoriesIntoPicker();
	}

	private async void LoadCategoriesIntoPicker()
	{
		categories = await new FirebaseHelper().GetAllCategories();
		CategoryPicker.ItemsSource = categories;
		CategoryPicker.ItemDisplayBinding = new Binding("CategoryName");
	}

	private async void AddSubCategoryBtnOnClick(object sender, EventArgs e)
	{
		// Validate all inputs here:
		// For CategoryPicker:
		if (CategoryPicker.SelectedItem == null)
		{
			await DisplayAlert("Alert", "Category not choosen!", "OK");
			return;	// Exit the method to prevent the codes below to run.
		}

		// For SubCatNameEntry:
		if (string.IsNullOrWhiteSpace(SubCatNameEntry.Text))
		{
			await DisplayAlert("Alert", "Sub Category Name can't be empty!", "OK");
			return;
		}

		// For DangerLvlEntry:
		if (DangerLvlEntry.Text.Contains('.') || !int.TryParse(DangerLvlEntry.Text, out int dangerLvl))
		{
			await DisplayAlert("Alert", "Invalid Danger Level!", "OK");
			return;
		}

		Category selectedCategory = (Category)CategoryPicker.SelectedItem;

        SubCategory subCategory = new SubCategory
		{
			CategoryID = selectedCategory.CategoryID,
			SubCatName = SubCatNameEntry.Text,
			DangerLvl = dangerLvl,
            SafetyTipsDescription = SafetyTipsDescriptionEditor.Text
        };

		await new FirebaseHelper().AddSubCategory(subCategory);

        // Inform insertion completed:
        await DisplayAlert("Completed", $"Sub Category: \"{SubCatNameEntry.Text}\" added!", "Back");
        await Navigation.PopAsync();
    }

	// Will ensure the input is int, if it is not int, it will change back to old value.
	private void EntryEnsureInt(object sender, TextChangedEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(e.NewTextValue))
		{
			if (!e.NewTextValue.All(char.IsDigit))
			{
				((Entry)sender).Text = e.OldTextValue;	// Revert to the old value.
			}
		}
	}

    private void BackSubCat(object obj, EventArgs e)
    {
        Navigation.PopAsync();   // Safetytips1 in original.
    }
}