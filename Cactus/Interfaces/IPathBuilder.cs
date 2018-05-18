using Cactus.Models;

namespace Cactus.Interfaces
{
    public interface IPathBuilder
    {
        string GetRootDirectory(EntryModel entry);
        string GetStorageDirectory(EntryModel entry);
        string GetSaveDirectory(EntryModel entry);
        string GetRootDataDirectory(EntryModel entry);
        string GetStorageDataDirectory(EntryModel entry);
    }
}
