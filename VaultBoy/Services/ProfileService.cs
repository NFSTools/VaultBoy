using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using VaultBoy.ProfileCore;

namespace VaultBoy.Services
{
    public class ProfileService
    {
        [ImportMany] private IEnumerable<Lazy<BaseProfile>> _profiles;

        public void LoadProfiles()
        {
            var catalog = new DirectoryCatalog(AppDomain.CurrentDomain.BaseDirectory);
            CompositionContainer container = new CompositionContainer(catalog);
            container.ComposeParts(this);

            foreach (var profile in _profiles)
            {
                profile.Value.GetDataModule().Load();
            }
        }

        public IEnumerable<BaseProfile> GetProfiles()
        {
            return _profiles.Select(p => p.Value);
        }
    }
}