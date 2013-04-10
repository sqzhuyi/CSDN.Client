using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace CSDN.Client.WinTest
{
    public partial class Form1 : Form
    {
        const string app_key = "1100002";
        const string app_secret = "a0b75f6a770d4f8982451481d09fb863";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var oauth = new SDK.OAuth(app_key, app_secret);
            textBox3.Text = oauth.GetAccessToken2(textBox1.Text, textBox2.Text).Token;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var api = new SDK.UserApi(textBox3.Text, app_key);
            var info = api.GetInfo();
            textBox4.Text = string.Format("城市：{0}，\r\n工作：{1}，\r\n行业：{2}，\r\n工作年限：{3}，\r\n性别：{4}，\r\n网站：{5}，\r\n个人简介：{6}"
                , info.City, info.Job, info.Industry, info.WorkYear, info.Gender, info.Website, info.Description);
            api.GetAvatars(new string[]{"sq_zhuyi","csdn"});
            api.GetEmail();
            api.GetMobile();

            var api2 = new SDK.BlogApi(textBox3.Text, app_key);
            api2.GetAllChannels();
            var page = new SDK.Entity.PageParameter();
            api2.GetArticleComments(ref page, 6312330);
            api2.GetArticleDetails(6312330);
            api2.GetArticles(ref page, false);
            api2.GetCategories();
            api2.GetColumnArticles(ref page, "java");
            api2.GetColumnDetails("java");
            api2.GetColumns();
            api2.GetComments(ref page);
            api2.GetExperts();
            api2.GetHomeNewArticles(ref page);
            api2.GetInfo();
            api2.GetMedals();
            api2.GetMyComments(ref page);
            api2.GetNewArticles(ref page);
            api2.GetStats();
            api2.GetTags();

            var art = new SDK.Entity.Article()
            {
                Title = "abcd",
                Content = "body",
                Categories = new string[] { "test" },
                Tags = new string[] { "abcd" }
            };
            api2.SaveArticle(ref art);

            var cmt = new SDK.Entity.Comment()
            {
                ArticleId = 6312330,
                Content = "body"
            };
            api2.SaveComment(cmt);

            api2.SaveInfo("road2", "一亩三分地");
        }
    }
}
