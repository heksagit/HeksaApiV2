using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace HeksaApiV2.Common.Extensions
{
    public static class IEnumerableExtension
    {
        public static int CountNull<T>(this IEnumerable<T> obj) where T : class
        {
            if (obj == null)
                return 0;
            else
                return obj.Count();
        }

        public static int CountNullStruct<T>(this IEnumerable<T> obj) where T : struct
        {
            if (obj == null)
                return 0;
            else
                return obj.Count();
        }

        public static T FirstOrDefaultNull<T>(this IEnumerable<T> obj) where T : class
        {
            if (obj == null)
                return null;
            else
                return obj.FirstOrDefault();
        }

        public static T FirstOrDefaultCustom<T>(this IEnumerable<T> obj) where T : class
        {
            if (obj.FirstOrDefault() == null)
                return default(T);
            else
                return obj.FirstOrDefault();
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            if (list != null)
            {
                RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
                int n = list.Count();
                while (n > 1)
                {
                    byte[] box = new byte[1];
                    do provider.GetBytes(box);
                    while (!(box[0] < n * (Byte.MaxValue / n)));
                    int k = (box[0] % n);
                    n--;
                    T value = list[k];
                    list[k] = list[n];
                    list[n] = value;
                }
            }
        }
    }
}