using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CSDN.Client.SDK.Entity;

namespace CSDN.Client.SDK
{
    public class UserApi:BaseApi
    {
        public UserApi(string accessToken, string appKey)
            :base(accessToken, appKey)
        {
        }

        public string GetEmail()
        {
            string json = Post("user/getemail");

            return GetValue("email", json);
        }

        public UserInfo GetInfo()
        {
            string json = Post("user/getinfo");

            return new UserInfo()
            {
                City = GetValue("city", json),
                Job = GetValue("job", json),
                Industry = GetValue("industry", json),
                WorkYear = GetValue("workyear", json),
                Gender = GetValue("gender", json),
                Website = GetValue("website", json),
                Description = GetValue("description", json)
            };
        }

        public string GetMobile()
        {
            string json = Post("user/getmobile");

            return GetValue("mobile", json);
        }

        public IDictionary<string, string> GetAvatars(string[] usernames,int size=1)
        {
            string data = "users=" + string.Join(",", usernames).UrlEncode() + "&size=" + size;
            string json = Post("user/getavatar", data);

            Regex reg = new Regex(@"\{""userName"":""(.+?)"",""avatar"":""(.+?)""\}", RegexOptions.IgnoreCase);
            var dict = new Dictionary<string, string>();
            foreach (Match mat in reg.Matches(json))
            {
                dict[mat.Groups[1].Value.ToLower()] = mat.Groups[2].Value;
            }
            return dict;
        }
    }
}
