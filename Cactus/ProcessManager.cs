using Cactus.Interfaces;
using Cactus.Models;
using System.Diagnostics;

namespace Cactus
{
    public class ProcessManager : IProcessManager
    {
        public bool AreProcessesRunning
        {
            get
            {
                return _processCount > 0;
            }
        }

        private int _processCount;

        public void Launch(EntryModel entry)
        {
            var processInfo = new ProcessStartInfo
            {
                FileName = entry.Path,
                Arguments = entry.Flags
            };

            _processCount++;
            var process = Process.Start(processInfo);
            process.WaitForExit();
            _processCount--;
        }
    }
}
