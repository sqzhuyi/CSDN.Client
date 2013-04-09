using System;

namespace CSDN.Client.SDK.Entity
{
    public class BlogStats
    {
        public int ViewCount
        {
            get;
            internal set;
        }

        public int CommentCount
        {
            get;
            internal set;
        }

        public int OriginalCount
        {
            get;
            internal set;
        }

        public int ReportCount
        {
            get;
            internal set;
        }

        public int TranslatedCount
        {
            get;
            internal set;
        }

        public int Point
        {
            get;
            internal set;
        }

        public int Rank
        {
            get;
            internal set;
        }
    }
}
