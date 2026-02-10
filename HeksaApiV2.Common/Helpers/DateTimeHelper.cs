using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace HeksaApiV2.Common.Helpers
{
    public class DateTimeHelper
    {
        public static DateTime GetTimeNowZone(string zonestring)
        {
            string ZoneId = zonestring;
            DateTime localtime = DateTime.Now;
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(ZoneId);
            DateTime dataTimeByZoneId = TimeZoneInfo.ConvertTime(localtime, TimeZoneInfo.Local, timeZoneInfo);
            return dataTimeByZoneId;
        }

        public static DateTime StringtoDate(string datestr, string format)
        {
            return DateTime.ParseExact(datestr, format, CultureInfo.InvariantCulture);
        }

        public static DateTime StringtoDate(string datestr, string format, DateTime defaultVal)
        {
            var result = defaultVal;
            try
            {
                result = DateTime.ParseExact(datestr, format, CultureInfo.InvariantCulture, DateTimeStyles.None);
            }
            catch
            {
                result = defaultVal;
            }

            return result;
        }

        public static int GetAge(DateTime reference, DateTime birthday)
        {
            int age = reference.Year - birthday.Year;
            if (reference < birthday.AddYears(age)) age--;

            return age;
        }

        public static int GetAgeLastBirthday(DateTime birthDate, DateTime referenceDate)
        {
            var culture = CultureInfo.InvariantCulture;

            // Calculate last birthday
            DateTime lastBirthday = new DateTime(referenceDate.Year, birthDate.Month, birthDate.Day);

            if (lastBirthday > referenceDate)
            {
                lastBirthday = lastBirthday.AddYears(-1);
            }

            return lastBirthday.Year - birthDate.Year;
        }

        public static int GetAgeFromBirthDate(DateTime dateVal)
        {
            int result = 0;

            if(dateVal > new DateTime(1901, 1, 1))
            {
                var today = DateTime.Now;
                var intCurrentYear = today.Year;
                var intCurrentMonth = today.Month;
                var intCurrentDay = today.Day;

                var nearsBday = intCurrentMonth - dateVal.Month;
                var currentAge = intCurrentYear - dateVal.Year;

                if (intCurrentDay < dateVal.Day)
                {
                    nearsBday = nearsBday - 1;
                }

                if (nearsBday >= 6)
                {
                    currentAge = currentAge + 1;
                }
                else if (nearsBday < -6)
                {
                    currentAge = currentAge - 1;
                }

                result = currentAge;
            }


            return result;
        }

        public static int GetAgeNearestBirthday(DateTime birthDate, DateTime referenceDate)
        {
            // Calculate age based on average year length (365.24 days)
            double totalDays = (referenceDate - birthDate).TotalDays;
            int age = (int)Math.Round(totalDays / 365.24);

            return age;
        }

        public static int GetAgeNearestBirthday183(DateTime birthDate, DateTime referenceDate)
        {
            // Base year difference
            int years = referenceDate.Year - birthDate.Year;

            // Adjust if birthday hasn't occurred yet this year
            if (referenceDate.Month < birthDate.Month ||
                (referenceDate.Month == birthDate.Month && referenceDate.Day < birthDate.Day))
            {
                years--;
            }

            // Calculate days since last birthday
            DateTime lastBirthday = new DateTime(referenceDate.Year, birthDate.Month, birthDate.Day);
            if (referenceDate < lastBirthday)
            {
                lastBirthday = lastBirthday.AddYears(-1);
            }

            int daysAfterLastBirthday = (referenceDate - lastBirthday).Days;

            // Add 1 if more than 183 days since last birthday
            int age = years + (daysAfterLastBirthday > 183 ? 1 : 0);

            return age;
        }

        public static int GetAgeFromBirthDate(string strDate)
        {
            var resultAge = 0;
            if (!string.IsNullOrEmpty(strDate))
            {
                var splitStrDate = strDate.Split('/'); //Format MM/dd/yyyy
                var intYearLahir = 0;
                int.TryParse(splitStrDate[2], out intYearLahir);
                if (intYearLahir == 0)
                {
                    return resultAge;
                }
                var intMonthLahir = 0;
                int.TryParse(splitStrDate[0], out intMonthLahir);
                if (intMonthLahir == 0)
                {
                    return resultAge;
                }
                var intDayLahir = 0;
                int.TryParse(splitStrDate[1], out intDayLahir);
                if (intDayLahir == 0)
                {
                    return resultAge;
                }

                var today = DateTime.Now;
                var intCurrentYear = today.Year;
                var intCurrentMonth = today.Month;
                var intCurrentDay = today.Day;

                var nearsBday = intCurrentMonth - intMonthLahir;
                var currentAge = intCurrentYear - intYearLahir;

                if (intCurrentDay < intDayLahir)
                {
                    nearsBday = nearsBday - 1;
                }

                if (nearsBday >= 6)
                {
                    currentAge = currentAge + 1;
                }
                else if (nearsBday < -6)
                {
                    currentAge = currentAge - 1;
                }

                resultAge = currentAge;
            }

            return resultAge;
        }
    }
}
