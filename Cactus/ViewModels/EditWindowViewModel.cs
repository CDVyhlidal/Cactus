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
        
        // Keep the old entry since we need it to restore all of the UI if the user cancels.
        private EntryModel _oldEntry;

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
            if (String.IsNullOrWhiteSpace(CurrentEntry.Label))
            {
                Console.WriteLine("Label cannot be empty. Aborting operation.");
                ReverseChanges();
                return;
            }

            // If the oldEntry's label is null, that means that the user made a copy of an
            // entry and now is trying to rename it. No renaming of directories needs to
            // happen here. Just saving.
            if (!String.IsNullOrWhiteSpace(_oldEntry.Label))
            {
                var oldStorageDirectory = _pathBuilder.GetStorageDirectory(_oldEntry);
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
            }

            _entryManager.SaveEntries();

            // Clear old entry so next updates work properly.
            _oldEntry = null;
        }

        private void Cancel()
        {
            ReverseChanges();
        }

        private void ReverseChanges()
        {
            CurrentEntry.Label = _oldEntry.Label;
            CurrentEntry.Version = _oldEntry.Version;
            CurrentEntry.Path = _oldEntry.Path;
            CurrentEntry.Flags = _oldEntry.Flags;
            CurrentEntry.IsExpansion = _oldEntry.IsExpansion;
            CurrentEntry.WasLastRan = _oldEntry.WasLastRan;

            _oldEntry = null;
        }

        public string Label
        {
            get
            {
                // Kinda dirty but we are using the label as the way to backup the entire object.
                if (_oldEntry == null)
                {
                    _oldEntry = new EntryModel
                    {
                        Flags = CurrentEntry.Flags,
                        Label = CurrentEntry.Label,
                        IsExpansion = CurrentEntry.IsExpansion,
                        Path = CurrentEntry.Path,
                        Version = CurrentEntry.Version,
                        WasLastRan = CurrentEntry.WasLastRan
                    };
                }
                return CurrentEntry.Label;
            }
            set
            {
                CurrentEntry.Label = value;
            }
        }

        public Dictionary<string, VersionModel> Versions
        {
            get
            {
                return _versionManager.Versions;
            }
        }

        public string SelectVersion
        {
            get
            {
                return _versionManager.Versions[CurrentEntry.Version].Version;
            }
            set
            {
                CurrentEntry.Version = value;
            }
        }
    }
}
