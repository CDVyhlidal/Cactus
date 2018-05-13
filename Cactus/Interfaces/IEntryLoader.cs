using Cactus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cactus.Interfaces
{
    /// <summary>
    /// This class is responsible for saving and loading the entries from the json file.
    /// </summary>
    public interface IEntryLoader
    {
        List<EntryModel> GetEntries();
        int SaveEntries();
    }
}
