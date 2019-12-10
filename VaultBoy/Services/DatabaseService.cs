using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using GalaSoft.MvvmLight.Messaging;
using VaultBoy.Messages;
using VaultBoy.ProfileCore;
using VaultLib.Core;
using VaultLib.Core.DB;
using VaultLib.Core.Hashing;

namespace VaultBoy.Services
{
    public class DatabaseService
    {
        private readonly ProfileService _profileService;

        private Database _database;

        private IList<LoadedDatabaseFile> _files;

        private BaseProfile _profile;

        public DatabaseService(ProfileService profileService)
        {
            _profileService = profileService;

            HashManager.LoadDictionary(@"Resources\hashes.txt");
        }

        public (bool success, BaseProfile profile, IList<LoadedDatabaseFile> files) LoadDatabase(string directory)
        {
            SetDatabase(null);
            foreach (var profile in _profileService.GetProfiles())
            {
                if (profile.CanLoad(directory))
                {
                    Database database = new Database(new DatabaseOptions(profile.GetGameID(), profile.GetDatabaseType()));
                    IList<LoadedDatabaseFile> files = profile.Load(database, directory);
                    database.CompleteLoad();
                    TypeRegistry.ListUnknownTypes();
                    _files = files;
                    _profile = profile;
                    SetDatabase(database);
                    return (true, profile, files);
                }
            }

            return (false, null, ImmutableList<LoadedDatabaseFile>.Empty);
        }

        public void SaveDatabase(string directory)
        {
            foreach (var file in _files)
            {
                string bakPath = Path.ChangeExtension(file.FullPath, "vltbak");

                if (!File.Exists(bakPath))
                    File.Copy(file.FullPath, bakPath);
            }

            _profile.Save(_files);
        }

        private void SetDatabase(Database database)
        {
            if (_database != database)
            {
                _database = database;
                Messenger.Default.Send(new CurrentDatabaseChangedMessage(_database));
            }
        }
    }
}