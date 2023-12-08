using ISafetyVer2.ViewModels;
using Microsoft.Maui.Controls.Maps;
using System.ComponentModel;

namespace ISafetyVer2.Views;

public partial class QuickReportPage : ContentPage
{
	private Location incidentLocation;
	private double incidentRadius = 0;
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

	private void RadiusSliderOnChange(Object sender, ValueChangedEventArgs e)
	{
        double increment = 1; // Define your increment
        double newValue = Math.Round(e.NewValue / increment) * increment;
        RadiusSlider.Value = newValue;
		incidentRadius = newValue;
		RadiusSliderLbl.Text = $"Radius: {newValue} meters";
		MapAddLocation();	// Update the map.
		((QuickReportViewModel)BindingContext).IncidentRadius = newValue;
	}

	private void MapOnClicked(Object sender, MapClickedEventArgs e)
	{
		incidentLocation = e.Location;
		MapAddLocation();
		((QuickReportViewModel)BindingContext).IncidentLocation = e.Location;
    }

    private async void UploadImgBtnOnClick(Object sender, EventArgs e)
	{
		try
		{
			FileResult file = await MediaPicker.PickPhotoAsync();
			string filePath = null;

			if (file != null)
			{
				filePath = file.FullPath;
            }

			((QuickReportViewModel)BindingContext).MediaPath = filePath;
		}
		catch (Exception ex)
		{
			await DisplayAlert("Error", ex.Message, "OK");
		}
	}
    private async void UploadVidBtnOnClick(Object sender, EventArgs e)
    {
        try
        {
            FileResult file = await MediaPicker.PickVideoAsync();
            string filePath = null;

            if (file != null)
            {
                filePath = file.FullPath;
            }

            ((QuickReportViewModel)BindingContext).MediaPath = filePath;
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

	private void MapAddLocation()
	{
		// Clear pins and circles:
		map.Pins.Clear();
		map.MapElements.Clear();

		if (incidentRadius == 0)
		{
            map.Pins.Add(new Pin
            {
                Label = "Location",
                Location = incidentLocation
            });
        }
		else
		{
			map.MapElements.Add(new Circle
			{
				Center = incidentLocation,
				Radius = new Microsoft.Maui.Maps.Distance(incidentRadius),
                StrokeColor = Color.FromArgb("#B0FF0000"),
                StrokeWidth = 8,
                FillColor = Color.FromArgb("#70FF0000")
            });
		}
	}
}