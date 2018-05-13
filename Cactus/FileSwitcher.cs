using Cactus.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cactus
{
    public class FileSwitcher : IFileSwitcher
    {
        private IEntryManager _entries;

        public FileSwitcher(IEntryManager entries)
        {
            _entries = entries;
        }

        public void Run()
        {
            // If nothing was ever ran, then ..
            // If what we are running matches what we last ran, then go directly
            // If we are switching entries, then do what you need to do to get the files in the correct order.
            Console.WriteLine("hello");

            if (_entries.LastRan == null)
            {

            }
            //else if (_entries.LastRan == "test")
            //{

            //}
            else
            {

            }
        }

    }
}
