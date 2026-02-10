using System.Globalization;

namespace HeksaApiV2.Common.Extensions
{
    public static class LongExtension
    {
        public static string ToRupiah(this long angka, bool isWithoutRP = false)
        {
            if (isWithoutRP)
                return angka.ToString("n", new CultureInfo("id-ID"));
            else
                return string.Format("Rp ", angka.ToString("n", new CultureInfo("id-ID")));
        }

        public static string ToRupiahDot(this long angka, string prefix = "", string suffix = "", string defaultResult = "")
        {
            string result = "";
            if (angka == 0)
                result = defaultResult;
            else
                result = prefix + angka.ToString("#,##0").Replace(",", ".") + suffix;

            return result;
        }

        public static string ToStringNotZero(this long angka)
        {
            if (angka != 0)
                return angka.ToString();
            else
                return "";
        }
    }
}