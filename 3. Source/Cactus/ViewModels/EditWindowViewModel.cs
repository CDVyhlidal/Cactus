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
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.IO;
using System.Windows;

namespace Cactus.ViewModels
{
    public class EditWindowViewModel : ViewModelBase, IEditWindowViewModel
    {
        private IEntryManager _entryManager;
        private IRegistryService _registryService;
        private IPathBuilder _pathBuilder;
        private IProcessManager _processManager;
        private ILogger _logger;

        public EntryModel CurrentEntry { get; set; }
        public EntryModel LastRanEntry { get; set; }

        // Keep the old entry since we need it to restore all of the UI if the user cancels.
        private EntryModel _oldEntry;

        public RelayCommand OkCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        public EditWindowViewModel(IEntryManager entryManager, IRegistryService registryService,
                                   IPathBuilder pathBuilder, IProcessManager processManager, ILogger logger)
        {
            _entryManager = entryManager;
            _registryService = registryService;
            _pathBuilder = pathBuilder;
            _processManager = processManager;
            _logger = logger;

            OkCommand = new RelayCommand(Ok);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Ok()
        {
            if (string.IsNullOrWhiteSpace(CurrentEntry.Platform) || string.IsNullOrWhiteSpace(CurrentEntry.Path) ||
                !_entryManager.IsRootDirectoryEqualToOthers(CurrentEntry) ||
                _pathBuilder.ContainsInvalidCharacters(CurrentEntry.Platform))
            {
                MessageBox.Show("Unable to change your Entry! Please make sure all fields are:\n\n" +
                    "- Populated (Flags are optional)\n" +
                    "- Path should match the rest of your Entries (.exe can vary)\n" +
                    "- No invalid characters\n\n" +
                    "If you have moved Cactus to a new machine, please manually edit the Entries.json and adjust all your paths accordingly.");
                ReverseChanges();
                return;
            }

            // If the oldEntry's platform is null, that means that the user
            // made a copy of an entry and now is trying to rename it.
            if (!string.IsNullOrWhiteSpace(_oldEntry.Platform))
            {
                // If we are switching the last ran state from on to off,
                // then we will not rename the directory, since if we do this,
                // we may effectively be renaming the storage folder which should
                // remain isolated.
                if (_oldEntry.WasLastRan == CurrentEntry.WasLastRan)
                {
                    var oldPlatformDirectory = _pathBuilder.GetPlatformDirectory(_oldEntry);
                    var newPlatformDirectory = _pathBuilder.GetPlatformDirectory(CurrentEntry);

                    var oldSavesDirectory = _pathBuilder.GetSaveDirectory(_oldEntry);
                    var newSavesDirectory = _pathBuilder.GetSaveDirectory(CurrentEntry);

                    // We can skip renaming if it's the same platform.
                    // No renaming of directories needs to happen here. Just saving.
                    if (!oldPlatformDirectory.EqualsIgnoreCase(newPlatformDirectory))
                    {
                        // If the target directory name already exists, we can't allow this rename to happen.
                        if (Directory.Exists(newPlatformDirectory) || Directory.Exists(newSavesDirectory))
                        {
                            MessageBox.Show($"A platform/save directory with the name \"{CurrentEntry.Platform}\" already exists.");
                            ReverseChanges();
                            return;
                        }

                        // If this entry is currently running, then we can't complete this
                        // operation since the game is still using that directory/save path.
                        if (CurrentEntry.WasLastRan && _processManager.AreProcessesRunning)
                        {
                            _logger.LogWarning("You can't edit this entry since the game is currently running and using its save directory.");
                            _logger.LogWarning("Please close all instances of Diablo II and try again.");

                            ReverseChanges();
                            return;
                        }

                        if (Directory.Exists(oldPlatformDirectory))
                        {
                            Directory.Move(oldPlatformDirectory, newPlatformDirectory);
                        }

                        if (Directory.Exists(oldSavesDirectory))
                        {
                            Directory.Move(oldSavesDirectory, newSavesDirectory);
                        }

                        // Rename any platforms with the same platform name
                        _entryManager.RenamePlatform(_oldEntry.Platform, CurrentEntry.Platform);
                    }
                }

                // If this entry was switched from not being the last ran to being the last ran,
                // Then we need to disable whichever entry was last ran if there was one.
                if (!_oldEntry.WasLastRan && CurrentEntry.WasLastRan && LastRanEntry != null)
                {
                    LastRanEntry.WasLastRan = false;
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
            CurrentEntry.Platform = _oldEntry.Platform;
            CurrentEntry.Path = _oldEntry.Path;
            CurrentEntry.Flags = _oldEntry.Flags;
            CurrentEntry.IsExpansion = _oldEntry.IsExpansion;
            CurrentEntry.WasLastRan = _oldEntry.WasLastRan;

            _oldEntry = null;
        }

        public string Platform
        {
            get
            {
                // Kinda dirty but we are using the platform as the way to backup the entire object.
                if (_oldEntry == null)
                {
                    _oldEntry = new EntryModel
                    {
                        Platform = CurrentEntry.Platform,
                        Path = CurrentEntry.Path,
                        Flags = CurrentEntry.Flags,
                        IsExpansion = CurrentEntry.IsExpansion,
                        WasLastRan = CurrentEntry.WasLastRan
                    };
                }
                return CurrentEntry.Platform;
            }
            set
            {
                CurrentEntry.Platform = value;
            }
        }
    }
}
