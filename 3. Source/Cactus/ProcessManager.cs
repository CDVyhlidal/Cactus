// Copyright (C) 2018 Jonathan Vasquez <jon@xyinn.org>
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program. If not, see<https://www.gnu.org/licenses/>.

using Cactus.Interfaces;
using Cactus.Models;
using System;
using System.Diagnostics;
using System.Windows;

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

        /// <summary>
        /// Checks to see if Cactus is already running.
        /// </summary>
        /// <remarks>
        /// This is a very very simple implementation of making sure only one process
        /// of Cactus runs. If the user is running an application that is also called
        /// "Cactus", this would trigger a false positive.
        /// </remarks>
        public static bool IsMainApplicationRunning()
        {
            var currentProcess = Process.GetCurrentProcess();
            var currentProcesses = Process.GetProcessesByName(currentProcess.ProcessName);

            return currentProcesses.Length > 1;
        }

        public void Launch(EntryModel entry, bool isAdmin)
        {
            try
            {
                _processCount++;

                var processInfo = new ProcessStartInfo
                {
                    FileName = entry.Path,
                    Arguments = entry.Flags
                };

                if (isAdmin)
                {
                    processInfo.Verb = "runas";
                }

                var process = Process.Start(processInfo);
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"There was an error launching the application.\n\n{ex.Message}\n\nLaunch Path: {entry.Path}");
            }

            _processCount--;
        }
    }
}
