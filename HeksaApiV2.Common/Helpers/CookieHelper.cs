using HeksaApiV2.Common.Object;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace HeksaApiV2.Common.Helpers
{
    public class CookieHelper
    {
        private GlobalSettings _configs;
        private IHttpContextAccessor _httpAccess;

        public CookieHelper(IConfiguration _iconfig,
            IHttpContextAccessor _ihttpAccess)
        {
            _configs = new GlobalSettings(_iconfig);
            _httpAccess = _ihttpAccess;
        }

        public void Add(string key, string value, bool persistent, bool encrypt = false)
        {
            var response = _httpAccess.HttpContext.Response;
            string strHost = _httpAccess.HttpContext.Request.Host.Value;

            string strCookieName = VariableResource.CookieVariable.PREFIX + key;
            CookieOptions option = new CookieOptions();
            if (encrypt)
                value = EncryptHelper.EncryptString(value);
            if (persistent)
                option.Expires = DateTime.Now.AddDays(VariableResource.CookieVariable.TIMEOUT_IN_DAY);
            if (strHost.ToLower().Contains("heksainsurance"))
                option.Domain = ".heksainsurance.co.id";
            response.Cookies.Append(strCookieName, value, option);
        }

        public string Get(string key, bool encrypted = true)
        {
            string result = string.Empty;
            var cookieVault = _httpAccess.HttpContext.Request.Cookies;
            if (cookieVault != null)
            {
                if (cookieVault.ContainsKey(VariableResource.CookieVariable.PREFIX + key))
                {
                    result = cookieVault[VariableResource.CookieVariable.PREFIX + key];

                    if (encrypted)
                        result = EncryptHelper.DecryptString(result);
                }
            }
            return result;
        }

        public void RemoveAll()
        {
            var response = _httpAccess.HttpContext.Response;
            var cookieVault = _httpAccess.HttpContext.Request.Cookies;
            string strHost = _httpAccess.HttpContext.Request.Host.Value;
            if (cookieVault != null)
            {
                string cookieName = string.Empty;
                for (int i = 0; i < cookieVault.Count; i++)
                {
                    cookieName = cookieVault.ElementAt(i).Key;
                    CookieOptions option = new CookieOptions();
                    option.Expires = DateTime.Now.AddDays(-1);
                    if (strHost.ToLower().Contains("heksainsurance"))
                        option.Domain = ".heksainsurance.co.id";
                    response.Cookies.Delete(cookieName, option);
                }
            }
        }
    }
}