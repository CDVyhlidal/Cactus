using Cactus.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cactus
{
    public class EntryManager : IEntryManager
    {
        public bool? LastRan { get; set; }
    }
}
