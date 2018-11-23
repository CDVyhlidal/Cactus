// Copyright (C) 2018 Jonathan Vasquez <jon@xyinn.org>
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program. If not, see<https://www.gnu.org/licenses/>.

using Cactus.Models;
using System.Collections.Generic;

namespace Cactus.Interfaces
{
    /// <summary>
    /// The Entry Manager is responsible for managing all of the entries in the
    /// collection of Diablo II versions that player wants to play.
    /// </summary>
    public interface IEntryManager
    {
        EntryModel GetLastRan();
        void Add(EntryModel entry);
        int Delete(EntryModel entry);
        EntryModel Copy(EntryModel entry);
        void MoveUp(EntryModel entry);
        void MoveDown(EntryModel entry);
        List<EntryModel> GetEntries();
        void MarkLastRan(EntryModel entry);
        void SwapLastRan(EntryModel oldEntry, EntryModel newEntry);
        void SaveEntries();
        bool IsRootDirectoryEqualToOthers(EntryModel entry);
        void RenamePlatform(string oldPlatformName, string newPlatformName);
    }
}
