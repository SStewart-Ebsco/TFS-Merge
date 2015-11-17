using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ER_BestPickReports_Dev.App_Code;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ER_BestPickReports_Dev.App_Code.Models;

namespace ER_BestPickReports_Dev.blogfiles
{
    public partial class articleListing : BasePage
    {
        //string postid = "";
        private List<BlogArticleListingItem> _posts;

        protected void Page_Load(object sender, EventArgs e)
        {
            //postid = (Request["postid"] == null) ? "" : Request["postid"].ToString();

            //VALIDATE THIS PAGE LOGGED IN ONLY
            if (!LoggedIn)
                Response.Redirect("/blogfiles/login.aspx");


            if (!IsPostBack)
            {
                _posts = BlogPostDL.GetBlogArticleListing();
                PostList.DataSource = _posts;
                PostList.DataBind();
            }
        }


        protected void PostList_DataBound(object sender, EventArgs e)
        {
            if (LoggedIn)
            {
                // We need to set this here for when the add new button in 
                // EmptyDataTemplate is used
                ListView lv = (ListView)sender;
                if (lv.Controls[0].FindControl("addlink") != null && lv.Items.Count == 0)
                {
                    ((HtmlGenericControl)lv.Controls[0].FindControl("addlink")).Visible = true;
                }
            }
        }

        protected void PostList_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            var recordNumber = e.Item.DataItemIndex;
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                Label title = (Label)e.Item.FindControl("PostTitle");
                title.Text = _posts[recordNumber].Title;
                
                //Regex regex = new Regex(pattern: @"<p>(By ([A-Za-z\s]+)| ([A-Za-z\s&;]+))</p>");
                //Match match = regex.Match(_posts[recordNumber].Body);
                //string arg0 = match.Groups[1].Value;    // = "53"
                //string arg1 = match.Groups[2].Value;    // = "22"

                //if (Regex.IsMatch(_posts[recordNumber].Body, ""))
                //{
                //    Label authorInfo = (Label)e.Item.FindControl("");
                //}


                var Categories = (Label)e.Item.FindControl("Categories");
                Categories.Text = _posts[recordNumber].Categories;

                var IsTopBlog = (CheckBox)e.Item.FindControl("IsTopBlog");
                IsTopBlog.Checked = _posts[recordNumber].IsTopBlog;
                IsTopBlog.Attributes.Add("postID", _posts[recordNumber].PostID.ToString());

                var PostDate = (Label)e.Item.FindControl("PostDate");
                PostDate.Text = DateTime.Parse(_posts[recordNumber].PublishDate).ToShortDateString();
            }
        }


        protected void AddPost_Click(object sender, EventArgs e)
        {
            Response.Redirect("/blogfiles/articleDetails.aspx");
        }


        protected void EditPost_Click(object sender, CommandEventArgs e)
        {
            string id = e.CommandArgument.ToString();
            Response.Redirect("/blogfiles/articleDetails.aspx?postid=" + id);
        }

        protected void IsTopBlog_Checked(object sender, EventArgs e)
        {
            var checkbox = sender as CheckBox;
            var postIDString = checkbox.Attributes["postID"];
            if (!String.IsNullOrEmpty(postIDString))
            {
                _posts = BlogPostDL.GetBlogArticleListing();
                var checkedCount = _posts.Count(p => p.IsTopBlog);
                if (checkedCount > 6 && checkbox.Checked)
                {
                    checkbox.Checked = !checkbox.Checked;
                }
                else
                {
                    BlogPostDL.UpdateBlogPost(postId: int.Parse(postIDString), isTopBlog: checkbox.Checked);
                }
            }
        }
    }
}
