using System;
using System.Collections.Generic;
using System.Text;

namespace HeksaApiV2.DataAccess.MasterData.Objects
{
    public interface IDbResult<T>
    {
        bool Success { get; set; }
        DbResponseState StatusCode { get; set; }
        string Message { get; set; }
        Exception ErrorException { get; set; }
        T Result { get; set; }

        void SetSuccess(string message);

        void SetFailed(string message);
    }
}
