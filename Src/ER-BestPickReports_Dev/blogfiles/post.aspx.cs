using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using ER_BestPickReports_Dev.App_Code;
using ER_BestPickReports_Dev.App_Code.Models;
using ER_BestPickReports_Dev.Helpers;
using System.Text.RegularExpressions;

namespace ER_BestPickReports_Dev.blogfiles
{
    public partial class post : BasePage
    {
        string postid = "";
        string strtitle = "";

        string areaid = String.Empty;
        string cityid = String.Empty;
        string catid = String.Empty;
        private string strarea = String.Empty;
        string strcat = String.Empty;
        string strcity = String.Empty;
        string strmonth = String.Empty;
        string stryear = String.Empty;

        private AppCookies bprPreferences = AppCookies.CreateInstance();

        private int _categoryId;
        private string _category;

        private string basedomain = String.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            var context = HttpContext.Current.Items;
            //Get value from route handler
            postid = (HttpContext.Current.Items["postid"] == null)
                         ? String.Empty
                         : HttpContext.Current.Items["postid"].ToString();
            strtitle = (HttpContext.Current.Items["title"] == null)
                           ? String.Empty
                           : HttpContext.Current.Items["title"].ToString();

            _categoryId = bprPreferences.CategoryId;
            _category = bprPreferences.CategoryName;

            areaid = context.Contains("areaid") ? context["areaid"] != "0" ? context["areaid"].ToString() : String.Empty : String.Empty;
            cityid = context.Contains("cityid") ? context["cityid"] != "0" ? context["cityid"].ToString() : String.Empty : String.Empty;
            catid = context.Contains("catid") ? context["catid"] != "0" ? context["catid"].ToString() : String.Empty : String.Empty;
            strarea = context.Contains("area") ? context["area"] != "" ? context["area"].ToString() : String.Empty : String.Empty;
            strcity = context.Contains("city") ? context["city"] != "" ? context["city"].ToString() : String.Empty : String.Empty;
            strcat = context.Contains("cat") ? context["cat"] != "" ? context["cat"].ToString() : String.Empty : String.Empty;
            strmonth = context.Contains("month") ? context["month"] != "0" ? context["month"].ToString() : String.Empty : String.Empty;
            stryear = context.Contains("year") ? context["year"] != "0" ? context["year"].ToString() : String.Empty : String.Empty;

            if (HttpContext.Current.Request.ServerVariables["SERVER_NAME"] != "localhost")
                basedomain = "http://" + HttpContext.Current.Request.ServerVariables["SERVER_NAME"];

            if (postid != "")
            {
                //Set masterpage variable
                ((ER_BestPickReports_Dev.blogfiles.Blog)Master).postid = postid;

                string sql = "SELECT * FROM BlogPosts WHERE PostID=@PID";

                using (SqlConnection conn = new SqlConnection(DataAccessHelper.ConnString))
                {
                    conn.Open();

                    SqlDataReader rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@PID", postid));

                    if (rdr.Read())
                    {
                        //Redirect if expired and not logged in
                        if (DateTime.Parse(rdr["PublishDate"].ToString()) > DateTime.Now && !LoggedIn)
                            Response.Redirect("/blogfiles/login.aspx");

                        Title = rdr["Title"].ToString();
                        PostTitle.Text = rdr["Title"].ToString();
                        PostMonth.Text = FormatDate(rdr["PublishDate"].ToString());
                        PostDate.Text = DateTime.Parse(rdr["PublishDate"].ToString()).Day.ToString();
                        PostYear.Value = DateTime.Parse(rdr["PublishDate"].ToString()).Year.ToString();
                        if (Regex.IsMatch(rdr["Body"].ToString(), @">By ([A-Za-z\s]+)\|([A-Za-z\s&;]+)</p>"))
                        {
                            Regex regex = new Regex(pattern: @">By ([A-Za-z\s]+)\|([A-Za-z\s&;]+)</p>");
                            Match match = regex.Match(rdr["Body"].ToString());
                            string removename = match.Groups[2].Value;
                            string body=rdr["Body"].ToString();
                            PostBody.Text = body.Replace("|"+removename, "");
                        }
                        else
                        {
                            PostBody.Text = rdr["Body"].ToString();
                        }

                        if (rdr["ImagePath"].ToString() != "" && bool.Parse(rdr["ImageBody"].ToString()))
                        {
                            PostImage.ImageUrl = "/blogfiles/assets/media/" + rdr["ImagePath"].ToString();
                            PostImage.Visible = true;
                        }

                        //Add meta info
                        HtmlMeta meta1;
                        HtmlMeta meta2;

                        meta2 = new HtmlMeta();
                        meta2.Name = "keywords";
                        meta2.Content = rdr["MetaKeywords"].ToString();
                        Page.Header.Controls.Add(meta2);
                        Page.Header.Controls.Add(new LiteralControl("\n"));

                        meta1 = new HtmlMeta();
                        meta1.Name = "description";
                        meta1.Content = rdr["MetaDesc"].ToString();
                        Page.Header.Controls.Add(meta1);
                        Page.Header.Controls.Add(new LiteralControl("\n"));

                        Page.Title = rdr["Title"].ToString();

                        if (!String.IsNullOrEmpty(rdr["AuthorNames"].ToString()))
                        {
                            PostAuthorName.Text = "By " + rdr["AuthorNames"];
                        }
                        if (!String.IsNullOrEmpty(rdr["AuthorTitles"].ToString()))
                        {
                            //PostAuthorTitle.Text = " | " + rdr["AuthorTitles"];
                        }
                        if (!String.IsNullOrEmpty(rdr["AuthorDescriptions"].ToString()))
                        {
                            PostAuthorDescription.Text = rdr["AuthorDescriptions"].ToString();
                            AuthorsInfo.Visible = true;
                        }
                    }
                    rdr.Close();

                    string zipCode = LocationHelper.GetZip(Request, Response, Session);

                    //Set posts naigation
                    Dictionary<string, BlogPostDL> navigationInfo = BlogPostDL.GetNextAndPreviousArticleLinks(Convert.ToInt32(postid), CategoriesDL.GetBlogCategoryIdById(_categoryId.ToString()), zipCode);

                    if (navigationInfo.ContainsKey("next"))
                    {

                        NextPostLink.Text = navigationInfo["next"].Title;
                        NextNavigation.NavigateUrl = String.Concat(basedomain, "/blog/post/",
                                                                   (String.IsNullOrEmpty(_category))
                                                                       ? String.Empty
                                                                       : String.Concat(_category, "/"),
                                                                   navigationInfo["next"].UrlTitle);
                    }
                    else
                    {
                        NextNavigation.Visible = false;
                    }
                    if (navigationInfo.ContainsKey("previous"))
                    {

                        PreviousPostLink.Text = navigationInfo["previous"].Title;
                        PrevNavigation.NavigateUrl = String.Concat(basedomain, "/blog/post/",
                                                                   (String.IsNullOrEmpty(_category))
                                                                       ? String.Empty
                                                                       : String.Concat(_category, "/"),
                                                                   navigationInfo["previous"].UrlTitle);
                    }
                    else
                    {
                        PrevNavigation.Visible = false;
                    }

                    //Get tag list
                    string taglist = "";
                    string searchcity = "";

                    searchcity = bprPreferences.CityUrlName;

                    char[] charsToTrim = { ',', ' ' };
                    sql = "SELECT * FROM BlogTags WHERE (PostID = @PID)";
                    rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@PID", postid));

                    while (rdr.Read())
                    {
                        taglist += "<a href=\"/blog/search/?tag=" + rdr["TagName"].ToString() + "&city=" + searchcity + "\">" + rdr["TagName"].ToString() + "</a>, ";
                    }
                    rdr.Close();

                    taglist = taglist.TrimEnd(charsToTrim);
                    Tags.Text = taglist;

                    //Get category list
                    string catlist = "";

                    sql = "SELECT * FROM BlogCategories INNER JOIN BlogPostsToCategories ON BlogCategories.BlogCatID=BlogPostsToCategories.BlogCatID WHERE (BlogPostsToCategories.PostID = @PID)";
                    rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@PID", postid));

                    string postfix = "";

                    //SEt postfix based on city
                    if (searchcity != "")
                        postfix += "/" + searchcity;

                    while (rdr.Read())
                    {
                        catlist += "<a href=\"/blog/" + rdr["BlogCatUrlName"].ToString() + postfix + "\">" + rdr["BlogCatName"].ToString() + "</a>, ";
                    }
                    rdr.Close();

                    catlist = catlist.TrimEnd(charsToTrim);
                    Cats.Text = catlist;

                    if (catlist == "")
                        CatPanel.Visible = false;

                    conn.Close();
                }
            }

            IsInMarketUser.Value = LocationHelper.CheckInMarketPosition(Request.Cookies.Get("bprpreferences")).ToString().ToLower();
        }
    }
}
