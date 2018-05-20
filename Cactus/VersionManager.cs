﻿// Copyright (C) 2018 Jonathan Vasquez <jon@xyinn.org>
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Cactus.Interfaces;
using Cactus.Models;
using System.Collections.Generic;

namespace Cactus
{
    public class VersionManager : IVersionManager
    {
        private Dictionary<string, VersionModel> _versions;
        public Dictionary<string, VersionModel> Versions
        {
            get
            {
                if (_versions != null) return _versions;

                _versions = new Dictionary<string, VersionModel>() {
                    { "1.00", new VersionModel() { Version = "1.00", Order = 1 } },
                    { "1.01", new VersionModel() { Version = "1.01", Order = 2 } },
                    { "1.02", new VersionModel() { Version = "1.02", Order = 3 } },
                    { "1.03", new VersionModel() { Version = "1.03", Order = 4 } },
                    { "1.04b", new VersionModel() { Version = "1.04b", Order = 5 } },
                    { "1.04c", new VersionModel() { Version = "1.04c", Order = 6 } },
                    { "1.05", new VersionModel() { Version = "1.05", Order = 7 } },
                    { "1.05b", new VersionModel() { Version = "1.05b", Order = 8 } },
                    { "1.06", new VersionModel() { Version = "1.06", Order = 9 } },
                    { "1.06b", new VersionModel() { Version = "1.06b", Order = 10 } },
                    { "1.07", new VersionModel() { Version = "1.07", Order = 11 } },
                    { "1.07.41", new VersionModel() { Version = "1.07.41", Order = 12 } },
                    { "1.08", new VersionModel() { Version = "1.08", Order = 13 } },
                    { "1.09", new VersionModel() { Version = "1.09", Order = 14 } },
                    { "1.09b", new VersionModel() { Version = "1.09b", Order = 15 } },
                    { "1.09d", new VersionModel() { Version = "1.09d", Order = 16 } },
                    { "1.10b", new VersionModel() { Version = "1.10b", Order = 17 } },
                    { "1.10s", new VersionModel() { Version = "1.10s", Order = 18 } },
                    { "1.10f", new VersionModel() { Version = "1.10f", Order = 19 } },
                    { "1.11", new VersionModel() { Version = "1.11", Order = 20 } },
                    { "1.11b", new VersionModel() { Version = "1.11b", Order = 21 } },
                    { "1.12a", new VersionModel() { Version = "1.12a", Order = 22 } },
                    { "1.13a", new VersionModel() { Version = "1.13a", Order = 23 } },
                    { "1.13c", new VersionModel() { Version = "1.13c", Order = 24 } },
                    { "1.13d", new VersionModel() { Version = "1.13d", Order = 25 } },
                    { "1.14b", new VersionModel() { Version = "1.14b", Order = 26 } },
                    { "1.14d", new VersionModel() { Version = "1.14d", Order = 27 } },
                };

                return _versions;
            }
        }

        public bool Is100(string version)
        {
            return FindIndex("1.00") == FindIndex(version);
        }

        public bool Is107(string version)
        {
            return FindIndex("1.07") == FindIndex(version);
        }

        public bool Is114OrNewer(string version)
        {
            return FindIndex(version) > FindIndex("1.13d");
        }

        public bool IsPreLod(string version)
        {
            int currentVersion = FindIndex(version);
            int pointSeven = FindIndex("1.07");
            
            return currentVersion < pointSeven;
        }

        public bool RequiresPatchFile(string version)
        {
            var currentVersionIndex = FindIndex(version);
            return currentVersionIndex != FindIndex("1.00") && currentVersionIndex != FindIndex("1.07");
        }

        private int FindIndex(string version)
        {
            int index = -1;

            if (Versions.ContainsKey(version))
            {
                index = Versions[version].Order;
            }

            return index;
        }
    }
}
