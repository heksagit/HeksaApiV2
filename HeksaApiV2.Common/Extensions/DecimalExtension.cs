using System;
using System.Collections.Generic;
using System.Text;

namespace HeksaApiV2.Common.Extensions
{
    public static class DecimalExtension
    {
        public static string ToStringNotZero(this decimal angka)
        {
            if (angka != 0)
                return angka.ToString();
            else
                return "";
        }

        public static string ToStringNotZero(this decimal? angka)
        {
            if (angka.HasValue && angka != 0)
                return angka.ToString();
            else
                return "";
        }

        public static string ToRupiahDot(this decimal angka, string prefix = "", string suffix = "", string defaultResult = "")
        {
            string result = "";
            if (angka == 0)
                result = defaultResult;
            else
                result = prefix + angka.ToString("#,##0").Replace(",", ".") + suffix;

            return result;
        }

        public static string ToRupiahDot(this decimal? angka, string prefix = "", string suffix = "", string defaultResult = "")
        {
            string result = "";
            if (!angka.HasValue || angka == 0)
                result = defaultResult;
            else
                result = prefix + angka.Value.ToString("#,##0").Replace(",", ".") + suffix;

            return result;
        }
    }
}
