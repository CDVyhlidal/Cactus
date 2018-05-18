using Cactus.Interfaces;
using Cactus.Models;
using System;
using System.Diagnostics;

namespace Cactus
{
    public class ProcessManager : IProcessManager
    {
        private int _processCount;

        public bool AreProcessesRunning
        {
            get
            {
                return _processCount > 0;
            }
        }

        public void Launch(EntryModel entry)
        {
            try
            {
                _processCount++;

                var processInfo = new ProcessStartInfo
                {
                    FileName = entry.Path,
                    Arguments = entry.Flags
                };

                var process = Process.Start(processInfo);
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            _processCount--;
        }
    }
}
