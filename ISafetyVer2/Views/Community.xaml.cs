namespace ISafetyVer2.Views;

public partial class Community : ContentPage
{
	public Community()
	{
		InitializeComponent();
	}

	
    private void Adminclick(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Admin());
    }


}