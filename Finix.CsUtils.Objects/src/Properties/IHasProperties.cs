
using System.Reflection;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Finix.CsUtils
{
    public interface IHasProperties
    {
        IProperties Properties { get; }
    }
}
