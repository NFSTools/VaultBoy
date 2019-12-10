using CoreLibraries.ModuleSystem;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using VaultBoy.ProfileCore;
using VaultLib.Core;
using VaultLib.Core.DB;
using VaultLib.Core.Hashing;
using VaultLib.Core.Loading;

namespace VaultBoy.Profiles.TheRun
{
    [Export(typeof(BaseProfile))]
    public class ProfileDef : BaseProfile
    {
        public ProfileDef()
        {
            HashManager.AddVLT("ai_skill");
            HashManager.AddVLT("ai_track_times");
            HashManager.AddVLT("car_bool");
            HashManager.AddVLT("car_common");
            HashManager.AddVLT("car_common_steering");
            HashManager.AddVLT("car_enum");
            HashManager.AddVLT("car_float");
            HashManager.AddVLT("car_info");
            HashManager.AddVLT("car_int");
            HashManager.AddVLT("car_ref");
            HashManager.AddVLT("car_text");
            HashManager.AddVLT("car_tuning");
            HashManager.AddVLT("demo");
            HashManager.AddVLT("dyno");
            HashManager.AddVLT("dyno_tuning");
            HashManager.AddVLT("forced_induction");
            HashManager.AddVLT("level_info");
            HashManager.AddVLT("physics_curves");
            HashManager.AddVLT("route_bool");
            HashManager.AddVLT("route_enum");
            HashManager.AddVLT("route_float");
            HashManager.AddVLT("route_info");
            HashManager.AddVLT("route_int");
            HashManager.AddVLT("route_ref");
            HashManager.AddVLT("route_text");
        }

        public override IDataModule GetDataModule()
        {
            return new ModuleDef();
        }

        public override string GetName()
        {
            return "The Run";
        }

        public override string GetGameID()
        {
            return "THE_RUN";
        }

        public override DatabaseType GetDatabaseType()
        {
            return DatabaseType.X86Database;
        }

        public override bool IsSaveSupported()
        {
            return false;
        }

        public override bool CanLoad(string directory)
        {
            return File.Exists(Path.Combine(directory, "Need for Speed The Run.exe")) &&
                   File.Exists(Path.Combine(directory, "c4indbcontent.res")) &&
                   File.Exists(Path.Combine(directory, "c4schema.res"));
        }

        public override IList<LoadedDatabaseFile> Load(Database database, string directory)
        {
            string[] paths = new[] { "c4schema.res", "c4indbcontent.res" };
            AttribSysPack pack = new AttribSysPack();
            IList<LoadedDatabaseFile> files = new List<LoadedDatabaseFile>();

            foreach (string path in paths)
            {
                string fullPath = Path.Combine(directory, path);

                using BinaryReader br = new BinaryReader(File.OpenRead(fullPath));
                IList<Vault> vaults = pack.Load(br, database, new PackLoadingOptions());

                Debug.WriteLine("loaded {0} vaults from {1}", vaults.Count, fullPath);

                DatabaseLoadedFile dlf = new DatabaseLoadedFile();
                dlf.Vaults.AddRange(vaults);

                database.Files.Add(dlf);

                LoadedDatabaseFile file = new LoadedDatabaseFile(fullPath, vaults, string.Empty);
                files.Add(file);
            }

            return files;
        }

        public override void Save(IList<LoadedDatabaseFile> files)
        {
            throw new System.NotImplementedException();
        }
    }
}