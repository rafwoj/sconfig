using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using SeevoConfig.Devices;

namespace SeevoConfig.Projects
{
    public class Project : INotifyPropertyChanged
    {
        private string filePath;
        private string description;
        private DateTime created;
        private DateTime updated;
        private ObservableCollection<SeevoModel> devices = new ObservableCollection<SeevoModel>();

        [JsonIgnore]
        public string FilePath { get => filePath; set { filePath = value; NotifyPropertyChanged(); } }

        public string Description { get => description; set { description = value; NotifyPropertyChanged(); } }

        public DateTime Created { get => created; set { created = value; NotifyPropertyChanged(); } }

        public DateTime Updated { get => updated; set { updated = value; NotifyPropertyChanged(); } }

        public ObservableCollection<SeevoModel> Devices { get => devices; set { devices = value; NotifyPropertyChanged(); } }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
