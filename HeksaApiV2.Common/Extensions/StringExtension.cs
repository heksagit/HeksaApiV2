using HeksaApiV2.Common.Object;
using System;
using System.Globalization;

namespace HeksaApiV2.Common.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        /// Convert string to int with default value
        /// </summary>
        /// <param name="value">string value</param>
        /// <param name="defaultValue">default value</param>
        /// <returns></returns>
        public static int GetToInt(this string value, int defaultValue = 0)
        {
            int res = 0;
            if (string.IsNullOrWhiteSpace(value))
            {
                if (defaultValue != 0)
                    return defaultValue;
                else
                    return 0;
            }
            else
            {
                int.TryParse(value, out res);
                return res;
            }
        }

        /// <summary>
        /// Convert string to decimal with default value
        /// </summary>
        /// <param name="value">string value</param>
        /// <param name="defaultValue">default value</param>
        /// <returns></returns>
        public static decimal GetToDecimal(this string value, decimal defaultValue = 0)
        {
            decimal res = 0;
            if (string.IsNullOrWhiteSpace(value))
            {
                if (defaultValue != 0)
                    return defaultValue;
                else
                    return 0;
            }
            else
            {
                decimal.TryParse(value, out res);
                return res;
            }
        }

        /// <summary>
        /// Convert string to decimal with default value
        /// </summary>
        /// <param name="value">string value</param>
        /// <param name="defaultValue">default value</param>
        /// <returns></returns>
        public static double GetToDouble(this string value, double defaultValue = 0)
        {
            double res = 0;
            if (string.IsNullOrWhiteSpace(value))
            {
                if (defaultValue != 0)
                    return defaultValue;
                else
                    return 0;
            }
            else
            {
                double.TryParse(value, out res);
                return res;
            }
        }

        /// <summary>
        /// Convert string to long with default value
        /// </summary>
        /// <param name="value">string value</param>
        /// <param name="defaultValue">default value</param>
        /// <returns></returns>
        public static long GetToLong(this string value, long defaultValue = 0)
        {
            long res = 0;
            if (string.IsNullOrWhiteSpace(value))
            {
                if (defaultValue != 0)
                    return defaultValue;
                else
                    return 0;
            }
            else
            {
                long.TryParse(value, out res);
                return res;
            }
        }

        /// <summary>
        /// Convert string to bool with default value
        /// </summary>
        /// <param name="value">string value</param>
        /// <param name="defaultValue">default value</param>
        /// <returns></returns>
        public static bool GetToBool(this string value, bool defaultValue = false)
        {
            bool res = false;
            if (string.IsNullOrWhiteSpace(value))
            {
                if (defaultValue != false)
                    return defaultValue;
                else
                    return false;
            }
            else
            {
                bool.TryParse(value, out res);
                return res;
            }
        }

        /// <summary>
        /// Convert string to DateTime with default value
        /// </summary>
        /// <param name="value">string value</param>
        /// <param name="defaultValue">default value</param>
        /// <returns></returns>
        public static DateTime GetToDateTime(this string value, DateTime? defaultValue = null)
        {
            DateTime result = DateTime.MinValue;
            if (string.IsNullOrWhiteSpace(value))
            {
                if (defaultValue != null)
                    return defaultValue.Value;
                else
                    return DateTime.MinValue;
            }
            else
            {
                DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
                return result;
            }
        }

        /// <summary>
        /// Convert string to formatted DateTime with default value
        /// </summary>
        /// <param name="value">string value</param>
        /// <param name="formatDate">datetime format</param>
        /// <param name="defaultValue">default value</param>
        /// <returns></returns>
        public static DateTime GetToDateTime(this string value, string formatDate, DateTime? defaultValue = null)
        {
            DateTime result = DateTime.MinValue;
            if (string.IsNullOrWhiteSpace(value))
            {
                if (defaultValue != null)
                    return defaultValue.Value;
                else
                    return DateTime.MinValue;
            }
            else
            {
                DateTime.TryParseExact(value, formatDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
                return result;
            }
        }

        /// <summary>
        /// Convert string to Byte with default value
        /// </summary>
        /// <param name="value">string value</param>
        /// <param name="defaultValue">default value</param>
        /// <returns></returns>
        public static Byte GetToByte(this string value, Byte defaultValue = 0)
        {
            Byte result = defaultValue;
            Byte.TryParse(value, out result);
            return result;
        }

        /// <summary>
        /// Convert string to UpperCase with null validation
        /// </summary>
        /// <param name="value">string value</param>
        /// <returns></returns>
        public static string ToUpperNull(this string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                return value.ToUpper();
            else
                return string.Empty;
        }

        /// <summary>
        /// Convert string to LowerCase with null validation
        /// </summary>
        /// <param name="value">string value</param>
        /// <returns></returns>
        public static string ToLowerNull(this string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                return value.ToLower();
            else
                return string.Empty;
        }

        /// <summary>
        /// Transform string to Title Case
        /// </summary>
        /// <param name="value">string value</param>
        /// <returns></returns>
        public static string ToTitleCase(this string value, string defaultVal = "")
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                if (defaultVal != "")
                    return defaultVal;
                else
                    return string.Empty;
            }
                

            CultureInfo ci = new CultureInfo("id-ID");
            value = value.ToLower();
            var strArray = value.Split(' ');
            if (strArray.Length > 1)
            {
                strArray[0] = ci.TextInfo.ToTitleCase(strArray[0]);
                return string.Join(" ", strArray);
            }
            return ci.TextInfo.ToTitleCase(value);
        }

        /// <summary>
        /// Transform string to Title Case with option which word will be title case only first or all word<
        /// </summary>
        /// <param name="value">string value</param>
        /// <param name="tcase">option which word will be title case only first or all word</param>
        /// <returns></returns>
        public static string ToTitleCase(this string value, TitleCaseEnum tcase)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            CultureInfo ci = new CultureInfo("id-ID");

            value = value.ToLower();
            switch (tcase)
            {
                case TitleCaseEnum.First:
                    var strArray = value.Split(' ');
                    if (strArray.Length > 1)
                    {
                        strArray[0] = ci.TextInfo.ToTitleCase(strArray[0]);
                        return string.Join(" ", strArray);
                    }
                    break;

                case TitleCaseEnum.All:
                    return ci.TextInfo.ToTitleCase(value);

                default:
                    break;
            }
            return ci.TextInfo.ToTitleCase(value);
        }

        /// <summary>
        /// Trim string with null validation
        /// </summary>
        /// <param name="value">string value</param>
        /// <returns></returns>
        public static string TrimNull(this string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                return value.Trim();
            else
                return string.Empty;
        }

        /// <summary>
        /// Compare string using Culture Info
        /// </summary>
        /// <param name="source">the source of string used for the search</param>
        /// <param name="searchKey">the search string used for the search</param>
        /// <param name="comparison">comparison method</param>
        /// <param name="culture">culture info</param>
        /// <returns></returns>
        public static bool Contains(this string source, string searchKey, StringComparison comparison, CultureInfo culture = null)
        {
            bool result = false;
            culture = CultureInfo.CurrentCulture;
            if (source != null && searchKey != null)
            {
                switch (comparison)
                {
                    case StringComparison.CurrentCulture:
                        {
                            source = source.ToString(culture);
                            searchKey = searchKey.ToString(culture);
                            result = source.Contains(searchKey);
                        }
                        break;

                    case StringComparison.CurrentCultureIgnoreCase:
                        {
                            source = source.ToLower(culture);
                            searchKey = searchKey.ToLower(culture);
                            result = source.Contains(searchKey);
                        }
                        break;

                    case StringComparison.InvariantCulture:
                        {
                            source = source.ToString(CultureInfo.InvariantCulture);
                            searchKey = searchKey.ToString(CultureInfo.InvariantCulture);
                            result = source.Contains(searchKey);
                        }
                        break;

                    case StringComparison.InvariantCultureIgnoreCase:
                        {
                            source = source.ToString(CultureInfo.InvariantCulture).ToLower();
                            searchKey = searchKey.ToString(CultureInfo.InvariantCulture).ToLower();
                            result = source.Contains(searchKey);
                        }
                        break;

                    case StringComparison.Ordinal:
                        result = source.Contains(searchKey);
                        break;

                    case StringComparison.OrdinalIgnoreCase:
                        {
                            source = source.ToLower();
                            searchKey = searchKey.ToLower();
                            result = source.Contains(searchKey);
                        }
                        break;

                    default:
                        result = source.Contains(searchKey);
                        break;
                }
            }
            return result;
        }
    }
}