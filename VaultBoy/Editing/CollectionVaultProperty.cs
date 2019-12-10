// This file is part of VaultEditor by heyitsleo.
// 
// Created: 10/17/2019 @ 3:04 PM.

using System;
using System.ComponentModel;
using VaultLib.Core.Data;

namespace VaultBoy.Editing
{
    public class CollectionVaultProperty : PropertyDescriptor
    {
        private readonly VLTCollection _collection;

        public CollectionVaultProperty(VLTCollection collection) : base("Vault", new Attribute[] { new ReadOnlyAttribute(true)  })
        {
            _collection = collection;
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override object GetValue(object component)
        {
            return _collection.Vault.Name;
        }

        public override void ResetValue(object component)
        {
            throw new NotImplementedException();
        }

        public override void SetValue(object component, object value)
        {
            throw new NotImplementedException();
        }

        public override bool ShouldSerializeValue(object component)
        {
            throw new NotImplementedException();
        }

        public override Type ComponentType => GetType();
        public override bool IsReadOnly => true;
        public override Type PropertyType => typeof(string);
        public override string Category => "[Info]";
    }
}