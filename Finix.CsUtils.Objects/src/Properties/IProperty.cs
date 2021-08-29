
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

        bool IsCommonCLRType { get; }

        bool IsEnumerableType { get; }

        bool IsStringType { get; }

        bool IsPrimitiveType { get; }

        bool IsArrayType { get; }

        bool IsDictionaryType { get; }

        bool IsCollectionType { get; }

        bool IsListType { get; }

        AttributeCollection Attributes { get; }
    }

    public interface IProperty<T> : IProperty
    {
        [MaybeNull, AllowNull]
        new T Value { get; set; }
    }
}
