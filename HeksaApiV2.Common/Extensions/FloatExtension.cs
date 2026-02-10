using System.Globalization;

namespace HeksaApiV2.Common.Extensions
{
    public static class FloatExtension
    {
        public static string ToRupiah(this float angka, bool isWithoutRP = false)
        {
            if (isWithoutRP)
                return angka.ToString("n", new CultureInfo("id-ID"));
            else
                return string.Format("Rp ", angka.ToString("n", new CultureInfo("id-ID")));
        }
    }
}