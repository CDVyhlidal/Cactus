using System.Collections.Generic;

namespace Cactus.Models
{
    public class RequiredFilesModel
    {
        public List<string> Directories { get; set; } = new List<string>();
        public List<string> Files { get; set; } = new List<string>();
    }
}
