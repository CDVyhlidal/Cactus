using Cactus.Interfaces;
using Cactus.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cactus
{
    /// <summary>
    /// This class is responsible for managing the entries.
    /// 
    /// - Contains a collection of Entries
    /// - Adding, Editing, and Deletion of Entries
    /// </summary>
    public class EntryManager : IEntryManager
    {
        public EntryModel LastRan
        {
            get
            {
                foreach(var entry in _entries)
                {
                    if (entry.WasLastRan) return entry;
                }
                return null;
            }
        }

        public ObservableCollection<EntryModel> _entries;

        public EntryManager(IEntryLoader entryLoader)
        {
            _entries = new ObservableCollection<EntryModel>(entryLoader.GetEntries());
        }

        public ObservableCollection<EntryModel> GetEntries()
        {
            return _entries;
        }
    }
}
