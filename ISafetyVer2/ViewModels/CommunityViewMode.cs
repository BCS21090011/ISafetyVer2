﻿using ISafetyVer2.Models;
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
    private ObservableCollection<AdminPost> _adminPosts;
    public ObservableCollection<AdminPost> AdminPosts
    {
        get => _adminPosts;
        set
        {
            if (_adminPosts != value)
            {
                _adminPosts = value;
                RaisePropertyChanged(nameof(AdminPosts));
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

        IDisposable adminPostObservable = new FirebaseHelper().firebaseClient
            .Child("AdminPosts")
            .AsObservable<AdminPost>()
            .Subscribe(ap => LoadAdminPosts());
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private async void LoadQuickReports()
    {
        try 
        {
            var reports = await new FirebaseHelper().GetAllQuickReportByUserID(Preferences.Get("UserID", "NoUserID"));
            QuickReports = new ObservableCollection<QuickReport>();
            foreach (var report in reports)
            {
                QuickReports.Add(report);
            }
        }
        catch(Exception ex) 
        {
            // Wish nothing bad happens :D
        }
        
    }
   
    private async void LoadAdminPosts()
    {
        try
        {
            var posts = await new FirebaseHelper().GetAllAdminPosts(); // Implement this method
            AdminPosts = new ObservableCollection<AdminPost>();
            foreach (var post in posts)
            {
                AdminPosts.Add(post);
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions
        }
    }
    public void RaisePropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}