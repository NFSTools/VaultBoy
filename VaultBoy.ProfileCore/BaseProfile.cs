using System.Collections.Generic;
using CoreLibraries.ModuleSystem;
using VaultLib.Core.DB;

namespace VaultBoy.ProfileCore
{
    public abstract class BaseProfile
    {
        public abstract IDataModule GetDataModule();

        public abstract string GetName();
        public abstract string GetGameID();
        public abstract DatabaseType GetDatabaseType();

        public abstract bool CanLoad(string directory);

        public abstract IList<LoadedDatabaseFile> Load(Database database, string directory);
        public abstract void Save(IList<LoadedDatabaseFile> files);

        public virtual bool IsLaunchSupported()
        {
            return false;
        }

        public virtual bool IsSaveSupported()
        {
            return true;
        }

        public virtual string GetLaunchExecutablePath()
        {
            return string.Empty;
        }
    }
}