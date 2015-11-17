using System;
using System.Collections.Generic;
using System.Data;
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

namespace ER_BestPickReports_Dev.blogfiles
{
    public partial class _default : BasePage
    {
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
        int pageSize = 10;
        int page = 1;
        private int _postsCount = 0;
        int totalPages = 0;
        string strredirect = String.Empty;
        string basedomain = String.Empty;

        private string _zipCode = String.Empty;

        private AppCookies bprPreferences = AppCookies.CreateInstance();


        protected void Page_Load(object sender, EventArgs e)
        {
            //Get value from route handler
            areaid = (HttpContext.Current.Items["areaid"].ToString() == "0") ? String.Empty : HttpContext.Current.Items["areaid"].ToString();
            cityid = (HttpContext.Current.Items["cityid"].ToString() == "0") ? String.Empty : HttpContext.Current.Items["cityid"].ToString();
            catid = (HttpContext.Current.Items["catid"].ToString() == "0") ? String.Empty : HttpContext.Current.Items["catid"].ToString();
            strarea = (String.IsNullOrEmpty(HttpContext.Current.Items["area"].ToString())) ? String.Empty : HttpContext.Current.Items["area"].ToString();
            strcity = (HttpContext.Current.Items["city"].ToString() == String.Empty) ? String.Empty : HttpContext.Current.Items["city"].ToString();
            strcat = (HttpContext.Current.Items["cat"].ToString() == String.Empty) ? String.Empty : HttpContext.Current.Items["cat"].ToString();
            strmonth = (HttpContext.Current.Items["month"].ToString() == "0") ? String.Empty : HttpContext.Current.Items["month"].ToString();
            stryear = (HttpContext.Current.Items["year"].ToString() == "0") ? String.Empty : HttpContext.Current.Items["year"].ToString();
            strredirect = Request["redirect"] ?? String.Empty;

            if (cityid != string.Empty && areaid == string.Empty
                && !AreaDL.IsAreaFromCity(bprPreferences.AreaId, Convert.ToInt32(cityid)))
            {
                areaid = AreaDL.GetAreaIdByCityId(cityid).ToString();
            }


            //Set masterpage variables
            ((ER_BestPickReports_Dev.blogfiles.Blog)Master).strcity = strcity;
            ((ER_BestPickReports_Dev.blogfiles.Blog)Master).strcat = strcat;
            ((ER_BestPickReports_Dev.blogfiles.Blog)Master).strmonth = strmonth;
            ((ER_BestPickReports_Dev.blogfiles.Blog)Master).stryear = stryear;

            //Set Domain
            if (HttpContext.Current.Request.ServerVariables["SERVER_NAME"] != "localhost")
                basedomain = "http://" + HttpContext.Current.Request.ServerVariables["SERVER_NAME"];

            //Redirect to market home page if global page with cookie
             if (String.IsNullOrEmpty(cityid) &&
                String.IsNullOrEmpty(strredirect) && String.IsNullOrEmpty(catid) && String.IsNullOrEmpty(strmonth) &&
                String.IsNullOrEmpty(stryear))
            {
                if (!String.IsNullOrEmpty(bprPreferences.CityUrlName) &&
                    !String.IsNullOrEmpty(bprPreferences.AreaUrlName))
                {
                    Response.Redirect(basedomain + "/blog/" + bprPreferences.CityUrlName);
                }
                //else
                //{
                //    Request.Cookies.Remove("bprpreferences");
                //}
            }
            //if the area name is in the URL as well we need to redirect
            string urlPath = Request.RawUrl;
            urlPath = urlPath.Replace(@"/blog", "");
            if(!String.IsNullOrEmpty(strcat))
                urlPath = urlPath.Replace(@"/" + strcat, "");
            if (!String.IsNullOrEmpty(strcity))
                urlPath = urlPath.Replace(@"/" + strcity, "");
            if (urlPath.Length > 1 && urlPath.Equals(@"/" + strarea))
            {
                //Set cookie for city/area preference
                bprPreferences.CityId = int.Parse(cityid);
                bprPreferences.CityUrlName = strcity;
                bprPreferences.AreaId = int.Parse(areaid);
                bprPreferences.AreaUrlName = strarea;
                bprPreferences.SetExpiration(DateTime.Now.AddDays(365));
                Response.Redirect(basedomain + "/blog/" + bprPreferences.CityUrlName);
            }

            //if category was selected we need to put it in the cookie
            if (!String.IsNullOrEmpty(catid) && !String.IsNullOrEmpty(strcat))
            {
                bprPreferences.CategoryId = int.Parse(catid);
                bprPreferences.CategoryName = strcat;
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

            if (strmonth != "" && stryear != "")
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
                    CategoryTitle.Text = String.Concat("Articles in ", category.Name);
                    CategoryIcon.ImageUrl = String.Concat("/assets/icons/", category.CatUrlName);

                    postcatid = category.BlogCatId.ToString();
                }
                else
                {
                    CategorySelected.CssClass = "hidden";
                }

                //Show posts
                ShowPosts();

                CategoriesCounter.Text = String.Concat("Total number of blog post(s): ", _postsCount);
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
                _postsCount = _posts.Count(p => !String.IsNullOrEmpty(p.Title));
                totalPages = (int)Math.Ceiling((float)_postsCount / (float)pageSize);
                TotalPagesSaved.Value = totalPages.ToString();
            }
            else
            {
                totalPages = int.Parse(TotalPagesSaved.Value);
            }

            int startRow = (page - 1) * pageSize;
            SetPaging();

            _posts = (_posts.Count - startRow > pageSize)
                         ? _posts.GetRange(startRow, pageSize)
                         : _posts.GetRange(startRow, _posts.Count - startRow );

            PostList.DataSource = _posts;
            PostList.DataBind();
        }

        protected void PostList_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var postUrl = String.Concat("/blog/post/", _posts[recordNumber].UrlTitle);
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
                    var postAuthorName = (Label)e.Item.FindControl("Names");
                    var postAuthorTitle = (Label)e.Item.FindControl("Position");
                    postAuthorName.Text = "By " + _posts[recordNumber].AuthorNames;
                    //postAuthorTitle.Text = _posts[recordNumber].AuthorTitles;
                }
                else if (Regex.IsMatch(_posts[recordNumber].Body, @">By ([A-Za-z\s]+)\|([A-Za-z\s&;]+)</p>"))
                {
                    var names = (Label) e.Item.FindControl("Names");
                    var position = (Label) e.Item.FindControl("Position");
                    Regex regex = new Regex(pattern: @">By ([A-Za-z\s]+)\|([A-Za-z\s&;]+)</p>");
                    Match match = regex.Match(_posts[recordNumber].Body);
                    names.Text = match.Groups[1].Value;
                    //position.Text = match.Groups[2].Value;
                }
                else
                {
                    var postAuthorPanel = (Panel)e.Item.FindControl("AuthorsInfo");
                    postAuthorPanel.Visible = false;
                }

                Label summary = (Label)e.Item.FindControl("Summary");
                summary.Text = _posts[recordNumber].Summary;

                HyperLink more = (HyperLink)e.Item.FindControl("ReadMoreLink");
                more.NavigateUrl = postUrl;

                var addThis = (Panel)e.Item.FindControl("AddThis");
                String strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
                String strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "");
                addThis.Attributes["addthis:url"] = strUrl + postUrl;
                addThis.Attributes["addthis:title"] = _posts[recordNumber].Title;
                addThis.Attributes["addthis:description"] = _posts[recordNumber].Summary;

                Label PostMonth = (Label)e.Item.FindControl("PostMonth");
                PostMonth.Text = FormatDate(_posts[recordNumber].PublishDate);
                Label PostDate = (Label)e.Item.FindControl("PostDate");
                PostDate.Text = DateTime.Parse(_posts[recordNumber].PublishDate).Day.ToString();

                recordNumber++;
            }
        }

        private void SetPaging()
        {
            PrevNavigation.Visible = false;
            NextNavigation.Visible = false;
            if (page < totalPages)
                PrevNavigation.Visible = true;
            if (page > 1)
                NextNavigation.Visible = true;
        }

        protected void Previous_Click(object sender, EventArgs e)
        {
            page += 1;
            PageSaved.Value = page.ToString();
            //Get posts
            ShowPosts();
        }

        protected void More_Click(object sender, EventArgs e)
        {
            page -= 1;
            PageSaved.Value = page.ToString();
            SetPaging();

            //Get posts
            ShowPosts();
        }
    }
}
