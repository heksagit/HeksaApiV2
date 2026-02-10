using System;
using System.Collections.Generic;
using System.Text;

namespace HeksaApiV2.Model.Enum
{
    public enum CommissionStatusCode
    {
        CREATED = 0,
        PROCESSED = 1,
        PAID = 2,
        PENDING = 3,
        REJECTED = 4,
        VOID = 5
    }
}
