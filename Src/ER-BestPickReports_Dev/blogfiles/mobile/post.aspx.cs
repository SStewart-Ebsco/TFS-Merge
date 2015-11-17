using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using ER_BestPickReports_Dev.App_Code;
using ER_BestPickReports_Dev.App_Code.Models;
using ER_BestPickReports_Dev.Helpers;
using System.Text.RegularExpressions;

namespace ER_BestPickReports_Dev.blogfiles.mobile
{
    public partial class post : BasePage
    {
        string postid = "";
        string catid = "";
        string strtitle = "";
        string strcat = "";

        private AppCookies bprPreferences = AppCookies.CreateInstance();

        

        protected void Page_Load(object sender, EventArgs e)
        {
            //Get value from route handler
            postid = (HttpContext.Current.Items["postid"] == null) ? "" : HttpContext.Current.Items["postid"].ToString();
            catid = (HttpContext.Current.Items["catid"] == null) ? "" : HttpContext.Current.Items["catid"].ToString();
            strcat = (HttpContext.Current.Items["category"] == null)
                                        ? String.Empty
                                        : HttpContext.Current.Items["category"].ToString();
            strtitle = (HttpContext.Current.Items["title"] == null) ? "" : HttpContext.Current.Items["title"].ToString();

            int postIdNumber;
            if (Int32.TryParse(postid, out postIdNumber))
            {
                //Set masterpage variable
                Master.postid = postid;

                var post = BlogPostDL.GetById(postIdNumber);

                //Redirect if expired and not logged in
                if (DateTime.Parse(post.PublishDate) > DateTime.Now && !LoggedIn)
                    Response.Redirect("/blogfiles/login.aspx");

                Title = post.Title;
                PostTitle.Text = post.Title;

                PostDateMonth.Text = FormatDate(post.PublishDate);
                PostDate.Text = DateTime.Parse(post.PublishDate).Day.ToString(CultureInfo.InvariantCulture);

                if (Regex.IsMatch(post.Body.ToString(), @">By ([A-Za-z\s]+)\|([A-Za-z\s&;]+)</p>"))
                {
                    Regex regex = new Regex(pattern: @">By ([A-Za-z\s]+)\|([A-Za-z\s&;]+)</p>");
                    Match match = regex.Match(post.Body.ToString());
                    string removename = match.Groups[2].Value;
                    string body = post.Body.ToString();
                    PostBody.Text = body.Replace("|" + removename, "");
                }
                else
                {
                    PostBody.Text = post.Body;
                }

                //PostBody.Text = post.Body;

                if (post.ImagePath != "" && bool.Parse(post.ImageBody))
                {
                    PostImage.ImageUrl = "/blogfiles/assets/media/" + post.ImagePath;
                    PostImage.Visible = true;
                }

                //Add meta info
                var meta2 = new HtmlMeta {Name = "keywords", Content = post.MetaKeywords};
                Page.Header.Controls.Add(meta2);
                Page.Header.Controls.Add(new LiteralControl("\n"));

                var meta1 = new HtmlMeta {Name = "description", Content = post.MetaDesc};
                Page.Header.Controls.Add(meta1);
                Page.Header.Controls.Add(new LiteralControl("\n"));

                Page.Title = post.Title;


                var tags = BlogPostDL.GetTagsForBlogPost(postIdNumber);
                //Get tag list
                string taglist = "";
                string searchcity = "";

                searchcity = bprPreferences.CityUrlName;

                foreach (var tag in tags)
                {
                    taglist += "<li><a href=\"/blog/search/?tag=" + tag + "&city=" + searchcity + "\">" + tag +
                               "</a></li>";
                }

                Tags.Text = taglist;

                var categories = CategoriesDL.GetCategoriesForBlogPost(postIdNumber);
                //Get category list
                var catlist = "";
                var postfix = "";
                //SEt postfix based on city
                if (searchcity != "")
                    postfix += "/" + searchcity;

                foreach (var category in categories)
                {
                    catlist += "<li><a href=\"/blog/" + category.CatUrlName + postfix + "\">" + category.Name +
                               "</a></li>";
                }
                Cats.Text = catlist;

                if (catlist == "")
                    CatPanel.Visible = false;

                string zipCode = LocationHelper.GetZip(Request, Response, Session);

                CategoriesDL currentCategory = (String.IsNullOrEmpty(catid) && String.IsNullOrEmpty(strcat)) ? null : CategoriesDL.GetCategoryInfo(catid, strcat);

                var prevNextUrls = BlogPostDL.GetNextAndPreviousArticleLinks(postIdNumber, (currentCategory == null) ? String.Empty : currentCategory.BlogCatId.ToString(), zipCode);
                if (prevNextUrls.ContainsKey("previous"))
                {
                    PreviousArticleLink.NavigateUrl = "~/blog/post/" +
                                                      ((currentCategory == null || String.IsNullOrEmpty(currentCategory.CatUrlName))
                                                           ? String.Empty
                                                           : String.Concat(currentCategory.CatUrlName, "/")) +
                                                      prevNextUrls["previous"].UrlTitle;
                    PreviousArticleLinkText.Text = prevNextUrls["previous"].Title;
                }
                else
                {
                    PreviousArticleLink.Visible = false;
                }
                if (prevNextUrls.ContainsKey("next"))
                {
                    NextArticleLink.NavigateUrl = "~/blog/post/" + ((currentCategory == null || String.IsNullOrEmpty(currentCategory.CatUrlName))
                                                                        ? String.Empty
                                                                        : String.Concat(currentCategory.CatUrlName, "/")) +
                                                  prevNextUrls["next"].UrlTitle;
                    NextArticleLinkText.Text = prevNextUrls["next"].Title;
                }
                else
                {
                    NextArticleLink.Visible = false;
                }

                PostAuthorName.Text = post.AuthorNames;
                //PostAuthorTitle.Text = post.AuthorTitles;
                if (!String.IsNullOrEmpty(post.AuthorDescriptions))
                {
                    PostAuthorDescription.Text = post.AuthorDescriptions;
                    PostAuthorDescriptionContainer.Visible = true;
                }
            }

            IsInMarketUser.Value = LocationHelper.CheckInMarketPosition(Request.Cookies.Get("bprpreferences")).ToString().ToLower();
        }
        
    }
}