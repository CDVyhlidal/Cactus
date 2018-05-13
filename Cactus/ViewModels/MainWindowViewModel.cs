using Cactus.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cactus.ViewModels
{
    public class MainWindowViewModel : IMainWindowViewModel
    {
        private IEntryManager _entryManager;

        public MainWindowViewModel(IEntryManager entryManager)
        {
            _entryManager = entryManager;
        }
    }
}
