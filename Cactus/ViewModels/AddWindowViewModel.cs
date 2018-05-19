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
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;

namespace Cactus.ViewModels
{
    public class AddWindowViewModel : ViewModelBase, IAddWindowViewModel
    {
        private IEntryManager _entryManager;
        private IVersionManager _versionManager;
        private IRegistryService _registryService;
        private IPathBuilder _pathBuilder;
        private IProcessManager _processManager;

        // Properties for new entry
        public string Label { get; set; }
        public string Version { get; set; }
        public string Path { get; set; }
        public string Flags { get; set; }
        public bool IsExpansion { get; set; }

        // Allow parent view model to retrieve this property.
        public EntryModel AddedEntry { get; set; }

        public RelayCommand OkCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        public AddWindowViewModel(IEntryManager entryManager, IVersionManager versionManager,
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
            var entry = new EntryModel
            {
                Label = Label,
                Version = Version,
                Path = Path,
                Flags = Flags,
                IsExpansion = IsExpansion
            };

            if (Label != null)
            {
                _entryManager.Add(entry);
                _entryManager.SaveEntries();

                AddedEntry = entry;
            }
            else
            {
                Console.WriteLine("Label must not be empty.");
            }

            ResetUI();
        }

        private void Cancel()
        {
            ResetUI();
        }

        private void ResetUI()
        {
            Label = null;
            Version = "1.00";
            Path = null;
            Flags = null;
            IsExpansion = false;
        }

        public Dictionary<string, VersionModel> Versions
        {
            get
            {
                return _versionManager.Versions;
            }
        }
    }
}
