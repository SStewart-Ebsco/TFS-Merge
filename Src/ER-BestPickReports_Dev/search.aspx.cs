using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ER_BestPickReports_Dev.App_Code.Models;

namespace ER_BestPickReports_Dev
{
    public delegate void ShowSectionDelegate();

    public partial class search : BasePage
    {
        string keywordPhrase = "";
        string areaID = "";
        string cityID = "";
        int moreCutoff = 100;
        string moreSection = "";
        int countBlog = 0;
        int countArticle = 0;
        int countContent = 0;
        string[] keywords = null;
        string areaUrlName = "";
        string cityUrlName = "";
        bool allresults = false;
        List<string> sections = new List<string>() { "CategoryPanel", "CompanyPanel", "BlogPanel", "ArticlePanel", "OtherPanel" };
        List<ShowSectionDelegate> showSections = new List<ShowSectionDelegate>();
        public string basedomain = "";
        bool showall = false;

        private AppCookies bprPreferences = AppCookies.CreateInstance();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Set Domain
            if (HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToString() != "localhost")
                basedomain = "http://" + HttpContext.Current.Request.ServerVariables["SERVER_NAME"];

            //Set variables to cookie value if it exists
            if (bprPreferences.Exists())
            {
                areaID = bprPreferences.AreaId.ToString();
                cityID = bprPreferences.CityId.ToString();
            }
            else
            {
                // TODO!!!! Need to handle a direct link and there is no cookie
                Response.Redirect(basedomain + "/");
            }

            // Set cityid and areaid based on ShowResultsFor value
            if (IsPostBack)
            {
                string[] vals = ShowResultsFor.SelectedValue.Split(new char[] { ':' });
                if (vals[1] != "0" && vals[0] != "0")
                {
                    cityID = vals[0];
                    areaID = vals[1];
                }
                else
                {
                    showall = true;
                }
            }

            //Get city and area url values from ids
            cityUrlName = DataAccessHelper.GetCityUrlFromID(cityID);
            areaUrlName = DataAccessHelper.GetAreaUrlFromID(areaID);

            // Make a list of functions to call depending on FilterBy.  No need to hit
            // the database for all of these if only one section is being displayed.
            showSections.Add(new ShowSectionDelegate(ShowCategoryResults));
            showSections.Add(new ShowSectionDelegate(ShowCompanyResults));
            showSections.Add(new ShowSectionDelegate(ShowBlogResults));
            showSections.Add(new ShowSectionDelegate(ShowArticleResults));
            showSections.Add(new ShowSectionDelegate(ShowOtherResults));

            #region Show simple or all

            if (showall)
            {
                // View all results
                CategorySimple.Visible = false;
                CategoryAll.Visible = true;
                CompanySimple.Visible = false;
                CompanyAll.Visible = true;
            }
            else
            {
                // View simple results
                CategorySimple.Visible = true;
                CategoryAll.Visible = false;
                CompanySimple.Visible = true;
                CompanyAll.Visible = false;
            }

            # endregion // Show simple or all

            #region SetSearchValues

            // Set keyword to search on
            keywordPhrase = "";
            if (Request["keyword"].Trim().Replace("'", "").ToLower() != "keyword")
                keywordPhrase = Request["keyword"].Trim().Replace("'", "''").ToLower();
            keywords = keywordPhrase.Split(' ');

            if (keywords[0] == "")
            {
                // Please enter something to search on
                CategoryPanel.Visible = false;
                CompanyPanel.Visible = false;
                ArticlePanel.Visible = false;
                OtherPanel.Visible = false;
                
                ErrorText.Text = "Please enter a search keyword.";
                ErrorPanel.Visible = true;
                return;
            }
            else
            {
                //Save search word
                string ssql = "INSERT INTO SearchTerms (SearchTerm, SearchDate) VALUES (@TERM, @DATE)";
                DataAccessHelper.Data.ExecuteNonQuery(ssql,
                    new SqlParameter("@TERM", keywordPhrase),
                    new SqlParameter("@DATE", DateTime.Now));

                ErrorPanel.Visible = false;
            }

            // Set the section to display if the user clicked on 'More'
            moreSection = Request["moresection"] ?? "";

            #endregion

            // Set what the user searched for
            string s = "";
            foreach (string word in keywords)
            {
                s += "'" + word + "' AND ";
            }
            s = s.Substring(0, s.Length - 5);
            KeywordTitle.Text = s.Replace("''", "'");

            if (!IsPostBack)
                FillShowResultsFor();

            ShowSections();
        }


        /// <summary>
        /// Show/hide sections depending on Filter By value
        /// </summary>
        private void ShowSections()
        {
            if (FilterBy.SelectedValue != "0")
            {
                // Show only the section selected in Filter By dropdown
                int sectionToShow = int.Parse(FilterBy.SelectedValue);
                for (int i = 0; i < sections.Count; i++)
                {
                    Panel p = (Panel)this.FindControl<Panel>(sections[i]);
                    if (i + 1 == sectionToShow)
                    {
                        p.Visible = true;
                        showSections[i](); // Call the "show" method for the type of section
                    }
                    else
                    {
                        p.Visible = false;
                    }
                }
            }
            else
            {
                // Show all sections
                for (int i = 0; i < sections.Count; i++)
                {
                    ((Panel)this.FindControl<Panel>(sections[i])).Visible = true;
                    showSections[i]();  // Call the "show" method for the type of section
                }
            }
        }

        private void FillShowResultsFor()
        {
            string sql = "SELECT ci.cityid, ci.displayname, ai.displayname, ci.displayname + ' / ' + ai.displayname name, ai.areaid " +
                        "FROM CityInfo ci, AreaInfo ai WHERE (ai.cityid = ci.cityid) " +
                        "ORDER BY ci.displayname, ai.displayname";
            using (SqlConnection conn = new SqlConnection(DataAccessHelper.ConnString))
            {
                SqlDataReader rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql);
                while (rdr.Read())
                    ShowResultsFor.Items.Add(new ListItem(rdr["name"].ToString(), rdr["cityid"].ToString() + ":" + rdr["areaid"].ToString()));
                rdr.Close();
                conn.Close();
            }
            //ShowResultsFor.Items.Add(new ListItem("All Areas", "0:0"));

            // Set initial value
            ShowResultsFor.SelectedValue = cityID + ":" + areaID;
        }


        private string GetCategoryListQuery()
        {
            string from_cc = " Info INNER JOIN CategoryCity ON Info.InfoID=CategoryCity.InfoID ";

            string from_ca = " Info INNER JOIN CategoryArea ON Info.InfoID=CategoryArea.InfoID ";

            string keywordWhere = "";
            string where = "";

            // Build WHERE clause for keywords
            foreach (string word in keywords)
            {
                if (word.Trim() != "")
                    keywordWhere += " ((Info.DisplayName LIKE '%" + word + "%') OR (Info.ShortDesc LIKE '%" + word + "%') OR (Info.MetaKey LIKE '%" + word + "%') OR (Info.LongDesc LIKE '%" + word + "%')) AND ";
            }

            // Include keywords
            if (keywordWhere != "")
            {
                keywordWhere = keywordWhere.Substring(0, keywordWhere.Length - "AND ".Length);
                if (where != "")
                    where += " AND ( " + keywordWhere + " ) ";
                else
                    where += " ( " + keywordWhere + " ) ";
            }

            if (where != "")
                where += " AND ";
          
            where += " Info.InfoType IN (1,5,6)";

            // Build the total SQL statement
            string select = "SELECT DISTINCT categoryid, displayname, urlname AS CategoryUrlName FROM";

            string sql = select + from_cc + " WHERE " + where + " AND (CategoryCity.CityID = @CITYID)";
            sql += " UNION " + select + from_ca + " WHERE " + where + " AND (CategoryArea.AreaID = @AREAID)";
            sql += " ORDER BY Info.DisplayName";

            return sql;
        }

        #region Category Lists

        private void ShowCategoryResults()
        {
            string sql = GetCategoryListQuery();

            // Build the total SQL statement
            //string sql = "SELECT DISTINCT categoryid, displayname, urlname AS CategoryUrlName FROM " + from + " WHERE " + where;
            using (SqlConnection conn = new SqlConnection(DataAccessHelper.ConnString))
            {
                SqlDataReader rdrCategory = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                    new SqlParameter("@CITYID", cityID),
                    new SqlParameter("@AREAID", areaID));
                CategorySimpleList.DataSource = rdrCategory;
                CategorySimpleList.DataBind();
                rdrCategory.Close();
                conn.Close();
            }

            ShowCategoryResultsAll();
            //Response.Write("CATEGORY: " + sql);
        }


        protected void CategorySimpleList_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                SqlDataReader rdr = (SqlDataReader)((ListView)sender).DataSource;
                HyperLink catlink = (HyperLink)e.Item.FindControl("CategoryLink");
                catlink.Text = rdr["displayname"].ToString();
                catlink.NavigateUrl = basedomain + "/" + rdr["CategoryUrlName"].ToString() + "/" + cityUrlName + "/" + areaUrlName;
            }
        }

        /// <summary>
        /// Populate the "all" category list
        /// </summary>
        private void ShowCategoryResultsAll()
        {
            string sql = GetCategoryListQuery();
            DataTable dtCategory = DataAccessHelper.Data.ExecuteDataset(sql,
                new SqlParameter("@AREAID", areaID),
                new SqlParameter("@CITYID", cityID)).Tables[0];
            CategoryAllList.DataSource = dtCategory;
            CategoryAllList.DataBind();
        }


        protected void CategoryAllList_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                Label catName = (Label)e.Item.FindControl("CategoryName");
                catName.Text = drv["displayname"].ToString();
                ListView stateList = (ListView)e.Item.FindControl("CategoryStateList");

                // Populate category states list
                string sql = "SELECT DISTINCT ai.cityid, statename categorystatedisplay FROM AreaInfo ai, CategoryCityAreaRel car " +
                        "WHERE (car.categoryid = @CATID) AND (ai.areaid = car.areaid) ORDER BY statename";
                //string sql = "SELECT DISTINCT categorystatedisplay FROM AreaInfo ai, CategoryAreaRel car " +
                //        "WHERE (car.categoryid = @CATID) AND (ai.areaid = car.areaid) GROUP BY categorystatedisplay";
                DataTable dtStates = DataAccessHelper.Data.ExecuteDataset(sql,
                    new SqlParameter("@CATID", drv["categoryid"].ToString())).Tables[0];
                stateList.DataSource = dtStates;
                stateList.DataBind();
            }
        }


        /// <summary>
        /// Populate the "all" categories list states/area links
        /// </summary>
        protected void CategoryStateList_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                Label catName = (Label)e.Item.FindControl("StateName");
                catName.Text = drv["categorystatedisplay"].ToString() + ":";

                // Get all the areas associated with the CategoryStateDisplay
                string sql = "SELECT * FROM AreaInfo WHERE (cityid = @CITYID) ORDER BY displayname";
                DataTable dtAreas = DataAccessHelper.Data.ExecuteDataset(sql,
                    new SqlParameter("@CITYID", drv["cityid"].ToString())).Tables[0];

                string s = "";
                // Build list of area links
                foreach (DataRow row in dtAreas.Rows)
                    s += String.Format("<a href='{0}'>{1}</a> | ", "link", row["displayname"].ToString());
                s = s.Substring(0, s.Length - 3);

                Literal areaLinks = (Literal)e.Item.FindControl("AreaLinks");
                areaLinks.Text = s;
            }
        }

        #endregion // Category Lists

        #region Company Lists

        private string GetCompanyListQuery()
        {
            string from_ccc = " Info INNER JOIN CategoryInfo AS ci INNER JOIN ContractorCategory AS cc ON ci.CategoryID = cc.CategoryID INNER JOIN ContractorCategoryArea AS cca INNER JOIN " +
                      "ContractorCategoryCity AS ccc ON cca.ContractorCategoryID = ccc.ContractorCategoryID ON cc.ContractorCategoryID = ccc.ContractorCategoryID ON Info.InfoID = ccc.InfoID ";

            string from_cca = " Info INNER JOIN CategoryInfo AS ci INNER JOIN ContractorCategory AS cc ON ci.CategoryID = cc.CategoryID INNER JOIN ContractorCategoryArea AS cca INNER JOIN " +
                      "ContractorCategoryCity AS ccc ON cca.ContractorCategoryID = ccc.ContractorCategoryID ON cc.ContractorCategoryID = ccc.ContractorCategoryID ON Info.InfoID = cca.InfoID ";

            string keywordWhere = "";
            string where = "";

            // Build WHERE clause for keywords
            foreach (string word in keywords)
            {
                if (word.Trim() != "")
                {
                    if (allresults)
                        keywordWhere += " ((Info.DisplayName LIKE '%" + word + "%') OR (Info.LongDesc LIKE '%" + word + "%') OR (Info.MetaKey LIKE '%" + word + "%')) AND ";
                    else
                        keywordWhere += " ((Info.DisplayName LIKE '%" + word + "%') AND (cca.AreaID = @AREAID) AND (ccc.CityID = @CITYID) OR (cca.AreaID = @AREAID) AND (ccc.CityID = @CITYID) AND (dbo.Info.LongDesc LIKE '%" + word + "%') OR (cca.AreaID = @AREAID) AND (ccc.CityID = @CITYID) AND (dbo.Info.MetaKey LIKE '%" + word + "%')) AND ";
                }
            }

            // Include keywords
            if (keywordWhere != "")
            {
                keywordWhere = keywordWhere.Substring(0, keywordWhere.Length - "AND ".Length);
                if (where != "")
                    where += " AND ( " + keywordWhere + " ) ";
                else
                    where += " ( " + keywordWhere + " ) ";
            }

            // Build the total SQL statement
            string select = "SELECT DISTINCT Info.UrlName, Info.DisplayName, ci.UrlName AS CategoryUrlName, ci.DisplayName AS CategoryName, cc.ContractorID, ccc.CityID, cc.CategoryID FROM";

            //string sql = select + from_cc + " WHERE " + where;
            //sql += " UNION " + select + from_ccc + " WHERE " + where;
            //sql += " UNION " + select + from_cca + " WHERE " + where;
            //sql += " ORDER BY Info.DisplayName";

            string sql = select + from_ccc + " WHERE " + where;
            sql += " UNION " + select + from_cca + " WHERE " + where;
            sql += " ORDER BY Info.DisplayName";

            return sql;
        }


        private void ShowCompanyResults()
        {
            string sql = GetCompanyListQuery();

            using (SqlConnection conn = new SqlConnection(DataAccessHelper.ConnString))
            {
                SqlDataReader rdrCompanies = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                    new SqlParameter("@AREAID", areaID),
                    new SqlParameter("@CITYID", cityID));
                CompanySimpleList.DataSource = rdrCompanies;
                CompanySimpleList.DataBind();
                rdrCompanies.Close();

                conn.Close();
            }
            //CompanySimple.Visible = true;
            ShowCompanyResultsAll();
        }


        protected void CompanySimpleList_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                SqlDataReader rdr = (SqlDataReader)((ListView)sender).DataSource;
                Literal catName = (Literal)e.Item.FindControl("CategoryName");
                catName.Text = rdr["categoryname"].ToString();
                HyperLink clink = (HyperLink)e.Item.FindControl("CompanyLink");
                clink.Text = rdr["displayname"].ToString();
                clink.NavigateUrl = basedomain + "/" + rdr["CategoryUrlName"].ToString() + "/" + cityUrlName + "/" + areaUrlName + "/" + rdr["urlname"].ToString();
            }
        }

        /// <summary>
        /// Populate the "all" company list
        /// </summary>
        private void ShowCompanyResultsAll()
        {
            string sql = GetCompanyListQuery();
            DataTable dtCompany = DataAccessHelper.Data.ExecuteDataset(sql,
                                    new SqlParameter("@AREAID", areaID),
                                    new SqlParameter("@CITYID", cityID)).Tables[0];
            CompanyAllList.DataSource = dtCompany;
            CompanyAllList.DataBind();
        }


        protected void CompanyAllList_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                Label companyName = (Label)e.Item.FindControl("CompanyName");
                companyName.Text = drv["displayname"].ToString();
                ListView catList = (ListView)e.Item.FindControl("CompanyCategoryList");

                // Populate company category list
                string sql = "SELECT DISTINCT cc.contractorid, cc.categoryid, ci.displayname CategoryName " +
                        "FROM ContractorCategory cc, CategoryInfo ci, ContractorCategoryArea cca WHERE " +
                        "(ci.categoryid = cc.categoryid) AND (cca.contractorcategoryid = cc.contractorcategoryid) AND " +
                        "(cc.contractorid = @CID) ORDER BY categoryname";
                DataTable dtStates = DataAccessHelper.Data.ExecuteDataset(sql,
                    new SqlParameter("@CID", drv["contractorid"].ToString())).Tables[0];
                catList.DataSource = dtStates;
                catList.DataBind();
            }
        }


        /// <summary>
        /// Populate the "all" categories list area links
        /// </summary>
        protected void CompanyCategoryList_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                Label catName = (Label)e.Item.FindControl("CategoryName");
                catName.Text = drv["categoryname"].ToString() + ":";

                // Get all the areas associated with the company's category
                string sql = "SELECT DISTINCT cc.contractorid, cca.areaname, cca.areaid " +
                        "FROM ContractorCategory cc, ContractorCategoryAreaRel cca WHERE " +
                        "(cca.contractorcategoryid = cc.contractorcategoryid) AND " +
                        "(cc.contractorid = @CID) ORDER BY areaname";
                DataTable dtAreas = DataAccessHelper.Data.ExecuteDataset(sql,
                    new SqlParameter("@CID", drv["contractorid"].ToString())).Tables[0];

                string s = "";
                // Build list of area links
                foreach (DataRow row in dtAreas.Rows)
                    s += String.Format("<a href='{0}'>{1}</a> | ", "link", row["areaname"].ToString());
                s = s.Substring(0, s.Length - 3);

                Literal areaLinks = (Literal)e.Item.FindControl("AreaLinks");
                areaLinks.Text = s;
            }
        }

        #endregion // Company Lists


        private void ShowArticleResults()
        {
            string keywordWhere = "";
            string where = "";

            // Build WHERE clause for keywords
            foreach (string word in keywords)
            {
                if (word.Trim() != "")
                    keywordWhere += " ((ta.body LIKE '%" + word + "%') OR (ta.title LIKE '%" + word + "%')) AND ";
            }

            // Include keywords
            if (keywordWhere != "")
            {
                keywordWhere = keywordWhere.Substring(0, keywordWhere.Length - "AND ".Length);
                if (where != "")
                    where += " AND ( " + keywordWhere + " ) ";
                else
                    where += " ( " + keywordWhere + " ) ";
            }

            // Build the total SQL statement
            string sql = "SELECT ta.articleid, ta.title, ta.urltitle, ci.urlname as categoryurlname FROM TipArticle ta, CategoryInfo ci, CategoryArea ca WHERE " +
                "(ci.categoryid = ta.categoryid) AND (ci.categoryid = ca.categoryid) AND (ca.areaid = @AREAID) AND " +
                where + " ORDER BY ta.publishdate desc";
            using (SqlConnection conn = new SqlConnection(DataAccessHelper.ConnString))
            {
                SqlDataReader rdrArticle = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                    new SqlParameter("@AREAID", areaID));
                ArticleList.DataSource = rdrArticle;
                ArticleList.DataBind();
                rdrArticle.Close();

                conn.Close();
            }

            //Response.Write("ARTICLE: " + sql);
        }


        protected void ArticleList_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                SqlDataReader rdr = (SqlDataReader)((ListView)sender).DataSource;
                HyperLink catName = (HyperLink)e.Item.FindControl("ArticleLink");
                catName.Text = rdr["title"].ToString();
                catName.NavigateUrl = basedomain + "/" + rdr["categoryurlname"] + "/tips/" + rdr["urltitle"].ToString();
            }
        }


        private void ShowBlogResults()
        {
            string keywordWhere = "";
            string where = "";

            // Build WHERE clause for keywords
            foreach (string word in keywords)
            {
                if (word.Trim() != "")
                    keywordWhere += " ((body LIKE '%" + word + "%') OR (title LIKE '%" + word + "%') OR (summary LIKE '%" + word + "%')) AND ";
            }

            // Include keywords
            if (keywordWhere != "")
            {
                keywordWhere = keywordWhere.Substring(0, keywordWhere.Length - "AND ".Length);
                if (where != "")
                    where += " AND ( " + keywordWhere + " ) ";
                else
                    where += " ( " + keywordWhere + " ) ";
            }

            // Build the total SQL statement
            string sql = "SELECT * FROM BlogPosts WHERE " +
                "(PublishDate <= @DATE) AND " +
                where + " ORDER BY publishdate desc";
            using (SqlConnection conn = new SqlConnection(DataAccessHelper.ConnString))
            {
                SqlDataReader rdrBlog = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                    new SqlParameter("@DATE", DateTime.Now));
                BlogList.DataSource = rdrBlog;
                BlogList.DataBind();
                rdrBlog.Close();

                conn.Close();
            }

            //Response.Write("ARTICLE: " + sql);
        }

        protected void BlogList_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                countBlog++;
                if (countBlog > moreCutoff)
                {
                    HtmlGenericControl listitem = (HtmlGenericControl)e.Item.FindControl("ListItem");
                    listitem.Visible = false;
                    return;
                }

                SqlDataReader rdr = (SqlDataReader)BlogList.DataSource;
                HyperLink link = (HyperLink)e.Item.FindControl("PostLink");

                link.NavigateUrl = basedomain + "/blog/post/" + rdr["urltitle"].ToString();
            }
        }

        private void ShowOtherResults()
        {
            string keywordWhere = "";
            string where = "";

            // Build WHERE clause for keywords
            foreach (string word in keywords)
            {
                if (word.Trim() != "")
                    keywordWhere += " ((pagecontent LIKE '%" + word + "%') OR (MetaTitle LIKE '%" + word + "%') OR (MetaDesc LIKE '%" + word + "%') OR (MetaKeywords LIKE '%" + word + "%')) AND ";
            }

            // Include keywords
            if (keywordWhere != "")
            {
                keywordWhere = keywordWhere.Substring(0, keywordWhere.Length - "AND ".Length);
                if (where != "")
                    where += " AND ( " + keywordWhere + " ) ";
                else
                    where += " ( " + keywordWhere + " ) ";
            }

            // Build the total SQL statement
            string sql = "SELECT pageid, pagename, urltitle FROM pagecontent WHERE " + where + " AND IsMobileApp=0 ORDER BY pagename";
            using (SqlConnection conn = new SqlConnection(DataAccessHelper.ConnString))
            {
                SqlDataReader rdrContent = DataAccessHelper.Data.ExecuteDatareader(conn, sql);
                ContentList.DataSource = rdrContent;
                ContentList.DataBind();
                rdrContent.Close();

                conn.Close();
            }

            //Response.Write("ARTICLE: " + sql);
        }

        protected void ContentList_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                countArticle++;
                if (countContent > moreCutoff)
                {
                    HtmlGenericControl listitem = (HtmlGenericControl)e.Item.FindControl("ListItem");
                    listitem.Visible = false;
                    return;
                }

                SqlDataReader rdr = (SqlDataReader)ContentList.DataSource;
                HyperLink link = (HyperLink)e.Item.FindControl("ContentLink");
                link.NavigateUrl = basedomain + "/content/" + rdr["urltitle"].ToString();
            }
        }
    }


    public class ShowResultsForItem
    {
        public string cityid = "";
        public string areaid = "";
    }
}