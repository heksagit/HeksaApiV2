using HeksaApiV2.Common.Object;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace HeksaApiV2.Common.Helpers
{
    public class SessionHelper
    {
        private GlobalSettings _configs;
        private IHttpContextAccessor _httpAccess;

        public SessionHelper(IConfiguration _iconfig,
            IHttpContextAccessor _ihttpAccess)
        {
            _configs = new GlobalSettings(_iconfig);
            _httpAccess = _ihttpAccess;
        }

        public void Add(string key, string value)
        {
            var sessionVault = _httpAccess.HttpContext.Session;
            if (sessionVault.IsAvailable)
            {
                string sessionName = VariableResource.SessionVariable.PREFIX + key;
                sessionVault.SetString(sessionName, value);
            }
        }

        public void Add<T>(string key, T value) where T : class
        {
            var sessionVault = _httpAccess.HttpContext.Session;
            if (sessionVault.IsAvailable)
            {
                string sessionName = VariableResource.SessionVariable.PREFIX + key;
                var objVal = ContentHelper.ObjectToByteArray(value);
                sessionVault.Set(sessionName, objVal);
            }
        }

        public string Get(string key)
        {
            var sessionVault = _httpAccess.HttpContext.Session;
            string sessionName = VariableResource.SessionVariable.PREFIX + key;
            return sessionVault.GetString(sessionName);
        }

        public T Get<T>(string key) where T : class
        {
            var sessionVault = _httpAccess.HttpContext.Session;
            if (sessionVault != null)
            {
                string sessionName = VariableResource.SessionVariable.PREFIX + key;

                byte[] varByte = sessionVault.Get(sessionName);
                if (varByte != null)
                {
                    return ContentHelper.ByteArrayToObject<T>(varByte);
                }
                else
                    return null;
            }
            else
                return null;
        }

        public void Remove(string key)
        {
            var sessionVault = _httpAccess.HttpContext.Session;
            if (sessionVault.IsAvailable)
            {
                string sessionName = VariableResource.SessionVariable.PREFIX + key;
                byte[] varOut = new byte[] { };
                if (sessionVault.TryGetValue(sessionName, out varOut))
                    sessionVault.Remove(sessionName);
            }
        }
    }
}