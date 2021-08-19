
using System.Reflection;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Finix.CsUtils
{
    public sealed class ObjectProperties : PropertiesBase
    {
        public override object Object { get; }

        public ObjectProperties(object obj)
        {
            Object = obj ?? throw new ArgumentNullException(nameof(obj));
        }
    }
}
