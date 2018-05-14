using Cactus.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cactus
{
    /// <summary>
    /// This class is responsible for returning a list of all of the files corresponding
    /// to a specific patch.
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

            // Add all common files
            requiredFiles.AddRange(_thirdPartyLibraries);


            // Add any remaining files that are version specific

            // 1.00 doesn't have BNUpdate.exe
            if (!_versionManager.Is100(version))
            {
                requiredFiles.AddRange(updateFile);
            }
        }

        private readonly List<string> _thirdPartyLibraries = new List<string>()
        {
            "binkw32.dll",
            "ijl11.dll",
            "SmackW32.dll"
        };

        // Keeping separate from commonFiles since 1.07 doesn't have it.
        private const string patchMpqFile = "Patch_D2.mpq";

        private readonly List<string> _commonFiles = new List<string>()
        { 
            "Diablo II.exe",
            "Game.exe"
        };

        private readonly List<string> _updateFile = new List<string>()
        {
            "BNUpdate.exe"
        };

        private readonly List<string> _required107BetaFiles = new List<string>()
        {
            "D2OpenGL.dll",
            "D2Server.dll",
            "Keyhook.dll"
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

        private readonly List<string> expansionMPQs = new List<string>()
        {
            "d2exp.mpq",
            "d2xmusic.mpq",
            "d2xvideo.mpq",
            "d2xtalk.mpq"
        };
    }
}
