using Cactus.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cactus
{
    public class ProcessManager : IProcessManager
    {
        public void LaunchApplication()
        {
            Console.WriteLine("launch application...");
        }
    }
}
