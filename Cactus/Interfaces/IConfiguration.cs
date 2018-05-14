using Cactus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cactus.Interfaces
{
    public interface IConfiguration
    {
        string RootDirectory { get; set; }
        Dictionary<string, EntryModel> Entries { get; set; }
    }
}
