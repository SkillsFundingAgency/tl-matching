using System;
using System.Reflection;

namespace Sfa.Tl.Matching.Models.Extensions
{
    public static class EnumExtensions
    {
        public static TAttribute GetCustomAttribute<TAttribute>(this Enum value)
            where TAttribute : Attribute
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            return type.GetField(name!)!
                .GetCustomAttribute<TAttribute>();
        }
    }
}
