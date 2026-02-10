using System.Linq;

namespace HeksaApiV2.Common.Extensions
{
    public static class IQueryableExtension
    {
        public static int CountNull<T>(this IQueryable<T> obj) where T : class
        {
            if (obj == null)
                return 0;
            else
                return obj.Count();
        }

        public static int CountNullStruct<T>(this IQueryable<T> obj) where T : struct
        {
            if (obj == null)
                return 0;
            else
                return obj.Count();
        }

        public static T FirstOrDefaultNull<T>(this IQueryable<T> obj) where T : class
        {
            if (obj == null)
                return null;
            else
                return obj.FirstOrDefault();
        }

        public static T FirstOrDefaultCustom<T>(this IQueryable<T> obj) where T : class
        {
            if (obj.FirstOrDefault() == null)
                return default(T);
            else
                return obj.FirstOrDefault();
        }
    }
}