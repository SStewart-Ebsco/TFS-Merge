using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using ER_BestPickReports_Dev.App_Code;
using ER_BestPickReports_Dev.App_Code.Models;

namespace ER_BestPickReports_Dev
{
    public partial class area : BasePage
    {
        public string basedomain = "";
        int cityID = 0;
        int areaID = 0;
        string strcity = "";
        string strarea = "";

        private AppCookies bprPreferences = AppCookies.CreateInstance();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Get value from route handler
            cityID = BWSession.tempCityID;
            areaID = BWSession.tempAreaID;
            strcity = (HttpContext.Current.Items["city"].ToString() == "" || HttpContext.Current.Items["city"].ToString() == null) ? "" : HttpContext.Current.Items["city"].ToString();
            strarea = (HttpContext.Current.Items["area"].ToString() == "" || HttpContext.Current.Items["area"].ToString() == null) ? "" : HttpContext.Current.Items["area"].ToString();

            //Set cityid and areaid on master page
            ((ER_BestPickReports_Dev.SiteMaster)Master).cityID = cityID;
            ((ER_BestPickReports_Dev.SiteMaster)Master).areaID = areaID;


			SliderGallery sliderGallery = new SliderGallery(cityID, areaID);
            ((ER_BestPickReports_Dev.SiteMaster)Master).AdjustSlider(sliderGallery);

            //Hide form errors on load
            NewsletterErrorPanel.Visible = false;
            GuideErrorPanel.Visible = false;

            //Set Domain
            if (HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToString() != "localhost")
                basedomain = "http://" + HttpContext.Current.Request.ServerVariables["SERVER_NAME"];

            //Hide items for Boston and Philly markets
            if (DataAccessHelper.GetCityUrlFromID(cityID.ToString()) == "philadelphia" || DataAccessHelper.GetCityUrlFromID(cityID.ToString()) == "boston")
            {
                RequestPanel.Visible = false;
            }

            string sql = "";

            using (SqlConnection conn = new SqlConnection(DataAccessHelper.ConnString))
            {
                conn.Open();

                //Show page content
                sql = "SELECT Area.AreaID, Area.CityID, CityInfo.MetaTitle AS CityMetaTitle, CityInfo.MetaDesc AS CityMetaDesc, CityInfo.MetaKey AS CityMetaKey, " +
                    "CityInfo.ShortDesc AS CityRibbonText, CityInfo.LongDesc AS CityBlogText, CityInfo.MobileYear AS DataYear, AreaInfo.MetaTitle AS AreaMetaTitle, AreaInfo.MetaDesc AS AreaMetaDesc, AreaInfo.MetaKey AS AreaMetaKey, " +
                    "AreaInfo.ShortDesc AS AreaRibbonText, AreaInfo.LongDesc AS AreaBlogText, AreaInfo.UrlName AS AreaUrlName, AreaInfo.DisplayName AS AreaDisplayName, CityInfo.UrlName AS CityUrlName, " +
                    "CityInfo.DisplayName AS CityDisplayName FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON Area.AreaID = AreaInfo.AreaID " +
                    "WHERE (AreaInfo.AreaID = @AREAID) AND (CityInfo.CityID = @CITYID)";

                SqlDataReader rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@AREAID", areaID),
                        new SqlParameter("@CITYID", cityID));

                if (rdr.Read())
                {
                    string ryear = DateTime.Today.Year.ToString();

                    if (rdr["DataYear"].ToString() != "0")
                        ryear = rdr["DataYear"].ToString();

                    RibbonImage.ImageUrl = "/images/" + ryear + "_ribbon_global_metro.png";
                    RibbonImage.AlternateText = ryear + " EBSCO Research Best Pick";
                    RibbonImage.ToolTip = ryear + " EBSCO Research Best Pick";

                    if (rdr["AreaRibbonText"].ToString() != "")
                        RibbonText.Text = rdr["AreaRibbonText"].ToString();
                    else
                        RibbonText.Text = rdr["CityRibbonText"].ToString();

                    //BlogCityLink.NavigateUrl = basedomain + "/blog/" + rdr["CityURLName"].ToString();

                    if (rdr["AreaBlogText"].ToString() != "")
                        BlogText.Text = rdr["AreaBlogText"].ToString();
                    else
                        BlogText.Text = rdr["CityBlogText"].ToString();

                    FeedCityLink.NavigateUrl = basedomain + "/blog/" + rdr["CityURLName"].ToString();
                    FeedCityName.Text = rdr["CityDisplayName"].ToString();

                    //Check for info at the area level - if empty, use city level info
                    string strMetaTitle = "";
                    string strMetaKey = "";
                    string strMetaDesc = "";

                    if (rdr["AreaMetaTitle"].ToString() != "")
                        strMetaTitle = rdr["AreaMetaTitle"].ToString();
                    else
                        strMetaTitle = rdr["CityMetaTitle"].ToString();

                    if (rdr["AreaMetaKey"].ToString() != "")
                        strMetaKey = rdr["AreaMetaKey"].ToString();
                    else
                        strMetaKey = rdr["CityMetaKey"].ToString();

                    if (rdr["AreaMetaDesc"].ToString() != "")
                        strMetaDesc = rdr["AreaMetaDesc"].ToString();
                    else
                        strMetaDesc = rdr["CityMetaDesc"].ToString();

                    //Add meta info
                    HtmlMeta meta1;
                    HtmlMeta meta2;

                    meta2 = new HtmlMeta();
                    meta2.Name = "keywords";
                    meta2.Content = strMetaKey;
                    Page.Header.Controls.Add(meta2);
                    Page.Header.Controls.Add(new LiteralControl("\n"));

                    meta1 = new HtmlMeta();
                    meta1.Name = "description";
                    meta1.Content = strMetaDesc;
                    Page.Header.Controls.Add(meta1);
                    Page.Header.Controls.Add(new LiteralControl("\n"));

                    Page.Title = strMetaTitle;

                    //Set cookie for city/area preference
                    bprPreferences.CityId = int.Parse(cityID.ToString());
                    bprPreferences.CityName = rdr["CityDisplayName"].ToString();
                    bprPreferences.CityUrlName = rdr["CityURLName"].ToString();
                    bprPreferences.AreaId = int.Parse(areaID.ToString());
                    bprPreferences.AreaName = rdr["AreaDisplayName"].ToString();
                    bprPreferences.AreaUrlName = rdr["AreaURLName"].ToString();
                    bprPreferences.SetExpiration(DateTime.Now.AddDays(365));
                }
                rdr.Close();

                conn.Close();
            }

            //Get homeowner testimonials
            sql = "SELECT TOP 20 * FROM HomeownerTestimonial WHERE (CityID = @CITYID) AND (PublishDate <= @DATE) ORDER BY NEWID()";

            DataTable allitems = DataAccessHelper.Data.ExecuteDataset(sql,
                new SqlParameter("@CITYID", cityID),
                new SqlParameter("@DATE", DateTime.Now)).Tables[0];

            TList.DataSource = allitems;
            TList.DataBind();

            //// A stackSize of 0 will guarantee that you have 'numLists' lists and 
            //// that the items are distributed equally from left to right.
            //List<DataTable> lists = GetListData(allitems, 0, numLists);

            //// How many lists actually have data
            //for (int i = 1; i <= lists.Count; i++)
            //{
            //    if (lists[i - 1].Rows.Count > 0)
            //        dataCount++;
            //}

            //// Bind to the lists.  There could be fewer populated lists based on your settings.
            //for (int i = 1; i <= lists.Count; i++)
            //{
            //    currentList = i;
            //    ListView listView = FindUIControl(i);
            //    listView.DataSource = lists[i - 1];
            //    listView.DataBind();
            //}
        }

        ///// <summary>
        ///// Get the individual DataTable's that will fill the lists
        ///// </summary>
        //private List<DataTable> GetListData(DataTable allItems, int stackSize, int numLists)
        //{
        //    // Get the count of items that should go in each List
        //    List<int> itemsPerList = GetListCounts(allItems, stackSize, numLists);

        //    // The individual datatables that will populate each list
        //    List<DataTable> dataTables = new List<DataTable>();
        //    for (int i = 1; i <= numLists; i++)
        //        dataTables.Add(allItems.Clone());

        //    int tableNum = 0;
        //    int cnt = 0;
        //    // Populate the individual datasets
        //    for (int i = 0; i < allItems.Rows.Count; i++)
        //    {
        //        cnt++;
        //        dataTables[tableNum].ImportRow(allItems.Rows[i]);
        //        if (cnt == itemsPerList[tableNum])
        //        {
        //            cnt = 0;
        //            tableNum++;
        //        }

        //        // Jump out of the loop... we're done 
        //        if (i == allItems.Rows.Count - 1)
        //            break;
        //    }

        //    return dataTables;
        //}

        ///// <summary>
        ///// Returns an array of the number of items in each list
        ///// </summary>
        ///// <param name="stackSize">The preferred number of items in each list from left to right</param>
        ///// <param name="listCount">Number of lists to populate</param>
        //private List<int> GetListCounts(DataTable allitems, int stackSize, int listCount)
        //{
        //    List<int> itemsPerList = new List<int>();
        //    int totalItemCount = allitems.Rows.Count;

        //    // Build the list that will hold the number of items
        //    // for each list on screen
        //    for (int i = 1; i <= listCount; i++)
        //        itemsPerList.Add(0);

        //    // See how many items per list we are working with
        //    double cnt = (double)totalItemCount / (double)listCount;

        //    // If we have more than 'stackSize' items per list, then distribute them evenly
        //    if (cnt > stackSize)
        //        stackSize = 0;

        //    if (stackSize == 0)
        //    {
        //        // Distribute evenly to the left
        //        cnt = Math.Round(cnt, MidpointRounding.AwayFromZero);
        //        for (int i = 1; i < listCount; i++)
        //        {
        //            itemsPerList[i - 1] = (int)cnt;
        //            totalItemCount -= (int)cnt;
        //        }
        //        // Put the remaining in the last list
        //        if (totalItemCount > 0)
        //            itemsPerList[listCount - 1] = totalItemCount;
        //    }
        //    else
        //    {
        //        int listNum = 0;
        //        cnt = 0;

        //        // Stack at least 'stackSize' items in each list from left to right
        //        for (int i = 1; i <= allitems.Rows.Count; i++)
        //        {
        //            cnt++;
        //            itemsPerList[listNum]++;
        //            if (cnt == stackSize)
        //            {
        //                cnt = 0;
        //                listNum++;
        //            }
        //        }
        //    }

        //    return itemsPerList;
        //}


        //private ListView FindUIControl(int listNum)
        //{
        //    ListView listView = null;

        //    // To make this more generic the UI tree will have to be
        //    // traversed to find the controls.
        //    switch (listNum)
        //    {
        //        case 1:
        //            listView = ListView1;
        //            break;
        //        case 2:
        //            listView = ListView2;
        //            break;
        //        case 3:
        //            listView = ListView3;
        //            break;
        //        case 4:
        //            listView = ListView4;
        //            break;
        //        default:
        //            throw new Exception("Invalid control number");
        //    }

        //    return listView;
        //}

        protected void Newsletter_Submit_Click(object sender, EventArgs e)
        {
            //validate form
            if (Newsletter_FirstName.Text.Trim() == "" || Newsletter_LastName.Text.Trim() == "" || Newsletter_EMail.Text.Trim() == "")
            {
                NewsletterError.Text = "Please fill out the form completely.";
                NewsletterErrorPanel.Attributes.Add("style", "color: Red; margin-bottom:15px;");
                NewsletterErrorPanel.Visible = true;
                return;
            }

            if (!Global.IsEmail(Newsletter_EMail.Text.Trim()))
            {
                NewsletterError.Text = "Please enter a valid email address.";
                NewsletterErrorPanel.Attributes.Add("style", "color: Red; margin-bottom:15px;");
                NewsletterErrorPanel.Visible = true;
                return;
            }

            Global.SaveFormData(Newsletter_FirstName.Text.Trim(), Newsletter_MI.Text.Trim(), Newsletter_LastName.Text.Trim(), Newsletter_EMail.Text.Trim(), "", "", "", "", false, false, 1, areaID, cityID);

            //Clear form
            Newsletter_FirstName.Text = "";
            Newsletter_LastName.Text = "";
            Newsletter_MI.Text = "";
            Newsletter_EMail.Text = "";
            
            NewsletterError.Text = "Thank you for signing up.";
            NewsletterErrorPanel.Attributes.Add("style", "color: Green; margin-bottom:15px;");
            NewsletterErrorPanel.Visible = true;
        }

        protected void Guide_Submit_Click(object sender, EventArgs e)
        {
            //validate form
            if (Guide_FirstName.Text.Trim() == "" || Guide_LastName.Text.Trim() == "" || Guide_Email.Text.Trim() == "" || Guide_Address.Text.Trim() == "" || Guide_City.Text.Trim() == "" || Guide_State.Text.Trim() == "" || Guide_Zip.Text.Trim() == "")
            {
                GuideError.Text = "Please fill out the form completely.";
                GuideErrorPanel.Attributes.Add("style", "color: Red; margin-bottom:15px;");
                GuideErrorPanel.Visible = true;
                return;
            }

            if (!Global.IsEmail(Guide_Email.Text.Trim()))
            {
                GuideError.Text = "Please enter a valid email address.";
                GuideErrorPanel.Attributes.Add("style", "color: Red; margin-bottom:15px;");
                GuideErrorPanel.Visible = true;
                return;
            }

            Global.SaveFormData(Guide_FirstName.Text.Trim(), Guide_MI.Text.Trim(), Guide_LastName.Text.Trim(), Guide_Email.Text.Trim(), Guide_Address.Text.Trim(), Guide_City.Text.Trim(), Guide_State.Text.Trim(), Guide_Zip.Text.Trim(), Guide_Updates.Checked, false, 2, areaID, cityID);

            //Clear form
            Guide_FirstName.Text = "";
            Guide_MI.Text = "";
            Guide_LastName.Text = "";
            Guide_Email.Text = "";
            Guide_Address.Text = "";
            Guide_City.Text = "";
            Guide_State.Text = "";
            Guide_Zip.Text = "";
            Guide_Updates.Checked = false;
            //Guide_FutureEditions.Checked = false;
            
            GuideError.Text = "Thank you for requesting a guide.";
            GuideErrorPanel.Attributes.Add("style", "color: Green; margin-bottom:15px;");
            GuideErrorPanel.Visible = true;
        }
    }
}