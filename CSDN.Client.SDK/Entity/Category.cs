using System;

namespace CSDN.Client.SDK.Entity
{
    public class Category
    {
        public int Id { get; internal set; }
        public string Name { get; internal set; }
        public bool Hide { get; internal set; }
        public int ArticleCount { get; internal set; }
    }
}
