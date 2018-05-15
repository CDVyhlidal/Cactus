using System.Collections.Generic;

namespace Cactus.Interfaces
{
    public interface IPatchFileGenerator
    {
        List<string> GetRequiredFiles(string version);
        List<string> ExpansionMpqs { get; }
    }
}
