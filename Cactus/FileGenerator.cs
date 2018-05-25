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
using System.Collections.Generic;
using System.IO;

namespace Cactus
{
    /// <summary>
    /// This class is responsible for returning a list of all of
    /// the files corresponding to a specific patch.
    /// </summary>
    public class FileGenerator : IFileGenerator
    {
        private readonly IPathBuilder _pathBuilder;

        public FileGenerator(IPathBuilder pathBuilder)
        {
            _pathBuilder = pathBuilder;
        }

        public RequiredFilesModel GetRequiredFiles(EntryModel entry)
        {
            var requiredFiles = new RequiredFilesModel();
            var platformDirectory = _pathBuilder.GetPlatformDirectory(entry);

            if (Directory.Exists(platformDirectory))
            {
                var directories = Directory.GetDirectories(platformDirectory);
                var processedDirectories = new List<string>();

                foreach (var directory in directories)
                {
                    processedDirectories.Add(Path.GetFileName(directory));
                }

                var files = Directory.GetFiles(platformDirectory);
                var processedFiles = new List<string>();

                foreach (var file in files)
                {
                    processedFiles.Add(Path.GetFileName(file));
                }

                requiredFiles.Directories = processedDirectories;
                requiredFiles.Files = processedFiles;
            }

            return requiredFiles;
        }

        public List<string> ExpansionMpqs
        {
            get
            {
                return new List<string>()
                {
                    "d2exp.mpq",
                    "d2xmusic.mpq",
                    "d2xvideo.mpq",
                    "d2xtalk.mpq"
                };
            }
        }
    }
}
