﻿using Cactus.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cactus.Interfaces
{
    /// <summary>
    /// The Entry Manager is responsible for managing all of the entries in the
    /// collection of Diablo II versions that player wants to play.
    /// </summary>
    public interface IEntryManager
    {
        EntryModel LastRan { get; }
        ObservableCollection<EntryModel> GetEntries();
    }
}
