using System;
using System.Web;
using System.Web.UI.WebControls;
using ER_BestPickReports_Dev.App_Code;
using System.Data;
using System.Web.UI.HtmlControls;
using ER_BestPickReports_Dev.App_Code.Models;
using ER_BestPickReports_Dev.Helpers;

namespace ER_BestPickReports_Dev.mobile
{
    public partial class _default : System.Web.UI.Page
    {
        public int cityID = 0;
        public int areaID = 0;
        private string basedomain;

        private AppCookies bprPreferences = AppCookies.CreateInstance();

        protected void Page_Load(object sender, EventArgs e)
        {
            LocationHelper.GetZip(Request, Response, Session);

            if (areaID <= 0 && cityID <= 0)
            {
                areaID = bprPreferences.AreaId;
                cityID = bprPreferences.CityId;
            }

            if (HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToString() != "localhost")
                basedomain = "http://" + HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
            
            if (!IsPostBack)
            {
                //Redirect if cookie and qs redirect is not false
                if (Request.QueryString["redirect"] == null)
                {
                    if (!String.IsNullOrEmpty(bprPreferences.CityUrlName) && !String.IsNullOrEmpty(bprPreferences.AreaUrlName))
                    {
                        string returnUrlEnd = bprPreferences.CityUrlName + "/" + bprPreferences.AreaUrlName;
                        if (!Request.Url.AbsolutePath.Contains(returnUrlEnd))
                        {
                            Response.Redirect(basedomain + "/" + returnUrlEnd);
                        }
                    }
                    else
                    {
                        bprPreferences.RemoveAll();
                    }
                }
            }

            if (areaID > 0 && cityID > 0)
            {
                // TODO get real inmarket value
                var isInMarket = true;
                if (isInMarket)
                {
                    ShowTopCategories(areaID);
                }
            }

            string ryear = DateTime.Today.Year.ToString();
            RibbonImage.ImageUrl = "/images/" + ryear + "_ribbon_global_metro.png";
            RibbonImage.AlternateText = ryear + " EBSCO Research Best Pick";
            RibbonImage.ToolTip = ryear + " EBSCO Research Best Pick";
        }

        private void ShowTopCategories(int areaId)
        {
            TopCategoriesDispenser dispenser = new TopCategoriesDispenser(areaId, true);
            if (dispenser.HasCategories)
            {
                TopCategories.DataSource = dispenser.TopCategories;
                TopCategories.DataBind();
                TopCategories.Visible = true;
                TopCategoriesPanel.Visible = true;
            }
        }

        #region Handlers

        protected void TopCategories_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string cityURL = CityDL.GetCityUrlByID(cityID);
                string areaURL = AreaDL.GetAreaUrlByID(areaID);
                DataRowView categoryRow = (DataRowView)e.Item.DataItem;
                var link = (HtmlAnchor)e.Item.FindControl("TopCategoryLink");
                link.Attributes["href"] = String.Format(@"{0}/{1}/{2}/{3}", basedomain, categoryRow["UrlName"], cityURL, areaURL);

                var icon = (Image)e.Item.FindControl("TopCategoryIcon");
                var imageName = categoryRow["Website"];
                if (!String.IsNullOrEmpty((string)imageName))
                {
                    icon.ImageUrl = "/assets/icons/" + imageName.ToString();
                } else {
                    icon.ImageUrl = "/assets/icons/temp.png";
                }

                var title = (Label)e.Item.FindControl("TopCategoryTitle");
                title.Text = categoryRow["CategoryName"].ToString();
            }
        }

        #endregion
    }
}