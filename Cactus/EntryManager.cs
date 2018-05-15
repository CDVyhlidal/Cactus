using Cactus.Interfaces;
using Cactus.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class EntryManager : IEntryManager
    {
        private string _jsonFile = "Entries.json";
        private string _jsonDirectory;
        private string _jsonPath;
        private List<EntryModel> _entries = new List<EntryModel>();
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

        public void MarkAsLastRan(EntryModel oldEntry, EntryModel newEntry)
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

        public ObservableCollection<EntryModel> GetObservableEntries()
        {
            var collection = new ObservableCollection<EntryModel>(_entries);
            return collection;
        }
    }
}
