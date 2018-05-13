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
        public List<EntryModel> GetEntries()
        {
            var exampleEntries = new List<EntryModel>()
            {
                new EntryModel()
                {
                    Label = "Singling Classic 1.14d",
                    Version = "1.14d",
                    Flags = "-w -ns -3dfx",
                    IsExpansion = false,
                    WasLastRan = true
                },
                new EntryModel()
                {
                    Label = "Singling Expansion 1.14d",
                    Version = "1.14d",
                    Flags = "-w -ns -3dfx",
                    IsExpansion = true,
                    WasLastRan = false
                },
                new EntryModel()
                {
                    Label = "Singling 1.10",
                    Version = "1.00",
                    Flags = "-w -ns -3dfx",
                    IsExpansion = false,
                    WasLastRan = false
                }
            };

            return exampleEntries;
        }

        public int SaveEntries()
        {
            Console.WriteLine("Saving Entries ..");
            return 0;
        }
    }
}
