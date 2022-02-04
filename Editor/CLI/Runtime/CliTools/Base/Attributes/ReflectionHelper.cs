using System;
using System.Collections.Generic;
using System.Linq;

namespace UniTools.CLI
{
    public static class ReflectionHelper
    {
        public static IEnumerable<TAttribute> Find<TAttribute>() where TAttribute : Attribute
        {
            return AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(x => x.GetCustomAttributes(typeof(TAttribute), true)).Cast<TAttribute>()
                .ToList();
        }
    }
}