using ISafetyVer2.Models;
using ISafetyVer2.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ISafetyVer2.ViewModels;

public partial class CommunityViewModel : INotifyPropertyChanged
{
    private ObservableCollection<QuickReport> _quickReports { get; set; }
    public ObservableCollection<QuickReport> QuickReports
    {
        get => _quickReports;
        set
        {
            if (_quickReports != value)
            {
                _quickReports = value;
                RaisePropertyChanged(nameof(QuickReports));
            }
        }
    }

    public CommunityViewModel()
    {
        // Realtime:
        IDisposable obserable = new FirebaseHelper().firebaseClient
            .Child("QuickReports")
            .AsObservable<QuickReport>()
            .Subscribe(qr => LoadQuickReports());
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private async void LoadQuickReports()
    {
        var reports = await new FirebaseHelper().GetAllQuickReports();
        QuickReports = new ObservableCollection<QuickReport>();
        foreach (var report in reports)
        {
            QuickReports.Add(report);
        }
    }

    public void RaisePropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}


