using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace com.fabioscagliola.Core.Data
{
    /// <summary>
    /// The method of the HTTP request issues by the <see cref="Util.GetResponse"/> method
    /// </summary>
    public enum Method
    {
        GET,
        POST,
    }

    /// <summary>
    /// Contains various utility methods 
    /// </summary>
    public static class Util
    {
        private static object locker = new object();

        /// <summary>
        /// Compresses an array of bytes 
        /// </summary>
        /// <param name="bytes">The array of bytes to be compressed</param>
        public static byte[] Compress(byte[] bytes)
        {
            byte[] value = null;

            MemoryStream memoryStream = null;
            GZipStream gZipStream = null;

            try
            {
                memoryStream = new MemoryStream();
                gZipStream = new GZipStream(memoryStream, CompressionMode.Compress);
                gZipStream.Write(bytes, 0, bytes.Length);
                value = memoryStream.ToArray();
            }
            finally
            {
                if (memoryStream != null)
                {
                    memoryStream.Close();
                    memoryStream.Dispose();
                }
                //if (gZipStream != null)
                //{
                //    gZipStream.Close();
                //    gZipStream.Dispose();
                //}
            }

            return value;
        }

        /// <summary>
        /// Decompresses an array of bytes 
        /// </summary>
        /// <param name="bytes">The array of bytes to be decompressed</param>
        public static byte[] Decompress(byte[] bytes)
        {
            byte[] value = null;

            MemoryStream memoryStream = null;
            GZipStream gZipStream = null;

            try
            {
                memoryStream = new MemoryStream();
                gZipStream = new GZipStream(new MemoryStream(bytes), CompressionMode.Decompress);
                byte[] b = new byte[4096];
                while (true)
                {
                    int n = gZipStream.Read(b, 0, b.Length);
                    if (n > 0)
                    {
                        memoryStream.Write(b, 0, n);
                    }
                    else
                    {
                        break;
                    }
                }
                value = memoryStream.ToArray();
            }
            finally
            {
                if (memoryStream != null)
                {
                    memoryStream.Close();
                    memoryStream.Dispose();
                }
                //if (gZipStream != null)
                //{
                //    gZipStream.Close();
                //    gZipStream.Dispose();
                //}
            }

            return value;
        }


        /// <summary>
        /// De-serializes an object from its XML representation 
        /// </summary>
        /// <param name="s">The XML representation of the object</param>
        /// <param name="t">The type of the object</param>
        public static object Deserialize(string s, Type t)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(t);
            StringReader stringReader = new StringReader(s);
            return xmlSerializer.Deserialize(stringReader);
        }

        /// <summary>
        /// Serializes an object to its XML representation 
        /// </summary>
        /// <param name="o">The object to be serialized</param>
        public static string Serialize(object o)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(o.GetType());
            StringBuilder stringBuilder = new StringBuilder();
            xmlSerializer.Serialize(new StringWriter(stringBuilder), o);
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Serializes an object to its XML representation using the specified encoding 
        /// </summary>
        /// <param name="o">The object to be serialized</param>
        /// <param name="encoding">The encoding to be used to serialize the object</param>
        public static string Serialize(object o, Encoding encoding)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(o.GetType());

            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Encoding = encoding;
            xmlWriterSettings.Indent = true;

            using (StringWriterWithEncoding stringWriter = new StringWriterWithEncoding(Encoding.UTF8))
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings))
                {
                    XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
                    xmlSerializerNamespaces.Add("", "");
                    xmlSerializer.Serialize(xmlWriter, o, xmlSerializerNamespaces);
                }

                return stringWriter.ToString();
            }
        }

        class StringWriterWithEncoding : StringWriter
        {
            protected Encoding encoding;

            public StringWriterWithEncoding(Encoding encoding)
            {
                this.encoding = encoding;
            }

            public override Encoding Encoding => encoding;

        }


        /// <summary>
        /// Returns a unique identifier based on the current date and time 
        /// </summary>
        public static string DoUniqueIdentifier()
        {
            lock (locker)
            {
                return DateTime.UtcNow.Ticks.ToString("x");
            }
        }


        /// <summary>
        /// De-serializes a struct from its binary representation 
        /// </summary>
        /// <typeparam name="T">The type of the struct</typeparam>
        /// <param name="bytes">The binary representation of the struct</param>
        public static T DeserializeStruct<T>(byte[] bytes)
        {
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            T o = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return o;
        }

        /// <summary>
        /// Serializes a struct to its binary representation 
        /// </summary>
        /// <param name="o">The struct to be serialized</param>
        public static byte[] SerializeStruct(object o)
        {
            int size = Marshal.SizeOf(o);
            byte[] data = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(o, ptr, true);
            Marshal.Copy(ptr, data, 0, size);
            Marshal.FreeHGlobal(ptr);
            return data;
        }


        /// <summary>
        /// Decodes an array of bytes from a Base64 string 
        /// </summary>
        /// <param name="s">The source base-64 string</param>
        public static byte[] FromBase64String(string s)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(s);
            while (sb.Length % 4 != 0)
                sb.Append('=');
            return Convert.FromBase64String(sb.ToString());
        }

        /// <summary>
        /// Encodes an array of bytes to a Base64 string 
        /// </summary>
        /// <param name="bytes">The source array of bytes</param>
        public static string ToBase64String(byte[] bytes)
        {
            return Convert.ToBase64String(bytes).Trim('=');
        }


        /// <summary>
        /// Formats the specified time value (in minutes) as a "hhh:mm" string - example: 123456.789 becomes "2,057:37" (invariant culture) 
        /// </summary>
        /// <param name="time">The time in minutes</param>
        public static string TimeToString(double time)
        {
            return TimeToString(time, Thread.CurrentThread.CurrentCulture);
        }

        /// <summary>
        /// Formats the specified time value (in minutes) as a "hhh:mm" string - example: 123456.789 becomes "2,057:37" 
        /// </summary>
        /// <param name="time">The time in minutes</param>
        /// <param name="provider"></param>
        public static string TimeToString(double time, IFormatProvider provider)
        {
            if (double.IsInfinity(time) || double.IsNaN(time))
                return time.ToString();

            int hours = (int)Math.Floor(time / 60);
            int minutes = (int)Math.Round(time - hours * 60);
            return string.Format(provider, "{0:N0}:{1:00}", hours, minutes);
        }

        /// <summary>
        /// Parses the specified "hhh:mm" string into a time value (in minutes) - example: "2,057:37" (invariant culture) becomes 123457.0 
        /// </summary>
        /// <param name="time">The time as a "hhh:mm" string</param>
        public static double StringToTime(string time)
        {
            return StringToTime(time, Thread.CurrentThread.CurrentCulture);
        }

        /// <summary>
        /// Parses the specified "hhh:mm" string into a time value (in minutes) - example: "2,057:37" becomes 123457.0 
        /// </summary>
        /// <param name="time">The time as a "hhh:mm" string</param>
        /// <param name="provider"></param>
        public static double StringToTime(string time, IFormatProvider provider)
        {
            int hours = 0;
            int minutes = 0;
            try
            {
                string[] splits = time.Split(':');
                hours = int.Parse(splits[0], NumberStyles.AllowThousands, provider);
                minutes = int.Parse(splits[1], NumberStyles.AllowThousands, provider);
            }
            catch
            {
                // Prevent parsing exceptions 
            }
            return hours * 60 + minutes;
        }


        /// <summary>
        /// Issues an HTTP request 
        /// </summary>
        /// <param name="requestUri">The target URL of the request</param>
        /// <param name="method">The method of the request</param>
        /// <param name="cookieCollection">The cookies to be sent with the request (may be an empty collection)</param>
        /// <param name="credentials">The credentials to be used for authentication (may be null)</param>
        /// <param name="requestContent">The content of the request (may be null, ignored if <paramref name="method"/> is <see cref="Method.GET"/>)</param>
        /// <param name="requestContentType">The MIME type of the content of the request</param>
        /// <param name="timeout">The time-out value in milliseconds</param>
        public static HttpWebResponse GetResponse(Uri requestUri, Method method, ref CookieCollection cookieCollection, ICredentials credentials, string requestContent, string requestContentType = "application/x-www-form-urlencoded", int? timeout = null)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
            ServicePointManager.ServerCertificateValidationCallback += (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => { return true; };

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUri);
            httpWebRequest.CookieContainer = new CookieContainer();
            httpWebRequest.Method = Enum.GetName(typeof(Method), method);

            if (timeout.HasValue)
            {
                httpWebRequest.Timeout = timeout.Value;
            }

            if (cookieCollection != null)
                foreach (Cookie cookie in cookieCollection)
                    httpWebRequest.CookieContainer.SetCookies(httpWebRequest.RequestUri, cookie.ToString());

            if (credentials != null)
                httpWebRequest.Credentials = credentials;

            if (method == Method.POST && !string.IsNullOrEmpty(requestContent))
            {
                httpWebRequest.ContentLength = requestContent.Length;
                // TODO: if (string.IsNullOrEmpty(requestContentType))
                httpWebRequest.ContentType = requestContentType;
                StreamWriter streamWriter = null;
                try
                {
                    streamWriter = new StreamWriter(httpWebRequest.GetRequestStream(), Encoding.Default);
                    //streamWriter.Write(Encoding.Default.GetBytes(requestContent));
                    streamWriter.Write(requestContent);
                }
                finally
                {
                    if (streamWriter != null)
                    {
                        streamWriter.Close();
                        streamWriter.Dispose();
                    }
                }
            }

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            cookieCollection = httpWebRequest.CookieContainer.GetCookies(httpWebRequest.RequestUri);

            return httpWebResponse;
        }

        /// <summary>
        /// Returns the content of an HTTP response as an array of bytes 
        /// </summary>
        /// <param name="httpWebResponse">The response</param>
        public static byte[] ReadBinaryResponse(HttpWebResponse httpWebResponse)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                Stream stream = httpWebResponse.GetResponseStream();
                byte[] b = new byte[4096];
                while (true)
                {
                    int n = stream.Read(b, 0, b.Length);
                    if (n > 0)
                    {
                        memoryStream.Write(b, 0, n);
                    }
                    else
                    {
                        break;
                    }
                }
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Returns the content of an HTTP response as string 
        /// </summary>
        /// <param name="httpWebResponse">The response</param>
        public static string ReadResponse(HttpWebResponse httpWebResponse)
        {
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.UTF8);
            string response = streamReader.ReadToEnd();
            streamReader.Close();
            return response;
        }


        /// <summary>
        /// Safely returns the value of the first child of an XML node 
        /// </summary>
        /// <param name="xmlNode">The XML node</param>
        public static string GetXmlNodeFirstChildValue(XmlNode xmlNode)
        {
            string value = null;
            if (xmlNode != null && xmlNode.FirstChild != null)
            {
                value = xmlNode.FirstChild.Value;
            }
            return value;
        }

    }
}

