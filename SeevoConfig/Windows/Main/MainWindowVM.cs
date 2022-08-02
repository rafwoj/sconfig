using System.ComponentModel;
using System.Runtime.CompilerServices;
using SeevoConfig.Devices;
using SeevoConfig.Projects;

namespace SeevoConfig.Windows.Main
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        private Project project;
        private SeevoModel selectedDevice;
        private SeevoModel editDevice;

        public Project Project { get => project; set { project = value; NotifyPropertyChanged(); } }

        public SeevoModel SelectedDevice { get => selectedDevice; set { selectedDevice = value; NotifyPropertyChanged(); } }

        public SeevoModel EditDevice { get => editDevice; set { editDevice = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(IsEditEnabled)); } }

        public bool IsEditEnabled { get => EditDevice != null; }

        private string log_text;

        public string LogTextWrite { get => log_text; set { log_text = log_text + value + "\r\n"; NotifyPropertyChanged(); } }
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
