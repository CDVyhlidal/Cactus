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
        public RelayCommand LaunchCommand { get; private set; }

        public MainWindowViewModel(IEntryManager entryManager, IFileSwitcher fileSwitcher)
        {
            _entryManager = entryManager;
            _fileSwitcher = fileSwitcher;

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

        public void Launch()
        {
            _fileSwitcher.Run();
        }
    }
}
