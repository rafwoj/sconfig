using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace SeevoConfig.Devices
{
    public class SeevoModel : INotifyPropertyChanged
    {
        private string id;
        private string ip;
        private int port;
        private string mac;
        private IList<string> availableEvents;
        private string selectedEvent;
        private bool updated;

        [JsonPropertyName("device-id")]
        public string Id { get => id; set { id = value; NotifyPropertyChanged(); } }

        [JsonPropertyName("device-mac")]
        public string Mac { get => mac; set { mac = value; NotifyPropertyChanged(); } }

        [JsonPropertyName("device-ip")]
        public string IP { get => ip; set { ip = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(Address)); } }

        [JsonPropertyName("device-port")]
        public int Port { get => port; set { port = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(Address)); } }

        [JsonPropertyName("device-available-events")]
        public IList<string> AvailableEvents { get => availableEvents; set { availableEvents = value; NotifyPropertyChanged(); } }

        [JsonPropertyName("device-selected-event")]
        public string SelectedEvent { get => selectedEvent; set { selectedEvent = value; NotifyPropertyChanged(); } }

        [JsonPropertyName("device-config-has-changed")]
        public bool HasChanged { get => updated; set { updated = value; NotifyPropertyChanged(); } }

        [JsonIgnore]
        public string Address { get => $"{IP}:{Port}"; }


        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
