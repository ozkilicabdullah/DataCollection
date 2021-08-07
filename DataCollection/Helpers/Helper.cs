using FluentValidation;
using FluentValidation.Results;
using Hangfire;
using DataCollection.Extensions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using System.IO.Compression;
using DataCollection.Contracts;

namespace DataCollection.Helpers
{
    public static class Helper
    {

        public static object DictionaryToObject(Type type, IDictionary<string, object> dict)
        {
            var t = Activator.CreateInstance(type);

            PropertyInfo[] properties = t.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance /*| BindingFlags.DeclaredOnly*/);

            foreach (PropertyInfo property in properties)
            {
                if (!dict.Any(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase)))
                    continue;

                KeyValuePair<string, object> item = dict.First(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase));

                // Find which property type (int, string, double? , enum etc) the CURRENT property is...
                Type tPropertyType = t.GetType().GetProperty(property.Name).PropertyType;
                if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    if (item.Value == null) continue;
                    var genericType = property.PropertyType.GetGenericArguments()[0];

                    if (genericType.IsEnum)
                        property.SetValue(t, Enum.Parse(property.PropertyType.GetGenericArguments()[0], (string)item.Value), null);
                    else
                        property.SetValue(t, Convert.ChangeType(item.Value, genericType), null);
                }
                else if (item.Value != null && item.Value.GetType() != typeof(JObject) && item.Value.GetType() != typeof(JArray))
                {
                    property.SetValue(t, Convert.ChangeType(item.Value, property.PropertyType), null);
                }
            }
            return t;
        }

        public static void PrepareObject(Type type, object instance)
        {
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var oo = instance.GetPropValue(property.Name);
                if (oo == null)
                {
                    if (property.PropertyType.Module.ScopeName.ToLower() == "DataCollection.dll" ||
                        property.PropertyType.GetInterfaces().Any(x => x.Name == "IList"))
                    {
                        oo = Activator.CreateInstance(property.PropertyType);

                        property.SetValue(instance, oo);
                        if (!oo.GetType().GetInterfaces().Any(x => x.Name == "IList"))
                            PrepareObject(oo.GetType(), oo);
                    }
                }
            }
        }

        public static string ToUrlSlug(string value)
        {

            //First to lower case 
            value = value.ToLowerInvariant();

            //Remove all accents
            var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(value);

            value = Encoding.ASCII.GetString(bytes);

            //Replace spaces 
            value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);

            //Remove invalid chars 
            value = Regex.Replace(value, @"[^\w\s\p{Pd}]", "", RegexOptions.Compiled);

            //Trim dashes from end 
            value = value.Trim('-', '_');

            //Replace double occurences of - or \_ 
            value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);

            return value;
        }

        public static byte[] ReadFully(string input)
        {
            using (FileStream sourceFile = new FileStream(input, FileMode.Open))
            {
                BinaryReader binReader = new BinaryReader(sourceFile);
                byte[] output = new byte[sourceFile.Length]; //create byte array of size file
                for (long i = 0; i < sourceFile.Length; i++)
                    output[i] = binReader.ReadByte(); //read until done

                sourceFile.Close(); //dispose streamer
                binReader.Close(); //dispose reader

                return output;
            }
        }

        public static T[] InitializeArray<T>(int length) where T : new()
        {
            T[] array = new T[length];
            for (int i = 0; i < length; ++i)
            {
                array[i] = new T();
            }

            return array;
        }

        public static string ComputeSha512Hash(string rawData)
        {
            // Create a SHA512   
            using (SHA512 sha512Hash = SHA512.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha512Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                    builder.Append(bytes[i].ToString("x2"));

                return builder.ToString();
            }
        }

        public static void SendEmail(Action<MailMessage> MailMessage, string subject, string body)
        {

            MailMessage mm = new MailMessage()
            {
                Subject = subject,
                Body = body
            };

            MailMessage(mm);

            mm.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.yandex.com.tr",
                EnableSsl = true
            };

            NetworkCredential NetworkCred = new NetworkCredential
            {
                UserName = "eposta@haso.com.tr",
                Password = "haso1234!"
            };

            smtp.UseDefaultCredentials = true;
            smtp.Credentials = NetworkCred;
            smtp.Port = 587;
            smtp.Send(mm);

        }

        public static string Serialize<T>(this T value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            try
            {
                var xns = new XmlSerializerNamespaces();
                xns.Add(string.Empty, string.Empty);

                var xmlserializer = new XmlSerializer(typeof(T));
                var stringWriter = new StringWriter();
                using (var writer = XmlWriter.Create(stringWriter, new XmlWriterSettings() { OmitXmlDeclaration = true, Encoding = Encoding.UTF8, Indent = true }))
                {
                    xmlserializer.Serialize(writer, value, xns);
                    return stringWriter.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred", ex);
            }
        }

        /// <summary>
        /// 1.26858E+7 gibi Üstel Gösterim olan bilimsel bir sayı gelirse onu da çevirelim 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal ToDecimal(string value)
        {
            if (string.IsNullOrEmpty(value))
                return 0;

            return (value.Contains("E") || value.Contains("e")) ?
                decimal.Parse(value.Replace('.', ','), NumberStyles.Any) : Convert.ToDecimal(value.Replace('.', ','));
        }

        public static bool IsNullOrEmpty(string value)
        {
            if (!string.IsNullOrEmpty(value))
                value = value.Replace("NULL", "").Replace("null", "");

            return string.IsNullOrEmpty(value);
        }

        public static int Length(string value)
        {
            if (!IsNullOrEmpty(value))
                return value.Length;

            return 0;
        }

        public static decimal Coalesce(string value, decimal _value)
        {
            var _v = ToDecimal(value);

            return _v == 0 ? _value : _v;
        }

        public static decimal Coalesce(decimal value, decimal _value)
        {
            if (value <= decimal.MinValue || value <= 0)
                return _value;

            return value;
        }

        public static bool BetweenNumber(decimal value, decimal from, decimal to)
        {
            return value >= from && value < to;
        }

        public static bool BetweenNumber(int value, int from, int to)
        {
            return value >= from && value < to;
        }

        public static T Deserialize<T>(string xml, string XmlRoot)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(XmlRoot));
            StringReader stringReader = new StringReader(xml);

            T Entity = (T)serializer.Deserialize(stringReader);

            PrepareObject(Entity.GetType(), Entity); //Create null 

            return Entity;
        }

        public static WebProxy GetProxy(IConfiguration Configuration)
        {
            if (!string.IsNullOrEmpty(Configuration.GetValue<string>("Proxy:Address")) &&
                !string.IsNullOrEmpty(Configuration.GetValue<string>("Proxy:Port")))
            {

                return new WebProxy(Configuration.GetValue<string>("Proxy:Address"), Configuration.GetValue<int>("Proxy:Port"));
            }

            return null;
        }

        public static WebProxy GetProxy2(IConfiguration Configuration)
        {
            if (!string.IsNullOrEmpty(Configuration.GetValue<string>("ProxyArenaForm:Address")) &&
                !string.IsNullOrEmpty(Configuration.GetValue<string>("ProxyArenaForm:Port")))
            {

                return new WebProxy(Configuration.GetValue<string>("ProxyArenaForm:Address"), Configuration.GetValue<int>("ProxyArenaForm:Port"));
            }

            return null;
        }

        public static List<string> ValidateModel<V>(this ModelBase model, AbstractValidator<V> validator) where V : ModelBase
        {

            ValidationResult results = validator.Validate<V>((model as V));

            bool success = results.IsValid;
            IList<ValidationFailure> failures = results.Errors;

            List<string> errors = new List<string>();
            foreach (var item in failures)
                errors.Add(item.ErrorMessage);
            //errors.Add(string.Format("{0} => Row: {1}", item.ErrorMessage, item.PropertyName));

            return errors;
        }

        public static bool DateIsMonthOld(string FromDate, decimal month, string _operator, DateTime DateOfAppraisal)
        {
            return DateIsMonthOld(ParseDateTime(FromDate), month, _operator, DateOfAppraisal);
        }

        public static bool DateIsMonthOld(DateTime? FromDate, decimal month, string _operator, DateTime DateOfAppraisal)
        {
            DateTime Today = DateOfAppraisal;
            if (FromDate.HasValue)
            {
                TimeSpan difference = Today - FromDate.Value;
                switch (_operator)
                {
                    case ">": return (Convert.ToDecimal(difference.TotalDays / 365) > (month / 12));
                    case "<": return (Convert.ToDecimal(difference.TotalDays / 365) < (month / 12));
                    case ">=": return (Convert.ToDecimal(difference.TotalDays / 365) >= (month / 12));
                    case "<=": return (Convert.ToDecimal(difference.TotalDays / 365) <= (month / 12));
                }
            }

            return false;
        }

        public static int GetDifferenceInYears(string date, DateTime DateOfAppraisal)
        {
            DateTime? startDate = ParseDateTime(date);
            if (!startDate.HasValue)
                return -1;

            int finalResult = 0;
            const int DaysInYear = 365;
            DateTime endDate = DateOfAppraisal;

            TimeSpan timeSpan = endDate - startDate.Value;
            if (timeSpan.TotalDays > 365)
            {
                finalResult = (int)Math.Round((timeSpan.TotalDays / DaysInYear), MidpointRounding.ToEven);
            }

            return finalResult;
        }

        public static string DateTimeStringTR(DateTime date)
        {
            return date.ToString("dd'/'MM'/'yyyy HH:mm");
        }

        public static string DateTimeStringEN(DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static DateTime? ParseDateTime(string dateToParse, string[] formats = null, IFormatProvider provider = null,
            DateTimeStyles styles = DateTimeStyles.None)
        {
            var CUSTOM_DATE_FORMATS = new string[]
                {
                "yyyyMMddTHHmmssZ",
                "yyyyMMddTHHmmZ",
                "yyyyMMddTHHmmss",
                "yyyyMMddTHHmm",
                "yyyyMMddHHmmss",
                "yyyyMMddHHmm",
                "yyyyMMdd",
                "yyyy-MM-ddTHH-mm-ss",
                "yyyy-MM-dd-HH-mm-ss",
                "yyyy-MM-dd-HH-mm",
                "yyyy-MM-dd",
                "MM-dd-yyyy",
                "mm.dd.yyyy",
                "dd-mm-yyyy HH:mm:ss",
                };

            if (formats == null || !formats.Any())
            {
                formats = CUSTOM_DATE_FORMATS;
            }

            DateTime validDate;

            foreach (var format in formats)
            {
                if (format.EndsWith("Z"))
                {
                    if (DateTime.TryParseExact(dateToParse, format,
                             provider,
                             DateTimeStyles.AssumeUniversal,
                             out validDate))
                    {
                        return validDate;
                    }
                }

                if (DateTime.TryParseExact(dateToParse, format,
                         provider, styles, out validDate))
                {
                    return validDate;
                }
            }

            return null;
        }

        public static bool ExpireHours(DateTime date, int hours)
        {
            DateTime now = DateTime.Now;
            return (date > now.AddHours(-hours) && date <= now);
        }

        public static string RemainedTime(DateTime date)
        {
            TimeSpan time_span = date.AddHours(24) - DateTime.Now;
            string time = string.Format("{0:00} Saat {1:00} Dakika", time_span.Hours, time_span.Minutes);
            return time;
        }

        public static bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }

        public static Dictionary<TKey, TValue> ConcatDictionary<TKey, TValue>(Dictionary<TKey, TValue> List1, Dictionary<TKey, TValue> List2)
        {
            return List1.Concat(List2.Where(kvp => !List2.ContainsKey(kvp.Key)))
                                .OrderBy(c => c.Value)
                                .ToDictionary(c => c.Key, c => c.Value);
        }

        public static Dictionary<string, object> ProcessStatus(string method, string identifer)
        {

            var result = new Dictionary<string, object>
            {
                { "queueID", identifer },
                { "method" , method},
                { "status", "None" }
            };

            var ProcessMethod = JobStorage.Current.GetMonitoringApi().ProcessingJobs(0, 1000).FindAll(x =>
            {
                if (x.Value.Job != null)
                {

                    result["method"] = x.Value.Job.Method.Name;

                    if (!string.IsNullOrEmpty(method))
                    {
                        if (x.Value.Job.Method.Name.Contains(method))
                            return (!string.IsNullOrEmpty(identifer) ? x.Value.Job.Args.Contains(identifer) : true);
                    }
                    else
                    {
                        return (!string.IsNullOrEmpty(identifer) ? x.Value.Job.Args.Contains(identifer) : true);
                    }
                }
                return false;
            });

            if (ProcessMethod.Count <= 0)
            {
                if (!string.IsNullOrEmpty(identifer))
                {
                    var FailedMethod = JobStorage.Current.GetMonitoringApi().FailedJobs(0, 1000).FindAll(x =>
                    {
                        if (x.Value.Job != null)
                        {
                            if (!string.IsNullOrEmpty(method))
                            {
                                if (x.Value.Job.Method.Name.Contains(method))
                                    return (!string.IsNullOrEmpty(identifer) ? x.Value.Job.Args.Contains(identifer) : true);
                            }
                            else
                            {
                                return (!string.IsNullOrEmpty(identifer) ? x.Value.Job.Args.Contains(identifer) : true);
                            }
                        }
                        return false;
                    });

                    if (FailedMethod.Count > 0)
                    {
                        result["method"] = FailedMethod[0].Value.Job.Method.Name;
                        result["status"] = Enum.GetName(typeof(Enums.ProcessStatus), Enums.ProcessStatus.Failed);
                        result.Add("data", FailedMethod[0].Value.Job.Args[0]);
                        result.Add("error", FailedMethod[0].Value.ExceptionMessage);
                        result.Add("failedAt", FailedMethod[0].Value.FailedAt);
                        return result;
                    }

                    var SucceededMethod = JobStorage.Current.GetMonitoringApi().SucceededJobs(0, 1000).FindAll(x =>
                    {
                        if (x.Value.Job != null)
                        {
                            if (!string.IsNullOrEmpty(method))
                            {
                                if (x.Value.Job.Method.Name.Contains(method))
                                    return (!string.IsNullOrEmpty(identifer) ? x.Value.Job.Args.Contains(identifer) : true);
                            }
                            else
                            {
                                return (!string.IsNullOrEmpty(identifer) ? x.Value.Job.Args.Contains(identifer) : true);
                            }
                        }
                        return false;
                    });

                    if (SucceededMethod.Count > 0)
                    {
                        result["method"] = SucceededMethod[0].Value.Job.Method.Name;
                        result["result"] = SucceededMethod[0].Value.Result;
                        result["status"] = Enum.GetName(typeof(Enums.ProcessStatus), Enums.ProcessStatus.Succeeded);
                        result.Add("data", SucceededMethod[0].Value.Job.Args[0]);
                        result.Add("totalDuration", string.Format("{0}{1}", SucceededMethod[0].Value.TotalDuration, "ms"));
                        result.Add("succeededAt", SucceededMethod[0].Value.SucceededAt.Value.ToString("dd.MM.yyyy hh:mm:ss"));

                        return result;
                    }
                }
            }
            else
            {

                result["method"] = ProcessMethod[0].Value.Job.Method.Name;
                result["queueID"] = ProcessMethod[0].Value.Job.Args[1].ToString();
                result["count"] = ProcessMethod.Count;
                result.Add("startedAt", ProcessMethod[0].Value.StartedAt.Value.ToString("dd.MM.yyyy hh:mm:ss"));
                result["status"] = Enum.GetName(typeof(Enums.ProcessStatus), Enums.ProcessStatus.Processing);
                result.Add("data", ProcessMethod[0].Value.Job.Args[0]);
            }

            return result;
        }

        public static string GetQueryString(object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

            return String.Join("&", properties.ToArray());
        }

        public static void CombineMultipleFilesIntoSingleFile(string inputDirectoryPath, string inputFileNamePattern, string outputFilePath)
        {
            string[] inputFilePaths = Directory.GetFiles(inputDirectoryPath, inputFileNamePattern);
            using (var outputStream = File.Create(outputFilePath))
            {
                foreach (var inputFilePath in inputFilePaths)
                {
                    using (var inputStream = File.OpenRead(inputFilePath))
                    {
                        inputStream.CopyTo(outputStream);
                    }
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TRequest">Gönderilecek olan JSON nesnesi</typeparam>
        /// <typeparam name="TResponse">Dönüş Nesnesi tür parametresi</typeparam>
        /// <param name="url">İstek Adresi</param>
        /// <param name="parameter">Request nesnesi</param>
        /// <param name="proxy">Kullanılacaksa proxy nesnesi</param>
        /// <param name="predicate">Web Request nesnesi (Headers v.s özellikleri düzenlemek için) </param>
        /// <returns></returns>
        public static TResponse SendWebRequest<TRequest, TResponse>(string url, TRequest parameter,
            Action<WebRequest> predicate = null,
            WebProxy proxy = null)
        {
            // Create the web request
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            if (proxy != null) webRequest.Proxy = proxy;

            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";

            predicate(webRequest);

            if (parameter != null && webRequest.Method == "POST")
            {
                using (var streamWriter = webRequest.GetRequestStream())
                {
                    string postData = string.Format("data={0}", JsonConvert.SerializeObject(parameter));
                    byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                    webRequest.ContentLength = byteArray.Length;

                    streamWriter.Write(byteArray, 0, byteArray.Length);
                }
            }

            string result;
            using (WebResponse response = webRequest.GetResponse())
            {
                using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                {
                    result = rd.ReadToEnd();
                }
            }

            return JsonConvert.DeserializeObject<TResponse>(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TRequest">Gönderilecek olan JSON nesnesi</typeparam>
        /// <typeparam name="TResponse">Dönüş Nesnesi tür parametresi</typeparam>
        /// <param name="url">İstek Adresi</param>
        /// <param name="parameter">Request nesnesi</param>
        /// <param name="proxy">Kullanılacaksa proxy nesnesi</param>
        /// <param name="predicate">Web Request nesnesi (Headers v.s özellikleri düzenlemek için) </param>
        /// <returns></returns>
        public static TResponse SendWebRequestJson<TRequest, TResponse>(string url, TRequest parameter,
            Action<WebRequest> predicate = null,
            WebProxy proxy = null)
        {
            // Create the web request
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            if (proxy != null) webRequest.Proxy = proxy;

            webRequest.ContentType = "application/json;charset=\"utf-8\"";
            webRequest.Accept = "application/json";
            webRequest.Method = "POST";

            predicate(webRequest);

            using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
            {
                streamWriter.Write(parameter);
            }

            string result;
            using (WebResponse response = webRequest.GetResponse())
            {
                using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                {
                    result = rd.ReadToEnd();
                }
            }

            return JsonConvert.DeserializeObject<TResponse>(result);
        }

        public static void Decompress(FileInfo fileToDecompress)
        {
            using (FileStream originalFileStream = fileToDecompress.OpenRead())
            {
                string currentFileName = fileToDecompress.FullName;
                string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

                using (FileStream decompressedFileStream = File.Create(newFileName))
                {
                    using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                        decompressionStream.CopyTo(decompressedFileStream);
                }
            }
        }

        private static void CreateHeader<T>(List<T> list, StreamWriter sw)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            for (int i = 0; i < properties.Length - 1; i++)
            {
                sw.Write(properties[i].Name + ",");
            }
            var lastProp = properties[properties.Length - 1].Name;
            sw.Write(lastProp + sw.NewLine);
        }

        private static void CreateRows<T>(List<T> list, StreamWriter sw)
        {
            foreach (var item in list)
            {
                PropertyInfo[] properties = typeof(T).GetProperties();
                for (int i = 0; i < properties.Length - 1; i++)
                {
                    var prop = properties[i];
                    sw.Write(prop.GetValue(item) + ",");
                }
                var lastProp = properties[properties.Length - 1];
                sw.Write(lastProp.GetValue(item) + sw.NewLine);
            }
        }

        public static void CreateCSV<T>(List<T> list, string filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                CreateHeader(list, sw);
                CreateRows(list, sw);
            }
        }

    }
}

