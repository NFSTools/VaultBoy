using System.Reflection;
using CoreLibraries.GameUtilities;
using CoreLibraries.ModuleSystem;
using VaultLib.Core;
using VaultLib.Core.Exports;
using VaultLib.Core.Exports.Implementations;
using VaultLib.Core.Structures;
using VaultLib.ModernBase;
using VaultLib.ModernBase.Exports;
using VaultLib.ModernBase.Structures;

namespace VaultBoy.Profiles.TheRun
{
    public class ModuleDef : IDataModule
    {
        public void Load()
        {
            TypeRegistry.Register<StringKey>("Attrib::StringKey", "THE_RUN");
            TypeRegistry.RegisterAssemblyTypes(Assembly.GetAssembly(GetType()), "THE_RUN");

            ExportFactory.SetClassLoadCreator<ClassLoad>("THE_RUN");
            ExportFactory.SetCollectionLoadCreator<CollectionLoad>("THE_RUN");
            ExportFactory.SetDatabaseLoadCreator<DatabaseLoad>("THE_RUN");
            ExportFactory.SetExportEntryCreator<ExportEntry>("THE_RUN");
            ExportFactory.SetPointerCreator<AttribPtrRef>("THE_RUN");
        }
    }
}