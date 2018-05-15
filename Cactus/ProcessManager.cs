using Cactus.Interfaces;
using Cactus.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cactus
{
    public class ProcessManager : IProcessManager
    {
        public int Launch(EntryModel entry)
        {
            var processInfo = new ProcessStartInfo
            {
                FileName = entry.Path,
                Arguments = entry.Flags
            };

            var process = Process.Start(processInfo);

           // var path = Path.Combine(_configuration.RootDirectory, entry.
            Console.WriteLine("launch application...");
            return 0;
        }
    }
}
