using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CSDN.Client.SDK
{
    public class BaseApi
    {        
        protected const string api_host = "http://newapi.csdn.net/";

        protected string access_token = null;
        protected string app_key = null;

        protected string clientIP = null;

        public BaseApi(string accessToken, string appKey)
        {
            access_token = accessToken;
            app_key = appKey;

            var curr = System.Web.HttpContext.Current;
            if (curr != null)
            {
                clientIP = curr.Request.UserHostAddress;
            }
            else
            {
                try
                {
                    SocketHelper socket = new SocketHelper(new Uri("http://iframe.ip138.com/ic.asp"));
                    string html = socket.Get(true);
                    clientIP = new Regex(@"\[([\d.]+?)\]").Match(html).Groups[1].Value;
                }
                catch
                {
                    clientIP = string.Empty;
                }
            }
        }

        protected string Post(string shortUri, string data=null)
        {
            if (data == null)
            {
                data = string.Empty;
            }            
            data += string.Format("&access_token={0}&client_id={1}", access_token, app_key);
            data = data.Trim();

            SocketHelper socket = new SocketHelper(new Uri(api_host + shortUri));

            string result = socket.Post(data, true);

            return result;
        }

        protected string GetValue(string name, string json)
        {
            var reg = new Regex("\"" + name + "\":((?:\".+?\")|(?:[0-9.]+|true|false))(?:[,}]|$)", RegexOptions.IgnoreCase);
            var mat = reg.Match(json);

            return mat.Groups[1].Value.Trim('"');
        }

    }
}
