using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SeevoConfig.Devices
{
    public static class SeevoModelExtensions
    {
        public static void CopyFrom(this SeevoModel currentObject, SeevoModel newObject)
        {
            if (newObject == null) { return; }

            currentObject.Id = newObject.Id;
            currentObject.Mac = newObject.Mac;
            currentObject.IP = newObject.IP;
            currentObject.Port = newObject.Port;
            currentObject.AvailableEvents = newObject.AvailableEvents;
            currentObject.SelectedEvent = newObject.SelectedEvent;
            currentObject.HasChanged = newObject.HasChanged;
        }

        public static void AddDevice(this ObservableCollection<SeevoModel> list, SeevoModel deviceConfig)
        {
            if (deviceConfig == null) { return; }

            if (!list.Any(x => x.Mac.Equals(deviceConfig.Mac, StringComparison.InvariantCultureIgnoreCase)))
            {
                list.Add(deviceConfig);
            }
        }
    }
}
