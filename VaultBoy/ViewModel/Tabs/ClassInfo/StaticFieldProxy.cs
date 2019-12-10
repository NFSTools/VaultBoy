using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using GalaSoft.MvvmLight.Messaging;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultBoy.ViewModel.Tabs.ClassInfo
{
    public class StaticFieldProxy : PropertyDescriptor
    {
        private readonly VLTClass _vltClass;
        private readonly VLTClassField _vltClassField;
        //private readonly EditorSchemaField _schemaField;

        public StaticFieldProxy(VLTClass vltClass, VLTClassField vltClassField/*, EditorSchemaField schemaField*/) : base(vltClassField.Name, null)
        {
            _vltClass = vltClass;
            _vltClassField = vltClassField;
            //_schemaField = schemaField;
        }
        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override AttributeCollection Attributes
        {
            get
            {
                List<Attribute> attributes = new List<Attribute>();

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

                return new AttributeCollection(attributes.ToArray());
            }
        }

        public override object GetValue(object component)
        {
            return GetEffectiveValue();
        }

        public override void ResetValue(object component)
        {
            throw new NotImplementedException();
        }

        public override void SetValue(object component, object value)
        {
            switch (_vltClassField.StaticValue)
            {
                case PrimitiveTypeBase ptb:
                    ptb.SetValue((IConvertible)value);
                    break;
                case IStringValue sv:
                    sv.SetString((string)value);
                    break;
                case VLTArrayType at:
                    at.Items = (VLTBaseType[])value;
                    break;
                default:
                    _vltClassField.StaticValue = (VLTBaseType)value;
                    break;
            }
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

        private object GetEffectiveValue()
        {
            switch (_vltClassField.StaticValue)
            {
                case PrimitiveTypeBase ptb:
                    return ptb.GetValue();
                case IStringValue sv:
                    return sv.GetString();
                case VLTArrayType at:
                    return at.Items;
            }

            return _vltClassField.StaticValue;
        }

        public override Type ComponentType => GetType();
        public override bool IsReadOnly => false;
        public override Type PropertyType => GetEffectiveValue().GetType();
        public override string Description => /*_schemaField?.ShortDescription ?? */$"Field {_vltClassField.Name} | type {_vltClassField.TypeName}";
        public override string DisplayName => /*_schemaField?.FieldDisplayName ?? */_vltClassField.Name;
    }
}