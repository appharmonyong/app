using System;
using System.ComponentModel;
using System.Reflection;

namespace Harmony.Common.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription<TEnum>(this TEnum value) where TEnum : Enum
        {
            var fieldInfo = value.GetType().GetField(value.GetName());
            var attribute = fieldInfo?.GetCustomAttribute<DescriptionAttribute>(false);
            return attribute?.Description ?? value.GetName();
        }

        public static string GetName<TEnum>(this TEnum value) where TEnum : Enum
        {
            return value.ToString();
        }

    }
}
