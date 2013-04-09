using System;

namespace CSDN.Client.SDK.Entity
{
    public class Comment
    {
        public int Id { get; internal set; }
        public int ParentId { get; internal set; }
        public int ArticleId { get; internal set; }
        public string ArticleTitle { get; internal set; }
        public string Blogger { get; internal set; }
        public string UserName { get; internal set; }
        public DateTime CreateAt { get; internal set; }
        public string Content { get; internal set; }
    }
}
