using System.ComponentModel.Composition;
using System.IO;
using CoreLibraries.GameUtilities;
using CoreLibraries.ModuleSystem;
using VaultBoy.ProfileCore;
using VaultBoy.Profiles.SpeedBase;
using VaultLib.Core.DB;
using VaultLib.Support.MostWanted;

namespace VaultBoy.Profiles.MostWanted
{
    [Export(typeof(BaseProfile))]
    public class ProfileDef : SpeedProfile
    {
        public override IDataModule GetDataModule()
        {
            return new ModuleDef();
        }

        public override string GetName()
        {
            return "Most Wanted";
        }

        public override string GetGameID()
        {
            return GameIdHelper.ID_MW;
        }

        public override DatabaseType GetDatabaseType()
        {
            return DatabaseType.X86Database;
        }

        public override bool CanLoad(string directory)
        {
            return File.Exists(Path.Combine(directory, "speed.exe")) &&
                   File.Exists(Path.Combine(directory, "TRACKS", "L2RA.BUN"));
        }

        public override bool IsLaunchSupported()
        {
            return true;
        }

        public override bool IsSaveSupported()
        {
            return true;
        }

        public override string GetLaunchExecutablePath()
        {
            return "speed.exe";
        }
    }
}