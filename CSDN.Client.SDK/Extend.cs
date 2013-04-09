using System;
using System.Web;
using System.Text;

namespace CSDN.Client.SDK
{
    public static class Extend
    {
        public static string UrlEncode(this string str)
        {
            if (str == null) return string.Empty;
            return HttpUtility.UrlEncode(str, Encoding.UTF8);
        }
        public static int ToInt(this string str)
        {
            int i;
            int.TryParse(str, out i);
            return i;
        }
        public static bool ToBool(this string str)
        {
            return str == "1" || str == "true";
        }
        public static DateTime ToDateTime(this string str)
        {
            DateTime dt;
            DateTime.TryParse(str, out dt);
            return dt;
        }
    }
}
