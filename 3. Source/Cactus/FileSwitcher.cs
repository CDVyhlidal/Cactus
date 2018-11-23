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
using Microsoft.VisualBasic.FileIO;
using System;
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
        private IJsonManager _jsonManager;
        
        private EntryModel _currentEntry;
        private EntryModel _lastRanEntry;

        private enum Mode
        {
            Classic,
            Expansion
        }

        public FileSwitcher(IEntryManager entries, IFileGenerator fileGenerator,
                            IProcessManager processManager, IRegistryService registryService,
                            ILogger logger, IPathBuilder pathBuilder, IJsonManager jsonManager)
        {
            _entries = entries;
            _fileGenerator = fileGenerator;
            _processManager = processManager;
            _registryService = registryService;
            _logger = logger;
            _pathBuilder = pathBuilder;
            _jsonManager = jsonManager;
        }

        public void Run(EntryModel entry)
        {
            _currentEntry = entry;
            _lastRanEntry = _entries.GetLastRan();

            if (_lastRanEntry == null)
            {
                _logger.LogInfo("No version was ever ran. Running this and setting it as main version.");

                _lastRanEntry = _currentEntry;
                SwitchFiles();

                _entries.MarkLastRan(_currentEntry);
                _registryService.Update(_currentEntry);
                _entries.SaveEntries();

                LaunchGame();
            }
            else if (_lastRanEntry.Platform.EqualsIgnoreCase(_currentEntry.Platform) &&
                     _lastRanEntry.Path.EqualsIgnoreCase(_currentEntry.Path) &&
                     _lastRanEntry.IsExpansion == _currentEntry.IsExpansion)
            {
                _logger.LogInfo("Running the same version, no change needed.");

                // Will be using the CurrentEntry in this situation since even though the
                // current entry is equal to the last ran entry, the launch flags may differ
                _lastRanEntry = _currentEntry;

                // The user can launch two different entries that are identical (Except for flags since
                // you may have different flags for the same platform) without switching files completely.
                LaunchGame();
            }
            else
            {
                _logger.LogInfo("A different version has been selected. Switching.");

                // If there is an existing process running, do not allow a switch.
                // Only identical versions can be launched.
                if (_processManager.AreProcessesRunning)
                {
                    MessageBox.Show("Another process related to another game mode/version is running.");
                    return;
                }

                SwitchFiles();
                _entries.SwapLastRan(_lastRanEntry, _currentEntry);
                _lastRanEntry = _currentEntry;
                _registryService.Update(_currentEntry);
                _entries.SaveEntries();

                LaunchGame();
            }
        }

        /// <summary>
        /// Switches the files in the root directory with the ones needed for this specific entry.
        /// </summary>
        private void SwitchFiles()
        {
            try
            {
                string rootDirectory = _pathBuilder.GetRootDirectory(_lastRanEntry);
                string platformDirectory = _pathBuilder.GetPlatformDirectory(_currentEntry);

                // Retrieve the old required files so we can clean them up when we switch entries.
                var lastRequiredFiles = _jsonManager.GetLastRequiredFiles();

                if (lastRequiredFiles == null)
                {
                    _logger.LogWarning("The last required files file doesn't exist. Using current version as a bases.");
                    lastRequiredFiles = _fileGenerator.GetRequiredFiles(_lastRanEntry);
                }

                var targetVersionRequiredFiles = _fileGenerator.GetRequiredFiles(_currentEntry);

                DeleteRequiredFiles(rootDirectory, lastRequiredFiles);
                InstallRequiredFiles(platformDirectory, rootDirectory, targetVersionRequiredFiles);

                // Make sure save directory exists
                CreateSaveDirectoryIfNeeded(_currentEntry);

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

                // Save the required files for the target since we will use these to clean up when we switch.
                _jsonManager.SaveLastRequiredFiles(targetVersionRequiredFiles);
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

        private void InstallRequiredFiles(string platformDirectory, string rootDirectory, RequiredFilesModel requiredFiles)
        {
            // In with the new
            foreach (var file in requiredFiles.Files)
            {
                var sourceFile = Path.Combine(platformDirectory, file);
                var targetFile = Path.Combine(rootDirectory, file);
                if (File.Exists(sourceFile))
                {
                    _logger.LogInfo($"Copying: {sourceFile} -> {targetFile}");
                    File.Copy(sourceFile, targetFile, true);
                }
            }

            foreach (var file in requiredFiles.Directories)
            {
                var sourceDirectory = Path.Combine(platformDirectory, file);
                var targetDirectory = Path.Combine(rootDirectory, file);
                if (Directory.Exists(sourceDirectory))
                {
                    _logger.LogInfo($"Copying: {sourceDirectory} -> {targetDirectory}");
                    FileSystem.CopyDirectory(sourceDirectory, targetDirectory, true);
                }
            }
        }

        private void DeleteRequiredFiles(string rootDirectory, RequiredFilesModel requiredFiles)
        {
            // Out with the old
            foreach (var file in requiredFiles.Files)
            {
                string targetFile = Path.Combine(rootDirectory, file);

                if (File.Exists(targetFile))
                {
                    _logger.LogInfo($"Deleting: {targetFile}");
                    File.Delete(targetFile);
                }
            }

            foreach (var directory in requiredFiles.Directories)
            {
                string targetDirectory = Path.Combine(rootDirectory, directory);

                if (Directory.Exists(targetDirectory))
                {
                    _logger.LogError($"Deleting: {targetDirectory}");
                    Directory.Delete(targetDirectory, true);
                }
            }
        }
        
        private void CreateSaveDirectoryIfNeeded(EntryModel entry)
        {
            string saveDirectory = _pathBuilder.GetSaveDirectory(entry);

            if (!Directory.Exists(saveDirectory))
            {
                Directory.CreateDirectory(saveDirectory);
            }
        }
    }
}
