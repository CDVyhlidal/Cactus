using Cactus.Interfaces;
using Cactus.Models;
using Cactus.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
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
                return "Cactus";
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
            Console.WriteLine("Copy");
        }

        public void Up()
        {
            Console.WriteLine("Up");
        }

        public void Down()
        {
            Console.WriteLine("Down");
        }

        public void Launch()
        {
            _fileSwitcher.Run(SelectedEntry);
        }
    }
}
