using Cactus.Interfaces;
using Cactus.Models;
using GalaSoft.MvvmLight;
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

        public MainWindowViewModel(IEntryManager entryManager)
        {
            _entryManager = entryManager;
        }
    }
}
