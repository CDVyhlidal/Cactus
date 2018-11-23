// Copyright (C) 2018 Jonathan Vasquez <jon@xyinn.org>
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program. If not, see<https://www.gnu.org/licenses/>.

using GalaSoft.MvvmLight;
using Newtonsoft.Json;

namespace Cactus.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class EntryModel : ViewModelBase
    {
        private bool _wasLastRan;
        private string _platform;
        private string _path;
        private string _flags;
        private bool _isExpansion;

        [JsonProperty("Platform", Order = 1)]
        public string Platform
        {
            get
            {
                return _platform;
            }
            set
            {
                _platform = value;
                RaisePropertyChanged("Platform");
            }
        }

        [JsonProperty("Path", Order = 2)]
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

        [JsonProperty("Flags", Order = 3)]
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

        [JsonProperty("IsExpansion", Order = 4)]
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

        [JsonProperty("WasLastRan", Order = 5)]
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
