
using System.Runtime.Serialization;
using System.Linq;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Collections;
using System.Diagnostics;

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
            foreach (var (name, value) in values)
            {
                GetProperty(name).Value = value;
            }
        }

        public virtual IDictionary<string, object?> GetAll(bool recurse = false)
        {
            var dict = new Dictionary<string, object?>();

            foreach (var prop in EnumerateProperties(recurse))
            {
                dict[prop.Path.TrimStart('.')] = prop.Value;
            }

            return dict;
        }

        public virtual bool TryGetProperty(string name, [MaybeNullWhen(false)] out IProperty prop)
        {
            if (!name.StartsWith('.'))
                name = '.' + name;

            prop = EnumerateProperties(recurse: true).FirstOrDefault(prop => prop.Path == name);
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
            return EnumeratedProperty.CreateFrom(this, prop, depth, path);

        }

        protected IProperty[] GetReflectedProperties(Type type)
        {
            var props = type.GetProperties()
                .Where(prop => // prop.SetMethod != null && prop.SetMethod.IsPublic &&
                    prop.GetMethod != null && prop.GetMethod.IsPublic
                    && prop.GetMethod.GetParameters().Count() == 0
                    && prop.GetCustomAttribute<IgnoreDataMemberAttribute>() == null
                    && prop.GetCustomAttribute<NonSerializedAttribute>() == null);

            var fields = type.GetFields()
                .Where(field =>
                    field.GetCustomAttribute<IgnoreDataMemberAttribute>() == null
                    && field.GetCustomAttribute<NonSerializedAttribute>() == null);

            return
                props.Concat<MemberInfo>(fields)
                .Select(CreateProperty)
                .OrderBy(prop => prop.PropertyName)
                .OrderBy(prop => prop.Order)
                .ToArray();
        }

        public virtual IEnumerable<IEnumeratedProperty> EnumerateProperties(bool recurse, int depth, string path)
        {
            foreach (var prop in GetProperties())
            {
#if DEBUG
                // Debug.WriteLine($"Found property {prop}");
#endif

                yield return CreateEnumeratedProperty(prop, depth, path);

                if (recurse && prop.IsComposedType && prop.Value != null)
                {
                    var nextPath = String.Join('.', path, prop.PropertyName);
                    var obj = new ObjectProperties(prop.Value);

                    foreach (var item in obj.EnumerateProperties(recurse: true, depth + 1, nextPath))
                        yield return item;
                }
            }
        }

        public virtual IEnumerable<IEnumeratedProperty> EnumerateProperties(bool recurse)
        {
            return EnumerateProperties(recurse, 0, String.Empty);
        }
    }
}
