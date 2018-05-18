// Copyright (C) 2018 Jonathan Vasquez <jon@xyinn.org>
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

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
