using System;

namespace CSDN.Client.SDK.Entity
{
    public class BlogInfo
    {
        public string Title
        {
            get;
            internal set;
        }
        public string SubTitle
        {
            get;
            internal set;
        }
        public DateTime CreateAt
        {
            get;
            internal set;
        }
        public bool Expert
        {
            get;
            internal set;
        }
    }
}
