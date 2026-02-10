using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace HeksaApiV2.Model.Enum
{
    public static class EnumExtensions
    {
        public static T GetEnum<T>(this string itemName)
        {
            return (T)System.Enum.Parse(typeof(T), itemName, true);
        }

        public static string GetEnumDescription(this System.Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return value.ToString();
            }
        }

        public static string GetCodeString(this System.Enum value)
        {
            return GetValue(value).ToString();
        }

        public static string GetEnumDisplayName(this System.Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DisplayAttribute[] attributes = (DisplayAttribute[])fi.GetCustomAttributes(typeof(DisplayAttribute), false);
            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].Name;
            }
            else
            {
                return value.ToString();
            }
        }

        public static List<T> GetItems<T>(bool excludeNone = false) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enum type.");
            }

            int noneValue = -1;
            if (excludeNone)
            {
                noneValue = GetNoneValue<T>();
            }

            List<T> list = new List<T>();
            foreach (T value in System.Enum.GetValues(typeof(T)))
            {
                if (noneValue > -1 && noneValue == GetValue<T>(value)) { continue; }
                list.Add(value);
            }

            return list;
        }

        public static int GetNoneValue<T>() where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enum type.");
            }

            if (System.Enum.TryParse("None", true, out T noneValue))
            {
                return GetValue<T>(noneValue);
            }
            else
            {
                return -1;
            }
        }

        public static int GetNoneValue(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Type must be an enum type.");
            }

            string[] names = System.Enum.GetNames(enumType);
            int indexOfNone = -1;
            for (int i = 0; i < names.Length; i++)
            {
                if (names[i] == "None")
                {
                    indexOfNone = i;
                    break;
                }
            }

            if (indexOfNone > -1)
            {
                int[] values = (int[])System.Enum.GetValues(enumType);
                return values[indexOfNone];
            }
            else
            {
                return -1;
            }
        }

        public static int GetValue<T>(T value) where T : struct, IConvertible
        {
            Type t = typeof(T);
            if (!t.IsEnum)
            {
                throw new ArgumentException("T must be an enum type.");
            }

            return value.ToInt32(CultureInfo.InvariantCulture.NumberFormat);
        }

        public static int GetValue(object enumValue)
        {
            Type t = enumValue.GetType();
            if (!t.IsEnum)
            {
                throw new ArgumentException("T must be an enum type.");
            }

            return ((IConvertible)enumValue).ToInt32(CultureInfo.InvariantCulture.NumberFormat);
        }

        public static List<int> GetValues(Type enumType, bool excludeNone = false)
        {
            int noneValue = -1;
            if (excludeNone) { noneValue = GetNoneValue(enumType); }

            Array items = System.Enum.GetValues(enumType);
            List<int> intValues = new List<int>();
            foreach (var en in items)
            {
                if (excludeNone && (int)en == noneValue)
                {
                    continue;
                }
                intValues.Add((int)en);
            }
            return intValues;
        }

        public static List<int> GetValues<T>(bool excludeNone = false)
        {
            return GetValues(typeof(T), excludeNone);
        }
    }
}
