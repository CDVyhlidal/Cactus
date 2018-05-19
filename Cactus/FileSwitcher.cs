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
using Microsoft.VisualBasic.FileIO;
using System;
using System.IO;
using System.Threading;
using System.Windows;

namespace Cactus
{
    /// <summary>
    /// This class is responsible for the file switching that occurs when
    /// the user requests to run a version of Diablo II.
    /// </summary>
    public class FileSwitcher : IFileSwitcher
    {
        private IEntryManager _entries;
        private IPatchFileGenerator _patchFileGenerator;
        private IProcessManager _processManager;
        private IRegistryService _registryService;
        private ILogger _logger;
        private IPathBuilder _pathBuilder;

        private EntryModel _currentEntry;
        private EntryModel _lastRanEntry;

        public FileSwitcher(IEntryManager entries, IPatchFileGenerator patchFileGenerator,
                            IProcessManager processManager, IRegistryService registryService,
                            ILogger logger, IPathBuilder pathBuilder)
        {
            _entries = entries;
            _patchFileGenerator = patchFileGenerator;
            _processManager = processManager;
            _registryService = registryService;
            _logger = logger;
            _pathBuilder = pathBuilder;
        }

        public void Run(EntryModel entry)
        {
            _currentEntry = entry;

            if (String.IsNullOrWhiteSpace(_currentEntry.Label))
            {
                MessageBox.Show("Cannot run an entry that has no label.");
                return;
            }

            _lastRanEntry = _entries.GetLastRan();

            if (_lastRanEntry == null)
            {
                _logger.LogInfo("No version was ever ran. Running this and setting it as main version.");

                // Set registry
                _registryService.Update(_currentEntry);

                // Set this entry as last ran
                _entries.MarkLastRan(_currentEntry);

                // Make last ran entry this entry
                _lastRanEntry = _currentEntry;

                // Backup files since this is the first time the user is running Cactus.
                CopyFilesToStorage();

                // Save
                _entries.SaveEntries();

                // Launch
                var launchThread = new Thread(() => _processManager.Launch(_lastRanEntry));
                launchThread.Start();
            }
            else if (_lastRanEntry.Label == _currentEntry.Label && _lastRanEntry.IsExpansion == _currentEntry.IsExpansion)
            {
                _logger.LogInfo("Running the same version, no change needed.");

                // TODO: If the labels and game mode is the same, but just the flags are different, then allow the launch.
                var launchThread = new Thread(() => _processManager.Launch(_lastRanEntry));
                launchThread.Start();
            }
            else
            {
                _logger.LogInfo("A different version has been selected. Switching.");

                // Before we switch, make sure to do validation and make sure user
                // has the files they need before attempting to switch.
                if (!HasRequiredFiles())
                {
                    return;
                }

                // If there is an existing process running, do not allow a switch.
                // Only identical versions can be launched.
                if (_processManager.AreProcessesRunning)
                {
                    MessageBox.Show("Another process related to another game mode/version is running. Ignoring switch request.");
                    return;
                }

                _registryService.Update(_currentEntry);
                SwitchFiles();
                _entries.SwapLastRan(_lastRanEntry, _currentEntry);
                _lastRanEntry = _currentEntry;
                _entries.SaveEntries();

                var launchThread = new Thread(() => _processManager.Launch(_lastRanEntry));
                launchThread.Start();
            }
        }

        private bool HasRequiredFiles()
        {
            var requiredFiles = _patchFileGenerator.GetRequiredFiles(_currentEntry.Version);
            string targetRootDirectory = _pathBuilder.GetStorageDirectory(_currentEntry);

            // Check that each of the files we need exist in the target path
            foreach (var file in requiredFiles)
            {
                string targetFile = Path.Combine(targetRootDirectory, file);

                if (!File.Exists(targetFile))
                {
                    MessageBox.Show($"The target file doesn't exist:\n\n{targetFile}");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// This copies the files from the root directory over to the storage directory.
        /// This is primarily used when the user never ran D2 before and needs the files
        /// from the root directory to be automatically backed up the first time they run
        /// the game.
        /// </summary>
        private void CopyFilesToStorage()
        {
            try
            {
                var requiredFiles = _patchFileGenerator.GetRequiredFiles(_lastRanEntry.Version);

                string rootDirectory = _pathBuilder.GetRootDirectory(_lastRanEntry);
                string targetDirectory = _pathBuilder.GetStorageDirectory(_lastRanEntry);

                if (!Directory.Exists(targetDirectory))
                { 
                    Directory.CreateDirectory(targetDirectory);
                }

                // Create save directory (user will need to migrate/copy over files as they please)
                string storageSaveDirectory = _pathBuilder.GetSaveDirectory(_lastRanEntry);
                if (!Directory.Exists(storageSaveDirectory))
                {
                    Directory.CreateDirectory(storageSaveDirectory);
                }

                // Copy required files to storage directory
                foreach (var file in requiredFiles)
                {
                    var sourceFile = Path.Combine(rootDirectory, file);
                    var targetFile = Path.Combine(targetDirectory, file);
                    _logger.LogInfo($"Copying: {sourceFile} -> {targetFile}");
                    File.Copy(sourceFile, targetFile, true);
                }

                // Copy data files to storage directory if needed
                string rootDataDirectory = _pathBuilder.GetRootDataDirectory(_lastRanEntry);
                string storageDirectory = _pathBuilder.GetStorageDataDirectory(_lastRanEntry);

                if (Directory.Exists(rootDataDirectory))
                {
                    _logger.LogInfo("Copying /data/ directory to root directory");
                    FileSystem.CopyDirectory(rootDataDirectory, storageDirectory, true);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        /// <summary>
        /// Switches the files in the root directory with the ones needed for this specific entry.
        /// </summary>
        private void SwitchFiles()
        {
            try
            {
                var currentVersionRequiredFiles = _patchFileGenerator.GetRequiredFiles(_lastRanEntry.Version);
                var targetVersionRequiredFiles = _patchFileGenerator.GetRequiredFiles(_currentEntry.Version);

                string rootDirectory = _pathBuilder.GetRootDirectory(_lastRanEntry);
                string copyFromDirectory = _pathBuilder.GetStorageDirectory(_currentEntry);

                // get the files for the current version and remove them from the root directory
                foreach (var file in currentVersionRequiredFiles)
                {
                    var targetFile = Path.Combine(rootDirectory, file);
                    _logger.LogInfo("Deleting: " + targetFile);
                    File.Delete(targetFile);
                }

                // get the files for the target version and copy them to the root directory
                foreach (var file in targetVersionRequiredFiles)
                {
                    var sourceFile = Path.Combine(copyFromDirectory, file);
                    var targetFile = Path.Combine(rootDirectory, file);
                    _logger.LogInfo($"Copying: {sourceFile} -> {targetFile}");
                    File.Copy(sourceFile, targetFile, true);
                }

                // if the data folder exists in the root directory, remove it
                var rootDataDirectory = _pathBuilder.GetRootDataDirectory(_lastRanEntry);

                if (Directory.Exists(rootDataDirectory))
                {
                    _logger.LogInfo("Deleting /data/ directory in root directory");
                    Directory.Delete(rootDataDirectory, true);
                }

                // if the data folder exists in the target version directory, copy it over to root
                var storageDataDirectory = _pathBuilder.GetStorageDataDirectory(_currentEntry);
                if (Directory.Exists(storageDataDirectory))
                {
                    _logger.LogInfo("Copying /data/ directory to root directory");
                    FileSystem.CopyDirectory(storageDataDirectory, rootDataDirectory, true);
                }

                // Move the MPQ files away if it's Classic, but make sure they are in the root if it's expansion.
                if (_lastRanEntry.IsExpansion)
                {
                    // If our last entry is Expansion and current entry is Classic, then move files.
                    if (!_currentEntry.IsExpansion)
                    {
                        var expansionMpqs = _patchFileGenerator.ExpansionMpqs;

                        foreach (var mpqFile in expansionMpqs)
                        {
                            string mpqPath = Path.Combine(rootDirectory, mpqFile);
                            string targetMpqPath = mpqPath + ".bak";

                            _logger.LogInfo($"Moving: {mpqPath} -> {targetMpqPath}");
                            File.Move(mpqPath, targetMpqPath);
                        }
                    }
                }
                else
                {
                    // If our last entry is Classic and current entry is Expansion, then move files.
                    if (_currentEntry.IsExpansion)
                    {
                        var expansionMpqs = _patchFileGenerator.ExpansionMpqs;

                        foreach (var mpqFile in expansionMpqs)
                        {
                            string mpqPath = Path.Combine(rootDirectory, mpqFile);
                            string targetMpqPath = mpqPath + ".bak";

                            _logger.LogInfo($"Moving: {targetMpqPath} -> {mpqPath}");
                            File.Move(targetMpqPath, mpqPath);
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show("A file is still being used (You are probably switching entries too fast?). " +
                                "Switch back to the previous version and wait a few seconds after you exit the game " +
                               $"so that Windows stops using the file.\n\nError\n--------\n{ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
