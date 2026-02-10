using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HeksaApiV2.Common.Helpers
{
    public class PhoneHelper
    {
        public bool isPhoneValid(string phoneNumber)
        {
            bool isValid = false;
            if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                Match match = Regex.Match(phoneNumber, @"08\d{8,11}$");
                if (match.Success)
                {
                    isValid = true;
                }
            }

            return isValid;
        }

        public bool isPhoneValid(string phoneNumber, int minlength, int maxlength)
        {
            bool isValid = false;
            if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                Match match = Regex.Match(phoneNumber, @"08\d{" + minlength + "," + maxlength + "}$");
                if (match.Success)
                {
                    isValid = true;
                }
            }

            return isValid;
        }

        public bool isHomePhoneValid(string phoneNumber)
        {
            bool isValid = false;
            if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                Match match = Regex.Match(phoneNumber, @"0\d{8,11}$");
                if (match.Success)
                {
                    isValid = true;
                }
            }

            return isValid;
        }

        public bool isHomePhoneValid(string phoneNumber, int minlength, int maxlength)
        {
            bool isValid = false;
            if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                Match match = Regex.Match(phoneNumber, @"0\d{" + (minlength - 1) + "," + maxlength + "}$");
                if (match.Success)
                {
                    isValid = true;
                }
            }

            return isValid;
        }
    }
}
