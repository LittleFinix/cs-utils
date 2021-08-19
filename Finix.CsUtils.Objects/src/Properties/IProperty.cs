
using System.Reflection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Finix.CsUtils
{
    public interface IProperty : INotifyPropertyChanged, ICustomAttributeProvider
    {
        bool IsReadOnly { get; }

        object? Value { get; set; }

        Type ValueType { get; }

        string PropertyName { get; }

        string DisplayName { get; }

        string Description { get; }

        int Order { get; }

        bool IsComposedType { get; }

        AttributeCollection Attributes { get; }
    }

    public interface IProperty<T> : IProperty
    {
        [MaybeNull, AllowNull]
        new T Value { get; set; }
    }
}
