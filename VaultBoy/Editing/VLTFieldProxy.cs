// This file is part of Editor by heyitsleo.
// 
// Created: 10/11/2019 @ 4:29 PM.

using System;
using System.ComponentModel;
using System.Diagnostics;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultBoy.Editing
{
    /// <summary>
    /// Proxies edits for an individual field of a collection
    /// </summary>
    public class VLTFieldProxy : PropertyDescriptor
    {
        //private readonly EditorSchemaField _schemaField;
        private readonly VLTCollection _collection;
        private readonly VLTClassField _classField;

        private VLTBaseType _value;

        public VLTFieldProxy(/*EditorSchemaField schemaField, */VLTCollection collection, VLTClassField classField) : base(
            classField.Name, null)
        {
            //_schemaField = schemaField;
            _collection = collection;
            _classField = classField;

            _value = _collection.DataRow[classField.Key];

            object effectiveValue = GetEffectiveValue();
            Debug.Assert(effectiveValue!=null);

            if (_classField.IsArray)
                PropertyType = ((VLTArrayType) _value).ItemType.MakeArrayType();
            else
                PropertyType = effectiveValue.GetType();
            //PropertyType = effectiveValue.GetType();
        }

        public override AttributeCollection Attributes
        {
            get
            {
                return new AttributeCollection();
                //List<Attribute> attributes = new List<Attribute>();

                //if (_schemaField != null)
                //{
                //    if (!string.IsNullOrEmpty(_schemaField.EditorId))
                //    {
                //        Type editorType = EditorRegistry.FindEditor(_schemaField.EditorId);
                //        if (editorType == null)
                //            throw new Exception("could not find editor " + _schemaField.EditorId);
                //        attributes.Add(new EditorAttribute(editorType, typeof(UITypeEditor)));
                //    }
                //}

                //return new AttributeCollection(attributes.ToArray());
            }
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override object GetValue(object component)
        {
            return GetEffectiveValue();
        }

        private object GetEffectiveValue()
        {
            switch (_value)
            {
                case PrimitiveTypeBase ptb:
                    return ptb.GetValue();
                case IStringValue sv:
                    return sv.GetString();
                case VLTArrayType at:
                    return at.Items;
            }

            return _value;
        }

        public override void ResetValue(object component)
        {
            throw new InvalidOperationException("Cannot reset value. This error should never appear.");
        }

        public override void SetValue(object component, object value)
        {
            //if (value != GetEffectiveValue())
            //{
            //    Messenger.Default.Send(new SetChangeStatusMessage(true));
            //}

            switch (_value)
            {
                case PrimitiveTypeBase ptb:
                    ptb.SetValue((IConvertible) value);
                    break;
                case IStringValue sv:
                    sv.SetString((string) value);
                    break;
                case VLTArrayType at:
                    at.Items = (VLTBaseType[]) value;
                    break;
                default:
                    _value = (VLTBaseType) value;
                    break;
            }

            _collection.DataRow[_classField.Key] = _value;
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

        public override Type ComponentType => GetType();
        public override bool IsReadOnly => false;

        public override Type PropertyType { get; }

        public override string Category => _classField.IsOptional ? "Optional" : "Base";
        public override string Description => /*_schemaField?.ShortDescription ??*/ $"Field {_classField.Name} | type {_classField.TypeName}";
        public override string DisplayName =>/* _schemaField?.FieldDisplayName ?? */_classField.Name;

        // The following Utility methods create a new AttributeCollection
        // by appending the specified attributes to an existing collection.
        static public AttributeCollection AppendAttributeCollection(
            AttributeCollection existing,
            params Attribute[] newAttrs)
        {
            return new AttributeCollection(AppendAttributes(existing, newAttrs));
        }


        static public Attribute[] AppendAttributes(
            AttributeCollection existing,
            params Attribute[] newAttrs)
        {
            if (existing == null)
            {
                throw new ArgumentNullException(nameof(existing));
            }

            newAttrs = newAttrs ?? new Attribute[0];

            Attribute[] attributes;

            Attribute[] newArray = new Attribute[existing.Count + newAttrs.Length];
            int actualCount = existing.Count;
            existing.CopyTo(newArray, 0);

            for (int idx = 0; idx < newAttrs.Length; idx++)
            {
                if (newAttrs[idx] == null)
                {
                    throw new ArgumentNullException("newAttrs");
                }

                // Check if this attribute is already in the existing
                // array.  If it is, replace it.
                bool match = false;
                for (int existingIdx = 0; existingIdx < existing.Count; existingIdx++)
                {
                    if (newArray[existingIdx].TypeId.Equals(newAttrs[idx].TypeId))
                    {
                        match = true;
                        newArray[existingIdx] = newAttrs[idx];
                        break;
                    }
                }

                if (!match)
                {
                    newArray[actualCount++] = newAttrs[idx];
                }
            }

            // If some attributes were collapsed, create a new array.
            if (actualCount < newArray.Length)
            {
                attributes = new Attribute[actualCount];
                Array.Copy(newArray, 0, attributes, 0, actualCount);
            }
            else
            {
                attributes = newArray;
            }

            return attributes;
        }
    }
}