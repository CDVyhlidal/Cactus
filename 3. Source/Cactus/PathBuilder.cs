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

        private readonly string _platformDirectoryName = "Platforms";
        private readonly string _savesDirectoryName = "Saves";

        public PathBuilder(ILogger logger)
        {
            _logger = logger;
        }

        public string GetRootDirectory(EntryModel entry)
        {
            return Path.GetDirectoryName(entry.Path);
        }

        public string GetPlatformDirectory(EntryModel entry)
        {
            return Path.Combine(GetPlatformsDirectory(entry), entry.Platform);
        }

        public string GetSaveDirectory(EntryModel entry)
        {
            string savesDirectory = GetSavesDirectory(entry);
            string saveDirectory = Path.Combine(savesDirectory, entry.Platform);
            return saveDirectory;
        }

        public bool ContainsInvalidCharacters(string word)
        {
            char[] invalidChars = Path.GetInvalidFileNameChars();
            foreach (char invalidChar in invalidChars)
            {
                if (word.Contains(invalidChar.ToString())) return true;
            }

            return false;
        }

        private string GetPlatformsDirectory(EntryModel entry)
        {
            string rootDirectory = GetRootDirectory(entry);
            return Path.Combine(rootDirectory, _platformDirectoryName);
        }

        private string GetSavesDirectory(EntryModel entry)
        {
            string rootDirectory = GetRootDirectory(entry);
            return Path.Combine(rootDirectory, _savesDirectoryName);
        }
    }
}
