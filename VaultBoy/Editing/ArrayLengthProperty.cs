// This file is part of VaultEditor by heyitsleo.
// 
// Created: 10/18/2019 @ 4:48 PM.

using System;
using System.ComponentModel;
using VaultLib.Core.Types;

namespace VaultBoy.Editing
{
    public class ArrayLengthProperty : PropertyDescriptor
    {
        private readonly VLTArrayType _array;
        public ArrayLengthProperty(VLTArrayType array) : base("Length", null)
        {
            _array = array;
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override object GetValue(object component)
        {
            return _array.Items.Length;
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
            return false;
        }

        public override Type ComponentType => GetType();
        public override bool IsReadOnly => true;
        public override Type PropertyType => typeof(int);

        public override string Category => "[Info]";
    }
}