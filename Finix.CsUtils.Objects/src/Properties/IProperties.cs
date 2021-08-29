
using System.Reflection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Finix.CsUtils
{
    public interface IProperties : INotifyPropertyChanged
    {
        public object Object { get; }

        public Type ObjectType { get; }

        void SetAll(IDictionary<string, object?> values);

        IDictionary<string, object?> GetAll(bool recurse = false);

        IEnumerable<IProperty> GetProperties();

        bool TryGetProperty(string name, [MaybeNullWhen(false)] out IProperty prop);

        bool TryGetProperty<T>(string name, [MaybeNullWhen(false)] out IProperty<T> prop);

        IProperty GetProperty(string name);

        IProperty<T> GetProperty<T>(string name);

        IEnumerable<IEnumeratedProperty> EnumerateProperties(bool recurse, int depth, string path);

        IEnumerable<IEnumeratedProperty> EnumerateProperties(bool recurse);
    }
}
