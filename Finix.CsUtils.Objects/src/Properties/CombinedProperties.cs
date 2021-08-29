
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
    public sealed class CombinedProperties : IProperties
    {
        public IReadOnlyDictionary<string, IProperties> Mappings { get; }

        public object Object => throw new InvalidOperationException();

        public Type ObjectType => throw new InvalidOperationException();

        public CombinedProperties(IDictionary<string, IProperties> mappings)
        {
            Mappings = new Dictionary<string, IProperties>(mappings);
        }

        public event PropertyChangedEventHandler? PropertyChanged
        {
            add
            {
                foreach (var props in Mappings.Values)
                {
                    props.PropertyChanged += value;
                }
            }

            remove
            {
                foreach (var props in Mappings.Values)
                {
                    props.PropertyChanged -= value;
                }
            }
        }

        public IDictionary<string, object?> GetAll(bool recurse = false)
        {
            var properties = new Dictionary<string, object?>();

            foreach (var (map, props) in Mappings)
            {
                foreach (var (prop, value) in props.GetAll(recurse))
                    properties[$"{map}.{prop}"] = value;
            }

            return properties;
        }

        public void SetAll(IDictionary<string, object?> values)
        {
            foreach (var (key, value) in values)
            {
                var split = key.Split('.', 2);
                var map = split[0];
                var prop = split[1];

                Mappings[map].GetProperty(prop).Value = value;
            }
        }

        public IEnumerable<IProperty> GetProperties()
        {
            foreach (var props in Mappings.Values)
            {
                foreach (var prop in props.GetProperties())
                    yield return prop;
            }
        }

        public IEnumerable<IEnumeratedProperty> EnumerateProperties(bool recurse, int depth, string path)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IEnumeratedProperty> EnumerateProperties(bool recurse)
        {
            throw new NotImplementedException();
        }

        private (string, string) SplitPropName(string name)
        {
            var split = name.Split('.', 2);
            return (split[0], split[1]);
        }

        public bool TryGetProperty(string name, [MaybeNullWhen(false)] out IProperty prop)
        {
            var (map, propName) = SplitPropName(name);
            return Mappings[map].TryGetProperty(propName, out prop);
        }

        public bool TryGetProperty<T>(string name, [MaybeNullWhen(false)] out IProperty<T> prop)
        {
            var (map, propName) = SplitPropName(name);
            return Mappings[map].TryGetProperty(propName, out prop);
        }

        public IProperty GetProperty(string name)
        {
            var (map, prop) = SplitPropName(name);
            return Mappings[map].GetProperty(prop);
        }

        public IProperty<T> GetProperty<T>(string name)
        {
            var (map, prop) = SplitPropName(name);
            return Mappings[map].GetProperty<T>(prop);
        }
    }
}
