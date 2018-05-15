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
                    Path = @"D:\Games\Diablo II\Game.exe",
                    Version = "1.14d",
                    Flags = "-w -ns",
                    IsExpansion = false,
                    WasLastRan = false
                },
                new EntryModel()
                {
                    Label = "Singling Expansion 1.14d",
                    Path = @"D:\Games\Diablo II\Game.exe",
                    Version = "1.14d",
                    Flags = "-w -ns -3dfx",
                    IsExpansion = true,
                    WasLastRan = false
                },
                new EntryModel()
                {
                    Label = "Singling 1.00",
                    Path = @"D:\Games\Diablo II\Game.exe",
                    Version = "1.00",
                    Flags = "-w -ns -3dfx",
                    IsExpansion = false,
                    WasLastRan = false
                },
                 new EntryModel()
                {
                    Label = "Vanilla 1.14d",
                    Path = @"D:\Games\Diablo II\Game.exe",
                    Version = "1.14d",
                    Flags = "-w -ns",
                    IsExpansion = true,
                    WasLastRan = false
                },
                 new EntryModel()
                {
                    Label = "1.13d",
                    Path = @"D:\Games\Diablo II\Game.exe",
                    Version = "1.13d",
                    Flags = "-w -ns",
                    IsExpansion = true,
                    WasLastRan = true
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
