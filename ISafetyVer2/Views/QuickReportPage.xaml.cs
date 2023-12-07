using ISafetyVer2.ViewModels;
using Microsoft.Maui.Controls.Maps;
using System.ComponentModel;

namespace ISafetyVer2.Views;

public partial class QuickReportPage : ContentPage, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    private Location _pinLocation = null;
	public Location PinLocation
	{
		get => _pinLocation;
		set
		{
			if (_pinLocation != value)
			{
				_pinLocation = value;
				RaisePropertyChanged(nameof(PinLocation));
			}
		}
	}
	public QuickReportPage()
	{
		InitializeComponent();

		BindingContext = new QuickReportViewModel(Navigation);
	}

	private async void MapOnClicked(Object sender, MapClickedEventArgs e)
	{
		map.Pins.Clear();
		map.Pins.Add(new Pin
		{
			Label = "Pin",
			Location = e.Location
		});
		PinLocation = e.Location;
    }

    private async void UploadImgBtnOnClick(Object sender, EventArgs e)
	{
		await DisplayAlert("Alert", "UploadImgBtn Clicked", "OK");
	}

    private async void SubmitBtnOnClick(Object sender, EventArgs e)
	{
		await DisplayAlert("Alert", "SubmitBtn Clicked", "OK");
	}

    private void Backclick(object obj, EventArgs e)
    {
       Navigation.PopAsync();   // Safetytips1 in original.
    }

    public void RaisePropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}