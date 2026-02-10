using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace HeksaApiV2.Common.Extensions
{
    public static class IntExtension
    {
        public static string ToRupiah(this int angka, bool isWithoutRP = false)
        {
            if (isWithoutRP)
                return angka.ToString("n", new CultureInfo("id-ID"));
            else
                return string.Format("Rp ", angka.ToString("n", new CultureInfo("id-ID")));
        }

        public static string ToStringNotZero(this int angka)
        {
            if (angka != 0)
                return angka.ToString();
            else
                return "";
        }
    }
}
