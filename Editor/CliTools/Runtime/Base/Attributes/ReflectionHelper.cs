using System;
using System.Collections.Generic;
using System.Linq;

namespace UniTools.Build
{
    public static class ReflectionHelper
    {
        public static IEnumerable<TAttribute> Find<TAttribute>() where TAttribute : Attribute
        {
            return
#if UNITY_6000_5_OR_NEWER
                UnityEngine.Assemblies.CurrentAssemblies.GetLoadedAssemblies()
#else
                AppDomain.CurrentDomain.GetAssemblies()
#endif
                .SelectMany(x => x.GetCustomAttributes(typeof(TAttribute), true)).Cast<TAttribute>()
                .ToList();
        }
    }
}
