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

using Cactus.Interfaces;
using Cactus.Models;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Cactus
{
    /// <summary>
    /// This class is responsible for managing the entries.
    /// 
    /// - Contains a collection of Entries
    /// - Adding, Editing, and Deletion of Entries
    /// - Saving to Json File
    /// </summary>
    public class EntryManager : ViewModelBase, IEntryManager
    {
        private readonly string _jsonFile = "Entries.json";
        private string _jsonDirectory;
        private string _jsonPath;
        private List<EntryModel> _entries;
        private ILogger _logger;

        public EntryManager(ILogger logger)
        {
            _logger = logger;
            _jsonDirectory = Directory.GetCurrentDirectory();
            _jsonPath = Path.Combine(_jsonDirectory, _jsonFile);
            _entries = GetEntries();
        }

        public EntryModel GetLastRan()
        {
            foreach(var entry in _entries)
            {
                if (entry.WasLastRan) return entry;
            }
            return null;
        }

        public void Add(EntryModel entry)
        {
            _entries.Add(entry);
        }

        public void Delete(EntryModel entry)
        {
            EntryModel elementToRemove = null;

            foreach (var e in _entries)
            {
                if (e.Label == entry.Label && e.Path == entry.Path &&
                    e.Flags == entry.Flags && e.Version == e.Version &&
                    e.IsExpansion == entry.IsExpansion)
                {
                    elementToRemove = e;
                    break;
                }
            }

            if (elementToRemove != null)
            {
                _entries.Remove(elementToRemove);
            }
        }

        public EntryModel Copy(EntryModel entry)
        {
            var newEntry = new EntryModel
            {
                Path = entry.Path,
                Version = entry.Version,
                Flags = entry.Flags,
                IsExpansion = entry.IsExpansion
            };

            _entries.Add(newEntry);
            return newEntry;
        }

        public void MoveUp(EntryModel entry)
        {
            int selectedEntryIndex = _entries.FindIndex(_ => _ == entry);

            // If we are already at the top, then nothing needs to be done.
            if (selectedEntryIndex != 0)
            {
                // Find previous entry and swap
                int previousEntryIndex = selectedEntryIndex - 1;

                var previousEntry = _entries[previousEntryIndex];
                _entries[previousEntryIndex] = entry;
                _entries[selectedEntryIndex] = previousEntry;
            }
        }

        public void MoveDown(EntryModel entry)
        {
            int selectedEntryIndex = _entries.FindIndex(_ => _ == entry);
            int lastIndex = _entries.Count - 1;

            // If we are already at the bottom, then nothing needs to be done.
            if (selectedEntryIndex != lastIndex)
            {
                // Find next entry and swap
                int nextEntryIndex = selectedEntryIndex + 1;

                var nextEntry = _entries[nextEntryIndex];
                _entries[nextEntryIndex] = entry;
                _entries[selectedEntryIndex] = nextEntry;
            }
        }

        /// <summary>
        /// This marks this specific entry as the last ran. Useful in situations where
        /// the user never ran a version before through this application.
        /// </summary>
        public void MarkLastRan(EntryModel entry)
        {
            entry.WasLastRan = true;
        }

        /// <summary>
        /// Switches the two entries as the last ran. Useful when switching versions.
        /// </summary>
        public void SwapLastRan(EntryModel oldEntry, EntryModel newEntry)
        {
            oldEntry.WasLastRan = false;
            newEntry.WasLastRan = true;
        }

        public List<EntryModel> GetEntries()
        {
            try
            {
                if (File.Exists(_jsonPath))
                {
                    if (_entries != null) return _entries;
                    var serializedEntries = File.ReadAllText(_jsonPath);
                    _entries = JsonConvert.DeserializeObject<List<EntryModel>>(serializedEntries);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogWarning("Wiping existing json file since it's corrupted.");
                SaveEntries();
            }
            
            return _entries;
        }

        public void SaveEntries()
        {
            string serializedEntries = JsonConvert.SerializeObject(_entries, Formatting.Indented);
            File.WriteAllText(_jsonPath, serializedEntries);
        }
    }
}
