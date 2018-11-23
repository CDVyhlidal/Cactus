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
