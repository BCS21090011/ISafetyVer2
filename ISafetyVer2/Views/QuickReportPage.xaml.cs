using ISafetyVer2.ViewModels;
using Microsoft.Maui.Controls.Maps;
using System.ComponentModel;

namespace ISafetyVer2.Views;

public partial class QuickReportPage : ContentPage
{
	public QuickReportPage()
	{
		InitializeComponent();

		BindingContext = new QuickReportViewModel(Navigation);
	}

	private void ShowMapGrid(Object sender, EventArgs e)
	{
		MapGrid.IsVisible = true;
	}

	private void HideMapGrid(Object sender, EventArgs e)
	{
		MapGrid.IsVisible = false;
	}

	private async void MapOnClicked(Object sender, MapClickedEventArgs e)
	{
		map.Pins.Clear();
		map.Pins.Add(new Pin
		{
			Label = "Pin",
			Location = e.Location
		});
		((QuickReportViewModel)BindingContext).IncidentLocation = e.Location;
    }

    private async void UploadImgBtnOnClick(Object sender, EventArgs e)
	{
		try
		{
			FileResult file = await MediaPicker.PickPhotoAsync();
			if (file == null)
			{
				return;
			}

			((QuickReportViewModel)BindingContext).MediaPath = file.FullPath;
		}
		catch (Exception ex)
		{
			await DisplayAlert("Error", ex.Message, "OK");
		}
	}

    private void Backclick(object obj, EventArgs e)
    {
       Navigation.PopAsync();   // Safetytips1 in original.
    }
}