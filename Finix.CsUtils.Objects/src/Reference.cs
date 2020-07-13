using System;

namespace Finix.CsUtils
{
    public static class Reference
    {
        public static T GetOrCreate<T>(ref T storage) where T : new()
        {
            return storage ??= new T();
        }

        public static T GetOrCreate<T>(ref T storage, params object[] parameters)
        {
            return storage ??= (T) Activator.CreateInstance(typeof(T), parameters);
        }

        public static T GetOrActivate<T>(ref T storage, Func<T> activator)
        {
            return storage ??= activator();
        }
    }
}
