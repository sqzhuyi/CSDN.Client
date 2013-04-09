using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace CSDN.Client.WebTest
{
    public partial class Default : System.Web.UI.Page
    {
        const string app_key = "1100003";
        const string app_secret = "41de1da60e524b9cb317ca163a81057d";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["code"] != null)
                {//登录成功后server返回的code
                    var oauth = new SDK.OAuth(app_key, app_secret);
                    var token = oauth.GetAccessToken(Request["code"], Request.Url.ToString());
                    Session["token"] = token;
                    Response.Redirect("~/");
                }
                if (Session["token"] != null)
                {
                    Literal1.Text = ((SDK.AccessToken)Session["token"]).Token;
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var oauth = new SDK.OAuth(app_key, app_secret);
            Response.Redirect(oauth.GetAuthorizeUrl(Request.Url.ToString()));
        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            string token = ((SDK.AccessToken)Session["token"]).Token;
            var api = new SDK.UserApi(token, app_key);
            var info = api.GetInfo();
            Literal2.Text = string.Format("城市：{0}，工作：{1}，行业：{2}，工作年限：{3}，性别：{4}，网站：{5}，个人简介：{6}"
                , info.City, info.Job, info.Industry, info.WorkYear, info.Gender, info.Website, info.Description);
        }
        protected void Button3_Click(object sender, EventArgs e)
        {
            string token = ((SDK.AccessToken)Session["token"]).Token;
            var api = new SDK.BlogApi(token, app_key);
            var columns = api.GetColumns();
            StringBuilder sb = new StringBuilder();
            foreach (var m in columns)
            {
                sb.AppendFormat("图标：<img src='{0}'><br>标题：{1}<br>链接：{2}<hr>"
                    , m.Logo,m.Title,m.Url);
            }
            Literal3.Text = sb.ToString();
        }
    }
}