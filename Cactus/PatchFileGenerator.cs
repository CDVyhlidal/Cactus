// Copyright (C) 2018 Jonathan Vasquez <jon@xyinn.org>
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Cactus.Interfaces;
using System.Collections.Generic;

namespace Cactus
{
    /// <summary>
    /// This class is responsible for returning a list of all of
    /// the files corresponding to a specific patch.
    /// </summary>
    public class PatchFileGenerator : IPatchFileGenerator
    {
        private IVersionManager _versionManager;

        public PatchFileGenerator(IVersionManager versionManager)
        {
            _versionManager = versionManager;
        }

        public List<string> GetRequiredFiles(string version)
        {
            var requiredFiles = new List<string>();

            requiredFiles.AddRange(_commonFiles);
            requiredFiles.AddRange(_thirdPartyLibraries);

            if (_versionManager.Is114OrNewer(version))
            {
                requiredFiles.AddRange(_requiredPost113Files);
                requiredFiles.Add(_patchMpqFile);
            }
            else
            {
                // Every other version is the same (1.00-1.13), just 1.00 and 1.07 don't have a Patch_D2.mpq.
                requiredFiles.AddRange(_requiredPre114Files);

                if (_versionManager.RequiresPatchFile(version))
                {
                    requiredFiles.Add(_patchMpqFile);
                }
            }

            return requiredFiles;
        }

        public List<string> ExpansionMpqs
        {
            get
            {
                return new List<string>()
                {
                    "d2exp.mpq",
                    "d2xmusic.mpq",
                    "d2xvideo.mpq",
                    "d2xtalk.mpq"
                };
            }
        }

        private readonly List<string> _thirdPartyLibraries = new List<string>()
        {
            "binkw32.dll",
            "ijl11.dll",
            "SmackW32.dll"
        };

        // Keeping separate from commonFiles since 1.07 doesn't have it.
        private const string _patchMpqFile = "Patch_D2.mpq";

        private readonly List<string> _commonFiles = new List<string>()
        {
            "Diablo II.exe",
            "Game.exe"
        };

        private readonly List<string> _requiredPre114Files = new List<string>()
        {
            "Bnclient.dll",
            "D2Client.dll",
            "D2CMP.dll",
            "D2Common.dll",
            "D2DDraw.dll",
            "D2Direct3D.dll",
            "D2Game.dll",
            "D2Gdi.dll",
            "D2gfx.dll",
            "D2Glide.dll",
            "D2Lang.dll",
            "D2Launch.dll",
            "D2MCPClient.dll",
            "D2Multi.dll",
            "D2Net.dll",
            "D2sound.dll",
            "D2Win.dll",
            "Fog.dll",
            "Storm.dll",
            "D2VidTst.exe"
        };

        private readonly List<string> _requiredPost113Files = new List<string>()
        {
            "BlizzardError.exe",
            "SystemSurvey.exe"
        };
    }
}
