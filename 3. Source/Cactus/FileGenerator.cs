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
        private readonly ILogger _logger;

        public FileGenerator(IPathBuilder pathBuilder, ILogger logger)
        {
            _pathBuilder = pathBuilder;
            _logger = logger;
        }

        private readonly List<string> _protectedDocuments = new List<string>()
        {
            "Platforms",
            "Saves",
            "save",
            "d2char.mpq",
            "d2data.mpq",
            "d2exp.mpq",
            "d2music.mpq",
            "d2sfx.mpq",
            "d2speech.mpq",
            "d2video.mpq",
            "d2xmusic.mpq",
            "d2xtalk.mpq",
            "d2xvideo.mpq",
            "D2.LNG",
            "Entries.json",
            "LastRequiredFiles.json"
        };

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
                    var directoryName = Path.GetFileName(directory);
                    processedDirectories.Add(directoryName);
                }

                var files = Directory.GetFiles(platformDirectory);
                var processedFiles = new List<string>();

                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file);
                    processedFiles.Add(fileName);
                }

                requiredFiles.Directories = processedDirectories;
                requiredFiles.Files = processedFiles;

                ValidateRequiredFiles(requiredFiles);
            }

            return requiredFiles;
        }

        /// <summary>
        /// Scans all of the files in the list and removes any files that are protected.
        /// </summary>
        public void ValidateRequiredFiles(RequiredFilesModel requiredFiles)
        {
            var directoriesToRemove = new List<string>();
            var filesToRemove = new List<string>();

            foreach (var directory in requiredFiles.Directories)
            {
                if (IsProtected(directory))
                {
                    directoriesToRemove.Add(directory);
                }
            }

            foreach (var file in requiredFiles.Files)
            {
                if (IsProtected(file))
                {
                    filesToRemove.Add(file);
                }
            }

            foreach (var directory in directoriesToRemove)
            {
                requiredFiles.Directories.Remove(directory);
            }

            foreach (var file in filesToRemove)
            {
                requiredFiles.Files.Remove(file);
            }
        }

        private bool IsProtected(string document)
        {
            // No files or directories that are within the protected list are allowed to be tracked/deleted.
            foreach (var protectedDocument in _protectedDocuments)
            {
                if (document.EqualsIgnoreCase(protectedDocument))
                {
                    _logger.LogWarning($"Protected file/directory \"{document}\" detected in list. Skipping it for protection.");
                    return true;
                }
            }
            return false;
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
