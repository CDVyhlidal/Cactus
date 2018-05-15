using Cactus.Models;
using Cactus.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cactus
{
    public class PathBuilder : IPathBuilder
    {
        //private EntryModel _entry;

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
    }
}
