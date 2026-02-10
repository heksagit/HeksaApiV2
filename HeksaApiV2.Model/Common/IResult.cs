using HeksaApiV2.Model.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeksaApiV2.Model.Common
{
    public interface IResult<T>
    {
        bool Success { get; set; }
        ResponseCode StatusCode { get; set; }
        string Message { get; set; }
        Exception ErrorException { get; set; }
        T Result { get; set; }

        void SetSuccess(string message);

        void SetSuccess(string message, T value);

        void SetFailed(string message);

        void SetFailed(string message, ResponseCode statusCode = ResponseCode.BadRequest, Exception ex = null);

        void SetFailed(string message, T value, ResponseCode statusCode = ResponseCode.BadRequest, Exception ex = null);

        IResult<T> ReturnFailed(string message, ResponseCode statusCode = ResponseCode.BadRequest, Exception ex = null);

        IResult<T> ReturnFailed(string message, T value, ResponseCode statusCode = ResponseCode.BadRequest, Exception ex = null);
    }

    public class ResultModel<T> : IResult<T>
    {
        private bool _isSuccess = false;

        public bool Success
        {
            get { return _isSuccess; }
            set { _isSuccess = value; }
        }

        public ResponseCode StatusCode { get; set; }
        public string Message { get; set; }
        public Exception ErrorException { get; set; }
        public T Result { get; set; }
        public object AdditionalInfo { get; set; }

        public void SetSuccess(string message)
        {
            _isSuccess = true;
            StatusCode = ResponseCode.Ok;
            Message = message != null ? message : "";
        }

        public void SetSuccess(string message, T value)
        {
            _isSuccess = true;
            StatusCode = ResponseCode.Ok;
            Message = message != null ? message : "";
            Result = value;
        }

        public void SetFailed(string message)
        {
            SetFailed(message, ResponseCode.InternalServerError, null);
        }

        public void SetFailed(string message, ResponseCode statusCode = ResponseCode.BadRequest, Exception ex = null)
        {
            _isSuccess = false;
            Message = message != null ? message : "";
            if (ex != null)
                ErrorException = ex;

            StatusCode = statusCode;
        }

        public void SetFailed(string message, T value, ResponseCode statusCode = ResponseCode.BadRequest, Exception ex = null)
        {
            _isSuccess = false;
            Message = message != null ? message : "";
            if (ex != null)
                ErrorException = ex;

            StatusCode = statusCode;
            Result = value;
        }

        public IResult<T> ReturnFailed(string message, T value, ResponseCode statusCode = ResponseCode.BadRequest, Exception ex = null)
        {
            SetFailed(message, value, statusCode, ex);
            return this;
        }

        public IResult<T> ReturnFailed(string message, ResponseCode statusCode = ResponseCode.BadRequest, Exception ex = null)
        {
            SetFailed(message, statusCode, ex);
            return this;
        }
    }
}
