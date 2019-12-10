using System;
using System.ComponentModel;
using GalaSoft.MvvmLight.Ioc;
using VaultLib.Core.Data;

namespace VaultBoy.ViewModel.Tabs.ClassInfo
{
    /// <summary>
    /// Acts as a proxy for editing a class's static fields
    /// </summary>
    public class StaticDataProxy : ICustomTypeDescriptor
    {
        private readonly VLTClass _class;

        public StaticDataProxy(VLTClass vltClass)
        {
            _class = vltClass;
        }

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);

        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);

        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);

        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(this, null, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        public PropertyDescriptorCollection GetProperties()
        {
            PropertyDescriptorCollection pdc = new PropertyDescriptorCollection(new PropertyDescriptor[0]);

            foreach (var staticField in _class.StaticFields)
            {
                //EditorSchemaField schemaField = null;

                //if (_schema != null)
                //    schemaField = _schema.Fields.Find(f => f.FieldName == staticField.Name);

                pdc.Add(new StaticFieldProxy(_class, staticField));
            }

            return pdc;
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return GetProperties();
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }
    }
}