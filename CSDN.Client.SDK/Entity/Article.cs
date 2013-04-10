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
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreateAt { get; internal set; }
        public int ViewCount { get; internal set; }
        public int CommentCount { get; internal set; }
        public bool CommentAllowed { get; set; }
        public string Type { get; set; }
        public int Channel { get; set; }
        public int Digg { get; internal set; }
        public int Bury { get; internal set; }
        public string Description { get; set; }
        public string[] Categories { get; set; }
        public string[] Tags { get; set; }
        public string Content { get; set; }

        public string Url { get; internal set; }
    }
}
