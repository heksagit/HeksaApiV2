using System;
using System.Collections.Generic;
using System.Text;

namespace HeksaApiV2.Model.Enum
{
    public enum RequestPaymentStatusCode
    {
        CREATED = 0,
        VERIFIED = 1,
        PROCESSED = 2,
        PAID = 3,
        REJECTED = 4,
        VOID = 5
    }
}
