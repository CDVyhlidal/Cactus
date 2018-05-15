using Cactus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cactus.Interfaces
{
    public interface IRegistryService
    {
        int Update(EntryModel entry);
    }
}
