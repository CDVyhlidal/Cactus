using Cactus.Interfaces;
using Cactus.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cactus.ViewModels
{
    public class MainWindowViewModel : IMainWindowViewModel
    {
        private IEntryManager _entryManager;

        public ObservableCollection<Entry> Entries
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
