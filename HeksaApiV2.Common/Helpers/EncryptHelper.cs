using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HeksaApiV2.Common.Helpers
{
    public class EncryptHelper
    {
        private const string passPhrase = "Pas5pr@se";
        private const string saltValue = "heks4IN5ur@Evalue";
        private const string hashAlgorithm = "SHA1";
        private const int passwordIterations = 2;
        private const string initVector = "@1B2c3D4e5F6g7H8"; // must be 16 bytes
        private const int keySize = 256;
        private const string tripleDESKey = "3De5k3ysHeks4In5";

        #region Encrypt Decrypt Rijndael

        /// <summary>
        /// Encrypting Method
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="strKey"></param>
        /// <param name="strIV"></param>
        /// <returns></returns>
        public static string EncryptRijndael(string plainText, string strKey, string strIV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (strKey == null || strKey.Length <= 0)
                throw new ArgumentNullException("strKey");
            if (strIV == null || strIV.Length <= 0)
                throw new ArgumentNullException("strIV");

            byte[] encrypted;
            byte[] cipherKey = ASCIIEncoding.ASCII.GetBytes(strKey);
            byte[] cipherIV = ASCIIEncoding.ASCII.GetBytes(strIV);

            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = cipherKey;
                rijAlg.IV = cipherIV;
                rijAlg.Padding = PaddingMode.Zeros;
                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        // encrypted bytes from the memory stream.
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < encrypted.Length; i++)
            {
                builder.Append(encrypted[i].ToString("x2"));
            }
            return builder.ToString();
        }

        /// <summary>
        /// Decrypting Method
        /// </summary>
        /// <param name="encryptedText"></param>
        /// <param name="strKey"></param>
        /// <param name="strIV"></param>
        /// <returns></returns>
        public static string DecryptRijndael(string encryptedText, string strKey, string strIV)
        {
            // Check arguments.
            if (encryptedText == null || encryptedText.Length <= 0)
                throw new ArgumentNullException("encryptedText");
            if (strKey == null || strKey.Length <= 0)
                throw new ArgumentNullException("strKey");
            if (strIV == null || strIV.Length <= 0)
                throw new ArgumentNullException("strIV");

            byte[] cipherText = StringToByteArray(encryptedText);
            byte[] cipherKey = ASCIIEncoding.ASCII.GetBytes(strKey);
            byte[] cipherIV = ASCIIEncoding.ASCII.GetBytes(strIV);

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = cipherKey;
                rijAlg.IV = cipherIV;
                rijAlg.Padding = PaddingMode.Zeros;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(plaintext))
                return plaintext.TrimEnd('\0');
            else
                return string.Empty;
        }

        #endregion Encrypt Decrypt Rijndael

        #region Encrypt Decrypt

        /// <summary>
        ///
        /// </summary>
        /// <param name="plainString"></param>
        /// <returns></returns>
        public static string EncryptString(string plainString)
        {
            string encryptedString = string.Empty;
            // Convert strings into byte arrays.
            // Let us assume that strings only contain ASCII codes.
            // If strings include Unicode characters, use Unicode, UTF7, or UTF8
            // encoding.
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

            // Convert our plaintext into a byte array.
            // Let us assume that plaintext contains UTF8-encoded characters.
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainString);

            // First, we must create a password, from which the key will be derived.
            // This password will be generated from the specified passphrase and
            // salt value. The password will be created using the specified hash
            // algorithm. Password creation can be done in several iterations.
            PasswordDeriveBytes password = new PasswordDeriveBytes(
                                                            passPhrase,
                                                            saltValueBytes,
                                                            hashAlgorithm,
                                                            passwordIterations);

            // Use the password to generate pseudo-random bytes for the encryption
            // key. Specify the size of the key in bytes (instead of bits).
            byte[] keyBytes = password.GetBytes(keySize / 8);

            // Create uninitialized Rijndael encryption object.
            RijndaelManaged symmetricKey = new RijndaelManaged();

            // It is reasonable to set encryption mode to Cipher Block Chaining
            // (CBC). Use default options for other symmetric key parameters.
            symmetricKey.Mode = CipherMode.CBC;
            //symmetricKey.Padding = PaddingMode.PKCS7;
            // Generate encryptor from the existing key bytes and initialization
            // vector. Key size will be defined based on the number of the key
            // bytes.
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(
                                                             keyBytes,
                                                             initVectorBytes);

            // Define memory stream which will be used to hold encrypted data.
            MemoryStream memoryStream = new MemoryStream();

            // Define cryptographic stream (always use Write mode for encryption).
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                         encryptor,
                                                         CryptoStreamMode.Write);
            // Start encrypting.
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

            // Finish encrypting.
            cryptoStream.FlushFinalBlock();

            // Convert our encrypted data from a memory stream into a byte array.
            byte[] cipherTextBytes = memoryStream.ToArray();

            // Close both streams.
            memoryStream.Close();
            cryptoStream.Close();

            // Convert encrypted data into a base64-encoded string.
            encryptedString = Convert.ToBase64String(cipherTextBytes);

            return encryptedString;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="encryptedString"></param>
        /// <returns></returns>
        public static string DecryptString(string encryptedString)
        {

            string decryptedString = string.Empty;
            try
            {
                // Convert strings defining encryption key characteristics into byte
                // arrays. Let us assume that strings only contain ASCII codes.
                // If strings include Unicode characters, use Unicode, UTF7, or UTF8
                // encoding.
                byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
                byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

                // Convert our ciphertext into a byte array.
                byte[] cipherTextBytes = Convert.FromBase64String(encryptedString);

                // First, we must create a password, from which the key will be
                // derived. This password will be generated from the specified
                // passphrase and salt value. The password will be created using
                // the specified hash algorithm. Password creation can be done in
                // several iterations.
                PasswordDeriveBytes password = new PasswordDeriveBytes(
                                                                passPhrase,
                                                                saltValueBytes,
                                                                hashAlgorithm,
                                                                passwordIterations);

                // Use the password to generate pseudo-random bytes for the encryption
                // key. Specify the size of the key in bytes (instead of bits).
                byte[] keyBytes = password.GetBytes(keySize / 8);

                // Create uninitialized Rijndael encryption object.
                RijndaelManaged symmetricKey = new RijndaelManaged();

                // It is reasonable to set encryption mode to Cipher Block Chaining
                // (CBC). Use default options for other symmetric key parameters.
                symmetricKey.Mode = CipherMode.CBC;
                // symmetricKey.Padding = PaddingMode.PKCS7;
                // Generate decryptor from the existing key bytes and initialization
                // vector. Key size will be defined based on the number of the key
                // bytes.
                ICryptoTransform decryptor = symmetricKey.CreateDecryptor(
                                                                 keyBytes,
                                                                 initVectorBytes);

                // Define memory stream which will be used to hold encrypted data.
                MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

                // Define cryptographic stream (always use Read mode for encryption).
                CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                              decryptor,
                                                              CryptoStreamMode.Read);

                // Since at this point we don't know what the size of decrypted data
                // will be, allocate the buffer long enough to hold ciphertext;
                // plaintext is never longer than ciphertext.
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];

                // Start decrypting.
                int decryptedByteCount = cryptoStream.Read(plainTextBytes,
                                                           0,
                                                           plainTextBytes.Length);

                // Close both streams.
                memoryStream.Close();
                cryptoStream.Close();

                // Convert decrypted data into a string.
                // Let us assume that the original plaintext string was UTF8-encoded.
                decryptedString = Encoding.UTF8.GetString(plainTextBytes,
                                                           0,
                                                           decryptedByteCount);

                // Return decrypted string.
            }
            catch
            {
                return "";
            }
            return decryptedString;
        }

        #endregion Encrypt Decrypt

        /// <summary>
        /// Use For Encryp password using SALTKey
        /// </summary>
        /// <param name="PlainPassword"></param>
        /// <param name="Salts"></param>
        /// <returns></returns>
        public static string OneWayEncryptPassword(string PlainPassword, string saltKey)
        {
            string raw = PlainPassword + saltKey;
            byte[] bytes = System.Text.Encoding.Unicode.GetBytes(PlainPassword + saltKey);
            var sha1 = SHA1.Create();
            byte[] hashBytes = sha1.ComputeHash(bytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }

        public static string CreateMD5Hash(string input, bool isLowerHash = false)
        {
            // Step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                if (isLowerHash)
                    sb.Append(hashBytes[i].ToString("x2"));
                else
                    sb.Append(hashBytes[i].ToString("X2"));
            }

            md5.Dispose();
            return sb.ToString();
        }

        /// <summary>
        /// Convert string to Byte
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static byte[] StringToByteArray(String hex)
        {
            try
            {
                int NumberChars = hex.Length;
                byte[] bytes = new byte[NumberChars / 2];
                for (int i = 0; i < NumberChars; i += 2)
                    bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
                return bytes;
            }
            catch { return null; }
        }
    }
}
