using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Linq;

namespace DataCollection.Helpers
{

    /// <summary>
    /// Helper class to send custom SOAP requests.
    /// </summary>
    public static class SOAPHelper
    {
        /// <summary>
        /// Sends a custom sync SOAP request to given URL and receive a request
        /// </summary>
        /// <param name="TResponse">The WebService Response type </param>
        /// <param name="TRequest">The WebService TRequest type </param>
        /// <param name="url">The WebService endpoint URL</param>
        /// <param name="proxy">The WebService via Proxy? </param>
        /// <param name="token">The WebService Bearer access token</param>
        /// <param name="action">The WebService action name</param>
        /// <param name="parameters">A dictionary containing the parameters in a key-value fashion</param>
        /// <param name="soapAction">The SOAPAction value, as specified in the Web Service's WSDL (or NULL to use the url parameter)</param>
        /// <param name="useSOAP12">Set this to TRUE to use the SOAP v1.2 protocol, FALSE to use the SOAP v1.1 (default)</param>
        /// <returns>A string containing the raw Web Service response</returns>
        public static TResponse SendSOAPRequest<TRequest, TResponse>(string url, 
            string action, TRequest parameter, 
            string token, string userName, string Password, string soapAction = null, bool useSOAP12 = false, WebProxy proxy = null)
        {
            // Create the SOAP envelope
            XmlDocument soapEnvelopeXml = new XmlDocument();
            var xmlStr = @"<?xml version=""1.0"" encoding=""utf-8""?> 
                    <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:wsv2=""http://wsv2.ers.kkb.com.tr/"">
                        <soapenv:Header>
                            <wsse:Security xmlns:wsse=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"" soapenv:mustUnderstand=""0"">
                            <wsse:UsernameToken xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"" wsu:Id=""UsernameToken-1"">
                                <wsse:Username>{2}</wsse:Username>
                                <wsse:Password Type=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText"">{3}</wsse:Password>
                          </wsse:UsernameToken>
                         </wsse:Security>
                        </soapenv:Header>
                        <soapenv:Body>
                           <{0}>{1}</{0}>
                        </soapenv:Body>
                    </soapenv:Envelope>";

            var s = string.Format(xmlStr, action, Helper.Serialize(parameter), userName, Password);
            soapEnvelopeXml.LoadXml(s);            

            // Create the web request
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            if (proxy != null) webRequest.Proxy = proxy;
            webRequest.Headers.Add("SOAPAction", soapAction ?? url);
            webRequest.Headers.Add("authorization", token);          

            webRequest.ContentType = (useSOAP12) ? "application/soap+xml;charset=\"utf-8\"" : "text/xml;charset=\"utf-8\"";
            webRequest.Accept = (useSOAP12) ? "application/soap+xml" : "text/xml";
            webRequest.Method = "POST";

            // Insert SOAP envelope
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }

            // Send request and retrieve result
            string result;
            using (WebResponse response = webRequest.GetResponse())
            {
                using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                {
                    result = rd.ReadToEnd();
                }
            }

            return ParseRequest<TResponse>(result);
        }

        private static TResponse ParseRequest<TResponse>(string XmlResponse)
        {
            XmlDocument xc = new XmlDocument();
            xc.LoadXml(XmlResponse);
            XmlElement root = xc.DocumentElement;
            var xml = root.ChildNodes[0].InnerXml;

            XmlNodeList list = xc.GetElementsByTagName("S:Body");
            var aac = list[0].FirstChild.InnerXml;

            XDocument doc = XDocument.Parse(aac);
            string jsonText = JsonConvert.SerializeXNode(doc);

            return JsonConvert.DeserializeObject<TResponse>(jsonText);
        }

    }

}
