using System.Runtime.CompilerServices;
using System.Linq;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Finix.CsUtils
{
    public static class ServiceActivator
    {
        public static IEnumerable<object?> Retrieve(IServiceProvider services, IEnumerable<Type> types)
        {
            return types.Select(t => t == typeof(IServiceProvider) ? services : services.GetService(t));
        }

        public static IEnumerable<object?> Retrieve(IServiceProvider services, IEnumerable<ParameterInfo> parameters)
        {
            return Retrieve(services, parameters.Select(p => p.ParameterType));
        }

        public static IEnumerable<object?> Retrieve(IServiceProvider services, MethodBase method)
        {
            return Retrieve(services, method.GetParameters());
        }

        private static float GetArity(MethodBase method)
        {
            var parameters = method.GetParameters();
            float count = parameters.Where(p => p.ParameterType != typeof(IServiceProvider)).Count();

            if (parameters.Any(p => p.ParameterType == typeof(IServiceProvider)))
                count += 0.5f;

            return count;
        }

        public static MethodBase? GetMatching(IServiceProvider services, IEnumerable<MethodBase> methods)
        {
            var frames = new System.Diagnostics.StackTrace(fNeedFileInfo: false).GetFrames();

            foreach (var method in methods.OrderByDescending(GetArity))
            {
                if (frames.Any(f => f.GetMethod() == method))
                    continue;
                else if (Retrieve(services, method).All(obj => obj != null))
                    return method;
                else if (method.GetParameters().Length == 0)
                    return method;
            }

            return null;
        }

        public static T? GetMatching<T>(IServiceProvider services, IEnumerable<T> methods) where T : MethodBase
        {
            return (T?) GetMatching(services, (IEnumerable<MethodBase>) methods);
        }

        public static IEnumerable<MethodInfo> GetMethods(Type type, string methodName, bool allowPrivate = false, bool allowStatic = false)
        {
            var flags = BindingFlags.Default | BindingFlags.Instance | BindingFlags.Public;

            if (allowPrivate)
                flags |= BindingFlags.NonPublic;

            if (allowStatic)
                flags |= BindingFlags.Static;

            return type.GetMethods(flags).Where(n => n.Name == methodName);
        }

        public static IEnumerable<ConstructorInfo> GetConstructors(Type type, bool allowPrivate = false)
        {
            var flags = BindingFlags.Default | BindingFlags.Instance | BindingFlags.Public;

            if (allowPrivate)
                flags |= BindingFlags.NonPublic;

            return type.GetConstructors(flags);
        }

        public static object? CallStatic(this IServiceProvider services, ref MethodInfo? cachedMethod, Type type, string name, Type? returnType = null, bool allowPrivate = false)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            bool IsReturnTypeValid(MethodInfo method)
            {
                return returnType is null ? typeof(void).IsAssignableFrom(method.ReturnType) : returnType.IsAssignableFrom(method.ReturnType);
            }

            if (cachedMethod is null || !IsReturnTypeValid(cachedMethod))
            {
                var methods = GetMethods(type, name, allowPrivate: allowPrivate, allowStatic: true).ToArray();
                var method = GetMatching(services, methods.Where(IsReturnTypeValid));

                if (method == null)
                {
                    if (methods.Length == 0)
                        throw new InvalidOperationException($"Object of type [{type}] does not contain a method called '{name}'.");
                    else
                        throw new InvalidOperationException($"No method called '{name}' on [{type}] has a signature that matches types available in the service provider.");
                }

                cachedMethod = method;
            }

            return cachedMethod.Invoke(null, Retrieve(services, cachedMethod).ToArray());
        }

        [return: MaybeNull]
        public static T CallStatic<T>(this IServiceProvider services, ref MethodInfo? cachedMethod, Type type, string name, bool allowPrivate = false)
        {
            return (T?) services.CallStatic(ref cachedMethod, type, name, returnType: typeof(T), allowPrivate: allowPrivate);
        }

        public static object? Call(this IServiceProvider services, ref MethodInfo? cachedMethod, object obj, string name, Type? returnType = null, bool allowPrivate = false)
        {
            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            bool IsReturnTypeValid(MethodInfo method)
            {
                return returnType is null ? typeof(void).IsAssignableFrom(method.ReturnType) : returnType.IsAssignableFrom(method.ReturnType);
            }

            if (cachedMethod is null || !IsReturnTypeValid(cachedMethod))
            {
                var methods = GetMethods(obj.GetType(), name, allowPrivate: allowPrivate).ToArray();
                var method = GetMatching(services, methods.Where(IsReturnTypeValid));

                if (method == null)
                {
                    if (methods.Length == 0)
                        throw new InvalidOperationException($"Object of type [{obj.GetType()}] does not contain a method called '{name}'.");
                    else
                        throw new InvalidOperationException($"No method called '{name}' on [{obj.GetType()}] has a signature that matches types available in the service provider.");
                }

                cachedMethod = method;
            }

            return cachedMethod.Invoke(obj, Retrieve(services, cachedMethod).ToArray());
        }

        [return: MaybeNull]
        public static T Call<T>(this IServiceProvider services, ref MethodInfo? cachedMethod, object obj, string name, bool allowPrivate = false)
        {
            return (T?) services.Call(ref cachedMethod, obj, name, returnType: typeof(T), allowPrivate: allowPrivate);
        }

        public static object Create(this IServiceProvider services, ref ConstructorInfo? cachedConstructor, Type type, bool allowPrivate = false)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            if (cachedConstructor is null)
            {
                var constructors = GetConstructors(type, allowPrivate: allowPrivate).ToArray();
                var constructor = GetMatching(services, constructors);

                if (constructor == null)
                {
                    if (constructors.Length == 0)
                        throw new InvalidOperationException($"Object of type [{type}] does not contain a callable constructor.");
                    else
                        throw new InvalidOperationException($"No constructor on [{type}] has a signature that matches types available in the service provider.");
                }

                cachedConstructor = constructor;
            }

            return cachedConstructor.Invoke(Retrieve(services, cachedConstructor).ToArray());
        }

        public static T Create<T>(this IServiceProvider services, ref ConstructorInfo? cachedConstructor, bool allowPrivate = false)
        {
            return (T) services.Create(ref cachedConstructor, typeof(T), allowPrivate: allowPrivate);
        }

        public static object? CallStatic(this IServiceProvider services, Type type, string name, Type? returnType = null, bool allowPrivate = false)
        {
            MethodInfo? cached = null;
            return services.CallStatic(ref cached, type, name, returnType: returnType, allowPrivate: allowPrivate);
        }

        [return: MaybeNull]
        public static object CallStatic<T>(this IServiceProvider services, Type type, string name, bool allowPrivate = false)
        {
            return (T?) services.CallStatic(type, name, returnType: typeof(T), allowPrivate: allowPrivate);
        }

        public static object? Call(this IServiceProvider services, object obj, string name, Type? returnType = null, bool allowPrivate = false)
        {
            MethodInfo? cached = null;
            return services.Call(ref cached, obj, name, returnType: returnType, allowPrivate: allowPrivate);
        }

        [return: MaybeNull]
        public static T Call<T>(this IServiceProvider services, object obj, string name, bool allowPrivate = false)
        {
            return (T?) services.Call(obj, name, returnType: typeof(T), allowPrivate: allowPrivate);
        }

        public static object Create(this IServiceProvider services, Type type, bool allowPrivate = false)
        {
            ConstructorInfo? cached = null;
            return services.Create(ref cached, type, allowPrivate: allowPrivate);
        }

        public static T Create<T>(this IServiceProvider services, bool allowPrivate = false)
        {
            return (T) services.Create(typeof(T), allowPrivate: allowPrivate);
        }
    }
}