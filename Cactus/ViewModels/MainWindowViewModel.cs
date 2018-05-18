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
using Cactus.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Windows;

namespace Cactus.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
    {
        private IEntryManager _entryManager;
        private IFileSwitcher _fileSwitcher;

        public EntryModel SelectedEntry { get; set; }
        public ObservableCollection<EntryModel> _entries;

        // Child View Models
        private IEditWindowViewModel _editWindowViewModel;

        // Commands
        public RelayCommand AddCommand { get; private set; }
        public RelayCommand EditCommand { get; private set; }
        public RelayCommand DeleteCommand { get; private set; }
        public RelayCommand CopyCommand { get; private set; }
        public RelayCommand UpCommand { get; private set; }
        public RelayCommand DownCommand { get; private set; }
        public RelayCommand LaunchCommand { get; private set; }

        private readonly string _appName = "Cactus";
        private readonly string _version = "0.0.2";

        public MainWindowViewModel(IEntryManager entryManager, IFileSwitcher fileSwitcher, IEditWindowViewModel editWindowViewModel)
        {
            _entryManager = entryManager;
            _fileSwitcher = fileSwitcher;
            _editWindowViewModel = editWindowViewModel;

            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand(Edit);
            DeleteCommand = new RelayCommand(Delete);
            CopyCommand = new RelayCommand(Copy);
            UpCommand = new RelayCommand(Up);
            DownCommand = new RelayCommand(Down);
            LaunchCommand = new RelayCommand(Launch);
        }

        public string Title
        {
            get
            {
                return $"{_appName} - {_version}";
            }
        }

        public ObservableCollection<EntryModel> Entries
        {
            get
            {
                if (_entries != null) return _entries;
                return new ObservableCollection<EntryModel>(_entryManager.GetEntries());
            }
            set
            {
                _entries = value;
                RaisePropertyChanged("Entries");
            }
        }

        public void Add()
        {
            var addWindow = new AddView()
            {
                Owner = Application.Current.MainWindow
            };

            addWindow.ShowDialog();

            // Get new entries so UI refreshes.
            Entries = new ObservableCollection<EntryModel>(_entryManager.GetEntries());
        }

        public void Edit()
        {
            if (SelectedEntry == null) return;

            _editWindowViewModel.CurrentEntry = SelectedEntry;

            var editWindow = new EditView()
            {
                Owner = Application.Current.MainWindow
            };

            editWindow.ShowDialog();
        }

        public void Delete()
        {
            if (SelectedEntry == null) return;

            _entryManager.Delete(SelectedEntry);
            _entryManager.SaveEntries();

            // Get new entries so UI refreshes.
            Entries = new ObservableCollection<EntryModel>(_entryManager.GetEntries());
        }

        public void Copy()
        {
            if (SelectedEntry == null)
            {
                MessageBox.Show("No entry to copy was selected.");
                return;
            }

            _entryManager.Copy(SelectedEntry);
            _entryManager.SaveEntries();

            // Get new entries so UI refreshes.
            Entries = new ObservableCollection<EntryModel>(_entryManager.GetEntries());
        }

        public void Up()
        {
            MessageBox.Show("This button has not been implemented yet.");
        }

        public void Down()
        {
            MessageBox.Show("This button has not been implemented yet.");
        }

        public void Launch()
        {
            _fileSwitcher.Run(SelectedEntry);
        }
    }
}
