
using System.Collections;
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
                throw new ArgumentException("Unsupported member type", nameof(member));
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

        public string DisplayName => TypeLoader.GetDisplayName(ValueType);

        public string Description => Display?.GetDescription() ?? DisplayName;

        public int Order => Display?.GetOrder() ?? Int32.MaxValue;

        public bool IsComposedType => Property.IsComposedType(ValueType);

        public bool IsCommonCLRType => Property.IsCommonCLRType(ValueType);

        public bool IsEnumerableType => Property.IsEnumerableType(ValueType);

        public bool IsStringType => Property.IsStringType(ValueType);

        public bool IsPrimitiveType => Property.IsPrimitiveType(ValueType);

        public bool IsArrayType => Property.IsArrayType(ValueType);

        public bool IsDictionaryType => Property.IsDictionaryType(ValueType);

        public bool IsCollectionType => Property.IsCollectionType(ValueType);

        public bool IsListType => Property.IsListType(ValueType);

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

    public static class Property
    {
        public static bool IsComposedType(Type type) =>
            (type.IsClass || (type.IsValueType && type.GetFields().Length > 0))
            && !(IsCommonCLRType(type) || IsArrayType(type) || IsDictionaryType(type) || IsCollectionType(type) || IsListType(type));

        public static bool IsCommonCLRType(Type type) => IsStringType(type) || IsPrimitiveType(type);

        public static bool IsEnumerableType(Type type) => IsArrayType(type) || IsDictionaryType(type) || IsCollectionType(type) || IsListType(type) || type.IsAssignableTo(typeof(IEnumerable));

        public static bool IsStringType(Type type) => type == typeof(string);

        public static bool IsPrimitiveType(Type type) => type.IsPrimitive;

        public static bool IsArrayType(Type type) => type.IsArray;

        public static bool IsDictionaryType(Type type) => type.IsGenericType
            && (
                type.GetGenericTypeDefinition().IsAssignableTo(typeof(IReadOnlyDictionary<,>))
                || type.GetGenericTypeDefinition().IsAssignableTo(typeof(IDictionary<,>))
            ) || type.IsAssignableTo(typeof(IDictionary));

        public static bool IsCollectionType(Type type) => type.IsGenericType
            && (
                type.GetGenericTypeDefinition().IsAssignableTo(typeof(IReadOnlyCollection<>))
                || type.GetGenericTypeDefinition().IsAssignableTo(typeof(ICollection<>))
            ) || type.IsAssignableTo(typeof(ICollection));

        public static bool IsListType(Type type) => type.IsGenericType
            && (
                type.GetGenericTypeDefinition().IsAssignableTo(typeof(IReadOnlyList<>))
                || type.GetGenericTypeDefinition().IsAssignableTo(typeof(IList<>))
            ) || type.IsAssignableTo(typeof(IList));
    }
}
