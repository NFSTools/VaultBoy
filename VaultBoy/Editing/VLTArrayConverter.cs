// This file is part of VaultEditor by heyitsleo.
// 
// Created: 10/18/2019 @ 4:59 PM.

using System;
using System.ComponentModel;
using VaultLib.Core.Types;

namespace VaultBoy.Editing
{
    public class VLTArrayConverter : ExpandableObjectConverter
    {
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            //if (value is VLTArrayType array)
            //{
            //    PropertyDescriptorCollection pdc = new PropertyDescriptorCollection(new PropertyDescriptor[0]);
            //    pdc.Add(new ArrayLengthProperty(array));

            //    foreach (var item in array.Items)
            //    {
            //        pdc.Add(new ArrayItemProperty(array.Collection, array.Field, array, item));
            //    }

            //    return pdc;
            //}

            return base.GetProperties(context, value, attributes);
        }
    }
}