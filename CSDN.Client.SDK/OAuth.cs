using System;
using System.Collections.Generic;
using System.Text;

namespace CSDN.Client.SDK
{
    public class OAuth
    {
        private const string AUTHORIZE_URL = "http://newapi.csdn.net/oauth2/authorize";
        private const string ACCESS_TOKEN_URL = "http://newapi.csdn.net/oauth2/access_token";

        private string app_key = null;
        private string app_secret = null;

        public OAuth(string appKey, string appSecret)
        {
            app_key = appKey;
            app_secret = appSecret;
        }
        /// <summary>
        /// 获取登录URL（适用于web应用）
        /// </summary>
        /// <param name="redirect_uri">回跳的URL</param>
        /// <returns></returns>
        public string GetAuthorizeUrl(string redirect_uri)
        {
            string data = "client_id={0}&redirect_uri={1}&response_type=code";
            data = string.Format(data, app_key, redirect_uri.UrlEncode());
            return AUTHORIZE_URL + "?" + data;
        }
        /// <summary>
        /// 获取access_token（适用于web应用）
        /// </summary>
        /// <param name="code">登录后返回的code</param>
        /// <param name="redirect_uri">回跳的URL（不回跳，仅用于安全验证）</param>
        /// <returns></returns>
        public AccessToken GetAccessToken(string code, string redirect_uri)
        {
            string data = "client_id={0}&client_secret={1}&grant_type=authorization_code&redirect_uri={2}&code={3}";
            data = string.Format(data, app_key, app_secret, redirect_uri.UrlEncode(), code);
            string uri = ACCESS_TOKEN_URL + "?" + data;

            return GetAccessToken(uri);
        }
        /// <summary>
        /// 获取access_token（适用于客户端应用）
        /// </summary>
        /// <param name="username">用户名/Email</param>
        /// <param name="password">登录密码</param>
        /// <returns></returns>
        public AccessToken GetAccessToken2(string username, string password)
        {
            string data = "client_id={0}&client_secret={1}&grant_type=password&username={2}&password={3}";
            data = string.Format(data, app_key, app_secret, username.UrlEncode(), password.UrlEncode());
            string uri = ACCESS_TOKEN_URL + "?" + data;

            return GetAccessToken(uri);
        }

        private AccessToken GetAccessToken(string uri)
        {
            SocketHelper socket = new SocketHelper(new Uri(uri));
            string json = socket.Get(true);
            string[] kvs = json.Split(',');
            var acc = new AccessToken()
            {
                Token = kvs[0].Split(':')[1].Trim('"'),
                Expires = kvs[1].Split(':')[1].ToInt(),
                UID = kvs[2].Split(':')[1].TrimEnd('}').ToInt()
            };
            return acc;
        }
    }
}
