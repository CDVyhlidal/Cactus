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
using System.Windows;

namespace Cactus.ViewModels
{
    public class AddWindowViewModel : ViewModelBase, IAddWindowViewModel
    {
        private IEntryManager _entryManager;
        private readonly IRegistryService _registryService;
        private readonly IPathBuilder _pathBuilder;
        private readonly IProcessManager _processManager;

        // Properties for new entry
        public string Platform { get; set; }
        public string Path { get; set; }
        public string Flags { get; set; }
        public bool IsExpansion { get; set; } = true;

        // Allow parent view model to retrieve this property.
        public EntryModel AddedEntry { get; set; }

        public RelayCommand OkCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        public AddWindowViewModel(IEntryManager entryManager, IRegistryService registryService,
                                  IPathBuilder pathBuilder, IProcessManager processManager)
        {
            _entryManager = entryManager;
            _registryService = registryService;
            _pathBuilder = pathBuilder;
            _processManager = processManager;

            OkCommand = new RelayCommand(Ok);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Ok()
        {
            var entry = new EntryModel
            {
                Platform = Platform,
                Path = Path,
                Flags = Flags,
                IsExpansion = IsExpansion
            };

            if (string.IsNullOrWhiteSpace(Platform) || string.IsNullOrWhiteSpace(Path) ||
                !_entryManager.IsRootDirectoryEqualToOthers(entry) || _pathBuilder.ContainsInvalidCharacters(entry.Platform))
            {
                MessageBox.Show("Please make sure all fields are populated, root path should match the rest of your entries (.exe can vary), and no invalid characters.");
            }
            else
            {
                _entryManager.Add(entry);
                _entryManager.SaveEntries();

                AddedEntry = entry;
            }

            ResetUI();
        }

        private void Cancel()
        {
            ResetUI();
        }

        private void ResetUI()
        {
            Platform = null;
            Path = null;
            Flags = null;
            IsExpansion = true;
        }
    }
}
