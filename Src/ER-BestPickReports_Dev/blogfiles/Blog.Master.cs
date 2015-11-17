using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web;
using System.Globalization;
using ER_BestPickReports_Dev.App_Code;
using ER_BestPickReports_Dev.App_Code.Models;
using ER_BestPickReports_Dev.Helpers;


namespace ER_BestPickReports_Dev.blogfiles
{
    public partial class Blog : System.Web.UI.MasterPage
    {
        private readonly DataAccessHelper dataAccessHelper = new DataAccessHelper();
        public bool hideSidebar = false;
        public string strcat = "";
        public string strcity = "";
        public string strmonth = "";
        public string stryear = "";
        public string postid = "";
        string filtercity = "";
        public bool isglobal = false;
        public string basedomain = "";

        public string catid = "0";
        public string contractorid = "0";
        private int recindex1 = 0;
        private int recindex2 = 0;
        private int recindex3 = 0;
        private int recindex4 = 0;
        private int _postIndex = 0;
        public int cityID = 0;
        public int areaID = 0;
        int numLists = 4;
        int currentList = 0;

        public string state = String.Empty;

        private AppCookies bprPreferences = AppCookies.CreateInstance();

        #region Email
        string isppc = "false";
        private int reccount = 0;
        int ecnt = 0;
        DataTable clistTable = new DataTable();
        #endregion email

        protected void Page_Load(object sender, EventArgs e)
        {
            if (areaID <= 0 && cityID <= 0)
            {
                areaID = Convert.ToInt32(bprPreferences.AreaId);
                cityID = Convert.ToInt32(bprPreferences.CityId);
            }

            if (!IsPostBack)
            {
                //Set city filter based on 1)URL 2)City
                if (strcity != "")
                    filtercity = strcity;
                else if (postid != "")
                    filtercity = bprPreferences.CityUrlName;


                //Set Domain
                if (HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToString() != "localhost")
                    basedomain = "http://" + HttpContext.Current.Request.ServerVariables["SERVER_NAME"];

                //Remove Out Of Market cookie if Area selected
                if (this.cityID > 0 && this.areaID > 0)
                {
                    bprPreferences.Remove(AppCookies.OUT_OF_MARKET_ZIP);
                }

                string zip = LocationHelper.GetZip(Request, Response, Session);

                bool isUserInMarket = LocationHelper.CheckInMarketPosition(Request.Cookies.Get("bprpreferences"));
                LocalBestPicks.Visible = isUserInMarket;
                MultipleEMailButton.Visible = isUserInMarket;

                Contractors.Visible = false;

                ZipCode.Attributes.Add("onkeypress", "return clickEnterButton(event,'" + ZipSearchButton.ClientID + "')");
                ZipCodeChange.Attributes.Add("onkeypress",
                                             "return clickEnterButton(event,'" + ZipCodeChangeButton.ClientID + "')");
                SearchZipBox.Attributes.Add("onkeypress",
                                            "return clickEnterButton(event,'" + SearchZipButton.ClientID + "')");

                if (!String.IsNullOrEmpty(filtercity))
                {
                    //Check to see if cookie matches city in url
                    if (filtercity != bprPreferences.CityUrlName)
                    {
                        SetCookie(filtercity);
                    }
                }

                SearchBox.Attributes.Add("onfocus", "if(this.value=='Search') this.value=''");
                SearchBox.Attributes.Add("onblur", "if(this.value=='') this.value='Search'");

                GetArchiveList();

                GetMostPopularArticles();

                //Show related articles module if on post
                if (postid != "")
                {
                    GetRelatedArticles();
                    GetRelatedLinks();
                    strcat = BlogPostDL.GetPlogPostCategoryUrl(postid);
                    catid = CategoriesDL.GetCategoryIdByUrl(strcat).ToString();

                    RequestATopic.Visible = isUserInMarket;
                    BottomAdds.Visible = !isUserInMarket;

                }
                else
                {
                    if (catid == "0" && !String.IsNullOrEmpty(strcat))
                    {
                        catid = CategoriesDL.GetCategoryIdByUrl(strcat).ToString();
                    }
                }

                if (!String.IsNullOrEmpty(strcat) && !String.Equals(strcat, strcity))
                {
                    SelectCatLbl.Text = "View Other Articles By Category";
                    SelectCatLbl.Style.Add("background-position", "280px -63px");
                    CategoriesWrapper.Style.Add("Width", "310px");
                }
                else
                {
                    MultipleEMailButton.Visible = false;
                }

                List<CategoriesDL> categories = null;
                if (isUserInMarket && areaID > 0 && cityID > 0)
                {
                    categories = CategoriesDL.GetCategoriesByZipCode(zip);
                }
                else
                {
                    categories = CategoriesDL.GetAll();
                }

                categories = categories.OrderBy(c => c.Name).ToList();

                int categoriesPerList = categories.Count / (numLists - 1);

                // Bind to the lists.  There could be fewer populated lists based on your settings.
                for (int i = 1; i <= numLists; i++)
                {
                    currentList = i;
                    ListView listView = FindUIControl(i);
                    int startFrom = (currentList - 1) * categoriesPerList;
                    int amount = (categories.Count - (currentList - 1) * categoriesPerList > categoriesPerList)
                                     ? categoriesPerList
                                     : categories.Count - (currentList - 1) * categoriesPerList;
                    listView.DataSource = categories.GetRange(startFrom, amount);
                    listView.DataBind();
                }

                categories = categories.Where(c => c.ContractorsAmount > 0).ToList();
                CategoriesChoice.DataSource = categories;
                CategoriesChoice.DataTextField = "Name";
                CategoriesChoice.DataValueField = "ID";
                CategoriesChoice.DataBind();
                CategoriesChoice.Items.Insert(0, new ListItem("Choose a Category", "0"));

                if (!String.IsNullOrEmpty(strcat) && catid != "0")
                {
                    int blogCategoryId = BlogCategoryDL.GetBlogCategoryIdByCatId(Convert.ToInt32(catid));
                    CategoriesDL category = categories.FirstOrDefault(c => c.BlogCatId == blogCategoryId);
                    if (category != null)
                    {
                        int index =
                            CategoriesChoice.Items.IndexOf(CategoriesChoice.Items.FindByText(category.Name));
                        CategoriesChoice.SelectedIndex = index;
                        TopCategories.Visible = false;
                        Contractors.Visible = true;

                        MultipleEMailButton.Visible = isUserInMarket && category.ContractorsAmount > 1;
                    }
                }

                TopAdds.Visible = !isUserInMarket;
                SideBarAdds.Visible = !isUserInMarket;

                LogoLinkBlog.NavigateUrl = "/blog?redirect=false";
                LogoLinkGlobal.NavigateUrl = basedomain + "/";
                if (areaID > 0 && cityID > 0)
                {
                    CurrentLocation.Text = dataAccessHelper.GetAreaNameFromID(areaID.ToString(), cityID.ToString());

                    //Show page navigation
                    PageNavPanel.Visible = true;
                    GlobalNavPanel.Visible = false;

                    if (isUserInMarket)
                    {
                        List<CategoriesDL> topCategories = CategoriesDL.GetTopCategoriesByAreaId(areaID);
                        topCategories = topCategories.Where(tc => tc.ContractorsAmount > 0).OrderBy(cat => cat.Name).ToList();
                        TopCategoriesList.DataSource = topCategories;
                        TopCategoriesList.DataBind();

                    }
                    if (string.IsNullOrEmpty(bprPreferences.OutOfMarketZip))
                    {
                        LogoLinkBlog.NavigateUrl = "/blog/" + CityDL.GetCityUrlByID(cityID);
                    }
                }
                else
                {
                    if (Session.Count > 0 && !String.IsNullOrEmpty(Session["areaname"] as String))
                    {
                        CurrentLocation.Text = Session["areaname"].ToString();
                        PageNavPanel.Visible = true;
                        GlobalNavPanel.Visible = false;
                    }
                    else
                    {
                        CurrentLocation.Text = "Not available";
                        //Show global navigation
                        PageNavPanel.Visible = false;
                        GlobalNavPanel.Visible = true;
                    }
                }

                if (hideSidebar)
                {
                    Sidebar.Visible = false;
                    Mainbar.Width = new Unit(100, UnitType.Percentage);
                }


                //Hide email form modal popup panel initially
                FormContainer.Style.Value = "display:none";
                RequestTopicContainer.Style.Value = "display:none";

                if (catid != "0" && !String.IsNullOrEmpty(catid) && isUserInMarket)
                {
                    SetContractorsList(catid, areaID.ToString());
                }

                if (this.cityID > 0 && this.areaID > 0)
                {
                    bprPreferences.Remove(AppCookies.OUT_OF_MARKET_ZIP);
                    // Setup previous cookie values
                    bprPreferences.CityId = bprPreferences.CityId;
                    bprPreferences.CityUrlName = bprPreferences.CityUrlName;
                    bprPreferences.AreaId = bprPreferences.AreaId;
                    bprPreferences.AreaUrlName = bprPreferences.AreaUrlName;
                }
                else
                {
                    bprPreferences.OutOfMarketZip = bprPreferences.OutOfMarketZip;
                    bprPreferences.Remove(AppCookies.CITY_ID);
                    bprPreferences.Remove(AppCookies.CITY_URL_NAME);
                    bprPreferences.Remove(AppCookies.AREA_ID);
                    bprPreferences.Remove(AppCookies.AREA_URL_NAME);
                }
            }
        }

        private int _companyIndex;

        private void SetContractorsList(string catId, string areaId)
        {
            int blogCatId = BlogCategoryDL.GetBlogCategoryIdByCatId(Convert.ToInt32(catId));
            List<int> categoryIds = BlogCategoryDL.GetCategoryIdsByBlogCatId(blogCatId);

            // 1 part of query
            string firstPartQuery = string.Format("SELECT * FROM ContractorAreaCategoryRel WHERE (AreaID = {0}) AND ( CategoryID = {1}", areaId, categoryIds[0]);
            // 2 part of query
            var secondPartQuery = new StringBuilder();
            var otherCategories = categoryIds.GetRange(1, categoryIds.Count - 1);
            foreach (var categoryId in otherCategories)
            {
                secondPartQuery.AppendFormat("OR CategoryID = {0}", categoryId);
            }
            // 3 part of query
            const string thirdPartQuery = ") ORDER BY OrderIndex";

            DataTable dt = dataAccessHelper.Data.ExecuteDataset(string.Format("{0}{1}{2}", firstPartQuery, secondPartQuery, thirdPartQuery)).Tables[0];
            bool isPrimary = false;

            clistTable.Clear();
            if (!clistTable.Columns.Contains("name"))
            {
                clistTable.Columns.Add("name");
            }
            if (!clistTable.Columns.Contains("urlname"))
            {
                clistTable.Columns.Add("urlname");
            }
            if (!clistTable.Columns.Contains("cid"))
            {
                clistTable.Columns.Add("cid");
            }
            if (!clistTable.Columns.Contains("email"))
            {
                clistTable.Columns.Add("email");
            }
            if (!clistTable.Columns.Contains("phone"))
            {
                clistTable.Columns.Add("phone");
            }

            foreach (DataRow row in dt.Rows)
            {
                InfoLevel level = InfoLevel.Area;
                DataRow conRow = dataAccessHelper.FindInfoRecord(InfoType.ContractorCategoryArea,
                                                         int.Parse(row["contractorcategoryareaid"].ToString()),
                                                         ref level, ref isPrimary);

                //Add to new data table
                clistTable.Rows.Add(new object[]
                            {
                                conRow["displayname"].ToString(),
                                row["urlname"].ToString(),
                                row["contractorcategoryareaid"].ToString(),
                                conRow["email"].ToString(),
                                (!String.IsNullOrEmpty(conRow["phone"].ToString()))
                                    ? conRow["phone"]
                                    : (!String.IsNullOrEmpty(conRow["organicphone"].ToString()))
                                          ? conRow["organicphone"]
                                          : (!String.IsNullOrEmpty(conRow["ppcphone"].ToString()))
                                                ? conRow["ppcphone"]
                                                : (!String.IsNullOrEmpty(conRow["facebookphone"].ToString()))
                                                      ? conRow["facebookphone"]
                                                      : String.Empty
                            });
            }
            clistTable.AcceptChanges();

            _companyIndex = 0;

            PreviewButtons.Visible = clistTable.Rows.Count > 3;
            MultipleEMailButton.Visible = clistTable.Rows.Count > 1;

            ContractorsList.DataSource = clistTable;
            ContractorsList.DataBind();

            //Bind to checkbox list all contractors on the page
            EmailContractorList.DataSource = clistTable;
            EmailContractorList.DataBind();

            Contractors.Visible = clistTable.Rows.Count > 0;
        }

        protected void ContractorsList_DataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HyperLink company = (HyperLink)e.Item.FindControl("CompanyName");

                DataTable dt = (DataTable)ContractorsList.DataSource;
                DataRow row = dt.Rows[_companyIndex];

                company.Text = row["name"].ToString().Trim();
                var cat = CategoriesDL.GetCategoryInfo(CategoriesChoice.SelectedItem.Value, "");
                company.NavigateUrl = String.Concat("/", cat.CatUrlName, "/", bprPreferences.CityUrlName, "/", AreaDL.GetAreaUrlByID(bprPreferences.AreaId), "/", row["urlname"].ToString().Trim());
                _companyIndex++;
            }
        }

        private void SetCookie(string city)
        {
            string redirecturl = basedomain + "/blog";

            //Set site cookie
            string sql = "SELECT CityInfo.CityID, CityInfo.DisplayName AS CityDisplayName, AreaInfo.AreaID, AreaInfo.DisplayName AS AreaDisplayName, AreaInfo.UrlName AS AreaURLName FROM CityInfo, AreaInfo " +
                "WHERE (CityInfo.CityID = AreaInfo.CityID) AND (CityInfo.UrlName = @CITY) AND (AreaInfo.IsPrimary = 1)";

            using (SqlConnection conn = new SqlConnection(dataAccessHelper.ConnString))
            {
                conn.Open();
                SqlDataReader rdr = dataAccessHelper.Data.ExecuteDatareader(conn, sql,
                    new SqlParameter("@CITY", city));

                if (rdr.Read())
                {
                    //Set cookie for city/area preference
                    bprPreferences.CityId = int.Parse(rdr["cityid"].ToString());
                    bprPreferences.CityName = rdr["CityDisplayName"].ToString();
                    bprPreferences.CityUrlName = city;
                    bprPreferences.AreaId = int.Parse(rdr["areaid"].ToString());
                    bprPreferences.AreaName = rdr["AreaDisplayName"].ToString();
                    bprPreferences.AreaUrlName = rdr["AreaURLName"].ToString();
                    bprPreferences.SetExpiration(DateTime.Now.AddDays(365));
                }

                rdr.Close();
                conn.Close();
            }

            if (strcat != "")
                redirecturl += "/" + strcat + "/" + city;
            else
            {
                if (strmonth != "" && stryear != "")
                {
                    redirecturl += "/" + city;
                }
                else
                {
                    redirecturl += "/" + city + "/" + bprPreferences.AreaUrlName;
                }
            }

            if (strmonth != "" && stryear != "")
                redirecturl += "/" + strmonth + "/" + stryear;

            Response.Redirect(redirecturl);
        }

        private void GetRelatedLinks()
        {
            //Get related categories if this is a post page
            if (postid != "")
            {
                string sql = "SELECT AtlantaLinks, ChicagoLinks, DallasLinks, NovaLinks, HoustonLinks, MarylandLinks, DCLinks, BhamLinks, bostonLinks, philiLinks FROM BlogPosts WHERE PostID=@PID";

                using (SqlConnection conn = new SqlConnection(dataAccessHelper.ConnString))
                {
                    conn.Open();
                    SqlDataReader rdr = dataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@PID", postid));

                    if (rdr.Read())
                    {
                        AtlantaLinks.Text = rdr["AtlantaLinks"].ToString();
                        ChicagoLinks.Text = rdr["ChicagoLinks"].ToString();
                        DallasLinks.Text = rdr["DallasLinks"].ToString();
                        NovaLinks.Text = rdr["NovaLinks"].ToString();
                        HoustonLinks.Text = rdr["HoustonLinks"].ToString();
                        MarylandLinks.Text = rdr["MarylandLinks"].ToString();
                        DCLinks.Text = rdr["DCLinks"].ToString();
                        BirminghamLinks.Text = rdr["BhamLinks"].ToString();
                        BostonLinks.Text = rdr["bostonLinks"].ToString();
                        PhiladelphiaLinks.Text = rdr["philiLinks"].ToString();
                    }

                    if (!String.IsNullOrEmpty(rdr["AtlantaLinks"].ToString()) ||
                        !String.IsNullOrEmpty(rdr["ChicagoLinks"].ToString()) ||
                        !String.IsNullOrEmpty(rdr["DallasLinks"].ToString()) ||
                        !String.IsNullOrEmpty(rdr["NovaLinks"].ToString()) ||
                        !String.IsNullOrEmpty(rdr["HoustonLinks"].ToString()) ||
                        !String.IsNullOrEmpty(rdr["MarylandLinks"].ToString()) ||
                        !String.IsNullOrEmpty(rdr["DCLinks"].ToString()) ||
                        !String.IsNullOrEmpty(rdr["BhamLinks"].ToString()) ||
                        !String.IsNullOrEmpty(rdr["bostonLinks"].ToString()) ||
                        !String.IsNullOrEmpty(rdr["philiLinks"].ToString()))
                    {
                        RelatedCatPanel.Visible = true;
                    }

                    rdr.Close();
                    conn.Close();
                }
            }
        }

        private void GetRelatedArticles()
        {
            string sql_add = "";
            string taglist = "";
            string sql = "SELECT * FROM BlogTags WHERE PostID = @PID";
            DataTable dt = dataAccessHelper.Data.ExecuteDataset(sql,
                new SqlParameter("@PID", postid)).Tables[0];

            foreach (DataRow row in dt.Rows)
            {
                string tagName = row["TagName"].ToString().Trim();
                if (!tagName.Equals("home reports") ||
                     !tagName.Equals("best pick reports"))
                {
                    if (taglist != "")
                        taglist += " OR";

                    taglist += " CONTAINS(BlogPosts.MetaKeywords, '\"" + tagName + "\"')";
                }
            }

            //Filter by PublsihDate if not logged in
            var basePage = new BasePage();
            if (!basePage.LoggedIn)
                sql_add += " AND (PublishDate <= @DATE)";

            sql = "SELECT TOP 3 * FROM BlogPosts WHERE (PostID <> @PID)" +
                  ((!String.IsNullOrEmpty(taglist)) ? " AND (" + taglist + ")" : String.Empty) + sql_add +
                  " ORDER BY PublishDate DESC";

            using (SqlConnection conn = new SqlConnection(dataAccessHelper.ConnString))
            {
                conn.Open();

                SqlDataReader rdr = dataAccessHelper.Data.ExecuteDatareader(conn, sql,
                    new SqlParameter("@PID", postid),
                    new SqlParameter("@DATE", DateTime.Now));
                RelatedList.DataSource = rdr;

                if (rdr.HasRows)
                {
                    RelatedArticlesPanel.Visible = true;
                    RelatedList.DataBind();
                }

                if (basePage.LoggedIn)
                    RelatedArticlesPanel.Visible = true;

                rdr.Close();

                conn.Close();
            }
        }

        private void GetMostPopularArticles()
        {
            var topPosts = BlogPostDL.GetTopPosts();
            MostPopularArticles.DataSource = topPosts;

            if (topPosts.Count > 0)
            {
                MostPopularArticlesPanel.Visible = true;
                MostPopularArticles.DataBind();
            }
        }

        private void GetArchiveList()
        {
            Dictionary<string, Dictionary<string, string>> archiveInfo = BlogPostDL.GetArchiveList(DateTime.Now, cityID);

            ArchiveList.DataSource = archiveInfo;
            ArchiveList.DataBind();
        }

        protected void RelatedList_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                SqlDataReader rdr = (SqlDataReader)RelatedList.DataSource;

                HyperLink title = (HyperLink)e.Item.FindControl("PostTitle");
                title.Text = rdr["Title"].ToString();
                title.NavigateUrl = "/blog/post/" + rdr["UrlTitle"].ToString();
                HyperLink image = (HyperLink)e.Item.FindControl("PostImage");
                image.ImageUrl = String.Concat("/blogfiles/assets/media/", rdr["ImagePath"].ToString());
                image.NavigateUrl = String.Concat("/blog/post/", rdr["UrlTitle"].ToString());
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            if (!SearchBox.Text.Equals("Search"))
            {
                if (SearchOptions.SelectedIndex == 0)
                    Response.Redirect("/blog/search/?tag=" + SearchBox.Text.Trim() + "&city=" + strcity);
                else if (SearchOptions.SelectedIndex == 1)
                    Response.Redirect(basedomain + "/search.aspx?keyword=" + SearchBox.Text.Trim() + "&zip=Zip%20Code");
            }
        }

        protected void ZipSearch_Click(object sender, EventArgs e)
        {
            string zipCode;

            try
            {
                // Set zipcode to search on
                int.Parse(ZipCode.Text.Trim());
                zipCode = ZipCode.Text.Trim();
            }
            catch (Exception)
            {
                // zipCode will be set to empty string
                zipCode = String.Empty;
            }

            ChangeLocation(zipCode);
        }

        protected void ZipCodeChangeButton_Click(object sender, EventArgs e)
        {
            string zipCode;

            try
            {
                // Set zipcode to search on
                int.Parse(ZipCodeChange.Text.Trim());
                zipCode = ZipCodeChange.Text.Trim();
            }
            catch (Exception)
            {
                // zipCode will be set to empty string
                zipCode = String.Empty;
            }

            ChangeLocation(zipCode);
        }

        protected void SearchZipButton_Click(object sender, EventArgs e)
        {
            string zipCode;

            try
            {
                // Set zipcode to search on
                int.Parse(SearchZipBox.Text.Trim());
                zipCode = SearchZipBox.Text.Trim();
            }
            catch (Exception)
            {
                // zipCode will be set to empty string
                zipCode = String.Empty;
            }

            ChangeLocation(zipCode);
        }

        protected void ChangeLocation(string zipCode)
        {
            if (!String.IsNullOrEmpty(zipCode))
            {
                CookiesInfo info = InfoDL.GetCookiesInfoByZipCode(zipCode);

                if (info != null && info.AreaId != null)
                {
                    string redirectUrl = String.Concat(basedomain, "/blog/",
                                                       ((!String.IsNullOrEmpty(postid))
                                                            ? String.Concat("post/",
                                                                            BlogPostDL.GetPlogPostCategoryUrl(postid),
                                                                            "/", BlogPostDL.GetPostUrlTitleById(postid))
                                                            : String.Concat(((catid != "0")
                                                                                  ? dataAccessHelper.GetCategoryUrlFromID(catid)
                                                                                  : (!String.IsNullOrEmpty(strcat) &&
                                                                                     !strcat.Equals(strcity))
                                                                                        ? strcat
                                                                                        : String.Empty),
                                                                             "/", info.CityUrl, "/", info.AreaUrl)));
                    //Set cookie for city/area preference based on url values

                    bprPreferences.CityId = int.Parse(info.CityId);
                    bprPreferences.CityName = info.CityDisplayName;
                    bprPreferences.CityUrlName = info.CityUrl;
                    bprPreferences.AreaId = int.Parse(info.AreaId);
                    bprPreferences.AreaName = info.AreaDisplayName;
                    bprPreferences.AreaUrlName = info.AreaUrl;
                    bprPreferences.SetExpiration(DateTime.Now.AddDays(365));
                    bprPreferences.Remove(AppCookies.OUT_OF_MARKET_ZIP);

                    Response.Redirect(redirectUrl);
                }
                else
                {
                    bprPreferences.OutOfMarketZip = zipCode;
                    //Show popup warning
                    bodytag.Attributes.Add("onload", String.Format("notFound({0});", zipCode));
                    return;
                }

            }
            else
            {
                //Show popup warning
                return;
            }
        }

        protected void ListView1_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                List<CategoriesDL> dt = (List<CategoriesDL>)ListView1.DataSource;
                CategoriesDL row = dt[recindex1];

                HyperLink clink = (HyperLink)e.Item.FindControl("CatLink");
                if (cityID > 0 && areaID > 0)
                {
                    clink.NavigateUrl = String.Concat(basedomain, "/blog/", row.CatUrlName, "/", CityDL.GetCityUrlByID(cityID));
                }
                else
                {
                    clink.NavigateUrl = String.Concat(basedomain, "/blog/", row.CatUrlName);
                }

                clink.Text = row.Name;

                if ((row.Id != Convert.ToInt32(catid)) && catid != "0")
                    clink.Attributes.Add("rel", "nofollow");

                recindex1++;
            }
        }

        protected void ListView2_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                List<CategoriesDL> dt = (List<CategoriesDL>)ListView2.DataSource;
                CategoriesDL row = dt[recindex2];

                HyperLink clink = (HyperLink)e.Item.FindControl("CatLink");
                if (cityID > 0 && areaID > 0)
                {
                    clink.NavigateUrl = String.Concat(basedomain, "/blog/", row.CatUrlName, "/", CityDL.GetCityUrlByID(cityID));
                }
                else
                {
                    clink.NavigateUrl = String.Concat(basedomain, "/blog/", row.CatUrlName);
                }
                clink.Text = row.Name;

                if ((row.Id != Convert.ToInt32(catid)) && catid != "0")
                    clink.Attributes.Add("rel", "nofollow");

                recindex2++;
            }
        }

        protected void ListView3_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                List<CategoriesDL> dt = (List<CategoriesDL>)ListView3.DataSource;
                CategoriesDL row = dt[recindex3];

                HyperLink clink = (HyperLink)e.Item.FindControl("CatLink");
                if (cityID > 0 && areaID > 0)
                {
                    clink.NavigateUrl = String.Concat(basedomain, "/blog/", row.CatUrlName, "/", CityDL.GetCityUrlByID(cityID));
                }
                else
                {
                    clink.NavigateUrl = String.Concat(basedomain, "/blog/", row.CatUrlName);
                }
                clink.Text = row.Name;

                if ((row.Id != Convert.ToInt32(catid)) && catid != "0")
                    clink.Attributes.Add("rel", "nofollow");

                recindex3++;
            }
        }

        protected void ListView4_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                List<CategoriesDL> dt = (List<CategoriesDL>)ListView4.DataSource;
                CategoriesDL row = dt[recindex4];

                HyperLink clink = (HyperLink)e.Item.FindControl("CatLink");
                if (cityID > 0 && areaID > 0)
                {
                    clink.NavigateUrl = String.Concat(basedomain, "/blog/", row.CatUrlName, "/", CityDL.GetCityUrlByID(cityID));
                }
                else
                {
                    clink.NavigateUrl = String.Concat(basedomain, "/blog/", row.CatUrlName);
                }
                clink.Text = row.Name;

                if ((row.Id != Convert.ToInt32(catid)) && catid != "0")
                    clink.Attributes.Add("rel", "nofollow");

                recindex4++;
            }
        }

        #region [Helpers]

        private ListView FindUIControl(int listNum)
        {
            ListView listView = null;

            // To make this more generic the UI tree will have to be
            // traversed to find the controls.
            switch (listNum)
            {
                case 1:
                    listView = ListView1;
                    break;
                case 2:
                    listView = ListView2;
                    break;
                case 3:
                    listView = ListView3;
                    break;
                case 4:
                    listView = ListView4;
                    break;
                default:
                    throw new Exception("Invalid control number");
            }

            return listView;
        }

        #endregion


        //private void VerifyGeoPosition()
        //{
        //    if (Request.Cookies["bprpreferences"] == null ||
        //        String.IsNullOrEmpty(Request.Cookies["bprpreferences"]["areaid"]) ||
        //        String.IsNullOrEmpty(Request.Cookies["bprpreferences"]["cityid"]))
        //    {
        //        SetGeocoding();
        //    }
        //    else
        //    {
        //        if (String.IsNullOrEmpty(zip))
        //        {
        //            if (!String.IsNullOrEmpty(Request.Cookies["bprpreferences"]["areaid"]))
        //            {
        //                zip = ZipDL.GetZipByAreaId(Request.Cookies["bprpreferences"]["areaid"]);
        //            }
        //            else
        //            {
        //                if (!String.IsNullOrEmpty(Request.Cookies["bprpreferences"]["cityid"]))
        //                {
        //                    int areaId = AreaDL.GetAreaIdByCityId(Request.Cookies["bprpreferences"]["cityid"]);
        //                    zip = ZipDL.GetZipByAreaId(areaId.ToString());
        //                }
        //            }
        //        }
        //    }
        //}

        //private void SetGeocoding()
        //{
        //    string state = String.Empty;
        //    string country = String.Empty;
        //    string city = String.Empty;

        //    if (Request.Cookies.AllKeys.Contains("bprpreferences") &&
        //        !String.IsNullOrEmpty(Request.Cookies["bprpreferences"]["outOfMarketZip"]))
        //    {
        //        GeocodingResponse location =
        //            HttpRequestHelper.GetJson<GeocodingResponse>(String.Format(BWConfig.GetClientZipInfoUrl,
        //                                                                       Request.Cookies["bprpreferences"][
        //                                                                           "outOfMarketZip"]));
        //        zip = Request.Cookies["bprpreferences"]["outOfMarketZip"];
        //        state = location.State;
        //        country = location.Country;
        //        city = location.City;
        //    }
        //    else
        //    {
        //        string clientIp = HttpRequestHelper.GetClientIp(Request);

        //        //debug action for localhost
        //        if (clientIp.Equals("::1") || clientIp.StartsWith("192.168.") || clientIp.Equals("localhost"))
        //        {
        //            clientIp = String.Empty;
        //        }

        //        //Required to check Status Code according to API: When incorrect user input is entered,
        //        // the server returns an HTTP 400 Error (Bad Request), along with a JSON-encoded error message.
        //        try
        //        {
        //            GeocodingResponse location =
        //                HttpRequestHelper.GetJson<GeocodingResponse>(String.Format(BWConfig.GetClientInfoUrl, clientIp));
        //            zip = location.PostalCode;
        //            state = location.Region;
        //            country = location.Country;
        //        }
        //        catch (Exception e)
        //        {

        //        }

        //        if (!String.IsNullOrEmpty(zip))
        //        {
        //            CookiesInfo cookies = InfoDL.GetCookiesInfoByZipCode(zip);

        //            if (cookies.AreaId != null && cookies.CityId != null)
        //            {
        //                Response.Cookies["bprpreferences"]["cityid"] = cookies.CityId;
        //                Response.Cookies["bprpreferences"]["cityname"] = cookies.CityDisplayName;
        //                Response.Cookies["bprpreferences"]["cityurlname"] = cookies.CityUrl;
        //                Response.Cookies["bprpreferences"]["areaid"] = cookies.AreaId;
        //                Response.Cookies["bprpreferences"]["areaname"] = cookies.AreaDisplayName;
        //                Response.Cookies["bprpreferences"]["areaurlname"] = cookies.AreaUrl;
        //                Response.Cookies["bprpreferences"].Expires = DateTime.Now.AddDays(365);

        //                Response.Redirect(String.Concat(basedomain, "/blog/", cookies.CityUrl, "/", cookies.AreaUrl));
        //            }
        //        }
        //    }

        //    Response.Cookies["bprpreferences"]["cityid"] = null;
        //    Response.Cookies["bprpreferences"]["cityname"] = String.Empty;
        //    Response.Cookies["bprpreferences"]["cityurlname"] = String.Empty;
        //    Response.Cookies["bprpreferences"]["areaid"] = null;
        //    Response.Cookies["bprpreferences"]["areaurlname"] = String.Empty;
        //    Response.Cookies["bprpreferences"].Expires = DateTime.Now.AddDays(365);

        //    string areaname = !String.IsNullOrEmpty(city)
        //                          ? city
        //                          : !String.IsNullOrEmpty(state)
        //                                ? state
        //                                : !String.IsNullOrEmpty(country) ? country : null;

        //    if (!String.IsNullOrEmpty(areaname))
        //    {
        //        Session.Add("areaname", areaname);
        //    }
        //    else
        //    {
        //        Response.Cookies.Remove("bprpreferences");
        //    }
        //}

        protected void CategoriesChoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            TopCategories.Visible = false;
            DropDownList ddl = (DropDownList)sender;
            if (ddl.SelectedIndex > 0)
            {
                var cat = CategoriesDL.GetCategoryInfo(ddl.SelectedItem.Value, "");

                SetContractorsList(cat.Id.ToString(), areaID.ToString());
            }
            else
            {
                TopCategories.Visible = true;
                Contractors.Visible = false;
            }
        }

        protected void ContinueButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in EmailContractorList.Items)
            {
                CheckBox chkbox = item.FindControl("ContractorEmailID") as CheckBox;
                HiddenField emailid = item.FindControl("HiddenListID") as HiddenField;

                if (chkbox.Checked)
                {
                    MultipleEmailIDs.Value += emailid.Value + ",";
                    ecnt++;
                }
            }

            EmailContractorName.Text = "Multiple Companies";

            if (ecnt > 0)
            {
                if (MultipleEmailIDs.Value[MultipleEmailIDs.Value.Length - 1] == ',')
                    MultipleEmailIDs.Value = MultipleEmailIDs.Value.Substring(0, MultipleEmailIDs.Value.Length - 1);

                //Check and see if we have one or more companies
                if (ecnt == 1)
                {
                    //Lookup contractor id)
                    string sql = "SELECT DisplayName FROM ContractorAreaCategoryRel WHERE (ContractorCategoryAreaID = @CCAID)";
                    object o = dataAccessHelper.Data.ExecuteScalar(sql,
                        new SqlParameter("@CCAID", MultipleEmailIDs.Value.Trim()));
                    if (o != null)
                        EmailContractorName.Text = o.ToString();
                }

                //Clear all fields every time the form is called to show
                Name.Text = "";
                LastName.Text = "";
                Address.Text = "";
                City.Text = "";
                ZipForMail.Text = "";
                Email.Text = "";
                FormPhone.Text = "";
                FormPhone2.Text = "";
                WorkType.Text = "";
                Message.Text = "";
                ReqPanel.Visible = false;
                EmailComplete.Visible = false;
                ChooseEmailPanel.Visible = false;
                ModalEmailForm.Visible = true;

                SendEmail.Show();
                Name.Focus();
            }
            else
            {
                errornote.Visible = true;
                SendEmail.Show();
            }
        }

        protected void Cancel_EmailClick(object sender, EventArgs e)
        {
            // Hide the sent div and show the form
            ModalEmailForm.Visible = true;
            EmailComplete.Visible = false;
            ReqPanel.Visible = false;
        }

        protected void OK_EmailClick(object sender, EventArgs e)
        {
            bool error = false;
            string name = Name.Text.Trim();
            string lastname = LastName.Text.Trim();
            string addr = Address.Text.Trim();
            string zip = ZipForMail.Text.Trim();
            string email = Email.Text.Trim();
            string phone = FormPhone.Text.Trim();
            string phone2 = FormPhone2.Text.Trim();
            string worktype = WorkType.Text.Trim();

            #region Error Checking
            ReqName.Visible = false;
            ReqLastName.Visible = false;
            ReqAddress.Visible = false;
            ReqZip.Visible = false;
            ReqEmail.Visible = false;
            ReqPhone.Visible = false;
            ReqWorkType.Visible = false;
            ReqMessage.Visible = false;
            if (name == "")
                ReqName.Visible = true;
            if (lastname == "")
                ReqLastName.Visible = true;
            if (addr == "")
                ReqAddress.Visible = true;
            if (zip == "")
                ReqZip.Visible = true;
            if (zip == "" || !Global.IsZipCode(zip))
                ReqZip.Visible = true;
            if (email == "" || !Global.IsEmail(email))
                ReqEmail.Visible = true;
            if (phone == "" || !Global.IsPhoneNumber(phone))
                ReqPhone.Visible = true;
            if (worktype == "")
                ReqWorkType.Visible = true;

            error = ReqName.Visible || ReqLastName.Visible || ReqAddress.Visible || ReqZip.Visible || ReqEmail.Visible || ReqPhone.Visible || ReqWorkType.Visible;

            ReqName.Visible = false;
            ReqLastName.Visible = false;
            ReqAddress.Visible = false;
            ReqZip.Visible = false;
            ReqEmail.Visible = false;
            ReqPhone.Visible = false;
            ReqWorkType.Visible = false;
            ReqMessage.Visible = false;

            #endregion // Error Checking

            if (!error)
            {
                string contractorMsg = "";
                contractorMsg += "<span style=\"font-family:Arial; font-size:12px;\"><strong>Name: </strong>" + name + " " + lastname + "<br/><strong>Address: </strong>" + addr + "<br/><strong>City: </strong>" + City.Text.Trim() + "<br/><strong>Zip Code: </strong>" + zip + "<br/><strong>Email: </strong>" + email + "<br>";
                contractorMsg += "<strong>Phone: </strong>" + phone + "<br/><strong>Phone 2: </strong>" + phone2 + "<br/><strong>Work Type: </strong>" + worktype + "<br/><strong>Message: </strong>" + Message.Text.Trim();
                contractorMsg += "</span>";
                //string adminMsg = "<font face='Arial' size='2'>Someone showed interest in <strong>" + ContractorName.Value + "</strong><br/><br/>" + contractorMsg + "</font>";

                string subjectMsg = "";

                //Parse comma separated list of emails - check if we are sending to multiples first
                if (MultipleEmailIDs.Value != "" && !BWSession.emailStartedAndSent)
                {
                    string localconid = "";

                    string[] ids = MultipleEmailIDs.Value.Trim().Split(',');
                    foreach (string strid in ids)
                    {
                        subjectMsg = "Best Pick Reports Website Lead";

                        //Lookup emails from contractorcategoryareaid
                        InfoLevel conInfoLevel = InfoLevel.Area;
                        bool isPrimary = false;
                        DataRow conRow = dataAccessHelper.FindInfoRecord(InfoType.ContractorCategoryArea, int.Parse(strid), ref conInfoLevel, ref isPrimary);
                        if (conRow != null)
                        {
                            //Lookup contractor id
                            string sql = "SELECT ContractorID FROM ContractorCategoryInfo WHERE ContractorCategoryID = @CCID";
                            object o = dataAccessHelper.Data.ExecuteScalar(sql,
                                new SqlParameter("@CCID", conRow["contractorcategoryid"].ToString()));
                            if (o != null)
                                localconid = o.ToString();

                            string[] emails = conRow["email"].ToString().Trim().Split(',');
                            foreach (string stremail in emails)
                            {
                                if (Global.IsEmail(stremail.Trim()))
                                {
                                    try
                                    {
                                        // Send email to contractor
                                        //!!!!HARD CODED FOR TLC DECKS
                                        if (localconid == "215")
                                            subjectMsg = "Home Reports – Dispatch 6106";

                                        Global.SendEmailNotification(stremail.Trim(), subjectMsg, ConfigurationManager.AppSettings["EmailNotification"], contractorMsg, true);
                                        BWSession.emailStartedAndSent = true;
                                    }
                                    catch (Exception)
                                    {
                                        // TODO: log error in database that email couldn't be sent???
                                    }
                                }
                            }

                            // commit contractorMsg to database
                            string insertsql = "INSERT INTO EmailData (DateSent, AreaID, CityID, CategoryID, ContractorID, FirstName, LastName, Address, City, Zip, Email, Phone, Phone2, WorkType, Message, IsPPC) " +
                                "VALUES (@DATESENT, @AREAID, @CITYID, @CATEGORYID, @CONTRACTORID, @FIRSTNAME, @LASTNAME, @ADDRESS, @CITY, @ZIP, @EMAIL, @PHONE, @PHONE2, @WORKTYPE, @MESSAGE, @PPC)";
                            dataAccessHelper.Data.ExecuteNonQuery(insertsql,
                                new SqlParameter("@DATESENT", DateTime.Now),
                                new SqlParameter("@AREAID", areaID),
                                new SqlParameter("@CITYID", cityID),
                                new SqlParameter("@CATEGORYID", catid),
                                new SqlParameter("@CONTRACTORID", int.Parse(localconid)),
                                new SqlParameter("@FIRSTNAME", name),
                                new SqlParameter("@LASTNAME", lastname),
                                new SqlParameter("@ADDRESS", addr),
                                new SqlParameter("@CITY", City.Text.Trim()),
                                new SqlParameter("@ZIP", ZipForMail.Text.Trim()),
                                new SqlParameter("@EMAIL", email),
                                new SqlParameter("@PHONE", phone),
                                new SqlParameter("@PHONE2", phone2),
                                new SqlParameter("@WORKTYPE", worktype),
                                new SqlParameter("@PPC", isppc),
                                new SqlParameter("@MESSAGE", Message.Text.Trim()));
                        }
                    }
                }
                else if (ContractorEmail.Value.Trim() != "" && !BWSession.emailStartedAndSent)
                {
                    string[] emails = ContractorEmail.Value.Trim().Split(',');
                    foreach (string stremail in emails)
                    {
                        subjectMsg = "Best Pick Reports Website Lead";

                        if (Global.IsEmail(stremail.Trim()))
                        {
                            try
                            {
                                // Send email to contractor
                                //!!!!HARD CODED FOR TLC DECKS
                                if (EmailContractorID.Value == "215")
                                    subjectMsg = "Home Reports – Dispatch 6106";

                                Global.SendEmailNotification(stremail.Trim(), subjectMsg, ConfigurationManager.AppSettings["EmailNotification"], contractorMsg, true);
                                BWSession.emailStartedAndSent = true;
                            }
                            catch (Exception)
                            {
                                // TODO: log error in database that email couldn't be sent???
                            }
                        }
                    }

                    // commit contractorMsg to database
                    string insertsql = "INSERT INTO EmailData (DateSent, AreaID, CityID, CategoryID, ContractorID, FirstName, LastName, Address, City, Zip, Email, Phone, Phone2, WorkType, Message, IsPPC) " +
                        "VALUES (@DATESENT, @AREAID, @CITYID, @CATEGORYID, @CONTRACTORID, @FIRSTNAME, @LASTNAME, @ADDRESS, @CITY, @ZIP, @EMAIL, @PHONE, @PHONE2, @WORKTYPE, @MESSAGE, @PPC)";
                    dataAccessHelper.Data.ExecuteNonQuery(insertsql,
                        new SqlParameter("@DATESENT", DateTime.Now),
                        new SqlParameter("@AREAID", areaID),
                        new SqlParameter("@CITYID", cityID),
                        new SqlParameter("@CATEGORYID", catid),
                        new SqlParameter("@CONTRACTORID", int.Parse(EmailContractorID.Value)),
                        new SqlParameter("@FIRSTNAME", name),
                        new SqlParameter("@LASTNAME", lastname),
                        new SqlParameter("@ADDRESS", addr),
                        new SqlParameter("@CITY", City.Text.Trim()),
                        new SqlParameter("@ZIP", ZipForMail.Text.Trim()),
                        new SqlParameter("@EMAIL", email),
                        new SqlParameter("@PHONE", phone),
                        new SqlParameter("@PHONE2", phone2),
                        new SqlParameter("@WORKTYPE", worktype),
                        new SqlParameter("@PPC", isppc),
                        new SqlParameter("@MESSAGE", Message.Text.Trim()));
                }

                // Hide the email form and show the "Email Sent" div
                ModalEmailForm.Visible = false;
                ReqPanel.Visible = false;
                EmailComplete.Visible = true;
            }

            // If this style isn't set, the modal popup won't display
            ModalEmailForm.Style.Value = "display:block";
            ReqPanel.Visible = true;
            SendEmail.Show();
        }

        protected void MultipleEMailButton_Click(object sender, EventArgs e)
        {
            //set this to false so we know they started to send email
            BWSession.emailStartedAndSent = false;
            ModalEmailForm.Visible = false;
            EmailComplete.Visible = false;
            ChooseEmailPanel.Visible = true;
            errornote.Visible = false;
            MultipleEmailIDs.Value = "";


            SendEmail.Show();
        }

        protected void EmailContractorList_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                int alertcnt = 0;

                DataTable dt = (DataTable)EmailContractorList.DataSource;
                DataRow row = dt.Rows[reccount];

                HiddenField cid = (HiddenField)e.Item.FindControl("HiddenListID");
                cid.Value = row["cid"].ToString().Trim();

                Label name = (Label)e.Item.FindControl("ContractorLabel");
                Label ast = (Label)e.Item.FindControl("Asterisk");
                name.Text = row["name"].ToString();

                Image img = (Image)e.Item.FindControl("EmailAlert");
                CheckBox chk = (CheckBox)e.Item.FindControl("ContractorEmailID");

                if (row["email"].ToString().Trim() != "")
                {
                    img.Visible = false;
                    chk.Visible = true;
                    chk.Checked = true;
                    ast.Visible = false;
                }
                else
                {
                    name.Attributes.Add("style", "color:#909090;");
                    img.Visible = true;
                    chk.Visible = false;
                    ast.Visible = true;
                    alertcnt++;
                }

                if (alertcnt > 0)
                    alertnote.Visible = true;

                reccount++;
            }
        }

        protected void CloseEmail_Click(object sender, EventArgs e)
        {
            // Hide the sent div and show the form
            ModalEmailForm.Visible = true;
            EmailComplete.Visible = false;
            ReqPanel.Visible = false;
        }

        protected void SingleEmailButton_Click(object sender, EventArgs e)
        {
            Button singleMail = (Button)sender;
            EmailContractorName.Text = singleMail.CommandArgument;

            //set this to false so we know they started to send email
            BWSession.emailStartedAndSent = false;
            MultipleEmailIDs.Value = "";

            if (singleMail.CommandArgument != null && singleMail.CommandArgument != "")
            {
                string[] comarg = singleMail.CommandArgument.Split('|');
                EmailContractorID.Value = comarg[2].Trim();
                EmailContractorName.Text = comarg[0].Trim();
                ContractorEmail.Value = comarg[1].Trim();
            }
            ModalEmailForm.Style.Value = "display:block";

            //TODO: Clear all fields every time the form is called to show
            Name.Text = "";
            LastName.Text = "";
            Address.Text = "";
            City.Text = "";
            ZipForMail.Text = "";
            Email.Text = "";
            FormPhone.Text = "";
            FormPhone2.Text = "";
            WorkType.Text = "";
            Message.Text = "";
            ReqPanel.Visible = false;
            errornote.Visible = false;
            EmailComplete.Visible = false;
            ChooseEmailPanel.Visible = false;
            ModalEmailForm.Visible = true;

            SendEmail.Show();
            Name.Focus();
        }

        protected void TopList_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                List<BlogPostDL> posts = (List<BlogPostDL>)MostPopularArticles.DataSource;

                HyperLink title = (HyperLink)e.Item.FindControl("PostTitle");
                title.Text = posts[_postIndex].Title;
                title.NavigateUrl = "/blog/post/" + posts[_postIndex].UrlTitle;
                HyperLink image = (HyperLink)e.Item.FindControl("PostImage");
                image.ImageUrl = String.Concat("/blogfiles/assets/media/", posts[_postIndex].ImagePath);
                image.NavigateUrl = String.Concat("/blog/post/", posts[_postIndex].UrlTitle);
                _postIndex++;
            }
        }

        private int _yearIndex;
        private int _monthIndex;

        protected void ArchiveList_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                Dictionary<string, Dictionary<string, string>> archiveList = (Dictionary<string, Dictionary<string, string>>)ArchiveList.DataSource;

                Label year = (Label)e.Item.FindControl("YearValue");
                year.Text = archiveList.Keys.ElementAt(_yearIndex);
                ListView monthly = (ListView)e.Item.FindControl("MonthlyArchiveList");
                monthly.DataSource = archiveList[year.Text];
                _monthIndex = 0;
                monthly.DataBind();
                _yearIndex++;
            }
        }

        protected void MonthlyArchiveList_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListView monthlyList = (ListView)e.Item.Parent.Parent;
                Dictionary<string, string> monthly = (Dictionary<string, string>)monthlyList.DataSource;
                HyperLink month = (HyperLink)e.Item.FindControl("MonthValue");
                string key = monthly.Keys.ElementAt(_monthIndex);
                Label year = (Label)e.Item.Parent.Parent.Parent.FindControl("YearValue");
                month.Text = String.Concat(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt16(key)), " (", monthly[key], ")");
                month.NavigateUrl = String.Concat(basedomain, "/blog/",
                                                  String.IsNullOrEmpty(strcity)
                                                      ? String.Empty
                                                      : String.Concat(strcity, "/"), key, "/", year.Text);
                _monthIndex++;
            }
        }

        protected void RequestATopic_OnClick(object sender, EventArgs e)
        {
            EmailContractorNameInReq.Text = String.Empty;

            //set this to false so we know they started to send email
            BWSession.emailStartedAndSent = false;

            RequestATopicEmail.Style.Value = "display:block";

            NameInReq.Text = String.Empty;
            LastNameInReq.Text = String.Empty;
            EmailInReq.Text = String.Empty;
            SbjInReq.Text = String.Empty;
            MessageInReq.Text = String.Empty;
            ReqPanelInReq.Visible = false;
            EmailCompleteRequest.Visible = false;
            RequestATopicEmail.Visible = true;

            SendRequestEmail.Show();
            NameInReq.Focus();
        }

        protected void Cancel_RequestEmailClick(object sender, EventArgs e)
        {
            // Hide the sent div and show the form
            RequestATopicEmail.Visible = true;
            EmailCompleteRequest.Visible = false;
            ReqPanelInReq.Visible = false;
        }

        protected void OK_RequestEmailClick(object sender, EventArgs e)
        {
            bool error = false;
            string name = NameInReq.Text.Trim();
            string lastname = LastNameInReq.Text.Trim();
            string email = EmailInReq.Text.Trim();
            string subject = SbjInReq.Text.Trim();

            #region Error Checking
            ReqNameInReq.Visible = false;
            ReqLastNameInReq.Visible = false;
            ReqEmailInReq.Visible = false;
            ReqSbjInReq.Visible = false;
            ReqMessageInReq.Visible = false;
            if (name == "")
                ReqNameInReq.Visible = true;
            if (lastname == "")
                ReqLastNameInReq.Visible = true;
            if (email == "" || !Global.IsEmail(email))
                ReqEmailInReq.Visible = true;
            if (subject == "")
                ReqSbjInReq.Visible = true;

            error = ReqNameInReq.Visible || ReqLastNameInReq.Visible || ReqEmailInReq.Visible || ReqSbjInReq.Visible;

            ReqNameInReq.Visible = false;
            ReqLastNameInReq.Visible = false;
            ReqEmailInReq.Visible = false;
            ReqSbjInReq.Visible = false;
            ReqMessageInReq.Visible = false;

            #endregion // Error Checking

            if (!error)
            {
                string msg = "<span style=\"font-family:Arial; font-size:12px;\"><strong>Name: </strong>" + name + " " + lastname + "<br/><strong>Email: </strong>" + email + "<br>";
                msg += "<strong>Message: </strong>" + Message.Text.Trim();
                msg += "</span>";

                //Parse comma separated list of emails - check if we are sending to multiples first
                if (!BWSession.emailStartedAndSent)
                {
                    try
                    {
                        Global.SendEmailNotification("Marketing@ebscoresearch.com", subject, email, msg, true);
                        BWSession.emailStartedAndSent = true;
                    }
                    catch (Exception)
                    {
                        // TODO: log error in database that email couldn't be sent???
                    }
                }

                // Hide the email form and show the "Email Sent" div
                RequestATopicEmail.Visible = false;
                ReqPanelInReq.Visible = false;
                EmailCompleteRequest.Visible = true;
            }

            // If this style isn't set, the modal popup won't display
            RequestATopicEmail.Style.Value = "display:block";
            ReqPanelInReq.Visible = true;
            SendRequestEmail.Show();
        }

        protected void CloseRequestEmail_Click(object sender, EventArgs e)
        {
            // Hide the sent div and show the form
            RequestATopicEmail.Visible = true;
            EmailCompleteRequest.Visible = false;
            ReqPanelInReq.Visible = false;
        }

        protected void Continue_OutOfMarket(object sender, EventArgs e)
        {
            bprPreferences = AppCookies.CreateInstance();

            var zipCode = PopupSorryZipcode.Value;
            bprPreferences.OutOfMarketZip = zipCode;

            bodytag.Attributes["onload"] = null;
            LocationHelper.GetZip(Request, Response, Session);
            //Response.Redirect(Request.RawUrl);

            Response.Redirect("/blog?redirect=true");
            if (Session.Count > 0 && !String.IsNullOrEmpty(Session["areaname"] as String))
            {
                CurrentLocation.Text = Session["areaname"].ToString();
                //PageNavPanel.Visible = true;
                //GlobalNavPanel.Visible = false;
            }
            else
            {
                CurrentLocation.Text = "Not available";
                //Show global navigation
                //PageNavPanel.Visible = false;
                //GlobalNavPanel.Visible = true;
            }


            LocalBestPicks.Visible = false;
            if (postid != "")
            {
                RequestATopic.Visible = false;
                BottomAdds.Visible = true;
            }
            TopAdds.Visible = true;
            SideBarAdds.Visible = true;
        }
    }
}
