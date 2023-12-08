namespace ISafetyVer2.Views;

public partial class AdminPost : ContentPage
{
	public AdminPost()
	{
		InitializeComponent();
	}

    private void BackPost(object obj, EventArgs e)
    {
        Navigation.PopAsync();   // Safetytips1 in original.
    }
}