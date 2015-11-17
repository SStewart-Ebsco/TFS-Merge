using System;
using System.Configuration;
using System.Web.UI.WebControls;
using ER_BestPickReports_Dev.App_Code.Models;
using ER_BestPickReports_Dev.Helpers;

namespace ER_BestPickReports_Dev.mobile
{
    public partial class SiteMobileMaster : System.Web.UI.MasterPage
    {
        private readonly DataAccessHelper _dataAccessHelper;
        private readonly AppCookies _bprPreferences;
        private readonly SiteMasterHelper _siteMasterHelper;

        public string basedomain = "";
        public int cityID = 0;
        public int areaID = 0;
        public string catid = "0";
        public string contractorid = "0";
		public string CoverImagePath = "";

        public SiteMobileMaster()
        {
            _dataAccessHelper = new DataAccessHelper();
            _bprPreferences = AppCookies.CreateInstance();
            _siteMasterHelper = new SiteMasterHelper(_bprPreferences, _dataAccessHelper);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Set Domain
            basedomain = _siteMasterHelper.GetBaseDomain();
            
            if (areaID <= 0 && cityID <= 0)
            {
                areaID = _bprPreferences.AreaId;
                cityID = _bprPreferences.CityId;
            }

            if (areaID > 0 && cityID > 0)
            {
                CurrentLocation.Text = _dataAccessHelper.GetAreaNameFromID(areaID.ToString(), cityID.ToString());
            }
            else
            {
                CurrentLocationPanel.Visible = false;
            }

            bool isUserInMarket = LocationHelper.CheckInMarketPosition(Request.Cookies.Get("bprpreferences"));

            bool displayAds = Convert.ToBoolean(ConfigurationManager.AppSettings["DisplayAds"]);
            if (isUserInMarket || !displayAds)
            {
                FooterAd.Visible = false;
            }
        }

		#region [Handlers]

		public void SearchButton_Click(object sender, EventArgs e)
        {
            if (_bprPreferences.CityId > 0 && _bprPreferences.AreaId > 0)
            {
                Response.Redirect(basedomain + "/search?keyword=" + Server.UrlEncode(SearchBox.Text.Trim()));
            }
            else
            {
                Body.Attributes.Add("onload", "showNotFoundZipModal();");
            }
        }

        protected void ZipCodeChangeButton_Click(object sender, EventArgs e)
        {
            string zipCode = "";
            var button = (Button)sender;

            try
            {
                switch (button.ID)
                {
                    case "ZipCodeChangeButton":
                        zipCode = ZipCodeChange.Text.Trim();
                        break;
                    case "ZipCodeChangeButton2":
                        zipCode = ZipCodeChange2.Text.Trim();
                        break;
                }
            }
            catch (Exception)
            {
                // zipCode will be set to empty string
            }

            string redirectPage = _siteMasterHelper.TryChangeZipCode(zipCode, basedomain, catid, contractorid);

            if (string.IsNullOrEmpty(redirectPage))
            {
                //Show popup warning
                Body.Attributes.Add("onload", "showNotFoundZipModal();");
            }
            else
            {
                Response.Redirect(redirectPage);
            }
        }
        
		#endregion
    }
}
