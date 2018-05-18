using Cactus.Models;
using System.Collections.Generic;

namespace Cactus.Interfaces
{
    /// <summary>
    /// The Entry Manager is responsible for managing all of the entries in the
    /// collection of Diablo II versions that player wants to play.
    /// </summary>
    public interface IEntryManager
    {
        EntryModel GetLastRan();
        void Add(EntryModel entry);
        void Delete(EntryModel entry);
        List<EntryModel> GetEntries();
        void MarkAsLastRan(EntryModel oldEntry, EntryModel newEntry);
        void SaveEntries();
    }
}
