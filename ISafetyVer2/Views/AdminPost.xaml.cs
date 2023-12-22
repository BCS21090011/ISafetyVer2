using ISafetyVer2.Models;
using ISafetyVer2.Services;
using ISafetyVer2.ViewModels;

namespace ISafetyVer2.Views;

public partial class AdminPost : ContentPage
{
	public AdminPost()
	{
		InitializeComponent(); 
        BindingContext = new AdminPostsViewModel(Navigation);
    }

    private void BackPost(object obj, EventArgs e)
    {
        Navigation.PopAsync();   // Safetytips1 in original.
    }
}