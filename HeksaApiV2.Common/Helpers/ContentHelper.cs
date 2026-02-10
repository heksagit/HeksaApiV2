using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;

namespace HeksaApiV2.Common.Helpers
{
    public static class ContentHelper
    {
        private static readonly string[] Angka =
        {
            "", "Satu", "Dua", "Tiga", "Empat", "Lima",
            "Enam", "Tujuh", "Delapan", "Sembilan", "Sepuluh", "Sebelas"
        };

        public static bool IsValidJsonString(string jsonString)
        {
            try
            {
                JToken token = JObject.Parse(jsonString);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static IEnumerable<int> DivideEvenly(int numerator, int denominator)
        {
            int rem;
            int div = Math.DivRem(numerator, denominator, out rem);

            for (int i = 0; i < denominator; i++)
            {
                yield return i < rem ? div + 1 : div;
            }
        }

        public static string CreateRandomString(int length, string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")
        {
            try
            {
                if (length < 0) throw new ArgumentOutOfRangeException("length", "length cannot be less than zero.");
                if (string.IsNullOrEmpty(allowedChars)) throw new ArgumentException("allowedChars may not be empty.");

                const int byteSize = 0x100;
                var allowedCharSet = new HashSet<char>(allowedChars).ToArray();
                if (byteSize < allowedCharSet.Length) throw new ArgumentException(String.Format("allowedChars may contain no more than {0} characters.", byteSize));

                // Guid.NewGuid and System.Random are not particularly random. By using a
                // cryptographically-secure random number generator, the caller is always
                // protected, regardless of use.
                using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
                {
                    var result = new StringBuilder();
                    var buf = new byte[128];
                    while (result.Length < length)
                    {
                        rng.GetBytes(buf);
                        for (var i = 0; i < buf.Length && result.Length < length; ++i)
                        {
                            // Divide the byte into allowedCharSet-sized groups. If the
                            // random value falls into the last group and the last group is
                            // too small to choose from the entire allowedCharSet, ignore
                            // the value in order to avoid biasing the result.
                            var outOfRangeStart = byteSize - (byteSize % allowedCharSet.Length);
                            if (outOfRangeStart <= buf[i]) continue;
                            result.Append(allowedCharSet[buf[i] % allowedCharSet.Length]);
                        }
                    }
                    return result.ToString();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public static byte[] ObjectToByteArray(object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static T ByteArrayToObject<T>(byte[] arrBytes) where T : class
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = binForm.Deserialize(memStream);
                return (T)obj;
            }
        }

        public static string GenerateFixedUsername(string fullName, string branchCode, int fixedLength = 12)
        {
            if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(branchCode))
                return string.Empty;

            var words = fullName.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            string namePart;
            if (words.Length == 1)
            {
                namePart = words[0];
            }
            else
            {
                namePart = $"{words[0]}.{words[1][0]}";
            }

            // Calculate max name length based on branchCode length
            int namePartMaxLength = fixedLength - branchCode.Length;

            if (namePartMaxLength <= 0)
            {
                // Edge case: branchCode is too long, truncate to fixedLength
                return branchCode.Substring(0, fixedLength).ToLower();
            }

            // Adjust name part
            if (namePart.Length > namePartMaxLength)
                namePart = namePart.Substring(0, namePartMaxLength);
            else if (namePart.Length < namePartMaxLength)
                namePart = namePart.PadRight(namePartMaxLength, 'x');

            return (namePart + branchCode).ToLower();
        }

        public static bool ValidatePasswordCombination(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }

            // The same powerful regex is used here:
            const string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*()_+=\{\}\[\]\:;""'<>,.?\/\\|`~-])(?=.{8,})";

            return Regex.IsMatch(password, pattern);
        }

        public static string Terbilang(decimal number)
        {
            if (number == 0)
                return "Nol";

            if (number < 0)
                return "Minus " + Terbilang(Math.Abs(number));

            long bagianBulat = (long)Math.Floor(number);
            decimal bagianDesimal = number - bagianBulat;

            string hasil = TerbilangBulat(bagianBulat).Trim();

            if (bagianDesimal > 0)
            {
                string desimal = bagianDesimal
                    .ToString("0.################")
                    .Split('.')[1];

                hasil += " Koma " + TerbilangDesimal(desimal);
            }

            return hasil.Trim();
        }

        private static string TerbilangBulat(long angka)
        {
            if (angka < 12)
                return Angka[angka];

            if (angka < 20)
                return Angka[angka - 10] + " Belas";

            if (angka < 100)
                return TerbilangBulat(angka / 10) + " Puluh " + TerbilangBulat(angka % 10);

            if (angka < 200)
                return "Seratus " + TerbilangBulat(angka - 100);

            if (angka < 1000)
                return TerbilangBulat(angka / 100) + " Ratus " + TerbilangBulat(angka % 100);

            if (angka < 2000)
                return "Seribu " + TerbilangBulat(angka - 1000);

            if (angka < 1_000_000)
                return TerbilangBulat(angka / 1000) + " Ribu " + TerbilangBulat(angka % 1000);

            if (angka < 1_000_000_000)
                return TerbilangBulat(angka / 1_000_000) + " Juta " + TerbilangBulat(angka % 1_000_000);

            if (angka < 1_000_000_000_000)
                return TerbilangBulat(angka / 1_000_000_000) + " Miliar " + TerbilangBulat(angka % 1_000_000_000);

            return TerbilangBulat(angka / 1_000_000_000_000) + " Triliun " + TerbilangBulat(angka % 1_000_000_000_000);
        }

        private static string TerbilangDesimal(string angka)
        {
            var hasil = "";
            foreach (char c in angka)
            {
                hasil += Angka[int.Parse(c.ToString())] + " ";
            }
            return hasil.Trim();
        }

    }
}
