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
using Cactus.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Cactus
{
    public class JsonManager : IJsonManager
    {
        private IFileGenerator _fileGenerator;

        private readonly string _jsonDirectory;
        private readonly string _entriesJsonFile = "Entries.json";
        private readonly string _lastRequiredJsonFile = "LastRequiredFiles.json";

        private string EntriesJsonPath { get; }
        private string LastRequiredJsonPath { get; }

        public JsonManager(IFileGenerator fileGenerator)
        {
            _fileGenerator = fileGenerator;

            _jsonDirectory = Directory.GetCurrentDirectory();
            EntriesJsonPath = Path.Combine(_jsonDirectory, _entriesJsonFile);
            LastRequiredJsonPath = Path.Combine(_jsonDirectory, _lastRequiredJsonFile);
        }

        public void SaveEntries(List<EntryModel> entries)
        {
            string serializedEntries = JsonConvert.SerializeObject(entries, Formatting.Indented);
            File.WriteAllText(EntriesJsonPath, serializedEntries);
        }

        public List<EntryModel> GetEntries()
        {
            if (File.Exists(EntriesJsonPath))
            {
                var serializedEntries = File.ReadAllText(EntriesJsonPath);
                return JsonConvert.DeserializeObject<List<EntryModel>>(serializedEntries);
            }
            return new List<EntryModel>();
        }

        public void SaveLastRequiredFiles(RequiredFilesModel requiredFiles)
        {
            string serializedFiles = JsonConvert.SerializeObject(requiredFiles, Formatting.Indented);
            SaveToJsonFile(serializedFiles, LastRequiredJsonPath);
        }

        public RequiredFilesModel GetLastRequiredFiles()
        {
            if (File.Exists(LastRequiredJsonPath))
            {
                var serializedFiles = File.ReadAllText(LastRequiredJsonPath);
                var deserializedModel = JsonConvert.DeserializeObject<RequiredFilesModel>(serializedFiles);

                _fileGenerator.ValidateRequiredFiles(deserializedModel);
                return deserializedModel;
            }
            return null;
        }

        private void SaveToJsonFile(string serializedText, string outputFile)
        {
            File.WriteAllText(outputFile, serializedText);
        }
    }
}
