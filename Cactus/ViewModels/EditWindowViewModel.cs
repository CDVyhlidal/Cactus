using Cactus.Interfaces;
using Cactus.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;

namespace Cactus.ViewModels
{
    public class EditWindowViewModel : ViewModelBase, IEditWindowViewModel
    {
        private IEntryManager _entryManager;
        private IVersionManager _versionManager;

        public EntryModel CurrentEntry { get; set; }

        public RelayCommand OkCommand { get; private set; }

        public EditWindowViewModel(IEntryManager entryManager, IVersionManager versionManager)
        {
            _entryManager = entryManager;
            _versionManager = versionManager;

            OkCommand = new RelayCommand(Ok);
        }

        private void Ok()
        {
            // TODO: Also rename folder on computer
            _entryManager.SaveEntries();
        }

        public List<VersionModel> Versions
        {
            get
            {
                return _versionManager.ClassicVersions;
            }
        }

        // TODO: Can probably be simplified/optimized.
        public VersionModel SelectVersion
        {
            get
            {
                var versions = _versionManager.ClassicVersions;

                foreach (var version in versions)
                {
                    if (version.Version == CurrentEntry.Version)
                    {
                        return version;
                    }
                }
                return versions[0];
            }
            set
            {
                CurrentEntry.Version = value.Version;
            }
        }
    }
}
