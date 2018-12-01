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

        // Child View Models
        private IAddWindowViewModel _addWindowViewModel;
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
        private readonly string _version = "1.2.2";

        public MainWindowViewModel(IEntryManager entryManager, IFileSwitcher fileSwitcher, IAddWindowViewModel addWindowViewModel, IEditWindowViewModel editWindowViewModel)
        {
            _entryManager = entryManager;
            _fileSwitcher = fileSwitcher;
            _addWindowViewModel = addWindowViewModel;
            _editWindowViewModel = editWindowViewModel;

            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand(Edit);
            DeleteCommand = new RelayCommand(Delete);
            CopyCommand = new RelayCommand(Copy);
            UpCommand = new RelayCommand(Up);
            DownCommand = new RelayCommand(Down);
            LaunchCommand = new RelayCommand(Launch);

            RefreshEntriesList();
            SelectLastRanEntry();
        }

        public string Title
        {
            get
            {
                return $"{_appName} - {_version}";
            }
        }

        private ObservableCollection<EntryModel> _entries;
        public ObservableCollection<EntryModel> Entries
        {
            get
            {
                return _entries;
            }
            set
            {
                _entries = value;
                RaisePropertyChanged("Entries");
            }
        }

        private EntryModel _selectedEntry;
        public EntryModel SelectedEntry
        {
            get
            {
                return _selectedEntry;
            }
            set
            {
                _selectedEntry = value;
                RaisePropertyChanged("SelectedEntry");
            }
        }

        public void Add()
        {
            var addWindow = new AddView()
            {
                Owner = Application.Current.MainWindow
            };

            addWindow.ShowDialog();

            SelectedEntry = _addWindowViewModel.AddedEntry;

            RefreshEntriesList();
        }

        public void Edit()
        {
            if (SelectedEntry == null)
            {
                MessageBox.Show("No entry to edit was selected.");
                return;
            }

            _editWindowViewModel.CurrentEntry = SelectedEntry;
            _editWindowViewModel.LastRanEntry = GetLastRanEntry();

            var editWindow = new EditView()
            {
                Owner = Application.Current.MainWindow
            };

            editWindow.ShowDialog();
        }

        public void Delete()
        {
            if (SelectedEntry == null)
            {
                MessageBox.Show("No entry to delete was selected.");
                return;
            }

            int deletedIndex = _entryManager.Delete(SelectedEntry);
            _entryManager.SaveEntries();

            RefreshEntriesList();

            if (_entries.Count != 0)
            {
                int targetIndex = 0;

                // If there are still entries but it isn't the ceiling entry, then adjust one up.
                if (deletedIndex != 0)
                {
                    targetIndex = deletedIndex - 1;
                }
                SelectedEntry = _entries[targetIndex];
            }
        }

        public void Copy()
        {
            if (SelectedEntry == null)
            {
                MessageBox.Show("No entry to copy was selected.");
                return;
            }

            var newEntry = _entryManager.Copy(SelectedEntry);
            _entryManager.SaveEntries();

            RefreshEntriesList();
            SelectedEntry = newEntry;
        }

        public void Up()
        {
            if (SelectedEntry == null)
            {
                MessageBox.Show("No entry to move up was selected.");
                return;
            }

            _entryManager.MoveUp(SelectedEntry);
            _entryManager.SaveEntries();

            RefreshEntriesList();
        }

        public void Down()
        {
            if (SelectedEntry == null)
            {
                MessageBox.Show("No entry to move down was selected.");
                return;
            }

            _entryManager.MoveDown(SelectedEntry);
            _entryManager.SaveEntries();

            RefreshEntriesList();
        }

        public void Launch()
        {
            if (SelectedEntry == null)
            {
                MessageBox.Show("No entry to launch was selected.");
                return;

            }

            if (string.IsNullOrWhiteSpace(SelectedEntry.Path) || string.IsNullOrWhiteSpace(SelectedEntry.Platform))
            {
                MessageBox.Show("This entry has no platform or path set.");
                return;
            }

            _fileSwitcher.Run(SelectedEntry);
        }

        private void RefreshEntriesList()
        {
            Entries = new ObservableCollection<EntryModel>(_entryManager.GetEntries());
        }

        private void SelectLastRanEntry()
        {
            var lastRanEntry = GetLastRanEntry();
            if (lastRanEntry != null)
            {
                SelectedEntry = lastRanEntry;
            }
        }

        private EntryModel GetLastRanEntry()
        {
            foreach (var entry in _entries)
            {
                if (entry.WasLastRan)
                {
                    return entry;
                }
            }
            return null;
        }
    }
}

