using Cactus.Interfaces;
using Cactus.Models;
using Microsoft.VisualBasic.FileIO;
using System;
using System.IO;

namespace Cactus
{
    /// <summary>
    /// This class is responsible for the file switching that occurs when
    /// the userrequests to run a version of Diablo II.
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

            if (_currentEntry == null)
            {
                _logger.LogWarning("No element was selected. Skipping.");
                return;
            }

            _lastRanEntry = _entries.GetLastRan();

            if (_lastRanEntry == null)
            {
                _logger.LogInfo("No version was ever ran. Running this and setting it as main version.");
            }
            else if (_lastRanEntry.Label == _currentEntry.Label && _lastRanEntry.IsExpansion == _currentEntry.IsExpansion)
            {
                _logger.LogInfo("Running the same version, no change needed.");
                //_processManager.Launch(lastRanEntry);
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

                // Update Registry
                _registryService.Update(_currentEntry);

                // Switch Files
                SwitchFiles();

                // mark entry as last ran and store
                _entries.MarkAsLastRan(_lastRanEntry, _currentEntry);

                // Save Entries
                _entries.SaveEntries();

                // launch the app
                //_processManager.Launch(lastRanEntry);
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
                    _logger.LogError($"The target file doesn't exist: {targetFile}");
                    return false;
                }
            }

            return true;
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
                    Console.WriteLine("Deleting: " + targetFile);
                    File.Delete(targetFile);
                }

                // get the files for the target version and copy them to the root directory
                foreach (var file in targetVersionRequiredFiles)
                {
                    var sourceFile = Path.Combine(copyFromDirectory, file);
                    var targetFile = Path.Combine(rootDirectory, file);
                    Console.WriteLine($"Copying: {sourceFile} -> {targetFile}");
                    File.Copy(sourceFile, targetFile);
                }

                // if the data folder exists in the root directory, remove it
                var rootDataDirectory = _pathBuilder.GetRootDataDirectory(_lastRanEntry);

                if (Directory.Exists(rootDataDirectory))
                {
                    _logger.LogInfo("Deleting /data/ directory in root directory");
                    Directory.Delete(rootDataDirectory);
                }

                // if the data folder exists in the target version directory, copy it over to root
                var storageDataDirectory = _pathBuilder.GetStorageDataDirectory(_currentEntry);
                if (Directory.Exists(storageDataDirectory))
                {
                    _logger.LogInfo("Copying /data/ directory to root directory");
                    FileSystem.CopyDirectory(storageDataDirectory, rootDataDirectory);
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
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
