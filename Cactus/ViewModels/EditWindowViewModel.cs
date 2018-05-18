using Cactus.Interfaces;
using Cactus.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.IO;

namespace Cactus.ViewModels
{
    public class EditWindowViewModel : ViewModelBase, IEditWindowViewModel
    {
        private IEntryManager _entryManager;
        private IVersionManager _versionManager;
        private IRegistryService _registryService;
        private IPathBuilder _pathBuilder;
        private IProcessManager _processManager;

        public EntryModel CurrentEntry { get; set; }

        // Keep the old label since if the user renames the label,
        // we will need to know the old one in order to rename the
        // directory on disk.
        private string _oldLabel;

        public RelayCommand OkCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        public EditWindowViewModel(IEntryManager entryManager, IVersionManager versionManager,
                                   IRegistryService registryService, IPathBuilder pathBuilder,
                                   IProcessManager processManager)
        {
            _entryManager = entryManager;
            _versionManager = versionManager;
            _registryService = registryService;
            _pathBuilder = pathBuilder;
            _processManager = processManager;

            OkCommand = new RelayCommand(Ok);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Ok()
        {
            var oldStorageDirectory = _pathBuilder.GetStorageDirectory(CurrentEntry, _oldLabel);
            var newStorageDirectory = _pathBuilder.GetStorageDirectory(CurrentEntry);

            if (Directory.Exists(oldStorageDirectory))
            {
                // If this entry is currently running, then we can't complete this
                // operation since the game is still using that directory/save path.
                if (CurrentEntry.WasLastRan && _processManager.AreProcessesRunning)
                {
                    Console.WriteLine("You can't edit this entry since the game is currently running and using its save directory.");
                    Console.WriteLine("Please close all instances of Diablo II and try again.");

                    ReverseChanges();
                    return;
                }

                if (oldStorageDirectory != newStorageDirectory)
                {
                    Directory.Move(oldStorageDirectory, newStorageDirectory);
                }
            }

            // If this was the last ran entry, then we need to also update the registry path.
            if (CurrentEntry.WasLastRan)
            {
                _registryService.Update(CurrentEntry);
            }

            _entryManager.SaveEntries();

            // Clear old label so next rename works.
            _oldLabel = null;
        }

        private void ReverseChanges()
        {
            Label = _oldLabel;
            _oldLabel = null;
        }

        private void Cancel()
        {
            ReverseChanges();
        }

        public string Label
        {
            get
            {
                if (_oldLabel == null)
                {
                    _oldLabel = CurrentEntry.Label;
                }
                return CurrentEntry.Label;
            }
            set
            {
                CurrentEntry.Label = value;
            }
        }

        public List<VersionModel> Versions
        {
            get
            {
                return _versionManager.ClassicVersions;
            }
        }

        // TODO: Can probably be simplified/optimized.
        public VersionModel SelectVersion
        {
            get
            {
                var versions = _versionManager.ClassicVersions;

                foreach (var version in versions)
                {
                    if (version.Version == CurrentEntry.Version)
                    {
                        return version;
                    }
                }
                return versions[0];
            }
            set
            {
                CurrentEntry.Version = value.Version;
            }
        }
    }
}
