using Firebase.Auth;
using ISafetyVer2.ViewModels;

namespace ISafetyVer2.Views;

public partial class AcountInformation : ContentPage
{
	public AcountInformation()
    {
		InitializeComponent();
        
    }

    private void AIClick(object obj, EventArgs e)
    {
        Navigation.PopAsync();   // Safetytips1 in original.
    }
}