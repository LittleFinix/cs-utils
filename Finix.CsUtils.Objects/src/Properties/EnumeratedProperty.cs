
using System.Reflection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Finix.CsUtils
{
    public class EnumeratedProperty<T> : IEnumeratedProperty<T>
    {
        public EnumeratedProperty(IProperties root, IProperty<T> baseProperty, string path, int depth)
        {
            Root = root;
            BaseProperty = baseProperty;
            Path = path;
            Depth = depth;
        }

        public IProperties Root { get; }

        public string Path { get; }

        public int Depth { get; }

        public IProperty<T> BaseProperty { get; }

        [MaybeNull, AllowNull]
        public T Value { get => BaseProperty.Value; set => BaseProperty.Value = value; }

        public bool IsReadOnly => BaseProperty.IsReadOnly;

        public Type ValueType => BaseProperty.ValueType;

        public string PropertyName => BaseProperty.PropertyName;

        public string DisplayName => BaseProperty.DisplayName;

        public string Description => BaseProperty.Description;

        public int Order => BaseProperty.Order;

        public bool IsComposedType => BaseProperty.IsComposedType;

        public AttributeCollection Attributes => BaseProperty.Attributes;

        object? IProperty.Value { get => ((IProperty) BaseProperty).Value; set => ((IProperty) BaseProperty).Value = value; }

        public event PropertyChangedEventHandler? PropertyChanged
        {
            add
            {
                BaseProperty.PropertyChanged += value;
            }

            remove
            {
                BaseProperty.PropertyChanged -= value;
            }
        }

        public object[] GetCustomAttributes(bool inherit)
        {
            return BaseProperty.GetCustomAttributes(inherit);
        }

        public object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return BaseProperty.GetCustomAttributes(attributeType, inherit);
        }

        public bool IsDefined(Type attributeType, bool inherit)
        {
            return BaseProperty.IsDefined(attributeType, inherit);
        }
    }
}
