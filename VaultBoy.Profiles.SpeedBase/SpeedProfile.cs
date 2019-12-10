using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using VaultBoy.ProfileCore;
using VaultLib.Core;
using VaultLib.Core.DB;
using VaultLib.Core.Loading;

namespace VaultBoy.Profiles.SpeedBase
{
    public abstract class SpeedProfile : BaseProfile
    {
        public override IList<LoadedDatabaseFile> Load(Database database, string directory)
        {
            IEnumerable<string> paths = GetFilesToLoad();
            StandardVaultPack svp = new StandardVaultPack();
            List<LoadedDatabaseFile> files = new List<LoadedDatabaseFile>();

            foreach (var path in paths)
            {
                string[] parts = path.Split(':');
                string fullPath = Path.Combine(directory, "GLOBAL", parts[0]);

                using BinaryReader br = new BinaryReader(File.OpenRead(fullPath));
                IList<Vault> vaults = svp.Load(br, database, new PackLoadingOptions());

                Debug.WriteLine("loaded {0} vaults from {1}", vaults.Count, fullPath);

                DatabaseLoadedFile dlf = new DatabaseLoadedFile();
                dlf.Vaults.AddRange(vaults);

                database.Files.Add(dlf);

                LoadedDatabaseFile file = new LoadedDatabaseFile(fullPath, vaults, parts[1]);
                files.Add(file);
            }

            return files;
        }

        public override void Save(IList<LoadedDatabaseFile> files)
        {
            StandardVaultPack svp = new StandardVaultPack();
            foreach (var file in files)
            {
                using MemoryStream ms = new MemoryStream();
                using BinaryWriter bw = new BinaryWriter(ms);

                svp.Save(bw, file.Vaults);

                using FileStream fs = new FileStream(file.FullPath, FileMode.Create, FileAccess.Write, FileShare.None);
                ms.Position = 0;
                ms.CopyTo(fs);

                if (file.Tag == "compress")
                {
                    // generate compressed file
                    var lzcPath = Path.ChangeExtension(file.FullPath, "lzc");
                    if (!File.Exists(lzcPath + ".bak"))
                        File.Copy(lzcPath, lzcPath + ".bak");
                    using FileStream cfs = new FileStream(lzcPath, FileMode.Create, FileAccess.Write, FileShare.None);
                    ms.Position = 0;

                    byte[] header = new byte[16];
                    header[0] = 0x52;
                    header[1] = 0x41;
                    header[2] = 0x57;
                    header[3] = 0x57;
                    header[4] = 0x01;
                    header[5] = 0x10;

                    int len = (int)ms.Length;
                    header[8] = (byte)(len & 0xff);
                    header[9] = (byte)((len >> 8) & 0xff);
                    header[10] = (byte)((len >> 16) & 0xff);
                    header[11] = (byte)((len >> 24) & 0xff);

                    len += 16;
                    header[12] = (byte)(len & 0xff);
                    header[13] = (byte)((len >> 8) & 0xff);
                    header[14] = (byte)((len >> 16) & 0xff);
                    header[15] = (byte)((len >> 24) & 0xff);

                    cfs.Write(header);

                    ms.CopyTo(cfs);
                }
            }
        }

        protected virtual IEnumerable<string> GetFilesToLoad()
        {
            return new[] {"attributes.bin:none", "fe_attrib.bin:none", "gameplay.bin:compress"};
        }
    }
}