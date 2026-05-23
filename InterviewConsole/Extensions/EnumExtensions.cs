using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;

namespace EmployeeService.Implementation.Extensions
{
    public static class EnumExtensions
    {
        private static readonly ConcurrentDictionary<Enum, string> DescriptionCache = new ConcurrentDictionary<Enum, string>();

        public static string GetDescription(this Enum value)
        {
            return DescriptionCache.GetOrAdd(value, GetDescriptionFromEnum);
        }

        private static string GetDescriptionFromEnum(Enum enumVal)
        {
            Type type = enumVal.GetType();

            string name = GetEnumFieldName(type, enumVal);
            if (name == null)
            {
                return enumVal.ToString();
            }

            var attribute = GetDescriptionAttribute(type, name);
            return attribute != null ? attribute.Description : enumVal.ToString();
        }

        private static string GetEnumFieldName(Type enumType, Enum enumVal)
        {
            return Enum.GetName(enumType, enumVal);
        }

        private static DescriptionAttribute GetDescriptionAttribute(Type enumType, string fieldName)
        {
            FieldInfo field = enumType.GetField(fieldName);
            if (field == null)
            {
                return null;
            }

            return field.GetCustomAttribute<DescriptionAttribute>();
        }
    }
}