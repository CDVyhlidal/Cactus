using Cactus.Interfaces;
using Cactus.Models;
using Microsoft.Win32;

namespace Cactus
{
    public class RegistryService : IRegistryService
    {
        IPathBuilder _pathBuilder;

        public RegistryService(IPathBuilder pathBuilder)
        {
            _pathBuilder = pathBuilder;
        }

        public void Update(EntryModel entry)
        {
            using (var key = Registry.CurrentUser.CreateSubKey(@"Software\Blizzard Entertainment\Diablo II"))
            {
                string saveDirectory = _pathBuilder.GetSaveDirectory(entry);
                int resolution = entry.IsExpansion ? 1 : 0;

                key.SetValue("Save Path", saveDirectory);
                key.SetValue("NewSavePath", saveDirectory);
                key.SetValue("Resolution", resolution);
            }
        }
    }
}
