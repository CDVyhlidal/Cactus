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
using Microsoft.Win32;

namespace Cactus
{
    public class RegistryService : IRegistryService
    {
        IPathBuilder _pathBuilder;

        public RegistryService(IPathBuilder pathBuilder)
        {
            _pathBuilder = pathBuilder;
        }

        public void Update(EntryModel entry)
        {
            using (var key = Registry.CurrentUser.CreateSubKey(@"Software\Blizzard Entertainment\Diablo II"))
            {
                string saveDirectory = _pathBuilder.GetSaveDirectory(entry);
                string rootDirectory = _pathBuilder.GetRootDirectory(entry);
                int resolution = entry.IsExpansion ? 1 : 0;

                key.SetValue("Save Path", saveDirectory);
                key.SetValue("NewSavePath", saveDirectory);
                key.SetValue("Resolution", resolution);
                key.SetValue("InstallPath", rootDirectory);
            }
        }
    }
}
