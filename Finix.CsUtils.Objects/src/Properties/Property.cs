
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using Finix.CsUtils;
using System.Diagnostics.CodeAnalysis;

namespace Finix.CsUtils
{
    public class Property<T> : IProperty<T>
    {
        private readonly Func<object?, object?> getProp;
        private readonly Action<object?, object?>? setProp;

        public Property(IProperties owner, MemberInfo member)
        {
            Owner = owner;
            ReflectedProperty = member;

            Owner.PropertyChanged += OnOwnerPropertyChanged;

            if (member is PropertyInfo prop)
            {
                getProp = prop.CanRead ? prop.GetValue : throw new ArgumentException("Expected member to be a property or field", nameof(member));
                setProp = prop.CanWrite ? prop.SetValue : null;
                ValueType = prop.PropertyType;
            }
            else if (member is FieldInfo field)
            {
                getProp = field.GetValue;
                setProp = field.SetValue;
                ValueType = field.FieldType;
            }
            else
            {
                throw new ArgumentException(nameof(member));
            }
        }

        ~Property()
        {
            Owner.PropertyChanged -= OnOwnerPropertyChanged;
        }

        public IProperties Owner { get; }

        public MemberInfo ReflectedProperty { get; }

        public bool IsReadOnly => setProp != null;

        [MaybeNull, AllowNull]
        public T Value
        {
            get => (T?) getProp(Owner.Object);
            set
            {
                if (setProp != null)
                    setProp(Owner.Object, value);
                else
                    throw new InvalidOperationException("Property is ReadOnly");
            }
        }

        object? IProperty.Value
        {
            get => Value;
            set => Value = (T?) value;
        }

        public Type ValueType { get; }

        public DisplayAttribute? Display => ReflectedProperty.GetCustomAttribute<DisplayAttribute>();

        public string PropertyName => ReflectedProperty.Name;

        public string DisplayName => TypeLoader.GetDisplayName(this);

        public string Description => Display?.GetDescription() ?? DisplayName;

        public int Order => Display?.GetOrder() ?? Int32.MaxValue;

        public bool IsComposedType => ValueType.IsClass || (ValueType.IsValueType && ValueType.GetFields().Length > 0);

        private AttributeCollection? attributes;

        public AttributeCollection Attributes => attributes ??= new AttributeCollection(ReflectedProperty.GetCustomAttributes(true).Cast<Attribute>().ToArray());

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnOwnerPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != PropertyName)
                return;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
        }

        public object[] GetCustomAttributes(bool inherit)
        {
            return ReflectedProperty.GetCustomAttributes(inherit);
        }

        public object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return ReflectedProperty.GetCustomAttributes(attributeType, inherit);
        }

        public bool IsDefined(Type attributeType, bool inherit)
        {
            return ReflectedProperty.IsDefined(attributeType, inherit);
        }
    }
}
