using System.Collections.Generic;
using System.IO;
using VaultLib.Core;

namespace VaultBoy.ProfileCore
{
    public class LoadedDatabaseFile
    {
        public string FullPath { get; }
        public string ShortPath => Path.GetFileName(FullPath);
        public IList<Vault> Vaults { get; }
        public string Tag { get; }

        public LoadedDatabaseFile(string fullPath, IList<Vault> vaults, string tag)
        {
            FullPath = fullPath;
            Vaults = vaults;
            Tag = tag;
        }
    }
}