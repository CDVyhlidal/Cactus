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
        private readonly IConfiguration _configuration;

        public ProcessManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int Launch(EntryModel entry)
        {
            var processInfo = new ProcessStartInfo();
            processInfo.FileName = entry.Path;
            processInfo.Arguments = entry.Flags;
            var process = Process.Start(processInfo);

           // var path = Path.Combine(_configuration.RootDirectory, entry.
            Console.WriteLine("launch application...");
            return 0;
        }
    }
}
