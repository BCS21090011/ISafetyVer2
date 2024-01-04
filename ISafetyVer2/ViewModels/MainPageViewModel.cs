using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISafetyVer2.Models;
using ISafetyVer2.Services;

namespace ISafetyVer2.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<QuickReport> qr;

        public ObservableCollection<QuickReport> QR
        {
            get => qr;
            set
            {
                if (qr != value)
                {
                    qr = value;
                    RaisePropertyChanged(nameof(QR));
                }
            }
        }

        public MainPageViewModel()
        {
        }
        public async Task GetAllQuickReport()
        {
            List<QuickReport> reports = await new FirebaseHelper().GetAllQuickReport();
            QR = new ObservableCollection<QuickReport>();
            foreach (QuickReport report in reports)
            {
                QR.Add(report);
            }
        }


        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
