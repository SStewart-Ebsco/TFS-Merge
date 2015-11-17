using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using ER_BestPickReports_Dev.App_Code;
using ER_BestPickReports_Dev.App_Code.Models;
using ER_BestPickReports_Dev.Helpers;

namespace ER_BestPickReports_Dev.blogfiles.mobile
{
    public partial class _default : BasePage
    {
        private const int pageSize = 10;

        private List<BlogPostDL> _posts;
        int recordNumber = 0;
        string areaid = String.Empty;
        string cityid = String.Empty;
        string catid = String.Empty;
        string postcatid = String.Empty;
        private string strarea = String.Empty;
        string strcat = String.Empty;
        string strcity = String.Empty;
        string strmonth = String.Empty;
        string stryear = String.Empty;
        string urltitle = String.Empty;
        int page = 1;
        int totalPages = 0;
        string strredirect = String.Empty;
        string basedomain = String.Empty;

        private string _zipCode = String.Empty;

        private readonly AppCookies bprPreferences = AppCookies.CreateInstance();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Get value from route handler
            areaid = (HttpContext.Current.Items["areaid"].ToString() == "0") ? String.Empty : HttpContext.Current.Items["areaid"].ToString();
            cityid = (HttpContext.Current.Items["cityid"].ToString() == "0") ? String.Empty : HttpContext.Current.Items["cityid"].ToString();
            catid = (HttpContext.Current.Items["catid"].ToString() == "0") ? String.Empty : HttpContext.Current.Items["catid"].ToString();
            strarea = (HttpContext.Current.Items["area"].ToString() == String.Empty) ? String.Empty : HttpContext.Current.Items["area"].ToString();
            strcity = (HttpContext.Current.Items["city"].ToString() == String.Empty) ? String.Empty : HttpContext.Current.Items["city"].ToString();
            strcat = (HttpContext.Current.Items["cat"].ToString() == String.Empty) ? String.Empty : HttpContext.Current.Items["cat"].ToString();
            strmonth = (HttpContext.Current.Items["month"].ToString() == "0") ? String.Empty : HttpContext.Current.Items["month"].ToString();
            stryear = (HttpContext.Current.Items["year"].ToString() == "0") ? String.Empty: HttpContext.Current.Items["year"].ToString();
            strredirect = Request["redirect"] ?? String.Empty;

            //Set masterpage variables
            ((ER_BestPickReports_Dev.blogfiles.mobile.BlogMobile)Master).strcity = strcity;
            ((ER_BestPickReports_Dev.blogfiles.mobile.BlogMobile)Master).strcat = strcat;
            ((ER_BestPickReports_Dev.blogfiles.mobile.BlogMobile)Master).strmonth = strmonth;
            ((ER_BestPickReports_Dev.blogfiles.mobile.BlogMobile)Master).stryear = stryear;

            //Set Domain
            if (HttpContext.Current.Request.ServerVariables["SERVER_NAME"] != "localhost")
                basedomain = "http://" + HttpContext.Current.Request.ServerVariables["SERVER_NAME"];

            //Redirect to market home page if global page with cookie
            if (String.IsNullOrEmpty(cityid) &&
               String.IsNullOrEmpty(strredirect) && String.IsNullOrEmpty(catid) && String.IsNullOrEmpty(strmonth) &&
               String.IsNullOrEmpty(stryear))
            {
                if (
                    !String.IsNullOrEmpty(bprPreferences.CityUrlName) &&
                    !String.IsNullOrEmpty(bprPreferences.AreaUrlName))
                {
                    Response.Redirect(basedomain + "/blog/" + bprPreferences.CityUrlName + "/" +
                                      bprPreferences.AreaUrlName);
                }
                //else
                //{
                //    Request.Cookies.Remove("bprpreferences");
                //}
            }

            //Set paging
            if (String.IsNullOrEmpty(PageSaved.Value))
                PageSaved.Value = page.ToString();
            else
                page = int.Parse(PageSaved.Value);

            //Get page url
            urltitle += "/blog/";

            if (strcat != "")
                urltitle += strcat + "/";

            if (strcity != "")
                urltitle += strcity + "/";

            if (strmonth != "0" && stryear != "0")
                urltitle += strmonth + "/" + stryear + "/";

            ////Add meta info
            HtmlMeta meta1;
            //HtmlMeta meta2;

            //meta2 = new HtmlMeta();
            //meta2.Name = "keywords";
            //meta2.Content = "";
            //Page.Header.Controls.Add(meta2);
            //Page.Header.Controls.Add(new LiteralControl("\n"));

            meta1 = new HtmlMeta();
            meta1.Name = "description";
            meta1.Content = "The Best Pick blog features weekly, in-depth articles—vetted by our Best Pick experts—on the latest industry trends and maintenance advice to help you stay informed while caring for your home.";
            Page.Header.Controls.Add(meta1);
            Page.Header.Controls.Add(new LiteralControl("\n"));

            if (!IsPostBack)
            {
                //Geocoding check
                _zipCode = LocationHelper.GetZip(Request, Response, Session);

                if ((!String.IsNullOrEmpty(catid) || !String.IsNullOrEmpty(strcat)) && !strcat.Equals(strcity))
                {
                    //!!! catUrlName as icon url for current situation
                    CategoriesDL category = CategoriesDL.GetCategoryInfo(catid, strcat, true);
                    CategoryFilterTitle.Text = String.Concat("Articles in ", category.Name);
                    CategoryFilterImage.BackImageUrl = String.Concat("/assets/icons/", category.CatUrlName);
                    CategoryFilterPanel.Visible = true;

                    postcatid = category.BlogCatId.ToString();
                }

                //Show posts
                ShowPosts();

                CategoryFilterTotal.Text = String.Concat("Total number of blog post(s): ", _posts.Count);
            }

            if (!String.IsNullOrEmpty(cityid) && !String.IsNullOrEmpty(areaid))
            {
                //Set cookie for city/area preference
                bprPreferences.CityId = int.Parse(cityid);
                bprPreferences.CityUrlName = strcity;
                bprPreferences.AreaId = int.Parse(areaid);
                bprPreferences.AreaUrlName = strarea;

                bprPreferences.SetExpiration(DateTime.Now.AddDays(365));
            }
        }


        private void ShowPosts()
        {
            _posts = (!String.IsNullOrEmpty(_zipCode) && LocationHelper.CheckInMarketPosition(Request.Cookies.Get("bprpreferences")))
                          ? ((!String.IsNullOrEmpty(postcatid))
                                 ? BlogPostDL.GetFilteredBlogPosts(CategoriesDL.GetBlogCategoryIdById(catid), _zipCode, strmonth, stryear)
                                 : BlogPostDL.GetFilteredBlogPosts(_zipCode, strmonth, stryear))
                          : BlogPostDL.GetAll(CategoriesDL.GetBlogCategoryIdById(catid), strmonth, stryear);
            
            //Get total count of posts
            if (TotalPagesSaved.Value == String.Empty)
            {
                int cnt = _posts.Count(p => !String.IsNullOrEmpty(p.Title));
                totalPages = (int)Math.Ceiling((float)cnt / (float)pageSize);
                TotalPagesSaved.Value = totalPages.ToString();
            }
            else
            {
                totalPages = int.Parse(TotalPagesSaved.Value);
            }
            int startRow = (page - 1) * pageSize;
            SetPaging();
            if (_posts.Count > 0)
            {
                _posts = (_posts.Count - startRow > pageSize)
                             ? _posts.GetRange(startRow, pageSize)
                             : _posts.GetRange(startRow, _posts.Count - startRow);
            }
            
            PostList.DataSource = _posts;
            PostList.DataBind();
        }

        protected void AddPost_Click(object sender, EventArgs e)
        {
            Response.Redirect("/blogfiles/postform.aspx?city=" + strcity + "&cat=" + strcat + "&month=" + strmonth + "&year=" + stryear);
        }

        protected void EditPost_Click(object sender, CommandEventArgs e)
        {
            string id = e.CommandArgument.ToString();

            Response.Redirect("/blogfiles/postform.aspx?postid=" + id + "&city=" + strcity + "&cat=" + strcat + "&month=" + strmonth + "&year=" + stryear);
        }

        protected void DeletePost_Click(object sender, CommandEventArgs e)
        {
            if (LoggedIn)
            {
                DeleteObjectID.Value = e.CommandArgument.ToString();

                //delete object
                string sql = "DELETE FROM BlogPosts WHERE (PostID = @ID);DELETE FROM BlogTags WHERE (PostID = @ID);DELETE FROM BlogPostsToCities WHERE (PostID = @ID);DELETE FROM BlogPostsToCategories WHERE (PostID = @ID)";
                DataAccessHelper.Data.ExecuteNonQuery(sql,
                    new SqlParameter("@ID", int.Parse(DeleteObjectID.Value)));
            }

            Response.Redirect(urltitle);
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
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var postUrl = "/blog/post/" + _posts[recordNumber].UrlTitle;
                HyperLink title = (HyperLink)e.Item.FindControl("PostTitle");
                title.Text = _posts[recordNumber].Title;
                title.NavigateUrl = postUrl;

                if (_posts[recordNumber].ImagePath != "" && bool.Parse(_posts[recordNumber].ImageSummary))
                {
                    Image img = (Image)e.Item.FindControl("PostImage");
                    img.ImageUrl = "/blogfiles/assets/media/" + _posts[recordNumber].ImagePath;
                    img.Visible = true;
                    var imgLink = (HyperLink)e.Item.FindControl("PostImageLink");
                    imgLink.NavigateUrl = postUrl;
                }

                if (!String.IsNullOrEmpty(_posts[recordNumber].AuthorNames))
                {
                    var postAuthorName = (Literal)e.Item.FindControl("PostAuthorName");
                    var postAuthorTitle = (Literal)e.Item.FindControl("PostAuthorTitle");
                    postAuthorName.Text = _posts[recordNumber].AuthorNames;
                    //postAuthorTitle.Text = _posts[recordNumber].AuthorTitles;
                }
              
                else  if (Regex.IsMatch(_posts[recordNumber].Body, @">By ([A-Za-z\s]+)\|([A-Za-z\s&;]+)</p>"))
                {
                    var postAuthorName = (Literal)e.Item.FindControl("PostAuthorName");
                    var postAuthorTitle = (Literal)e.Item.FindControl("PostAuthorTitle");
                    Regex regex = new Regex(pattern: @">By ([A-Za-z\s]+)\|([A-Za-z\s&;]+)</p>");
                    Match match = regex.Match(_posts[recordNumber].Body);
                    postAuthorName.Text = match.Groups[1].Value;
                    //postAuthorTitle.Text = match.Groups[2].Value;
                }
                else
                {
                    var postAuthorPanel = (Panel)e.Item.FindControl("PostAuthor");
                    postAuthorPanel.Visible = false;
                }

                //Regex regex = new Regex(pattern: @"<p>(By ([A-Za-z\s]+)| ([A-Za-z\s&;]+))</p>");
                //Match match = regex.Match(_posts[recordNumber].Body);
                //string arg0 = match.Groups[1].Value;    // = "53"
                //string arg1 = match.Groups[2].Value;    // = "22"


              

                var summary = (Literal)e.Item.FindControl("Summary");
                summary.Text = _posts[recordNumber].Summary;

                var more = (HyperLink)e.Item.FindControl("ReadMoreLink");
                more.NavigateUrl = postUrl;

                var postMonth = (Label)e.Item.FindControl("PostDateMonth");
                postMonth.Text = FormatDate(_posts[recordNumber].PublishDate);
                var postDate = (Label)e.Item.FindControl("PostDate");
                postDate.Text = DateTime.Parse(_posts[recordNumber].PublishDate).Day.ToString();

                recordNumber++;
            }
        }

        private void SetPaging()
        {
            Previous.Visible = false;
            Newer.Visible = false;
            if (page < totalPages)
                Previous.Visible = true;
            if (page > 1)
                Newer.Visible = true;
        }

        protected void Previous_Click(object sender, EventArgs e)
        {
            page -= 1;
            PageSaved.Value = page.ToString();
            //Get posts
            ShowPosts();
        }

        protected void More_Click(object sender, EventArgs e)
        {
            page += 1;
            PageSaved.Value = page.ToString();
            SetPaging();

            //Get posts
            ShowPosts();
        }
    }
}
