// This file is part of VaultEditor by heyitsleo.
// 
// Created: 10/18/2019 @ 4:51 PM.

using System;
using GalaSoft.MvvmLight.Messaging;
using VaultBoy.Utils;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultBoy.Editing
{
    public class ArrayItemProperty : SimplePropertyDescriptor
    {
        private readonly VLTCollection _collection;
        private readonly VLTClassField _field;
        private readonly VLTArrayType _array;
        private readonly VLTBaseType _item;

        public ArrayItemProperty(VLTCollection collection, VLTClassField field, VLTArrayType array, VLTBaseType item) :
            base(array.GetType(), $"[{Array.IndexOf(array.Items, item)}]", array.ItemType)
        {
            _collection = collection;
            _field = field;
            _array = array;
            _item = item;
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override object GetValue(object component)
        {
            return _item;
        }

        public override void ResetValue(object component)
        {
            throw new NotImplementedException();
        }

        public override void SetValue(object component, object value)
        {
            //if (value != _array.Items[Array.IndexOf(_array.Items, _item)])
            //{
            //    Messenger.Default.Send(new SetChangeStatusMessage(true));
            //}

            //_array.Items[_item.ArrayIndex] = _item;
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

        public override Type ComponentType => GetType();
        public override bool IsReadOnly => false;
        public override Type PropertyType => _item.GetType();
        public override string Category => "Data";

        
    }
}