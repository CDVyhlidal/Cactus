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
using System.Collections.Generic;
using System.IO;
using System.Security.Principal;
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
        private IFileGenerator _fileGenerator;
        private IProcessManager _processManager;
        private IRegistryService _registryService;
        private ILogger _logger;
        private IPathBuilder _pathBuilder;

        private EntryModel _currentEntry;
        private EntryModel _lastRanEntry;

        private enum Mode
        {
            Classic,
            Expansion
        }

        public FileSwitcher(IEntryManager entries, IFileGenerator fileGenerator,
                            IProcessManager processManager, IRegistryService registryService,
                            ILogger logger, IPathBuilder pathBuilder)
        {
            _entries = entries;
            _fileGenerator = fileGenerator;
            _processManager = processManager;
            _registryService = registryService;
            _logger = logger;
            _pathBuilder = pathBuilder;
        }

        public void Run(EntryModel entry)
        {
            _currentEntry = entry;
            _lastRanEntry = _entries.GetLastRan();

            if (_lastRanEntry == null)
            {
                _logger.LogInfo("No version was ever ran. Running this and setting it as main version.");

                // Before we switch, make sure to do validation and make sure
                // the user has the files they need before attempting to switch.
                if (!HasRequiredFiles())
                {
                    return;
                }

                _registryService.Update(_currentEntry);
                _entries.MarkLastRan(_currentEntry);
                _lastRanEntry = _currentEntry;

                // Backup files since this is the first time the user is running Cactus.
                CopyFilesToStorage();

                _entries.SaveEntries();

                LaunchGame();
            }
            else if (_lastRanEntry.Label == _currentEntry.Label && _lastRanEntry.IsExpansion == _currentEntry.IsExpansion &&
                     _lastRanEntry.Path == _currentEntry.Path && _lastRanEntry.Version == _currentEntry.Version)
            {
                _logger.LogInfo("Running the same version, no change needed.");

                // Will be using the CurrentEntry in this situation since even though the
                // current entry is equal to the last ran entry, the launch flags may differ
                _lastRanEntry = _currentEntry;

                // The user can launch two different entries that are identical (Except for flags since
                // you may have different flags for the same label) without switching files completely.
                LaunchGame();
            }
            else
            {
                _logger.LogInfo("A different version has been selected. Switching.");

                // Before we switch, make sure to do validation and make sure
                // the user has the files they need before attempting to switch.
                if (!HasRequiredFiles())
                {
                    return;
                }

                // If there is an existing process running, do not allow a switch.
                // Only identical versions can be launched.
                if (_processManager.AreProcessesRunning)
                {
                    MessageBox.Show("Another process related to another game mode/version is running.");
                    return;
                }

                _registryService.Update(_currentEntry);
                SwitchFiles();
                _entries.SwapLastRan(_lastRanEntry, _currentEntry);
                _lastRanEntry = _currentEntry;
                _entries.SaveEntries();

                LaunchGame();
            }
        }

        private bool HasRequiredFiles()
        {
            var requiredFiles = _fileGenerator.GetRequiredFiles(_currentEntry.Version);
            string targetRootDirectory = _pathBuilder.GetStorageDirectory(_currentEntry);

            // Add any remaining required files for mods that this entry is using
            if (_currentEntry.IsPlugy)
            {
                requiredFiles.AddRange(_fileGenerator.GetPlugyRequiredFiles);
            }

            if (_currentEntry.IsMedianXl)
            {
                requiredFiles.AddRange(_fileGenerator.GetMedianXlRequiredFiles);
            }

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
                var requiredFiles = _fileGenerator.GetRequiredFiles(_lastRanEntry.Version);

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
                var currentVersionRequiredFiles = _fileGenerator.GetRequiredFiles(_lastRanEntry.Version);
                var targetVersionRequiredFiles = _fileGenerator.GetRequiredFiles(_currentEntry.Version);

                string rootDirectory = _pathBuilder.GetRootDirectory(_lastRanEntry);
                string storageDirectory = _pathBuilder.GetStorageDirectory(_currentEntry);

                // Get the files for the current version and remove them from the root directory
                DeleteRequiredFiles(rootDirectory, currentVersionRequiredFiles);

                // Get the files for the target version and copy them to the root directory
                InstallRequiredFiles(rootDirectory, storageDirectory, targetVersionRequiredFiles);

                // Install data folder if needed.
                var rootDataDirectory = _pathBuilder.GetRootDataDirectory(_lastRanEntry);
                var storageDataDirectory = _pathBuilder.GetStorageDataDirectory(_currentEntry);

                CleanlyInstallExtraFolder("data", rootDataDirectory, storageDataDirectory);

                // Move the MPQ files away if it's Classic, but make sure they are in the root if it's expansion.
                if (_lastRanEntry.IsExpansion)
                {
                    // If our last entry is Expansion and current entry is Classic, then move files.
                    if (!_currentEntry.IsExpansion)
                    {
                        SwitchMpqs(Mode.Classic);
                    }
                }
                else
                {
                    // If our last entry is Classic and current entry is Expansion, then move files.
                    if (_currentEntry.IsExpansion)
                    {
                        SwitchMpqs(Mode.Expansion);
                    }
                }

                InstallPlugy(rootDirectory, storageDirectory);
                InstallMedianXl(rootDirectory, storageDirectory);
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

        /// <summary>
        /// Switches the Expansion MPQs in order to Enable/Disable Expansion or Classic modes.
        /// </summary>
        /// <param name="mode">Mode you want to switch to</param>
        private void SwitchMpqs(Mode mode)
        {
            string rootDirectory = _pathBuilder.GetRootDirectory(_lastRanEntry);
            var expansionMpqs = _fileGenerator.ExpansionMpqs;

            foreach (var mpqFile in expansionMpqs)
            {
                string basePath = Path.Combine(rootDirectory, mpqFile);
                string backupMpqPath = basePath + ".bak";

                string sourceMpqPath = mode == Mode.Classic ? basePath : backupMpqPath;
                string targetMpqPath = mode == Mode.Classic ? backupMpqPath : basePath;

                _logger.LogInfo($"Moving: {sourceMpqPath} -> {targetMpqPath}");
                File.Move(sourceMpqPath, targetMpqPath);
            }
        }

        private void LaunchGame()
        {
            WindowsIdentity user = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(user);
            bool isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);

            var launchThread = new Thread(() => _processManager.Launch(_lastRanEntry, isAdmin));
            launchThread.Start();
        }

        private void InstallRequiredFiles(string rootDirectory, string storageDirectory, List<string> requiredFiles)
        {
            // In with the new
            foreach (var file in requiredFiles)
            {
                var sourceFile = Path.Combine(storageDirectory, file);
                var targetFile = Path.Combine(rootDirectory, file);
                if (File.Exists(sourceFile))
                {
                    _logger.LogInfo($"Copying: {sourceFile} -> {targetFile}");
                    File.Copy(sourceFile, targetFile, true);
                }
            }
        }

        private void DeleteRequiredFiles(string rootDirectory, List<string> requiredFiles)
        {
            // Out with the old
            foreach (var file in requiredFiles)
            {
                var targetFile = Path.Combine(rootDirectory, file);
                if (File.Exists(file))
                {
                    _logger.LogInfo("Deleting: " + targetFile);
                    File.Delete(file);
                }
            }
        }

        /// <summary>
        /// Cleanly install the required files into the root directory.
        /// </summary>
        private void CleanlyInstallRequiredFiles(string rootDirectory, string storageDirectory, List<string> requiredFiles)
        {
            DeleteRequiredFiles(rootDirectory, requiredFiles);
            InstallRequiredFiles(rootDirectory, storageDirectory, requiredFiles);
        }

        private void InstallExtraFolder(string folderName, string extraFolderRootDirectory, string extraFolderStorageDirectory)
        {
            if (Directory.Exists(extraFolderStorageDirectory))
            {
                _logger.LogInfo($"Copying /{folderName}/ directory to root directory");
                FileSystem.CopyDirectory(extraFolderStorageDirectory, extraFolderRootDirectory, true);
            }
        }

        private void DeleteExtraFolder(string folderName, string extraFolderRootDirectory)
        {
            if (Directory.Exists(extraFolderRootDirectory))
            {
                _logger.LogInfo($"Deleting old /{folderName}/ directory from root directory");
                Directory.Delete(extraFolderRootDirectory, true);
            }
        }

        /// <summary>
        /// Cleanly installs an extra folder (/data/, /PlugY/, etc) into the root directory.
        /// </summary>
        private void CleanlyInstallExtraFolder(string folderName, string extraFolderRootDirectory, string extraFolderStorageDirectory)
        {
            DeleteExtraFolder(folderName, extraFolderRootDirectory);
            InstallExtraFolder(folderName, extraFolderRootDirectory, extraFolderStorageDirectory);
        }

        private void InstallPlugy(string rootDirectory, string storageDirectory)
        {
            _logger.LogInfo("Running PlugY Hook ...");

            string plugyRootDirectory = _pathBuilder.GetPlugyRootDirectory(_currentEntry);
            string plugyStorageDirectory = _pathBuilder.GetPlugyStorageDirectory(_currentEntry);
            var requiredFiles = _fileGenerator.GetPlugyRequiredFiles;

            DeleteRequiredFiles(rootDirectory, requiredFiles);
            DeleteExtraFolder("PlugY", plugyRootDirectory);

            if (_currentEntry.IsPlugy)
            {
                InstallRequiredFiles(rootDirectory, storageDirectory, requiredFiles);
                InstallExtraFolder("PlugY", plugyRootDirectory, plugyStorageDirectory);
            }

            // Delay the app a bit so things can settle on the disk
            // Or else the user may get an error when starting PlugY.
            Thread.Sleep(2000);
        }

        private void InstallMedianXl(string rootDirectory, string storageDirectory)
        {
            _logger.LogInfo("Running Median XL Hook ...");
            var requiredFiles = _fileGenerator.GetMedianXlRequiredFiles;

            DeleteRequiredFiles(rootDirectory, requiredFiles);

            if (_currentEntry.IsMedianXl)
            {
                InstallRequiredFiles(rootDirectory, storageDirectory, requiredFiles);
            }
        }
    }
}
