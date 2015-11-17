using System;
using System.Data.SqlClient;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using ER_BestPickReports_Dev.App_Code;
using ER_BestPickReports_Dev.App_Code.Models;
using ER_BestPickReports_Dev.Helpers;

namespace ER_BestPickReports_Dev
{
    public partial class _default : BasePage
    {
        public string basedomain = "";
        int pagecityid = 0;
        int pageareaid = 0;

        private AppCookies bprPreferences = AppCookies.CreateInstance();

        protected void Page_Init(object sender, EventArgs e)
        {
            if (MobileBlogHelper.isMobileVersionRequested(Request.RequestContext))
            {
                if (MobileBlogHelper.isAdPageRequired(Request.RequestContext))
                {
                    Server.Transfer("~/mobile/ad.aspx");
                }
                Server.Transfer("~/mobile/default.aspx");
            }
            // do the bartman
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            string sql = "";

            //Set Domain
            if (HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToString() != "localhost")
                basedomain = "http://" + HttpContext.Current.Request.ServerVariables["SERVER_NAME"];

            //Show slider since we are on the global home page
	        SliderGallery sliderGallery = new SliderGallery();
            ((ER_BestPickReports_Dev.SiteMaster)Master).AdjustSlider(sliderGallery);

            //Set masterpage variable for footer city rel attribute
            ((ER_BestPickReports_Dev.SiteMaster)Master).setnofollow = false;

            //Hide form errors on load
            NewsletterErrorPanel.Visible = false;
            GuideErrorPanel.Visible = false;

            if (!IsPostBack)
            {
                //Redirect if cookie and qs redirect is not false
                if (Request.QueryString["redirect"] == null)
                {
                    if (
                        !String.IsNullOrEmpty(bprPreferences.CityUrlName) &&
                        !String.IsNullOrEmpty(bprPreferences.AreaUrlName))
                    {
                        Response.Redirect(basedomain + "/" + bprPreferences.CityUrlName + "/" +
                                          bprPreferences.AreaUrlName);
                    }
                    else
                    {
                        bprPreferences.RemoveAll();
                    }
                }
            }

            RibbonImage.ImageUrl = "/images/" + DateTime.Today.Year.ToString() + "_ribbon_global_metro.png";

            if (!String.IsNullOrEmpty(bprPreferences.CityUrlName) &&
                !String.IsNullOrEmpty(bprPreferences.AreaUrlName))
            {
                pagecityid = bprPreferences.CityId;
                pageareaid = bprPreferences.AreaId;
            }

            //Add meta info
            HtmlMeta meta1;
            //HtmlMeta meta2;

            //meta2 = new HtmlMeta();
            //meta2.Name = "keywords";
            //meta2.Content = "";
            //Page.Header.Controls.Add(meta2);
            //Page.Header.Controls.Add(new LiteralControl("\n"));

            meta1 = new HtmlMeta();
            meta1.Name = "description";
            meta1.Content = "Best Pick Reports and Home Reports are annual publications produced by EBSCO Research, an independent research firm founded in 1997.";
            Page.Header.Controls.Add(meta1);
            Page.Header.Controls.Add(new LiteralControl("\n"));

            //Get homeowner testimonials
            sql = "SELECT TOP 20 * FROM HomeownerTestimonial WHERE (PublishDate <= @DATE) ORDER BY NEWID()";

            DataTable allitems = DataAccessHelper.Data.ExecuteDataset(sql,
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

            Global.SaveFormData(Newsletter_FirstName.Text.Trim(), Newsletter_MI.Text.Trim(), Newsletter_LastName.Text.Trim(), Newsletter_EMail.Text.Trim(), "", "", "", "", false, false, 1, pageareaid, pagecityid);

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

            Global.SaveFormData(Guide_FirstName.Text.Trim(), Guide_MI.Text.Trim(), Guide_LastName.Text.Trim(), Guide_Email.Text.Trim(), Guide_Address.Text.Trim(), Guide_City.Text.Trim(), Guide_State.Text.Trim(), Guide_Zip.Text.Trim(), Guide_Updates.Checked, false, 2, pageareaid, pagecityid);

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
