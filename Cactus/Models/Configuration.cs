using Cactus.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cactus.Models
{
    /// <summary>
    /// This class is responsible for receiving all of the entries and settings from the JsonFile.
    /// </summary>
    public class Configuration : IConfiguration
    {
        public string RootDirectory
        {
            get => @"D:\Games\Diablo II";
            set
            {
                
            }
        }
        public Dictionary<string, EntryModel> Entries { get; set; }
    }
}
