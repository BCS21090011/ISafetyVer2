using ISafetyVer2.ViewModels;
using Microsoft.Maui.Controls.Maps;
using System.Timers;
using ISafetyVer2.Models;
using ISafetyVer2.Services;

namespace ISafetyVer2.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class MainPage : ContentPage
{
    private string userRole;

    List<QuickReport> qR = new List<QuickReport>();
    List<QuickReport> QR
    {
        get { return qR; }
        set
        {
            qR = value;
            MapAddLocation();
        }
    }
    public MainPage()
	{
        InitializeComponent();

        var panGesture = new PanGestureRecognizer();
        panGesture.PanUpdated += OnPanUpdated;
        SwipeButton.GestureRecognizers.Add(panGesture);

    }


    private async void GetAllQuickReport()
    {
        List<QuickReport> newQR = await new FirebaseHelper().GetAllQuickReport();

        QR = newQR;

    }
    protected override void OnAppearing()
    {
        base.OnAppearing();


        userRole = Preferences.Get("UserRole", "Default");
        if (userRole == "Admin")
        {
            Navigation.PushAsync(new Admin());
        }
        GetAllQuickReport();
        
    }

    private void CategoriesBtnOnClick(object obj, EventArgs e)
	{
        Navigation.PushAsync(new SafetyTipsCategoriesPage());   // Safetytips1 in original.
    }

    private void QuickReportBtnOnClick(object obj, EventArgs e)
	{
        Navigation.PushAsync(new QuickReportPage());    // Quickreport in original.
	}

    private void ChatContactsBtnOnClick(object obj, EventArgs e)
	{
		// Go to ChatContacts (or Chat in original).
	}

    // Emergency swipe button by Song:
    double startX;
    System.Timers.Timer holdTimer;

    [Obsolete]
    void SetupHoldTimer()
    {
        // Initialize the timer
        holdTimer = new System.Timers.Timer(1000) { AutoReset = true }; // Set to tick every second
        holdTimer.Elapsed += OnHoldTimerTicked;
    }

    [Obsolete]
    void StartHoldTimer()
    {
        Device.BeginInvokeOnMainThread(() =>
        {
            CountdownLabel.Text = "5"; // Start countdown from 3 seconds
            CountdownLabel.IsVisible = true;
        });
        holdTimer.Start();
    }

    [Obsolete]
    void OnPanUpdated(object sender, PanUpdatedEventArgs e)
    {
        switch (e.StatusType)
        {
            case GestureStatus.Started:
                startX = SwipeButton.TranslationX;
                SetupHoldTimer();
                break;
            case GestureStatus.Running:
                SwipeButton.TranslationX = Math.Max(0, startX + e.TotalX);
                // If the user has swiped enough to trigger the timer but it hasn't started yet
                if (!holdTimer.Enabled && SwipeButton.TranslationX > SwipeArea.Width * 0.4)
                {
                    StartHoldTimer();
                    // Show the overlay grid with the countdown
                    OverlayGrid.IsVisible = true;
                }
                break;
            case GestureStatus.Completed:
            case GestureStatus.Canceled:
                ResetSwipe();
                // Hide the overlay grid as the user has released the button
                OverlayGrid.IsVisible = false;
                break;
        }
    }

    [Obsolete]
    void OnHoldTimerTicked(object sender, ElapsedEventArgs e)
    {
        // Decrement the countdown by 1
        int secondsLeft = int.Parse(CountdownLabel.Text) - 1;

        Device.BeginInvokeOnMainThread(() =>
        {
            if (secondsLeft < 0)
            {
                // Stop the timer as the action is about to be triggered
                holdTimer?.Stop();
                TriggerEmergencyCall();
                ResetSwipe();
            }
            else
            {
                // Update the countdown label
                CountdownLabel.Text = secondsLeft.ToString();
            }
        });
    }

    [Obsolete]
    void ResetSwipe()
    {
        if (holdTimer != null)
        {
            holdTimer.Stop();
            holdTimer.Dispose();
            SetupHoldTimer();
        }
        Device.BeginInvokeOnMainThread(() =>
        {
            // Reset the UI elements
            SwipeButton.TranslationX = 0;
            CountdownLabel.IsVisible = false;
            OverlayGrid.IsVisible = false; // Ensure the overlay grid is hidden
        });
    }

    [Obsolete]
    void TriggerEmergencyCall()
    {
        Device.BeginInvokeOnMainThread(() =>
        {
            // Only trigger the call if the countdown has finished


            DisplayAlert("Emergency Call", "Calling emergency services!", "OK");
            OverlayGrid.IsVisible = false; // Hide overlay after triggering the call

            if (PhoneDialer.Default.IsSupported)
            {
                PhoneDialer.Default.Open("999");
            }

        });
    }

   
    private void MapAddLocation()

    {
        // Clear pins and circles:
        map.Pins.Clear();
        map.MapElements.Clear();
        foreach (QuickReport report in QR) 
        {
            MapAddLocation(report.Latitude, report.Longitude, report.Radius);
        }
    }
    private void MapAddLocation(double latitude, double longitude, double radius)
    {
       

        if (radius == 0)
        {
            map.Pins.Add(new Pin
            {
                Label = "Location",
                Location = new Location(latitude, longitude)

            });
        }
        else
        {
            map.MapElements.Add(new Circle
            {
                Center = new Location
                {
                    Latitude = latitude,
                    Longitude = longitude
                },
                Radius = new Microsoft.Maui.Maps.Distance(radius),
                StrokeColor = Color.FromArgb("#B0FF0000"),
                StrokeWidth = 8,
                FillColor = Color.FromArgb("#70FF0000")
            });
        }
    }
}