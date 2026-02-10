using System;

namespace HeksaApiV2.DataAccess.MasterData.Objects
{
    public class DbResult<T> : IDbResult<T>
    {
        private bool _isSuccess = false;

        public bool Success
        {
            get { return _isSuccess; }
            set { _isSuccess = value; }
        }

        public DbResponseState StatusCode { get; set; }
        public string Message { get; set; }
        public Exception ErrorException { get; set; }
        public T Result { get; set; }
        public object AdditionalInfo { get; set; }

        public void SetSuccess(string message)
        {
            _isSuccess = true;
            StatusCode = DbResponseState.Ok;
            Message = message != null ? message : "";
        }

        public void SetSuccess(string message, T value)
        {
            _isSuccess = true;
            StatusCode = DbResponseState.Ok;
            Message = message != null ? message : "";
            Result = value;
        }

        public void SetFailed(string message)
        {
            SetFailed(message, DbResponseState.InternalServerError, null);
        }

        public void SetFailed(string message, DbResponseState statusCode = DbResponseState.BadRequest, Exception ex = null)
        {
            _isSuccess = false;
            Message = message != null ? message : "";
            if (ex != null)
                ErrorException = ex;

            StatusCode = statusCode;
        }

        public void SetFailed(string message, T value, DbResponseState statusCode = DbResponseState.BadRequest, Exception ex = null)
        {
            _isSuccess = false;
            Message = message != null ? message : "";
            if (ex != null)
                ErrorException = ex;

            StatusCode = statusCode;
            Result = value;
        }

        public DbResult<T> ReturnFailed(string message, T value, DbResponseState statusCode = DbResponseState.BadRequest, Exception ex = null)
        {
            SetFailed(message, value, statusCode, ex);
            return this;
        }

        public DbResult<T> ReturnFailed(string message, DbResponseState statusCode = DbResponseState.BadRequest, Exception ex = null)
        {
            SetFailed(message, statusCode, ex);
            return this;
        }
    }
}