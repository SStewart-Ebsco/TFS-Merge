using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Data;
using ER_BestPickReports_Dev.App_Code;
using ER_BestPickReports_Dev.App_Code.Models;
using ER_BestPickReports_Dev.Helpers;

namespace ER_BestPickReports_Dev
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        private const int NumLists = 4;

        private readonly DataAccessHelper _dataAccessHelper;
        private readonly AppCookies _bprPreferences;
        private readonly SiteMasterHelper _siteMasterHelper;

        private int _recIndex1 = 0;
        private int _recIndex2 = 0;
        private int _recIndex3 = 0;
        private int _recIndex4 = 0;

        public string basedomain = "";
        public int cityID = 0;
        public int areaID = 0;
        public string catid = "0";
        public string contractorid = "0";
		public string CoverImagePath = "";
        public bool setnofollow = true;

        public SiteMaster()
        {
            _dataAccessHelper = new DataAccessHelper();
            _bprPreferences = AppCookies.CreateInstance();
            _siteMasterHelper = new SiteMasterHelper(_bprPreferences, _dataAccessHelper);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Set Domain
            basedomain = _siteMasterHelper.GetBaseDomain();

            //Set button attributes for enter click
            SearchValue.Attributes.Add("onkeypress", "return clickEnterButton(event,'" + SearchButton.ClientID + "')");
            ZipCode.Attributes.Add("onkeypress", "return clickEnterButton(event,'" + ZipSearchButton.ClientID + "')");
            ZipCodeChange.Attributes.Add("onkeypress", "return clickEnterButton(event,'" + ZipCodeChangeButton.ClientID + "')");
            SearchZipBox.Attributes.Add("onkeypress", "return clickEnterButton(event,'" + SearchZipButton.ClientID + "')");

            //Set footer city links
            atlantalink.HRef = basedomain + "/atlanta";
            chicagolink.HRef = basedomain + "/chicago";
            dallaslink.HRef = basedomain + "/dallas";
            houstonlink.HRef = basedomain + "/houston";
            nvlink.HRef = basedomain + "/northern-virginia";
            marylandlink.HRef = basedomain + "/maryland";
            masslink.HRef = basedomain + "/boston";
            pennlink.HRef = basedomain + "/philadelphia";
            dclink.HRef = basedomain + "/washington-dc";
            bhamlink.HRef = basedomain + "/birmingham";

            //Set nofollow attribut on footer city links if this is not the global home page
            if (setnofollow)
            {
                atlantalink.Attributes.Add("rel", "nofollow");
                chicagolink.Attributes.Add("rel", "nofollow");
                dallaslink.Attributes.Add("rel", "nofollow");
                houstonlink.Attributes.Add("rel", "nofollow");
                nvlink.Attributes.Add("rel", "nofollow");
                marylandlink.Attributes.Add("rel", "nofollow");
                masslink.Attributes.Add("rel", "nofollow");
                pennlink.Attributes.Add("rel", "nofollow");
                dclink.Attributes.Add("rel", "nofollow");
                bhamlink.Attributes.Add("rel", "nofollow");
            }

            if (areaID <= 0 && cityID <= 0)
            {
                areaID = _bprPreferences.AreaId;
                cityID = _bprPreferences.CityId;
            }

            if (areaID > 0 && cityID > 0)
            {
                LogoLink.NavigateUrl = basedomain + "/" + CityDL.GetCityUrlByID(cityID) + "/" + AreaDL.GetAreaUrlByID(areaID);

                PreserveAreaSkylineImageFileName();

                //Hide items for Boston and Philly markets
                if (_dataAccessHelper.GetCityUrlFromID(cityID.ToString()) == "philadelphia" || _dataAccessHelper.GetCityUrlFromID(cityID.ToString()) == "boston")
                {
                    requestitem.Visible = false;
                    footerrequestitem.Visible = false;
                }

                CurrentLocation.Text = _dataAccessHelper.GetAreaNameFromID(areaID.ToString(), cityID.ToString());

                //Build category menu from database
                string sql = "SELECT CityInfo.UrlName AS CityUrlName, CategoryInfo.CategoryID, CategoryInfo.DisplayName AS CatName, CategoryInfo.UrlName AS CatUrlName, " +
                             "AreaInfo.UrlName AS AreaUrlName FROM CategoryArea INNER JOIN CategoryInfo ON CategoryArea.CategoryID = CategoryInfo.CategoryID INNER JOIN " +
                             "AreaInfo ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CityInfo ON AreaInfo.CityID = CityInfo.CityID WHERE (CategoryArea.AreaID = @AREAID) AND " +
                             "(AreaInfo.CityID = @CITYID) ORDER BY CategoryInfo.DisplayName";

                DataTable allitems = _dataAccessHelper.Data.ExecuteDataset(sql,
                    new SqlParameter("@AREAID", areaID),
                    new SqlParameter("@CITYID", cityID)).Tables[0];

                // A stackSize of 0 will guarantee that you have 'numLists' lists and 
                // that the items are distributed equally from left to right.
                List<DataTable> lists = GetListData(allitems, 0, NumLists);

                // Bind to the lists.  There could be fewer populated lists based on your settings.
                for (int i = 1; i <= lists.Count; i++)
                {
                    ListView listView = FindUIControl(i);
                    listView.DataSource = lists[i - 1];
                    listView.DataBind();
                }

				if (TopCategoriesDispenser.DoesObeyRulesToBeShown(Page.AppRelativeVirtualPath))
				{
                    AdjustTopCategories(areaID);
					ShowCategoryPointer();
				}
				
                //Show page navigation
                PageNavPanel.Visible = true;
                GlobalNavPanel.Visible = false;
            }
            else
            {
                LogoLink.NavigateUrl = basedomain + "/";

                //Show global navigation
                PageNavPanel.Visible = false;
                GlobalNavPanel.Visible = true;

                if (!String.IsNullOrEmpty(_bprPreferences.OutOfMarketZip))
                    _bprPreferences.OutOfMarketZip = _bprPreferences.OutOfMarketZip;
            }
        }

		#region [Helpers]

		/// <summary>
        /// Get the individual DataTable's that will fill the lists
        /// </summary>
        private List<DataTable> GetListData(DataTable allItems, int stackSize, int numLists)
        {
            // Get the count of items that should go in each List
            List<int> itemsPerList = GetListCounts(allItems, stackSize, numLists);

            // The individual datatables that will populate each list
            var dataTables = new List<DataTable>();
            for (int i = 1; i <= numLists; i++)
                dataTables.Add(allItems.Clone());

            int tableNum = 0;
            int cnt = 0;
            // Populate the individual datasets
            for (int i = 0; i < allItems.Rows.Count; i++)
            {
                cnt++;
                dataTables[tableNum].ImportRow(allItems.Rows[i]);
                if (cnt == itemsPerList[tableNum])
                {
                    cnt = 0;
                    tableNum++;
                }

                // Jump out of the loop... we're done 
                if (i == allItems.Rows.Count - 1)
                    break;
            }

            return dataTables;
        }

        /// <summary>
        /// Returns an array of the number of items in each list
        /// </summary>
        /// <param name="stackSize">The preferred number of items in each list from left to right</param>
        /// <param name="listCount">Number of lists to populate</param>
        private List<int> GetListCounts(DataTable allItems, int stackSize, int listCount)
        {
            var itemsPerList = new List<int>();
            int totalItemCount = allItems.Rows.Count;

            // Build the list that will hold the number of items
            // for each list on screen
            for (int i = 1; i <= listCount; i++)
                itemsPerList.Add(0);

            // See how many items per list we are working with
            double cnt = (double)totalItemCount / (double)listCount;

            // If we have more than 'stackSize' items per list, then distribute them evenly
            if (cnt > stackSize)
                stackSize = 0;

            if (stackSize == 0)
            {
                // Distribute evenly to the left
                cnt = Math.Round(cnt, MidpointRounding.AwayFromZero);
                for (int i = 1; i < listCount; i++)
                {
                    itemsPerList[i - 1] = (int)cnt;
                    totalItemCount -= (int)cnt;
                }
                // Put the remaining in the last list
                if (totalItemCount > 0)
                    itemsPerList[listCount - 1] = totalItemCount;
            }
            else
            {
                int listNum = 0;
                cnt = 0;

                // Stack at least 'stackSize' items in each list from left to right
                for (int i = 1; i <= allItems.Rows.Count; i++)
                {
                    cnt++;
                    itemsPerList[listNum]++;
                    if (cnt == stackSize)
                    {
                        cnt = 0;
                        listNum++;
                    }
                }
            }

            return itemsPerList;
        }


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

		private void PreserveAreaSkylineImageFileName()
		{
			var areaGroupDistributor = new AreaGroupDistributor();
			AreaGroup areaCluster = areaGroupDistributor.GetAreaGroupByAreaId(areaID);
			if (areaCluster != null)
			{
				hdnAreaSkylineImageFileName.Value =  areaCluster.SkylineImageFileName;
			}
		}

		public void AdjustSlider(SliderGallery sliderGallery)
		{
			if (sliderGallery.HasSlides)
			{
				CoverImagePath = sliderGallery.CoverImageFileName;
				PromoClipLink.NavigateUrl = sliderGallery.PromoClipUri;
				ShowSlider(sliderGallery.Slides);
			}
		}

		private void ShowSlider(DataTable slides)
		{
			SlideList.DataSource = slides;
			SlideList.DataBind();
			SliderPanel.Visible = true;
		}

		private void AdjustTopCategories(int areaId)
		{
			var dispenser = new TopCategoriesDispenser(areaId);
			if (dispenser.HasCategories)
			{
				ShowTopCategories(dispenser.TopCategories);
			}
		}

		private void ShowTopCategories(DataTable categories)
		{
			rptrTopCategories.DataSource = categories;
			rptrTopCategories.DataBind();
			pnlTopCategories.Visible = true;
		}

		private void ShowCategoryPointer()
		{
			categoryPointer.Visible = true;
		}

		#endregion

		#region [Handlers]

		protected void SearchButton_Click(object sender, EventArgs e)
        {
            HiddenSearchValue.Value = SearchValue.Text.Trim();

            if (_bprPreferences.Exists())
            {
                Response.Redirect(basedomain + "/search.aspx?keyword=" + Server.UrlEncode(SearchValue.Text.Trim()));
            }
            else
            {
                bodytag.Attributes.Add("onload", "requestZip();");
            }
        }

        protected void ZipCodeChangeButton_Click(object sender, EventArgs e)
        {
            string zipCode = "";

            try
            {
                // Set zipcode to search on
                int.Parse(ZipCodeChange.Text.Trim());
                zipCode = ZipCodeChange.Text.Trim();
            }
            catch (Exception)
            {
                // zipCode will be set to empty string
            }

            string redirectPage = _siteMasterHelper.TryChangeZipCode(zipCode, basedomain, catid, contractorid);

            if (string.IsNullOrEmpty(redirectPage))
            {
                bodytag.Attributes.Add("onload", "notFound();");
            }
            else
            {
                Response.Redirect(redirectPage);
            }
        }

        protected void ZipSearch_Click(object sender, EventArgs e)
        {
            string zipCode = "";

            try
            {
                // Set zipcode to search on
                int.Parse(ZipCode.Text.Trim());
                zipCode = ZipCode.Text.Trim();
            }
            catch (Exception)
            {
                // zipCode will be set to empty string
            }

            bool wasLocationChanged = LocationHelper.TryChangeLocationByZip(zipCode);
            if (wasLocationChanged)
            {
                Response.Redirect(basedomain + "/" + _bprPreferences.CityUrlName + "/" + _bprPreferences.AreaUrlName);
            }
            else
            {
                bodytag.Attributes.Add("onload", "notFound();");
            }
        }

        protected void SearchZipButton_Click(object sender, EventArgs e)
        {
            string zipCode = "";
            try
            {
                // Set zipcode to search on
                int.Parse(SearchZipBox.Text.Trim());
                zipCode = SearchZipBox.Text.Trim();
            }
            catch (Exception)
            {
                // zipCode will be set to empty string
            }

            bool wasLocationChanged = LocationHelper.TryChangeLocationByZip(zipCode);
            if (wasLocationChanged)
            {
                Response.Redirect(basedomain + "/search.aspx?keyword=" + Server.UrlEncode(HiddenSearchValue.Value.Trim()));
            }
            else
            {
                bodytag.Attributes.Add("onload", "notFound();");
            }
        }

		protected void SlideList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                string locationName = drv["Location"].ToString();
                Literal loc = (Literal)e.Item.FindControl("Location");

                if (!String.IsNullOrEmpty(locationName))
                    loc.Text = " | " + locationName;
            }
        }

        protected void ListView1_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                DataTable dt = (DataTable)ListView1.DataSource;
                DataRow row = dt.Rows[_recIndex1];

                HyperLink clink = (HyperLink)e.Item.FindControl("CatLink");
                clink.NavigateUrl = basedomain + "/" + row["CatURLName"] + "/" + row["CityURLName"] + "/" + row["AreaURLName"];
                clink.Text = row["CatName"].ToString();

                if ((row["CategoryID"].ToString() != catid) && catid != "0")
                    clink.Attributes.Add("rel", "nofollow");

                _recIndex1++;
            }
        }

        protected void ListView2_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                DataTable dt = (DataTable)ListView2.DataSource;
                DataRow row = dt.Rows[_recIndex2];

                HyperLink clink = (HyperLink)e.Item.FindControl("CatLink");
                clink.NavigateUrl = basedomain + "/" + row["CatURLName"].ToString() + "/" + row["CityURLName"].ToString() + "/" + row["AreaURLName"].ToString();
                clink.Text = row["CatName"].ToString();

                if ((row["CategoryID"].ToString() != catid.ToString()) && catid != "0")
                    clink.Attributes.Add("rel", "nofollow");

                _recIndex2++;
            }
        }

        protected void ListView3_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                DataTable dt = (DataTable)ListView3.DataSource;
                DataRow row = dt.Rows[_recIndex3];

                HyperLink clink = (HyperLink)e.Item.FindControl("CatLink");
                clink.NavigateUrl = basedomain + "/" + row["CatURLName"].ToString() + "/" + row["CityURLName"].ToString() + "/" + row["AreaURLName"].ToString();
                clink.Text = row["CatName"].ToString();

                if ((row["CategoryID"].ToString() != catid.ToString()) && catid != "0")
                    clink.Attributes.Add("rel", "nofollow");

                _recIndex3++;
            }
        }

        protected void ListView4_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                DataTable dt = (DataTable)ListView4.DataSource;
                DataRow row = dt.Rows[_recIndex4];

                HyperLink clink = (HyperLink)e.Item.FindControl("CatLink");
                clink.NavigateUrl = basedomain + "/" + row["CatURLName"].ToString() + "/" + row["CityURLName"].ToString() + "/" + row["AreaURLName"].ToString();
                clink.Text = row["CatName"].ToString();

                if ((row["CategoryID"].ToString() != catid.ToString()) && catid != "0")
                    clink.Attributes.Add("rel", "nofollow");

                _recIndex4++;
            }
		}

		protected void rptrTopCategories_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
                string cityURL = CityDL.GetCityUrlByID(cityID);
                string areaURL = AreaDL.GetAreaUrlByID(areaID);
				DataRowView categoryRow = (DataRowView) e.Item.DataItem;
				HyperLink link = (HyperLink)e.Item.FindControl("topCategoryLink");
				link.NavigateUrl = String.Format(@"{0}/{1}/{2}/{3}", basedomain, categoryRow["UrlName"], cityURL, areaURL);
				link.Text = categoryRow["CategoryName"].ToString();
			}
		}

		#endregion
	}
}
