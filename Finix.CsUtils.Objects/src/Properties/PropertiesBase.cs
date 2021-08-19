
using System.Runtime.Serialization;
using System.Linq;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Finix.CsUtils
{
    public abstract class PropertiesBase : IProperties
    {
        protected IProperty[]? properties;

#pragma warning disable CS0067
        public event PropertyChangedEventHandler? PropertyChanged;
#pragma warning restore CS0067

        public virtual object Object => this;

        public virtual Type ObjectType => Object.GetType();

        public virtual void SetAll(IDictionary<string, object?> values)
        {
            foreach (var prop in GetProperties())
            {
                if (values.TryGetValue(prop.PropertyName, out var value))
                    prop.Value = value;
            }
        }

        public virtual IDictionary<string, object?> GetAll()
        {
            var dict = new Dictionary<string, object?>();

            foreach (var prop in EnumerateProperties(recurse: true))
            {
                dict[prop.PropertyName] = prop.Value;
            }

            return dict;
        }

        public virtual bool TryGetProperty(string name, [MaybeNullWhen(false)] out IProperty prop)
        {
            prop = GetProperties().FirstOrDefault(prop => prop.PropertyName == name);
            return prop != null;
        }

        public bool TryGetProperty<T>(string name, [MaybeNullWhen(false)] out IProperty<T> prop)
        {
            prop = null;

            if (TryGetProperty(name, out var p))
                prop = p as IProperty<T>;

            return prop != null;
        }

        public IProperty GetProperty(string name)
        {
            if (TryGetProperty(name, out var prop))
                return prop;

            throw new KeyNotFoundException($"No property with name '{name}' found.");
        }

        public IProperty<T> GetProperty<T>(string name)
        {
            if (TryGetProperty<T>(name, out var prop))
                return prop;

            throw new KeyNotFoundException($"No property with name '{name}' found.");
        }

        public virtual IEnumerable<IProperty> GetProperties()
        {
            return properties ??= GetReflectedProperties(ObjectType);
        }

        protected virtual IProperty CreateProperty(MemberInfo member)
        {
            Type propType;

            if (member is PropertyInfo prop)
                propType = prop.PropertyType;
            else if (member is FieldInfo field)
                propType = field.FieldType;
            else
                throw new ArgumentException("Expected member to be a property or field", nameof(member));

            var type = typeof(Property<>).MakeGenericType(propType);
            return (IProperty) Activator.CreateInstance(type, this, member)!;
        }

        protected virtual IEnumeratedProperty CreateEnumeratedProperty(IProperty prop, int depth, string path)
        {
            path = String.Join('.', path, prop.PropertyName);

            var type = typeof(EnumeratedProperty<>).MakeGenericType(prop.ValueType);

            return (IEnumeratedProperty) Activator.CreateInstance(type, this, prop, depth, prop)!;

        }

        protected IProperty[] GetReflectedProperties(Type type)
        {
            return type.GetRuntimeProperties()
                .Where(prop => // prop.SetMethod != null && prop.SetMethod.IsPublic &&
                    prop.GetMethod != null && prop.GetMethod.IsPublic
                    && prop.GetMethod.GetParameters().Count() == 0
                    && prop.GetCustomAttribute<IgnoreDataMemberAttribute>() == null
                    && prop.GetCustomAttribute<NonSerializedAttribute>() == null)
                .Select(CreateProperty)
                .OrderBy(prop => prop.Order)
                .ToArray();
        }

        protected virtual IEnumerable<IEnumeratedProperty> EnumeratePropertiesInternal(bool recurse, int depth, string path)
        {
            foreach (var prop in GetProperties())
            {
                yield return CreateEnumeratedProperty(prop, depth, path);

                if (recurse && prop.IsComposedType && prop.Value != null)
                {
                    var obj = new ObjectProperties(prop.Value);
                    var nextPath = String.Join('.', path, prop.PropertyName);

                    foreach (var item in obj.EnumeratePropertiesInternal(recurse: true, depth + 1, nextPath))
                        yield return item;
                }
            }
        }

        public virtual IEnumerable<IEnumeratedProperty> EnumerateProperties(bool recurse)
        {
            return EnumeratePropertiesInternal(recurse, 0, String.Empty);
        }
    }
}
