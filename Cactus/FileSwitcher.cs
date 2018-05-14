using Cactus.Interfaces;
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

        public void Run()
        {
            // If nothing was ever ran, then ..
            // If what we are running matches what we last ran, then go directly
            // If we are switching entries, then do what you need to do to get the files in the correct order.
            Console.WriteLine("hello");

            var lastRanEntry = _entries.LastRan;

            if (lastRanEntry == null)
            {

            }
            //else if (lastRanEntry)
            //{

            //}
            else
            {

            }
        }
    }
}
