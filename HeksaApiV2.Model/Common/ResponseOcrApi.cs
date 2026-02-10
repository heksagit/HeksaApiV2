using HeksaApiV2.Model.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeksaApiV2.Model.Common
{
    public interface IResponseOcrApi<T>
    {
        string status { get; set; }
        string message { get; set; }
        T data { get; set; }

        void SetSuccess(string _message, T value);

        void SetFailed(string _message, string statusCode, T value);
    }

    public class ResponseOcrApi<T> : IResponseOcrApi<T>
    {
        public string status { get; set; }
        public string message { get; set; }
        public T data { get; set; }

        public void SetSuccess(string _message, T value = default(T))
        {
            status = ResponseCode.Ok.GetCodeString();
            message = (!string.IsNullOrWhiteSpace(_message)) ? _message : "success";
            data = value;
        }

        public void SetFailed(string _message, string statusCode = "", T value = default(T))
        {
            status = string.IsNullOrWhiteSpace(statusCode) ? ResponseCode.InternalServerError.GetCodeString() : statusCode;
            message = (!string.IsNullOrWhiteSpace(_message)) ? _message : "failed"; ;
            data = value;
        }

        public ResponseOcrApi<T> ReturnSuccess(string _message, T value = default(T))
        {
            SetSuccess(_message, value);
            return this;
        }

        public ResponseOcrApi<T> ReturnFailed(string _message, string statusCode = "", T value = default(T))
        {
            SetFailed(_message, statusCode, value);
            return this;
        }
    }

    public class ScanOcrKtpParam
    {
        public string product_code { get; set; }
        public string service_sender { get; set; }
        public string base64image { get; set; }
    }

    public class ScanOcrKtpResponse
    {
        public string nik { get; set; }
        public string full_name { get; set; }
        public string dob { get; set; }
        public string place_dob { get; set; }
        public string gender { get; set; }
        public string religion { get; set; }
        public string job { get; set; }
        public string nationality { get; set; }
        public string marital_status { get; set; }
        public string address { get; set; }
        public string rt_rw { get; set; }
        public string sub_district { get; set; }
        public string district { get; set; }
        public string city { get; set; }
        public string province { get; set; }
        public string full_address { get; set; }
        public string full_district { get; set; }
        public bool is_blur { get; set; }
        public bool is_dark { get; set; }
        public bool is_flashlight { get; set; }
        public bool is_grayscale { get; set; }
    }

    public class OcrKtpModel
    {
        public string nik { get; set; }
        public string full_name { get; set; }
        public string dob { get; set; }
        public string place_dob { get; set; }
        public string gender { get; set; }
        public string religion { get; set; }
        public string job { get; set; }
        public string nationality { get; set; }
        public string marital_status { get; set; }
        public string full_district { get; set; }
        public string address { get; set; }
    }
}
