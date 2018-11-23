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
using System.Diagnostics;

namespace Cactus
{
    public class Logger : ILogger
    {
        public void LogInfo(string message)
        {
            Trace.WriteLine($"[Info] {message}");
        }

        public void LogWarning(string message)
        {
            Trace.WriteLine($"[Warning] {message}");
        }

        public void LogError(string message)
        {
            Trace.WriteLine($"[Error] {message}");
        }
    }
}
