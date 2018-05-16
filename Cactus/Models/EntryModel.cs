using GalaSoft.MvvmLight;
using Newtonsoft.Json;

namespace Cactus.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class EntryModel : ViewModelBase
    {
        private bool _wasLastRan;
        private string _label;
        private string _version;
        private string _path;
        private string _flags;
        private bool _isExpansion;

        [JsonProperty("Label", Order = 1)]
        public string Label
        {
            get
            {
                return _label;
            }
            set
            {
                _label = value;
                RaisePropertyChanged("Label");
            }
        }

        [JsonProperty("Version", Order = 2)]
        public string Version
        {
            get
            {
                return _version;
            }
            set
            {
                _version = value;
                RaisePropertyChanged("Version");
            }
        }

        [JsonProperty("Path", Order = 3)]
        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
                RaisePropertyChanged("Path");
            }
        }

        [JsonProperty("Flags", Order = 4)]
        public string Flags
        {
            get
            {
                return _flags;
            }
            set
            {
                _flags = value;
                RaisePropertyChanged("Flags");
            }
        }

        [JsonProperty("IsExpansion", Order = 5)]
        public bool IsExpansion
        {
            get
            {
                return _isExpansion;
            }
            set
            {
                _isExpansion = value;
                RaisePropertyChanged("IsExpansion");
            }
        }

        [JsonProperty("WasLastRan", Order = 6)]
        public bool WasLastRan
        {
            get
            {
                return _wasLastRan;
            }
            set
            {
                _wasLastRan = value;
                RaisePropertyChanged("WasLastRan");
            }
        }
    }
}
