using System;
using System.Collections.Generic;
using System.Text;
using CSDN.Client.SDK.Entity;

namespace CSDN.Client.SDK
{
    public class BlogApi : BaseApi
    {
        public BlogApi(string accessToken, string appKey)
            : base(accessToken, appKey)
        {
        }

        #region private get api
        public BlogInfo GetInfo()
        {
            string json = Post("blog/getinfo");
            return new BlogInfo()
            {
                Title = GetValue("title", json),
                SubTitle = GetValue("subtitle", json),
                CreateAt = GetValue("create_at", json).ToDateTime(),
                Expert = GetValue("expert", json).ToBool()
            };
        }

        public BlogStats GetStats()
        {
            string json = Post("blog/getstats");
            return new BlogStats() {
                ViewCount = GetValue("view_count", json).ToInt(),
                CommentCount = GetValue("comment_count", json).ToInt(),
                OriginalCount = GetValue("original_count", json).ToInt(),
                ReportCount = GetValue("report_count", json).ToInt(),
                TranslatedCount = GetValue("translated_count", json).ToInt(),
                Point = GetValue("point", json).ToInt(),
                Rank = GetValue("rank", json).ToInt()
            };
        }

        public Medal[] GetMedals()
        {
            string json = Post("blog/getmedal");
            string[] items = json.Split(new string[] { "},{" }, StringSplitOptions.RemoveEmptyEntries);
            if (!items[0].Contains("\"icon\""))
            {
                return new Medal[0];
            }
            var list = new Medal[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                list[i] = new Medal()
                {
                    Icon = GetValue("icon", items[i]),
                    Title = GetValue("title", items[i]),
                    Description = GetValue("description", items[i]),
                    Reason = GetValue("reason", items[i]),
                    Count = GetValue("count", items[i]).ToInt()
                };
            }
            return list;
        }

        public Column[] GetColumns()
        {
            string json = Post("blog/getcolumn");
            string[] items = json.Split(new string[] { "},{" }, StringSplitOptions.None);
            if (!items[0].Contains("\"alias\""))
            {
                return new Column[0];
            }
            var list = new Column[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                list[i] = new Column()
                {
                    Alias = GetValue("alias", items[i]),
                    Channel = GetValue("channel", items[i]).ToInt(),
                    Title = GetValue("title", items[i]),
                    Description = GetValue("description", items[i]),
                    Url = GetValue("url", items[i]),
                    Logo = GetValue("logo", items[i]),
                    ViewCount = GetValue("view_count", items[i]).ToInt()
                };
            }
            return list;
        }

        public Article[] GetArticles(ref PageParameter page, bool isDraft = false)
        {
            if (page == null) page = new PageParameter();

            string data = string.Format("status={0}&page={1}&size={2}"
                , (isDraft ? "draft" : "enabled"), page.PageIndex, page.PageSize);
            
            string json = Post("blog/getarticlelist", data);
            page.RowCount = GetValue("count", json).ToInt();

            return Json2Articles(json);
        }

        public Article GetArticleDetails(int id)
        {
            string json = Post("blog/getarticle", "id=" + id);
            return new Article()
            {
                Id = id,
                Title = GetValue("title", json),
                CreateAt = GetValue("create_at", json).ToDateTime(),
                ViewCount = GetValue("view_count", json).ToInt(),
                CommentCount = GetValue("comment_count", json).ToInt(),
                CommentAllowed = GetValue("comment_allowed", json).ToBool(),
                Type = GetValue("type", json),
                Channel = GetValue("channel", json).ToInt(),
                Digg = GetValue("digg", json).ToInt(),
                Bury = GetValue("bury", json).ToInt(),
                Description = GetValue("description", json),
                Url = GetValue("url", json),
                Categories = GetValue("categories", json).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries),
                Tags = GetValue("tags", json).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries),
                Content = GetValue("content", json)
            };
        }

        public Category[] GetCategories()
        {
            string json = Post("blog/getcategorylist");
            string[] items = json.Split(new string[] { "},{" }, StringSplitOptions.None);
            if (!items[0].Contains("\"id\""))
            {
                return new Category[0];
            }
            var list = new Category[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                list[i] = new Category()
                {
                    Id = GetValue("id", items[i]).ToInt(),
                    Name = GetValue("name", items[i]),
                    Hide = GetValue("hide", items[i]).ToBool(),
                    ArticleCount = GetValue("article_count", items[i]).ToInt()
                };
            }
            return list;
        }

        public string[] GetTags()
        {
            string json = Post("blog/gettaglist");
            string[] tags = json.Substring(1, json.Length - 2).Replace("\"",string.Empty).Split(',');
            return tags;
        }

        public Comment[] GetComments(ref PageParameter page)
        {
            string data = string.Format("page={0}&size={1}", page.PageIndex, page.PageSize);
            string json = Post("blog/getcommentlist", data);
            page.RowCount = GetValue("count", json).ToInt();

            return Json2Comments(json);
        }

        public Comment[] GetMyComments(ref PageParameter page)
        {
            string data = string.Format("page={0}&size={1}", page.PageIndex, page.PageSize);
            string json = Post("blog/getmycommentlist", data);
            page.RowCount = GetValue("count", json).ToInt();

            return Json2Comments(json);
        }
        
        public Comment[] GetArticleComments(ref PageParameter page, int article)
        {
            string data = string.Format("article={0}&page={1}&size={2}", article, page.PageIndex, page.PageSize);
            string json = Post("blog/getcommentlist", data);
            page.RowCount = GetValue("count", json).ToInt();

            return Json2Comments(json);
        }
        #endregion

        #region private set api

        public bool SaveInfo(string title, string subtitle)
        {
            string data = string.Format("title={0}&subtitle={1}"
                , title.UrlEncode(), subtitle.UrlEncode());
            string json = Post("blog/saveinfo", data);

            return json.Contains("\"status\":true");
        }

        public bool SaveArticle(ref Article entity)
        {
            string data = string.Format("id={0}&title={1}&type={2}&description={3}&content={4}&categories={5}&tags={6}&ip={7}"
                , entity.Id, entity.Title.UrlEncode(), entity.Type.UrlEncode()
                , entity.Description.UrlEncode(), entity.Content.UrlEncode()
                , string.Join(",", entity.Categories).UrlEncode()
                , string.Join(",", entity.Tags).UrlEncode()
                , clientIP
                );
            string json = Post("blog/savearticle", data);
            entity.Id = GetValue("id", json).ToInt();
            entity.Url = GetValue("url", json);

            return entity.Id > 0;
        }

        public bool SaveComment(Comment entity)
        {
            string data = string.Format("article={0}&reply_id={1}&content={2}&ip={3}"
                , entity.ArticleId, entity.ParentId
                , entity.Content.UrlEncode()
                , clientIP
                );
            string json = Post("blog/postcomment", data);

            return json.Contains("\"status\":true");
        }

        #endregion

        #region public api

        public Article[] GetNewArticles(ref PageParameter page)
        {
            if (page == null) page = new PageParameter();

            string data = string.Format("page={0}&size={1}"
                , page.PageIndex, page.PageSize);

            string json = Post("blog/getnewarticlelist", data);
            page.RowCount = GetValue("count", json).ToInt();

            return Json2Articles(json);
        }

        public Article[] GetHomeNewArticles(ref PageParameter page, int channel=0)
        {
            if (page == null) page = new PageParameter();

            string data = string.Format("channel={0}&page={1}&size={2}"
                , channel, page.PageIndex, page.PageSize);

            string json = Post("blog/gethomenewest", data);
            page.RowCount = GetValue("count", json).ToInt();

            return Json2Articles(json);
        }

        public string[] GetExperts(int channel=0)
        {
            string json = Post("blog/getexpertlist");
            string[] experts = json.Substring(1, json.Length - 2).Replace("\"", string.Empty).Split(',');
            return experts;
        }

        public Column[] GetColumns(ref PageParameter page, int channel = 0)
        {
            if (page == null) page = new PageParameter();

            string data = string.Format("channel={0}&page={1}&size={2}"
                , channel, page.PageIndex, page.PageSize);

            string json = Post("blog/getcolumnlist");
            page.RowCount = GetValue("count", json).ToInt();

            string[] items = json.Split(new string[] { "},{" }, StringSplitOptions.None);
            if (!items[0].Contains("\"alias\""))
            {
                return new Column[0];
            }
            var list = new Column[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                list[i] = new Column()
                {
                    Alias = GetValue("alias", items[i]),
                    Channel = GetValue("channel", items[i]).ToInt(),
                    Title = GetValue("title", items[i]),
                    Description = GetValue("description", items[i]),
                    Url = GetValue("url", items[i]),
                    Logo = GetValue("logo", items[i])
                };
            }
            return list;
        }

        public Column GetColumnDetails(string alias)
        {
            string data = string.Format("alias=" + alias);
            string json = Post("blog/getcolumndetails", data);
            return new Column()
            {
                Alias = GetValue("alias", json),
                Channel = GetValue("channel", json).ToInt(),
                Title = GetValue("title", json),
                Description = GetValue("description", json),
                Url = GetValue("url", json),
                Logo = GetValue("logo", json),
                ViewCount = GetValue("view_count", json).ToInt()
            };
        }

        public Article[] GetColumnArticles(ref PageParameter page, string alias)
        {
            string data = string.Format("alias={0}&page={1}&size={2}"
                , alias, page.PageIndex, page.PageSize);
            string json = Post("blog/getcolumnarticles", data);

            page.RowCount = GetValue("count", json).ToInt();

            return Json2Articles(json);
        }

        public Channel[] GetAllChannels()
        {
            string json = Post("blog/getchannel");

            string[] items = json.Split(new string[] { "},{" }, StringSplitOptions.None);
            if (!items[0].Contains("\"id\""))
            {
                return new Channel[0];
            }
            var list = new Channel[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                list[i] = new Channel()
                {
                    Id = GetValue("id", json).ToInt(),
                    Name = GetValue("name", json),
                    Alias = GetValue("alias", json)
                };
            }
            return list;
        }

        #endregion

        #region private help function

        private Article[] Json2Articles(string json)
        {
            string[] items = json.Split(new string[] { "},{" }, StringSplitOptions.None);
            if (!items[0].Contains("\"id\""))
            {
                return new Article[0];
            }
            var list = new Article[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                list[i] = new Article()
                {
                    Id = GetValue("id", items[i]).ToInt(),
                    Title = GetValue("title", items[i]),
                    CreateAt = GetValue("create_at", items[i]).ToDateTime(),
                    ViewCount = GetValue("view_count", items[i]).ToInt(),
                    CommentCount = GetValue("comment_count", items[i]).ToInt(),
                    CommentAllowed = GetValue("comment_allowed", items[i]).ToBool(),
                    Type = GetValue("type", items[i]),
                    Channel = GetValue("channel", items[i]).ToInt(),
                    Digg = GetValue("digg", items[i]).ToInt(),
                    Bury = GetValue("bury", items[i]).ToInt(),
                    Description = GetValue("description", items[i]),
                    Url = GetValue("url", items[i])
                };
            }
            return list;
        }

        private Comment[] Json2Comments(string json)
        {
            string[] items = json.Split(new string[] { "},{" }, StringSplitOptions.None);
            if (!items[0].Contains("\"id\""))
            {
                return new Comment[0];
            }
            var list = new Comment[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                list[i] = new Comment()
                {
                    Id = GetValue("id", items[i]).ToInt(),
                    ParentId = GetValue("parent_id", items[i]).ToInt(),
                    ArticleId = GetValue("article_id", items[i]).ToInt(),
                    ArticleTitle = GetValue("article_title", items[i]),
                    Blogger = GetValue("blogger", items[i]),
                    UserName = GetValue("username", items[i]),
                    CreateAt = GetValue("create_at", items[i]).ToDateTime(),
                    Content = GetValue("content", items[i])
                };
            }
            return list;
        }

        #endregion

    }
}
