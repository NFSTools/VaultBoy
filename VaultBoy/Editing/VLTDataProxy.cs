// This file is part of Editor by heyitsleo.
// 
// Created: 10/03/2019 @ 9:25 PM.

using System;
using System.ComponentModel;
using VaultLib.Core.Data;

namespace VaultBoy.Editing
{
    /// <summary>
    /// Acts as a proxy for editing collection data
    /// </summary>
    public class VLTDataProxy : ICustomTypeDescriptor
    {
        //private readonly ISchemaService _schemaService;

        private VLTCollection _collection;
        //private EditorSchema _schema;

        //public VLTDataProxy(ISchemaService schemaService)
        //{
        //    _schemaService = schemaService;
        //}

        #region Type Descriptor stuff

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
            return TypeDescriptor.GetEvents(this, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        public PropertyDescriptorCollection GetProperties()
        {
            PropertyDescriptorCollection pdc = new PropertyDescriptorCollection(new PropertyDescriptor[0]);

            pdc.Add(new CollectionVaultProperty(_collection));

            foreach (var dataPair in _collection.DataRow)
            {
                VLTClassField classField = _collection.Class.Fields[dataPair.Key];
                //EditorSchemaField schemaField = null;

                //if (_schema != null)
                //    schemaField = _schema.Fields.Find(f => f.FieldName == classField.Name);

                var fieldProxy = new VLTFieldProxy(/*schemaField,*/ _collection, classField);
                pdc.Add(fieldProxy);
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

        #endregion

        /// <summary>
        /// Sets the collection that is being proxied
        /// </summary>
        /// <param name="collection">The collection that is being proxied</param>
        public void SetCollection(VLTCollection collection)
        {
            _collection = collection;
            //_schema = _schemaService.FindSchema(collection.ClassName);
        }
    }
}