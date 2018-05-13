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
    public class EntryManager : IEntryManager
    {
        public bool? LastRan { get; set; }

        public ObservableCollection<Entry> _entries;

        public EntryManager(IEntryLoader entryLoader)
        {
            _entries = new ObservableCollection<Entry>(entryLoader.GetEntries());
        }

        public ObservableCollection<Entry> GetEntries()
        {
            return _entries;
        }
    }
}
