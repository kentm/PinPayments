using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using PinPayments.Models;
using System.IO;

namespace PinPayments
{
    public static class Requestor
    {
        public static string GetString(string url)
        {
            var wr = GetWebRequest(url, "GET","");

            return ExecuteWebRequest(wr);
        }

        public static string PostString(string url, string parameters)
        {
            var wr = GetWebRequest(url, "POST", parameters);

            return ExecuteWebRequest(wr);
        }

        public static string PutString(string url, string parameters)
        {
            var wr = GetWebRequest(url, "PUT", parameters);

            return ExecuteWebRequest(wr);
        }

        public static string Delete(string url, string apiKey = null)
        {
            var wr = GetWebRequest(url, "DELETE", apiKey);

            return ExecuteWebRequest(wr);
        }

        private static WebRequest GetWebRequest(string url, string method, string postData)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var request = HttpWebRequest.Create(url) as HttpWebRequest;

            request.Method = method;
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = "C# API Wrapper v001 - otron.com Fork";

            string apiKey = PinPaymentsConfig.GetApiKey();
            request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(apiKey + ":"));

            if (!string.IsNullOrEmpty(postData))
            {
                var paramBytes = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = paramBytes.Length;
              
                var requestStream = request.GetRequestStream();
                requestStream.Write(paramBytes, 0, paramBytes.Length);
            }
            return request;
        }

        private static string ExecuteWebRequest(WebRequest webRequest)
        {
            try
            {
                using (var response = (HttpWebResponse)webRequest.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK || 
                        response.StatusCode == HttpStatusCode.NoContent || 
                        response.StatusCode == HttpStatusCode.Created || 
                        response.StatusCode == HttpStatusCode.Accepted ||
                        response.StatusCode == HttpStatusCode.NonAuthoritativeInformation ||
                        response.StatusCode == HttpStatusCode.ResetContent ||
                        response.StatusCode == HttpStatusCode.PartialContent)
                        return ReadStream(response.GetResponseStream());
                    else
                        throw (new InvalidOperationException(response.StatusCode.ToString()));
                }
            }
            catch(InvalidOperationException exception)
            {
                throw new WebException(exception.Message);
            }
            /*
                if (webException.Response != null)
                {
                    var statusCode = ((HttpWebResponse)webException.Response).StatusCode;

                    var pinError = new PinError();

                    pinError = Mapper<PinError>.MapFromJson(ReadStream(webException.Response.GetResponseStream()));


                    throw new PinException(statusCode, pinError, webException.Message);
                }

                throw;
            }
             * */
        }

        private static string ReadStream(Stream stream)
        {
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
