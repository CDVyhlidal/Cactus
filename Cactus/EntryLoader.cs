using Cactus.Interfaces;
using Cactus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cactus
{
    public class EntryLoader : IEntryLoader
    {
        public List<Entry> GetEntries()
        {
            return new List<Entry>();
        }

        public int SaveEntries()
        {
            return 0;
        }
    }
}
