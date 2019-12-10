using System;
using System.ComponentModel;

namespace VaultBoy.Utils
{
    /// <summary>Represents an <see langword="abstract" /> class that provides properties for objects that do not have properties.</summary>
    /// <remarks>Copied from .NET Framework code, since it's inaccessible by default.</remarks>
    public abstract class SimplePropertyDescriptor : PropertyDescriptor
    {
        private Type componentType;
        private Type propertyType;

        /// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.TypeConverter.SimplePropertyDescriptor" /> class.</summary>
        /// <param name="componentType">A <see cref="T:System.Type" /> that represents the type of component to which this property descriptor binds. </param>
        /// <param name="name">The name of the property. </param>
        /// <param name="propertyType">A <see cref="T:System.Type" /> that represents the data type for this property. </param>
        protected SimplePropertyDescriptor(Type componentType, string name, Type propertyType)
          : this(componentType, name, propertyType, new Attribute[0])
        {
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.TypeConverter.SimplePropertyDescriptor" /> class.</summary>
        /// <param name="componentType">A <see cref="T:System.Type" /> that represents the type of component to which this property descriptor binds. </param>
        /// <param name="name">The name of the property. </param>
        /// <param name="propertyType">A <see cref="T:System.Type" /> that represents the data type for this property. </param>
        /// <param name="attributes">An <see cref="T:System.Attribute" /> array with the attributes to associate with the property. </param>
        protected SimplePropertyDescriptor(
          Type componentType,
          string name,
          Type propertyType,
          Attribute[] attributes)
          : base(name, attributes)
        {
            this.componentType = componentType;
            this.propertyType = propertyType;
        }

        /// <summary>Gets the type of component to which this property description binds.</summary>
        /// <returns>A <see cref="T:System.Type" /> that represents the type of component to which this property binds.</returns>
        public override Type ComponentType
        {
            get
            {
                return this.componentType;
            }
        }

        /// <summary>Gets a value indicating whether this property is read-only.</summary>
        /// <returns>
        /// <see langword="true" /> if the property is read-only; <see langword="false" /> if the property is read/write.</returns>
        public override bool IsReadOnly
        {
            get
            {
                return this.Attributes.Contains((Attribute)ReadOnlyAttribute.Yes);
            }
        }

        /// <summary>Gets the type of the property.</summary>
        /// <returns>A <see cref="T:System.Type" /> that represents the type of the property.</returns>
        public override Type PropertyType
        {
            get
            {
                return this.propertyType;
            }
        }

        /// <summary>Returns whether resetting the component changes the value of the component.</summary>
        /// <param name="component">The component to test for reset capability. </param>
        /// <returns>
        /// <see langword="true" /> if resetting the component changes the value of the component; otherwise, <see langword="false" />.</returns>
        public override bool CanResetValue(object component)
        {
            DefaultValueAttribute attribute = (DefaultValueAttribute)this.Attributes[typeof(DefaultValueAttribute)];
            if (attribute == null)
                return false;
            return attribute.Value.Equals(this.GetValue(component));
        }

        /// <summary>Resets the value for this property of the component.</summary>
        /// <param name="component">The component with the property value to be reset. </param>
        public override void ResetValue(object component)
        {
            DefaultValueAttribute attribute = (DefaultValueAttribute)this.Attributes[typeof(DefaultValueAttribute)];
            if (attribute == null)
                return;
            this.SetValue(component, attribute.Value);
        }

        /// <summary>Returns whether the value of this property can persist.</summary>
        /// <param name="component">The component with the property that is to be examined for persistence. </param>
        /// <returns>
        /// <see langword="true" /> if the value of the property can persist; otherwise, <see langword="false" />.</returns>
        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }
    }
}
