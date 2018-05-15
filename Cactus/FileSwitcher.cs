using Cactus.Interfaces;
using Cactus.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cactus
{
    /// <summary>
    /// This class is responsible for the file switching that occurs when the user
    /// requests to run a version of Diablo II.
    /// </summary>
    public class FileSwitcher : IFileSwitcher
    {
        private IEntryManager _entries;
        private IPatchFileGenerator _patchFileGenerator;
        private IProcessManager _processManager;
        private IRegistryService _registryService;
        private ILogger _logger;
        private IPathBuilder _pathBuilder;

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
            if (entry == null)
            {
                Console.WriteLine("No element was selected. Skipping.");
                return;
            }

            // If nothing was ever ran, then ..
            // If what we are running matches what we last ran, then go directly
            // If we are switching entries, then do what you need to do to get the files in the correct order.
            var lastRanEntry = _entries.LastRan;

            if (lastRanEntry == null)
            {
                Console.WriteLine("No version was ever ran. Running this and setting it as main version.");
            }
            else if (lastRanEntry.Label == entry.Label && lastRanEntry.IsExpansion == entry.IsExpansion)
            {
                Console.WriteLine("Running the same version, no change needed.");
                //_processManager.Launch(lastRanEntry);
            }
            else
            {
                Console.WriteLine("A different version has been selected. Switching.");

                // Before we switch, make sure to do validation and make sure user
                // has the files they need before attempting to switch.
                if (!HasRequiredFiles(entry))
                {
                    return;
                }

                // Update Registry
                // Switch Files
                // What about data folder?
                // mark entry as last ran and store
                // launch the app
                _registryService.Update(entry);
            }
        }

        private bool HasRequiredFiles(EntryModel entry)
        {
            var requiredFiles = _patchFileGenerator.GetRequiredFiles(entry.Version);
            string targetRootDirectory = _pathBuilder.GetStorageDirectory(entry);

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
    }
}
