using Cactus.Models;
using Cactus.Interfaces;
using System.IO;

namespace Cactus
{
    public class PathBuilder : IPathBuilder
    {
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
    }
}
