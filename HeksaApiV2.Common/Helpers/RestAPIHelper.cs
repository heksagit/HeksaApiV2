using HeksaApiV2.Common.Object;
using HeksaApiV2.Model.Common;
using HeksaApiV2.Model.Enum;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace HeksaApiV2.Common.Helpers
{
    public class RestAPIHelper
    {
        private GlobalSettings _configs;

        public RestAPIHelper(IConfiguration _iconfig)
        {
            _configs = new GlobalSettings(_iconfig);
        }

        public IResult<T> GetBasicAuth<T>(string url, string username, string password, out string rawResult)
        {
            return SendRequestWithBasicAuth<T>(RestAPIRequestType.Get, url, null, username, password, out rawResult);
        }

        public IResult<T> PostBasicAuth<T>(string url, object param, string username, string password, out string rawResult)
        {
            return SendRequestWithBasicAuth<T>(RestAPIRequestType.Post, url, param, username, password, out rawResult);
        }

        private IResult<T> SendRequestWithBasicAuth<T>(RestAPIRequestType requestType, string url, object param, string username, string password, out string rawResult, int? timeout = null)
        {
            ResultModel<T> result = new ResultModel<T>();
            using (HttpClient client = new HttpClient())
            {
                if (timeout.HasValue)
                    client.Timeout = new TimeSpan(0, 0, timeout.GetValueOrDefault());
                var byteArray = Encoding.ASCII.GetBytes(username + ":" + password);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                HttpResponseMessage responseMessage = null;
                try
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                    string postBody = JsonConvert.SerializeObject(param);
                    switch (requestType)
                    {
                        case RestAPIRequestType.Post:
                            responseMessage = client.PostAsync(url, new StringContent(postBody, Encoding.UTF8, "application/json")).Result;
                            break;

                        case RestAPIRequestType.Get:
                            responseMessage = client.GetAsync(url).Result;
                            break;

                        case RestAPIRequestType.Put:
                            responseMessage = client.PutAsync(url, new StringContent(postBody, Encoding.UTF8, "application/json")).Result;
                            break;

                        case RestAPIRequestType.Delete:
                            responseMessage = client.DeleteAsync(url).Result; ;
                            break;
                    }

                    if (responseMessage == null)
                    {
                        rawResult = string.Empty;
                        result.SetFailed("No Response", ResponseCode.NoContent, null);
                    }
                    else
                    {
                        var objRes = ResultHandler<T>(responseMessage, out rawResult);
                        if (objRes != null)
                            result.SetSuccess("success", objRes);
                        else
                            result.SetFailed("Failed convert response api", ResponseCode.InternalServerError, null);
                    }
                }
                catch (TimeoutException ex)
                {
                    rawResult = string.Empty;
                    result.SetFailed(ex.Message, ResponseCode.GatewayTimeout, ex);
                }
                catch (WebException ex)
                {
                    rawResult = string.Empty;
                    result.SetFailed(ex.Message, ResponseCode.BadRequest, ex);
                }
                catch (Exception ex)
                {
                    rawResult = string.Empty;
                    result.SetFailed(ex.Message, ResponseCode.InternalServerError, ex);
                }
            }
            return result;
        }

        public T ResultHandler<T>(HttpResponseMessage responseMessage, out string rawResult)
        {
            string responseString = responseMessage.Content.ReadAsStringAsync().Result;
            rawResult = responseString;
            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {
                if (ContentHelper.IsValidJsonString(responseString))
                {
                    T dummy = default(T);
                    JsonSerializerSettings _settings = new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    };
                    return JsonConvert.DeserializeAnonymousType(responseString, dummy, _settings);
                }
                else
                {
                    return default(T);
                }
            }
            else
            {
                return default(T);
            }
        }

        public IResult<T> Post<T>(string url, object param, out string rawResult)
        {
            return SendRequest<T>(RestAPIRequestType.Post, url, param, out rawResult);
        }

        private IResult<T> SendRequest<T>(RestAPIRequestType requestType, string url, object param, out string rawResult)
        {
            ResultModel<T> result = new ResultModel<T>();
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage responseMessage = null;
                try
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                    string postBody = JsonConvert.SerializeObject(param);
                    switch (requestType)
                    {
                        case RestAPIRequestType.Post:
                            responseMessage = client.PostAsync(url, new StringContent(postBody, Encoding.UTF8, "application/json")).Result;
                            break;

                        case RestAPIRequestType.Get:
                            responseMessage = client.GetAsync(url).Result;
                            break;

                        case RestAPIRequestType.Put:
                            responseMessage = client.PutAsync(url, new StringContent(postBody, Encoding.UTF8, "application/json")).Result;
                            break;

                        case RestAPIRequestType.Delete:
                            responseMessage = client.DeleteAsync(url).Result; ;
                            break;
                    }

                    if (responseMessage == null)
                    {
                        rawResult = string.Empty;
                        result.SetFailed("No Response", ResponseCode.NoContent, null);
                    }
                    else
                    {
                        var obj = ResultHandler<T>(responseMessage, out rawResult);
                        if (obj == null)
                            result.SetFailed("Failed convert response api", ResponseCode.InternalServerError, null);
                        else
                            result.SetSuccess("success", obj);
                    }
                }
                catch (TimeoutException ex)
                {
                    rawResult = string.Empty;
                    result.SetFailed(ex.Message, ResponseCode.GatewayTimeout, ex);
                }
                catch (WebException ex)
                {
                    rawResult = string.Empty;
                    result.SetFailed(ex.Message, ResponseCode.BadRequest, ex);
                }
                catch (Exception ex)
                {
                    rawResult = string.Empty;
                    result.SetFailed(ex.Message, ResponseCode.InternalServerError, ex);
                }
            }

            return result;
        }

        public Stream DownloadFromWebSALSA(string url, out string rawResult)
        {
            rawResult = "";
            using (HttpClient client = new HttpClient())
            {

                client.DefaultRequestHeaders.Add(_configs.SITE_SECURITY_HEADER_NAME, _configs.SITE_SECURITY_HEADER_VALUE);
                HttpResponseMessage responseMessage = null;
                try
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    responseMessage = client.GetAsync(url).Result;
                }
                catch (Exception ex)
                {
                    rawResult = ex.StackTrace;
                    return null;
                }

                if (responseMessage == null || responseMessage.Content == null)
                    return null;
                else
                {
                    if (responseMessage.StatusCode == HttpStatusCode.OK)
                    {
                        return responseMessage.Content.ReadAsStreamAsync().Result;
                    }
                    else
                        return null;
                }
            }
        }
    }
}
