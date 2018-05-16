using Cactus.Models;
using System.Threading.Tasks;

namespace Cactus.Interfaces
{
    public interface IProcessManager
    {
        bool AreProcessesRunning { get; }
        void Launch(EntryModel entry);
    }
}
