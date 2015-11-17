using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web;
using System.Globalization;
using ER_BestPickReports_Dev.App_Code;
using ER_BestPickReports_Dev.App_Code.Models;
using ER_BestPickReports_Dev.Helpers;


namespace ER_BestPickReports_Dev.blogfiles.mobile
{
    public partial class BlogMobile : System.Web.UI.MasterPage
    {
        private readonly DataAccessHelper dataAccessHelper = new DataAccessHelper();
        public string strcat = "";
        public string strcity = "";
        public string strmonth = "";
        public string stryear = "";
        public string postid = "";
        string filtercity = "";
        int numtags = 5;
        DataTable dt_cat = null;
        int recordNumber = 0;
        public bool isglobal = false;
        public string basedomain = "";

        public string catid = "0";
        public string contractorid = "0";
        private int recindex1 = 0;
        private int recindex2 = 0;
        private int recindex3 = 0;
        private int recindex4 = 0;
        public int cityID = 0;
        public int areaID = 0;
        int numLists = 2;
        int dataCount = 0;
        int currentList = 0;

        protected string CategoriesMenuState = "";

        public string state = String.Empty;

        protected AppCookies bprPreferences = AppCookies.CreateInstance();

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterAsyncPostBackControl(CategoriesChoice);

            if (areaID <= 0 && cityID <= 0)
            {
                //Int32.TryParse(bprPreferences.AreaId, out areaID);
                //Int32.TryParse(bprPreferences.CityId, out cityID);
                areaID = bprPreferences.AreaId;
                cityID = bprPreferences.CityId;
            }

            if (!IsPostBack)
            {
                //Set city filter based on 1)URL 2)City
                if (strcity != "")
                    filtercity = strcity;
                else if (postid != "")
                    filtercity = bprPreferences.CityUrlName;

                string zip = LocationHelper.GetZip(Request, Response, Session);

                bool isUserInMarket = LocationHelper.CheckInMarketPosition(Request.Cookies.Get("bprpreferences"));

                //Set Domain
                if (HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToString() != "localhost")
                    basedomain = "http://" + HttpContext.Current.Request.ServerVariables["SERVER_NAME"];

                if (filtercity != "")
                {
                    //Check to see if cookie matches city in url
                    if (filtercity != bprPreferences.CityUrlName)
                    {
                        SetCookie(filtercity);
                    }
                }


                //Show related articles module if on post
                if (postid != "")
                {
                    RequestTopic.Visible = isUserInMarket;
                }
                else
                {
                    if (catid == "0" && !String.IsNullOrEmpty(strcat))
                    {
                        catid = CategoriesDL.GetCategoryIdByUrl(strcat).ToString();
                    }
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

                if (categories.Count > 0)
                {
                    int categoriesPerList = (int)Math.Ceiling((double)categories.Count / numLists);

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
                        CategoriesDL category = categories.FirstOrDefault(c => c.CatUrlName == strcat);
                        if (category != null)
                        {
                            int index =
                                CategoriesChoice.Items.IndexOf(CategoriesChoice.Items.FindByText(category.Name));
                            CategoriesChoice.SelectedIndex = index;
                            TopCategories.Visible = false;
                            Contractors.Visible = true;
                        }
                    }
                }
                else
                {
                    CategoriesMenuState = "hidden";
                }

                if (areaID > 0 && cityID > 0)
                {
                    CurrentLocation.Text = dataAccessHelper.GetAreaNameFromID(areaID.ToString(), cityID.ToString());
                    
					if (isUserInMarket)
					{
                        List<CategoriesDL> topCategories = CategoriesDL.GetTopCategoriesByAreaId(areaID);
                        topCategories = topCategories.Where(tc => tc.ContractorsAmount > 0).OrderBy(cat => cat.Name).ToList();
						TopCategoriesList.DataSource = topCategories;
						TopCategoriesList.DataBind();
					}
					
				}
				else
                {
                    if (Session.Count > 0 && !String.IsNullOrEmpty(Session["areaname"] as string))
                    {
                        CurrentLocation.Text = Session["areaname"].ToString();
                    }
                    else
                    {
                        CurrentLocation.Text = "Not available";
                    }
                }

                if (isUserInMarket)
                {
                    if (catid != "0" && !String.IsNullOrEmpty(catid) && isUserInMarket)
                    {
                        SetContractorsList(catid, areaID.ToString());
                    }
                    else
                    {
                        Contractors.Visible = false;
                        MultipleEmailPanel.Visible = false;
                    }
                    FooterAd.Visible = false;
                }
                else
                {
                    ViewLocalBestPicksMenuItem.Visible = false;
                }
            }

            bool displayAds = Convert.ToBoolean(ConfigurationManager.AppSettings["DisplayAds"]);
            if (!displayAds)
            {
                FooterAd.Visible = false;
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

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            //if (SearchOptions.SelectedIndex == 0)
                Response.Redirect("/blog/search/?tag=" + SearchBox.Text.Trim() + "&city=" + strcity);
            //else if (SearchOptions.SelectedIndex == 1) 
            //Response.Redirect(basedomain + "/search.aspx?keyword=" + SearchBox.Text.Trim() + "&zip=Zip%20Code");
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

            if (!String.IsNullOrEmpty(zipCode))
            {
                CookiesInfo info = InfoDL.GetCookiesInfoByZipCode(zipCode);
                string redirectUrl;
                if (info != null && info.AreaId != null)
                {
                    redirectUrl = String.Concat(basedomain, "/blog/",
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
            }

            OutOfMarketZipCode.Value = zipCode;
            Body.Attributes.Add("onload", "showNotFoundZipModal();");
        }
        
        protected void CategoriesList1_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                List<CategoriesDL> dt = (List<CategoriesDL>)CategoriesList1.DataSource;
                CategoriesDL row = dt[recindex1];

                HyperLink clink = (HyperLink)e.Item.FindControl("CatLink");
                if (cityID > 0 && areaID > 0)
                {
                    clink.NavigateUrl = String.Concat(basedomain, "/blog/", row.CatUrlName, "/", CityDL.GetCityUrlByID(cityID), "/", AreaDL.GetAreaUrlByID(areaID));
                }
                else
                {
                    clink.NavigateUrl = String.Concat(basedomain, "/blog/", row.CatUrlName);
                }
                clink.Text = row.Name;

                if ((row.Id != Convert.ToInt32(catid)) && catid != "0")
                {
                    clink.Attributes.Add("rel", "nofollow");
                }
                
                if (row.Id == Convert.ToInt32(catid))
                {
                    clink.CssClass += " selected";
                }
                recindex1++;
            }
        }

        protected void CategoriesList2_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                List<CategoriesDL> dt = (List<CategoriesDL>)CategoriesList2.DataSource;
                CategoriesDL row = dt[recindex2];

                HyperLink clink = (HyperLink)e.Item.FindControl("CatLink");
                if (cityID > 0 && areaID > 0)
                {
                    clink.NavigateUrl = String.Concat(basedomain, "/blog/", row.CatUrlName, "/", CityDL.GetCityUrlByID(cityID), "/", AreaDL.GetAreaUrlByID(areaID));
                }
                else
                {
                    clink.NavigateUrl = String.Concat(basedomain, "/blog/", row.CatUrlName);
                }
                clink.Text = row.Name;

                if ((row.Id != Convert.ToInt32(catid)) && catid != "0")
                    clink.Attributes.Add("rel", "nofollow");

                if (row.Id == Convert.ToInt32(catid))
                {
                    clink.CssClass += " selected";
                }
                recindex2++;
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
            List<DataTable> dataTables = new List<DataTable>();
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
        private List<int> GetListCounts(DataTable allitems, int stackSize, int listCount)
        {
            List<int> itemsPerList = new List<int>();
            int totalItemCount = allitems.Rows.Count;

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
                for (int i = 1; i <= allitems.Rows.Count; i++)
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
                    listView = CategoriesList1;
                    break;
                case 2:
                    listView = CategoriesList2;
                    break;
                default:
                    throw new Exception("Invalid control number");
            }

            return listView;
        }

        #endregion

        DataTable clistTable = new DataTable();
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
                MultipleEmailPanel.Visible = false;
            }
        }

        private void SetContractorsList(string catId, string areaId)
        {
            string sql = "SELECT * FROM ContractorAreaCategoryRel WHERE (CategoryID = @CATID) AND (AreaID = @AREAID) ORDER BY OrderIndex";
            DataTable dt = dataAccessHelper.Data.ExecuteDataset(sql,
                                                        new SqlParameter("@CATID", catId),
                                                        new SqlParameter("@AREAID", areaId)).Tables[0];
            bool isPrimary = false;

            clistTable.Clear();
            if (!clistTable.Columns.Contains("name"))
            {
                clistTable.Columns.Add("name");
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

            int contractorsWithEmail = 0;
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
                if (conRow["email"].ToString().Trim() != "")
                {
                    contractorsWithEmail++;
                }
            }
            clistTable.AcceptChanges();

            ContractorsList.DataSource = clistTable;
            ContractorsList.DataBind();

            //Bind to checkbox list all contractors on the page
            EmailContractorList.DataSource = clistTable;
            EmailContractorList.DataBind();
            alertnote.Visible = clistTable.Rows.Count > contractorsWithEmail;

            if (clistTable.Rows.Count > 0)
            {
                MultipleEmailPanel.Visible = contractorsWithEmail > 1;
                ViewMoreButton.Visible = clistTable.Rows.Count > 3;
                Contractors.Visible = true;
            }
            else
            {
                MultipleEmailPanel.Visible = false;
                Contractors.Visible = false;
            }
        }
        
        private int reccount = 0;
        protected void EmailContractorList_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                DataTable dt = (DataTable)EmailContractorList.DataSource;
                DataRow row = dt.Rows[reccount];

                HiddenField cid = (HiddenField)e.Item.FindControl("HiddenListID");
                cid.Value = row["cid"].ToString().Trim();

                Label name = (Label)e.Item.FindControl("ContractorLabel");
                name.Text = row["name"].ToString();

                CheckBox chk = (CheckBox)e.Item.FindControl("ContractorEmailID");

                if (row["email"].ToString().Trim() != "")
                {
                    chk.Visible = true;
                    chk.Checked = true;
                }
                else
                {
                    chk.Attributes["disabled"] = "disabled";
                }

                reccount++;
            }
        }
        
        protected void OK_EmailClick(object sender, EventArgs e)
        {
            const string isppc = "false";
            bool error = false;
            string name = Name.Text.Trim();
            string lastname = LastName.Text.Trim();
            string addr = Address.Text.Trim();
            string zip = ZipForMail.Text.Trim();
            string email = Email.Text.Trim();
            string phone = FormPhone.Text.Trim();
            string phone2 = FormPhone2.Text.Trim();
            string worktype = WorkType.Text.Trim();
            string message = Message.Text.Trim();

            #region Error Checking

            if (name == "")
            {
                NameFieldValidator.Visible = true;
                NameFieldValidator.IsValid = false;
            }
            if (lastname == "")
            {
                LastNameFieldValidator.IsValid = false;
            }
            if (addr == "")
            {
                AddressFieldValidator.IsValid = false;
            }
            if (zip == "")
            {
                ZipFieldValidator.IsValid = false;
            }
            if (zip == "" || !Global.IsZipCode(zip))
            {
                ZipFieldValidator.IsValid = false;
                ZipFieldValidator.ErrorMessage = "ZIP Code must be 5 digits long";
            }
            if (email == "" || !Global.IsEmail(email))
            {
                EmailFieldValidator.IsValid = false;
            }
            if (phone == "" || !Global.IsPhoneNumber(phone))
            {
                FormPhoneFieldValidator.IsValid = false;
                FormPhoneFieldValidator.ErrorMessage = "Phone should contain 10 digits";
            }
            if (worktype == "")
            {
                WorkTypeFieldValidator.IsValid = false;
            }
            if (message == String.Empty)
            {
                MessageFieldValidator.IsValid = false;
            }

            error = !(NameFieldValidator.IsValid && LastNameFieldValidator.IsValid && AddressFieldValidator.IsValid &&
                      ZipFieldValidator.IsValid && EmailFieldValidator.IsValid && FormPhoneFieldValidator.IsValid &&
                      WorkTypeFieldValidator.IsValid && MessageFieldValidator.IsValid);


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
                ///ModalEmailForm.Visible = false;
                ReqPanel.Visible = false;
                EmailComplete.Style["display"] = "block";
                ModalEmailForm.Style["display"] = "none";
                Body.Attributes["class"] = "";
            }
            else
            {
                // enable Email Form modal state
                ReqPanel.Visible = true;
                ModalEmailForm.Style["display"] = "block";
                Body.Attributes["class"] = "on-modal";
                EmailContractorName.Text = EmailFormContractorName.Value;
                EmailComplete.Style["display"] = "none";
            }
        }


        protected void CloseEmail_Click(object sender, EventArgs e)
        {
            throw new NotSupportedException();
            // Hide the sent div and show the form
            ///ModalEmailForm.Visible = true;
            EmailComplete.Visible = false;
            ReqPanel.Visible = false;
        }

        protected void CloseRequestEmail_Click(object sender, EventArgs e)
        {
            RequesTopicEmailForm.Visible = true;
            EmailRequestComplete.Visible = false;
            ReqPanelInReq.Visible = false;
        }

        protected void Cancel_RequestEmailClick(object sender, EventArgs e)
        {
            RequesTopicEmailForm.Visible = true;
            EmailRequestComplete.Visible = false;
            ReqPanelInReq.Visible = false;
        }

        protected void OK_RequestEmailClick(object sender, EventArgs e)
        {
            bool error = false;
            string name = NameInReq.Text.Trim();
            string lastname = LastNameInReq.Text.Trim();
            string email = EmailInReq.Text.Trim();
            string subject = Subject.Text.Trim();
            string message = MessageInReq.Text.Trim();

            #region Error Checking
            if (name == "")
            {
                NameInReqFieldValidator.IsValid = false;
            }
            if (lastname == "")
            {
                LastNameInReqFieldValidator.IsValid = false;
            }
            if (email == "" || !Global.IsEmail(email))
            {
                EmailInReqFieldValidator.IsValid = false;
            }
            if (subject == "")
            {
                SubjectFieldValidator.IsValid = false;
            }
            if (message == "")
            {
                MessageInReqFieldValidator.IsValid = false;
            }

            error = !(NameInReqFieldValidator.IsValid && LastNameInReqFieldValidator.IsValid &&
                EmailInReqFieldValidator.IsValid && SubjectFieldValidator.IsValid && MessageInReqFieldValidator.IsValid);
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

                ReqPanelInReq.Visible = false;
                EmailComplete.Visible = true;
                RequesTopicEmailForm.Style["display"] = "none";
                EmailRequestComplete.Style["display"] = "block";
                Body.Attributes["class"] = "";
            }
            else
            {
                EmailRequestComplete.Style["display"] = "none";
                RequesTopicEmailForm.Visible = true;
                RequesTopicEmailForm.Style["display"] = "block";
                ReqPanelInReq.Visible = true;
                Body.Attributes["class"] = "on-modal";
            }
        }

        protected void GoOutOfMarket(object sender, EventArgs e)
        {
            bprPreferences.Remove(AppCookies.CITY_ID);
            bprPreferences.Remove(AppCookies.CITY_URL_NAME);
            bprPreferences.Remove(AppCookies.AREA_ID);
            bprPreferences.Remove(AppCookies.AREA_URL_NAME);
            bprPreferences.OutOfMarketZip = OutOfMarketZipCode.Value;
            Response.Redirect(string.Concat(basedomain, "/blog"));
        }
    }
}
