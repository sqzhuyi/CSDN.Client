using System;

namespace CSDN.Client.SDK
{
    /// <summary>
    /// 授权信息
    /// </summary>
    public class AccessToken
    {
        /// <summary>
        /// access_token
        /// </summary>
        public string Token
        {
            get;
            internal set;
        }
        /// <summary>
        /// 过期时间（秒）
        /// </summary>
        public int Expires
        {
            get;
            internal set;
        }
        /// <summary>
        /// CSDN用户ID
        /// </summary>
        public int UID
        {
            get;
            internal set;
        }
    }
}
