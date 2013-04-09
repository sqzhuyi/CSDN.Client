using System;

namespace CSDN.Client.SDK.Entity
{
    public class Article
    {
        public Article()
        {
            Categories = new string[0];
            Tags = new string[0];
        }
        public int Id { get; internal set; }
        public string Title { get; internal set; }
        public DateTime CreateAt { get; internal set; }
        public int ViewCount { get; internal set; }
        public int CommentCount { get; internal set; }
        public bool CommentAllowed { get; internal set; }
        public string Type { get; internal set; }
        public int Channel { get; internal set; }
        public int Digg { get; internal set; }
        public int Bury { get; internal set; }
        public string Description { get; internal set; }
        public string[] Categories { get; internal set; }
        public string[] Tags { get; internal set; }
        public string Content { get; internal set; }

        public string Url { get; internal set; }
    }
}
