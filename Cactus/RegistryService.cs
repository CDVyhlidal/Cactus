﻿// Copyright (C) 2018 Jonathan Vasquez <jon@xyinn.org>
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
