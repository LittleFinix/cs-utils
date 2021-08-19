using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Finix.CsUtils
{
    public static class TypeLoader
    {
        public class Query
        {
            public Func<Type, Result, bool>? Where { get; set; }

            public bool AllowAbstract { get; set; }

            public Type? Inherits { get; set; }

            public IList<Type> Implements { get; set; } = new List<Type>();

            public IList<Type> ImplementsGeneric { get; set; } = new List<Type>();

            public IList<Type> RequiredAttributes { get; set; } = new List<Type>();

            public IList<Type> OptionalAttributes { get; set; } = new List<Type>();
        }

        public class Query<T> : Query
        {
            public Query(bool allowAbstract = false)
            {
                Inherits = typeof(T);
                AllowAbstract = allowAbstract;
            }
        }

        public class Result
        {
            public Result(Type type, IList<Attribute>? attributes = null)
            {
                Type = type;
                Attributes = new List<Attribute>(attributes ?? Enumerable.Empty<Attribute>());
            }

            public Type Type { get; }

            public IList<Attribute> Attributes { get; }

            public IList<Type> Interfaces { get; } = new List<Type>();

            public IList<Type> GenericInterfaces { get; } = new List<Type>();

            public virtual object Instance(params object[] parameters)
            {
                return Activator.CreateInstance(Type, parameters) ?? throw new Exception($"Failed to create instance of type {Type}");
            }

            public virtual T Instance<T>(params object[] parameters)
            {
                return (T) Instance(parameters);
            }

            public virtual T? Attribute<T>() where T : Attribute
            {
                return Attributes.OfType<T>().FirstOrDefault();
            }
        }

        public class Result<T> : Result
        {
            public Result(Type type, IList<Attribute>? attributes = null) : base(type, attributes)
            {
            }

            public new T Instance(params object[] parameters)
            {
                return base.Instance<T>();
            }
        }

        public static IEnumerable<Result> LoadFromAssembly(Assembly assembly, Query query)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (type.IsAbstract && !query.AllowAbstract || query.Inherits?.IsAssignableFrom(type) == false)
                    continue;

                var result = new Result(type);

                foreach (var intf in query.Implements)
                {
                    if (!intf.IsAssignableFrom(type))
                        goto skip;

                    result.Interfaces.Add(intf);
                }

                foreach (var intf in query.ImplementsGeneric)
                {
                    if (!intf.IsGenericTypeDefinition)
                        throw new InvalidOperationException("Interfaces in ImplementsGeneric must be generic type definitions.");

                    var count = 0;
                    foreach (var actualIntf in type.GetInterfaces())
                    {
                        if (!actualIntf.IsGenericType || actualIntf.GetGenericTypeDefinition() != intf)
                            continue;

                        result.GenericInterfaces.Add(actualIntf);
                        count++;
                    }

                    if (count == 0)
                        goto skip;
                }

                foreach (var attrType in query.RequiredAttributes)
                {
                    if (type.GetCustomAttribute(attrType) is Attribute attr)
                    {
                        result.Attributes.Add(attr);
                        continue;
                    }

                    goto skip;
                }

                foreach (var attrType in query.OptionalAttributes)
                {
                    if (type.GetCustomAttribute(attrType) is Attribute attr)
                        result.Attributes.Add(attr);
                }

                if (query.Where?.Invoke(type, result) ?? true)
                    yield return result;

                skip:;
            }
        }

        public static IEnumerable<Result> LoadAll(Query query)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (!assembly.IsFullyTrusted
                    || assembly.IsDynamic
                    || assembly.ReflectionOnly)
                    continue;

                IEnumerable<Result> results;

                try
                {
                    results = LoadFromAssembly(assembly, query).ToArray();
                }
                catch (ReflectionTypeLoadException)
                {
                    // ignore
                    continue;
                }

                foreach (var type in results)
                    yield return type;
            }
        }

        public static IEnumerable<Result<T>> LoadFromAssembly<T>(Assembly assembly, Query<T> query)
        {
            foreach (var result in LoadFromAssembly(assembly, (Query) query))
            {
                yield return new Result<T>(result.Type, result.Attributes);
            }
        }

        public static IEnumerable<Result<T>> LoadAll<T>(Query<T> query)
        {
            foreach (var result in LoadAll((Query) query))
            {
                yield return new Result<T>(result.Type, result.Attributes);
            }
        }

        public static IEnumerable<Type> LoadFromAssembly<T>(Assembly assembly, bool allowAbstract = false)
        {
            var q = new Query {
                Inherits = typeof(T),
                AllowAbstract = allowAbstract
            };

            return LoadFromAssembly(assembly, q).Select(r => r.Type);
        }

        public static IEnumerable<Type> LoadAll<T>(bool allowAbstract = false)
        {
            var q = new Query {
                Inherits = typeof(T),
                AllowAbstract = allowAbstract
            };

            return LoadAll(q).Select(r => r.Type);
        }

        public static IEnumerable<T> InstancesOf<T>(IEnumerable<Type> types)
        {
            foreach (var type in types.Where(t => typeof(T).IsAssignableFrom(t)))
            {
                yield return (T) Activator.CreateInstance(type)! ?? throw new Exception($"Failed to create instance of type {type}");
            }
        }


        public static string GetDisplayName(ICustomAttributeProvider provider, string backup = "MISSING")
        {
            return GetCustomAttribute<DisplayNameAttribute>(provider)?.DisplayName
                ?? GetCustomAttribute<DisplayAttribute>(provider)?.GetName()
                ?? (provider is MemberInfo mi ? mi.Name : null) // Also matches System.Type
                ?? backup;
        }

        public static T? GetCustomAttribute<T>(ICustomAttributeProvider provider, bool inherit = true) where T : Attribute
        {
            return provider.GetCustomAttributes(typeof(T), inherit)?.OfType<T>().FirstOrDefault();
        }

        public static IEnumerable<object> Instances(this IEnumerable<Result> results, params object[] arguments)
        {
            return results.Select(r => r.Instance(arguments));
        }

        public static IEnumerable<T> Instances<T>(this IEnumerable<Result> results, params object[] arguments)
        {
            return results.Select(r => r.Instance<T>(arguments));
        }

        public static IEnumerable<T> Instances<T>(this IEnumerable<Result<T>> results, params object[] arguments)
        {
            return results.Select(r => r.Instance(arguments));
        }
    }
}
