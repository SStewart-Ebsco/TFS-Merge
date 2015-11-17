using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ER_BestPickReports_Dev.App_Code;
using System.Data;
using System.Data.SqlClient;
using ER_BestPickReports_Dev.App_Code.Models;

namespace ER_BestPickReports_Dev.mobile
{
    public partial class CategoriesList : BasePage
    {
        public string basedomain = "";
        private int recindex1 = 0;
        public int cityID = 0;
        public int areaID = 0;

        private AppCookies bprPreferences = AppCookies.CreateInstance();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (areaID <= 0 && cityID <= 0)
            {
                areaID = bprPreferences.AreaId;
                cityID = bprPreferences.CityId;
            }

            if (areaID > 0 && cityID > 0)
            {
                //Build category menu from database
                List<MobileCategoriesDL> categoriesList = MobileCategoriesDL.GetByCityIdAreaId(cityID, areaID);

                CategoriesList1.DataSource = categoriesList;
                CategoriesList1.DataBind();
            }

            // Replace search to filter
            TextBox searchBox = Master.FindChildControl<TextBox>("SearchBox");
            searchBox.Attributes.Add("placeholder", "Type to filter categories");

            Button searchBtn = Master.FindChildControl<Button>("SearchBtn");
            searchBtn.Click -= ((ER_BestPickReports_Dev.mobile.SiteMobileMaster)Master).SearchButton_Click;
            searchBtn.Click += new EventHandler(searchBtn_Click);
            recindex1 = 0;
        }

        void searchBtn_Click(object sender, EventArgs e)
        {
            TextBox searchBox = Master.FindChildControl<TextBox>("SearchBox");

            CategoriesList1.DataSource = null;
            CategoriesList1.DataSource = MobileCategoriesDL.GetByCityIdAreaId(cityID, areaID)
                .Where(cat => cat.Name.ToLower().Contains(searchBox.Text.Trim().ToLower())).ToList();
            CategoriesList1.DataBind();
        }

        protected void CategoriesList1_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                List<MobileCategoriesDL> dt = (List<MobileCategoriesDL>)CategoriesList1.DataSource;
                MobileCategoriesDL row = dt[recindex1];

                System.Web.UI.HtmlControls.HtmlAnchor clink = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("CatLink");
                if (cityID > 0 && areaID > 0)
                {
                    clink.HRef = String.Concat(basedomain, "/", row.CategoryUrlName, "/", CityDL.GetCityUrlByID(cityID), "/", AreaDL.GetAreaUrlByID(areaID));
                }
                else
                {
                    clink.HRef = String.Concat(basedomain, "/", row.CategoryUrlName);
                }
                

                Image cicon = e.Item.FindControl<Image>("CatIcon");
                Label text = e.Item.FindControl<Label>("CatText");
                text.Text = row.Name;

                //Check for icon
                var iconUrl = row.CategoryIcon as String;
                cicon.AlternateText = row.Name;
                if (!String.IsNullOrEmpty(iconUrl))
                {
                    cicon.ImageUrl = "/assets/icons/" + iconUrl;
                }
                else
                {
                    cicon.ImageUrl = "/assets/icons/temp.png";
                }

                recindex1++;
            }
        }
    }
}