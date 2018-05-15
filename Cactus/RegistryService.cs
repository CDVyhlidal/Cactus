using Cactus.Interfaces;
using Cactus.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cactus
{
    public class RegistryService : IRegistryService
    {
        public int Update(EntryModel entry)
        {
            using (var key = Registry.CurrentUser.CreateSubKey(@"Software\Blizzard Entertainment\Diablo II"))
            {
                string baseDir = entry.IsExpansion ? "Expansion" : "Classic";
                string versionDir = Path.Combine(baseDir, entry.Label);
                string rootDirectory = Path.GetDirectoryName(entry.Path);
                int resolution = entry.IsExpansion ? 1 : 0;

                key.SetValue("Save Path", Path.Combine(rootDirectory, versionDir));
                key.SetValue("NewSavePath", Path.Combine(rootDirectory, versionDir));
                key.SetValue("Resolution", resolution);
            }

            return 0;
        }
    }
}
