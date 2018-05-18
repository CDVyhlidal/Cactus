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

        /// <summary>
        /// Similar to the above function but we will call the directory according to our preferred label.
        /// </summary>
        public string GetStorageDirectory(EntryModel entry, string preferredLabel)
        {
            string baseGameType = entry.IsExpansion ? "Expansion" : "Classic";
            string targetRootDirectory = Path.Combine(Path.GetDirectoryName(entry.Path), baseGameType, preferredLabel);
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
