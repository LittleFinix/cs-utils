using System.Linq;
using System.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Finix.CsUtils
{
    internal static class Dict
    {
        public static IDictionary Proxy(object obj)
        {
            return new DictProxy(obj);
        }

        public static void Clear(object obj)
        {
            var m = obj.GetType().GetMethod("Clear", 0, Type.EmptyTypes);
            m.Invoke(obj, null);
        }

        public static IEnumerable<(object, object)> Iterate(dynamic obj)
        {
            PropertyInfo? keyProp = null, valueProp = null;

            foreach (var kv in (IEnumerable) obj)
            {
                keyProp ??= kv.GetType().GetProperty("Key");
                valueProp ??= kv.GetType().GetProperty("Value");
                var key = keyProp.GetValue(kv);
                var value = valueProp.GetValue(kv);

                yield return (key, value);
            }
        }

        public class DictProxy : IDictionary
        {
            private readonly PropertyInfo index, keys, values, count, readOnly;
            private readonly MethodInfo add, clear, contains, copyTo, getEnumerator, remove;

            private readonly object obj;

            public DictProxy(object obj)
            {
                this.obj = obj;
                var type = obj.GetType();

                if (!Property.IsDictionaryType(type))
                    throw new ArgumentException("Argument must be of a dictionary type", nameof(obj));

                index = type.GetProperties().Single(p => p.GetIndexParameters().Length > 0);
                keys = type.GetProperty(nameof(Keys))!;
                values = type.GetProperty(nameof(Values))!;
                count = type.GetProperty(nameof(Count))!;
                readOnly = type.GetProperty(nameof(IsReadOnly))!;

                add = type.GetMethods().Single(m => m.Name == nameof(Add) && m.GetParameters().Length == 2);
                remove = type.GetMethods().Single(m => m.Name == nameof(Remove) && m.GetParameters().Length == 1);
                clear = type.GetMethod("Clear");
                contains = type.GetMethods().Single(m => m.Name is nameof(Contains) or "ContainsKey" && m.GetParameters().Length == 1);
                getEnumerator = type.GetMethods().Single(m => m.Name == nameof(GetEnumerator));
            }

            public object this[object key]
            {
                get => index.GetValue(obj, new[] { key })!;
                set => index.SetValue(obj, value, new[] { key });
            }

            public bool IsFixedSize => false;

            public bool IsReadOnly => (bool) readOnly.GetValue(obj)!;

            public ICollection Keys => (ICollection) keys.GetValue(obj)!;

            public ICollection Values => (ICollection) values.GetValue(obj)!;

            public int Count => (int) count.GetValue(obj)!;

            public bool IsSynchronized => false;

            public object SyncRoot => throw new NotImplementedException();

            public void Add(object key, object? value)
            {
                add.Invoke(obj, new[] { key, value });
            }

            public void Clear()
            {
                clear.Invoke(obj, Array.Empty<object>());
            }

            public bool Contains(object key)
            {
                return (bool) contains.Invoke(obj, new[] { key })!;
            }

            public void CopyTo(Array array, int index)
            {
                copyTo.Invoke(obj, new object[] { array, index });
            }

            public IDictionaryEnumerator GetEnumerator()
            {
                throw new NotSupportedException();
            }

            public void Remove(object key)
            {
                remove.Invoke(obj, new[] { key });
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable) obj).GetEnumerator();
            }
        }
    }
}