using System;
using System.Collections.Generic;
using System.Text;

namespace HeksaApiV2.Model.Common
{
    public class APIMasterDataResultModel<T>
    {
        public APIMasterDataResultModel(bool IsSuccess)
        {
            this.StatusCode = (IsSuccess) ? "00" : "01";
            this.StatusMessage = (IsSuccess) ? "success" : "failed";
        }

        public APIMasterDataResultModel<T> SetSuccess(string message, T value = default(T))
        {
            this.StatusCode = "00";
            this.StatusMessage = message != null ? message : "success";
            this.Value = value;
            return this;
        }

        public APIMasterDataResultModel<T> SetFailed(string message, string statusCode = "01", T value = default(T), Exception Ex = null)
        {
            this.StatusCode = statusCode;
            this.StatusMessage = message != null ? message : "failed";
            this.Value = value;
            this.ValException = Ex;
            return this;
        }

        public string StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public T Value { get; set; }
        public Exception ValException { get; set; }
    }
}
