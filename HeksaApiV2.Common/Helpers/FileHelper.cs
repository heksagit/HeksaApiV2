using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;

namespace HeksaApiV2.Common.Helpers
{
    public class FileHelper
    {
        /// <summary>
        /// Only If App Pool Set to Enable-32bit Applications
        /// </summary>
        /// <param name="pBC"></param>
        /// <param name="pwzUrl"></param>
        /// <param name="pBuffer"></param>
        /// <param name="cbSize"></param>
        /// <param name="pwzMimeProposed"></param>
        /// <param name="dwMimeFlags"></param>
        /// <param name="ppwzMimeOut"></param>
        /// <param name="dwReserverd"></param>
        /// <returns></returns>
        [DllImport(@"urlmon.dll", CharSet = CharSet.Auto)]
        private static extern System.UInt32 FindMimeFromData(IntPtr pBC,
        [MarshalAs(UnmanagedType.LPStr)] System.String pwzUrl,
        [MarshalAs(UnmanagedType.LPArray)] byte[] pBuffer,
        System.UInt32 cbSize, [MarshalAs(UnmanagedType.LPStr)] System.String pwzMimeProposed,
        System.UInt32 dwMimeFlags,
        out IntPtr ppwzMimeOut,
        System.UInt32 dwReserverd);

        /// <summary>
        /// Check Content File
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetMimeType(IFormFile file)
        {
            string result = string.Empty;
            if (file != null)
            {
                byte[] document = new byte[file.Length];
                file.OpenReadStream().Read(document, 0, (int)file.Length);
                IntPtr mimetype = IntPtr.Zero;
                FindMimeFromData(IntPtr.Zero, null, document, 256, null, 0, out mimetype, 0);
                result = Marshal.PtrToStringUni(mimetype);
                Marshal.FreeCoTaskMem(mimetype);
            }

            return result;
        }

        public static byte[] ConvertToBytes(IFormFile file)
        {
            if (file == null)
                return null;

            var array = new Byte[file.Length];
            file.OpenReadStream().Position = 0;
            file.OpenReadStream().Read(array, 0, (int)file.Length);
            return array;
        }

        public static byte[] ConvertToBytes(Stream file)
        {
            if (file == null)
                return null;

            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = file.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public static byte[] ConvertToBytes(string pathfile)
        {
            if (!File.Exists(pathfile))
                return null;

            FileStream stream = File.OpenRead(pathfile);
            byte[] array = new byte[stream.Length];
            stream.Read(array, 0, array.Length);
            stream.Close();
            return array;
        }

        public static byte[] ConvertToBytesFromBase64String(string strBase64)
        {
            if (string.IsNullOrWhiteSpace(strBase64))
                return null;

            return Convert.FromBase64String(strBase64);
        }

        public static string ConvertFromByteToBase64(byte[] value)
        {
            if (value == null)
                return string.Empty;
            return Convert.ToBase64String(value);
        }

        public static void SaveCompress(IFormFile file, string fullName, long minimunSizeForCompress, long qualityCompress)
        {
            using (Stream fs = file.OpenReadStream())
            {
                Bitmap bmp = new Bitmap(fs);
                ImageCodecInfo picEncod = GetEncoder(ImageFormat.Jpeg);
                Encoder myEncoder = Encoder.Quality;
                EncoderParameters paramsEncods = new EncoderParameters(1);
                paramsEncods.Param[0] = new EncoderParameter(myEncoder, qualityCompress);
                if (fs.Length > minimunSizeForCompress)
                {
                    bmp.Save(fullName, picEncod, paramsEncods);
                }
                else
                {
                    bmp.Save(fullName);
                }
                bmp.Dispose();
            }
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        public static void CheckCreatePath(string folder)
        {
            if (Path.IsPathRooted(folder))
            {
                if (!Directory.Exists(folder))
                {
                    DirectoryInfo di = Directory.CreateDirectory(Path.Combine(folder));
                }
            }
        }

        public static bool CreateFolder(string path)
        {
            bool result = true;
            if (!Directory.Exists(path))
            {
                try
                {
                    if (GrantAccess(path))
                        Directory.CreateDirectory(path);
                    else
                        result = false;
                }
                catch
                {
                    result = false;
                }
            }
            return result;
        }

        public static bool DeleteFolder(string path, bool isRecursive)
        {
            bool result = true;
            if (Directory.Exists(path))
            {
                try
                {
                    if (GrantAccess(path))
                        Directory.Delete(path, isRecursive);
                    else
                        result = false;
                }
                catch
                {
                    result = false;
                }
            }
            return result;
        }

        public static bool GrantAccess(string fullPath)
        {
            DirectoryInfo dInfo = new DirectoryInfo(fullPath);
            DirectorySecurity dSecurity = dInfo.GetAccessControl();
            dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
            dInfo.SetAccessControl(dSecurity);
            return true;
        }

        public static IFormFile StreamToFormFile(Stream inputStream, string fileName, string contentType)
        {
            // Validate parameters
            if (inputStream == null)
                throw new ArgumentNullException(nameof(inputStream));

            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException("File name cannot be null or empty", nameof(fileName));

            if (string.IsNullOrEmpty(contentType))
                throw new ArgumentException("Content type cannot be null or empty", nameof(contentType));

            // Create a memory stream to hold the file data
            var memoryStream = new MemoryStream();
            inputStream.CopyTo(memoryStream);
            memoryStream.Position = 0;

            // Create the IFormFile implementation
            return new FormFile(memoryStream, 0, memoryStream.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };
        }

        public static string ConvertIFormFileToBase64(IFormFile formFile)
        {
            if (formFile == null)
                throw new ArgumentNullException(nameof(formFile));

            // Read the content of the IFormFile into a byte array
            using (var memoryStream = new MemoryStream())
            {
                formFile.CopyTo(memoryStream);
                byte[] fileData = memoryStream.ToArray();

                // Convert the byte array to a Base64 string
                return Convert.ToBase64String(fileData);
            }
        }
    }
}
