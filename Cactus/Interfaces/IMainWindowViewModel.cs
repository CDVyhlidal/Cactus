using Cactus.Models;
using System.Collections.ObjectModel;

namespace Cactus.Interfaces
{
    public interface IMainWindowViewModel
    {
        ObservableCollection<EntryModel> Entries { get; set; }
    }
}
