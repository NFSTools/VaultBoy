using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.DB;
using VaultLib.Core.Loading;

namespace VaultBoy.Profiles.BurnoutParadise
{
    public class BurnoutPack : IVaultPack
    {
        public IList<Vault> Load(BinaryReader br, Database database, PackLoadingOptions loadingOptions = null)
        {
            throw new System.NotImplementedException();
        }

        public void Save(BinaryWriter bw, IList<Vault> vaults)
        {
            throw new System.NotImplementedException();
        }
    }
}