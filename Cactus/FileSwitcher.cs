using Cactus.Interfaces;
using Cactus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cactus
{
    /// <summary>
    /// This class is responsible for the file switching that occurs when the user
    /// requests to run a version of Diablo II.
    /// </summary>
    public class FileSwitcher : IFileSwitcher
    {
        private IEntryManager _entries;
        private IPatchFileGenerator _patchFileGenerator;

        public FileSwitcher(IEntryManager entries, IPatchFileGenerator patchFileGenerator)
        {
            _entries = entries;
            _patchFileGenerator = patchFileGenerator;
        }

        public void Run(EntryModel entry)
        {
            if (entry == null)
            {
                Console.WriteLine("No element was selected. Skipping.");
                return;
            }

            // If nothing was ever ran, then ..
            // If what we are running matches what we last ran, then go directly
            // If we are switching entries, then do what you need to do to get the files in the correct order.
            var lastRanEntry = _entries.LastRan;

            if (lastRanEntry == null)
            {
                Console.WriteLine("No version was ever ran. Running this and setting it as main version.");
            }
            else if (lastRanEntry.Label == entry.Label && lastRanEntry.IsExpansion == entry.IsExpansion)
            {
                Console.WriteLine("Running the same version, no change needed.");
            }
            else
            {
                Console.WriteLine("A different version has been selected. Switching.");
            }
        }
    }
}
