using Cactus.Interfaces;
using Cactus.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cactus.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
    {
        private IEntryManager _entryManager;
        private IFileSwitcher _fileSwitcher;

        // Commands
        public RelayCommand AddCommand { get; private set; }
        public RelayCommand EditCommand { get; private set; }
        public RelayCommand DeleteCommand { get; private set; }
        public RelayCommand UpCommand { get; private set; }
        public RelayCommand DownCommand { get; private set; }
        public RelayCommand AboutCommand { get; private set; }
        public RelayCommand LaunchCommand { get; private set; }

        public MainWindowViewModel(IEntryManager entryManager, IFileSwitcher fileSwitcher)
        {
            _entryManager = entryManager;
            _fileSwitcher = fileSwitcher;

            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand(Edit);
            DeleteCommand = new RelayCommand(Delete);
            UpCommand = new RelayCommand(Up);
            DownCommand = new RelayCommand(Down);
            AboutCommand = new RelayCommand(About);
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
                return _entryManager.GetEntries();
            }
        }

        // Command Functions
        public void Add()
        {
            Console.WriteLine("Add");
        }

        public void Edit()
        {
            Console.WriteLine("Edit");
        }

        public void Delete()
        {
            Console.WriteLine("Delete");
        }

        public void Up()
        {
            Console.WriteLine("Up");
        }

        public void Down()
        {
            Console.WriteLine("Down");
        }

        public void About()
        {
            Console.WriteLine("About");
        }

        public void Launch()
        {
            _fileSwitcher.Run();
        }
    }
}
