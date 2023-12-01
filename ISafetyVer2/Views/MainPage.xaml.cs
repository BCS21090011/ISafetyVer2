using System.Timers;

namespace ISafetyVer2.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class MainPage : ContentPage
{
    private string userRole;
	public MainPage()
	{
        InitializeComponent();

        var panGesture = new PanGestureRecognizer();
        panGesture.PanUpdated += OnPanUpdated;
        SwipeButton.GestureRecognizers.Add(panGesture);
        
        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        userRole = Preferences.Get("UserRole", "Default");
        
        if (userRole == "Admin")
        {
            Navigation.PushAsync(new Admin());
        }
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
            CountdownLabel.Text = "10"; // Start countdown from 3 seconds
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

        });
    }
}