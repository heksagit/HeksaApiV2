using System;

namespace HeksaApiV2.Common.Extensions
{
    public static class GuidExtension
    {
        public static bool IsNotNullOrEmpty(this Guid source)
        {
            if (source != null)
            {
                if (source != Guid.Empty)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
    }
}