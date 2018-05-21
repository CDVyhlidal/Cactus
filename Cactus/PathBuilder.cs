// Copyright (C) 2018 Jonathan Vasquez <jon@xyinn.org>
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

using Cactus.Models;
using Cactus.Interfaces;
using System.IO;

namespace Cactus
{
    public class PathBuilder : IPathBuilder
    {
        private readonly ILogger _logger;

        public PathBuilder(ILogger logger)
        {
            _logger = logger;
        }

        public string GetRootDirectory(EntryModel entry)
        {
            return Path.GetDirectoryName(entry.Path);
        }

        public string GetStorageDirectory(EntryModel entry)
        {
            string baseGameType = entry.IsExpansion ? "Expansion" : "Classic";
            string targetRootDirectory = Path.Combine(Path.GetDirectoryName(entry.Path), baseGameType, entry.Label);
            return targetRootDirectory;
        }

        public string GetSaveDirectory(EntryModel entry)
        {
            string targetRootDirectory = GetStorageDirectory(entry);
            string saveDirectory = Path.Combine(targetRootDirectory, "save");
            return saveDirectory;
        }

        public string GetRootDataDirectory(EntryModel entry)
        {
            return Path.Combine(GetRootDirectory(entry), "data");
        }

        public string GetStorageDataDirectory(EntryModel entry)
        {
            return Path.Combine(GetStorageDirectory(entry), "data");
        }

        public string GetPlugyRootDirectory(EntryModel entry)
        {
            return Path.Combine(GetRootDirectory(entry), "PlugY");
        }

        public string GetPlugyStorageDirectory(EntryModel entry)
        {
            return Path.Combine(GetStorageDirectory(entry), "PlugY");
        }
    }
}
