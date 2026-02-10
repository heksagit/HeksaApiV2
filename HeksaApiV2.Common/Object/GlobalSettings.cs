using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeksaApiV2.Common.Object
{
    public class GlobalSettings
    {
        private IConfiguration _config { get; }

        public GlobalSettings(IConfiguration configuration)
        {
            _config = configuration;
        }

        public string SITE_URL
        {
            get
            {
                return _config.GetSection("GlobalSettings")["SITE_URL"];
            }
        }

        public string SITE_URL_FOR_MENU
        {
            get
            {
                return _config.GetSection("GlobalSettings")["SITE_URL_FOR_MENU"];
            }
        }

        public string SITE_URL_FOR_HTTP_REQUEST
        {
            get
            {
                return _config.GetSection("GlobalSettings")["SITE_URL_FOR_HTTP_REQUEST"];
            }
        }

        public bool IS_DEVELOPMENT
        {
            get
            {
                bool isDev = false;
                bool.TryParse(_config.GetSection("GlobalSettings")["IS_DEVELOPMENT"], out isDev);
                return isDev;
            }
        }

        public string SITE_SECURITY_HEADER_NAME
        {
            get
            {
                return _config.GetSection("GlobalSettings")["SITE_SECURITY_HEADER_NAME"];
            }
        }

        public string SITE_SECURITY_HEADER_VALUE
        {
            get
            {
                return _config.GetSection("GlobalSettings")["SITE_SECURITY_HEADER_VALUE"];
            }
        }

        public string COOKIE_PREFIX
        {
            get
            {
                return _config.GetSection("GlobalSettings")["COOKIE_PREFIX"];
            }
        }

        public string LOCAL_WEB_ASSET_PATH_FOLDER
        {
            get
            {
                return _config.GetSection("GlobalSettings")["LOCAL_WEB_ASSET_PATH_FOLDER"];
            }
        }

        public string SMTP_HOST
        {
            get
            {
                return _config.GetSection("GlobalSettings")["SMTP_HOST"];
            }
        }

        public string SMTP_USERNAME
        {
            get
            {
                return _config.GetSection("GlobalSettings")["SMTP_USERNAME"];
            }
        }

        public string SMTP_PASSWORD
        {
            get
            {
                return _config.GetSection("GlobalSettings")["SMTP_PASSWORD"];
            }
        }

        public string SMTP_PORT
        {
            get
            {
                return _config.GetSection("GlobalSettings")["SMTP_PORT"];
            }
        }

        public string SMTP_SSL
        {
            get
            {
                return _config.GetSection("GlobalSettings")["SMTP_SSL"];
            }
        }

        public string SITE_EMAIL
        {
            get
            {
                return _config.GetSection("GlobalSettings")["SITE_EMAIL"];
            }
        }

        public string SMTP_NAME_DISPLAY
        {
            get
            {
                return _config.GetSection("GlobalSettings")["SMTP_NAME_DISPLAY"];
            }
        }
    }
}
