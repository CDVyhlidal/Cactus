using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cactus.Models
{
    public class Entry
    {
        public string Label { get; set; }
        public string Version { get; set; }
        public string Flags { get; set; }
        public bool IsExpansion { get; set; }
        public bool WasLastRan { get; set; }
    }
}
