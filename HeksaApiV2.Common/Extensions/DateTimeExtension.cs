using System;
using System.Globalization;

namespace HeksaApiV2.Common.Extensions
{
    public static class DateTimeExtension
    {
        public static string ToStringCulture(this DateTime value, string langCode = "id")
        {
            string result = "";
            if (value != null)
            {
                switch (langCode.ToLower())
                {
                    case "id":
                        result = value.ToString("dd MMMM yyyy", new CultureInfo("id-ID"));
                        break;

                    case "en":
                        result = value.ToString("dd MMMM yyyy", new CultureInfo("en-US"));
                        break;

                    default:
                        result = value.ToString("dd MMMM yyyy");
                        break;
                }
            }
            return result;
        }
        public static string ToStringNull(this DateTime value, string format)
        {
            string result = "";

            if (value > new DateTime(1900, 1, 1))
                result = value.ToString(format);

            return result;
        }

        public static string ToStringNull(this DateTime? value, string format)
        {
            string result = "";

            if(value.HasValue || value > new DateTime(1900,1,1))
                result = value.Value.ToString(format);
            
            return result;
        }
    }
}