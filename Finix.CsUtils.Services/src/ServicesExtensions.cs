using System;

namespace Finix.CsUtils
{
    public static class ServicesExtensions
    {
        public static T GetService<T>(this IServiceProvider provider)
        {
            return (T) provider.GetService(typeof(T));
        }
    }
}
