using System;
using System.Web.UI;
using System.Text.RegularExpressions;
using ER_BestPickReports_Dev.Helpers;
using Telerik.Web.UI;
using System.Web;

namespace ER_BestPickReports_Dev
{
    public class BasePage : System.Web.UI.Page
    {
        protected readonly DataAccessHelper DataAccessHelper = new DataAccessHelper();

        public bool LoggedIn
        {
            get { return (Session["loggedin"] == null) ? false : (bool)Session["loggedin"]; }
            set { Session["loggedin"] = value; }
        }

        //Session variable for refer type 0 = direct, 1 = organic search, 2 = ppc, 3 = facebook
        public string refertype
        {
            get { return (Session["refertype"] == null) ? "0" : Session["refertype"].ToString(); }
            set { Session["refertype"] = value; }
        }

        //public string domain
        //{
        //    get { return (Session["domain"] == null) ? "" : Session["domain"].ToString(); }
        //    set { Session["domain"] = value; }
        //}

        public string renameCat(string oldcat)
        {
            string newcat = oldcat;
            string temp = oldcat.ToLower();

            switch (temp)
            {
                case "air-conditioning-and-heating-contractors":
                    newcat = "air-conditioning-and-heating";
                    break;
                case "appliance-repair-services":
                    newcat = "appliance-repair";
                    break;
                case "bathroom-remodelers":
                    newcat = "bathroom-remodeling";
                    break;
                case "carpet-and-upholstery-cleaners":
                    newcat = "carpet-and-upholstery-cleaning";
                    break;
                case "chimney-and-fireplace-cleaning-and-repair-companies":
                    newcat = "chimney-and-fireplace-work";
                    break;
                case "countertop-and-custom-stone-companies":
                    newcat = "countertops-and-stone-custom";
                    break;
                case "deck-contractors":
                    newcat = "deck-building-and-maintenance";
                    break;
                case "decks-under-deck-system-contractors":
                    newcat = "decks-under-deck-systems";
                    break;
                case "driveway-contractors-concrete":
                    newcat = "driveway-concrete";
                    break;
                case "fencing-contractors":
                    newcat = "fences";
                    break;
                case "fences-pet-containment-systems":
                    newcat = "fences-pet-containment";
                    break;
                case "flooring-contractors-hardwood":
                    newcat = "flooring-hardwood";
                    break;
                case "foundation-repair-contractors":
                    newcat = "foundation-repair";
                    break;
                case "garage-door-contractors":
                    newcat = "garage-doors";
                    break;
                case "gutter-installers":
                    newcat = "gutter-installation";
                    break;
                case "landscapers":
                    newcat = "landscaping";
                    break;
                case "lawn-treatment-companies":
                    newcat = "lawn-treatment";
                    break;
                case "mason-brick-and-stone":
                    newcat = "masonry-brick-and-stone";
                    break;
                case "pest-and-termite-control-companies":
                    newcat = "pest-and-termite-control";
                    break;
                case "plantation-shutters-and-custom-blinds":
                    newcat = "shutters-and-blinds-custom";
                    break;
                case "sprinkler-system-companies":
                    newcat = "sprinkler-systems";
                    break;
                case "stucco-repair-companies":
                    newcat = "stucco-repair";
                    break;
                case "tile-installers":
                    newcat = "tile-installation";
                    break;
                case "waterproofing-contractors":
                    newcat = "waterproofing";
                    break;
                case "window-and-door-replacement-contractors":
                    newcat = "window-and-door-replacement";
                    break;
                case "window-cleaners,-gutter-cleaners-and-pressure-washers":
                    newcat = "window-cleaning-and-pressure-washing";
                    break;
                case "basement-remodelers":
                    newcat = "basement-remodeling";
                    break;
                case "kitchen-remodelers":
                    newcat = "kitchen-remodeling";
                    break;
                case "paver-installation-contractors":
                    newcat = "paver-installation";
                    break;
            }

            return newcat;
        }

        protected override void OnLoad(EventArgs e)
        {
            string referstring = "";

            if (BWConfig.IsPreStaging())
            {
                //Authenticate Login
                if (HttpContext.Current.Session["BPRDevLogin"] != null)
                {
                    if (!bool.Parse(HttpContext.Current.Session["BPRDevLogin"].ToString()))
                    {
                        //Redirect to an Error page or Login page
                        Response.Redirect("/login.aspx");
                    }
                }
                else
                {
                    //Redirect to an Error page or Login page
                    Response.Redirect("/login.aspx");
                }

                //Show test label if organic link referer
                //Set session variable based on referer
                referstring = HttpContext.Current.Session["BPRDevReferer"].ToString();
            }
            else
            {
                //Show test label if organic link referer
                //Set session variable based on referer
                if (Request.UrlReferrer != null)
                    referstring = Request.UrlReferrer.ToString();
            }

            Regex regEx_se = new Regex("google|Google|yahoo|Yahoo|bing.com|Bing.com");
            Regex regEx_exclude = new Regex("home reports|home report|ebsco|best pick|homereports|best pick reports|Home Reports|Home Report|EBSCO|Best Pick|Homereports|Best pick reports|HomeReports|Best Pick Reports");
            if (regEx_se.Match(referstring).Success && !regEx_exclude.Match(referstring).Success)
            {
                //System.Collections.Specialized.NameValueCollection qs = HttpUtility.ParseQueryString(referstring);
                //string searchPhrase = qs["Q"];
                refertype = "1";
            }

            Regex regEx_fb = new Regex("facebook|Facebook");
            if (regEx_fb.Match(referstring).Success)
            {
                refertype = "3";
            }

            if (refertype != "1" && refertype != "2" && refertype != "3")
                refertype = "0";

            //Set Domain
            //domain = HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToString();

            base.OnLoad(e);
        }

        /// <summary>
        /// Create urltitle from title
        /// </summary>
        public string CreateURLName(string title)
        {
            string urltitle = title.ToLower().Replace("&", "-and-");
            urltitle = Regex.Replace(urltitle, @"[^a-zA-Z0-9 -]", "");
            urltitle = urltitle.Replace(" ", "-");
            urltitle = urltitle.Replace("--", "-");
            urltitle = urltitle.Replace("---", "-");

            return urltitle.ToLower();
        }

        public void SetTelerikEditorOptions(RadEditor ctrl)
        {
            String[] imgsPath = { "/blogfiles/assets/images" };
            String[] docsPath = { "/blogfiles/assets/documents" };
            String[] imgPatterns = { "*.jpg", "*.jpeg", "*.gif", "*.png" };
            String[] docPatterns = { "*.pdf", "*.doc", "*.docx", "*.xls", "*.xlsx", "*.txt", "*.htm", "*.html" };

            ctrl.ToolsFile = "~/blogfiles/ToolsFile.xml";
            ctrl.DisableFilter(EditorFilters.FixEnclosingP);
            ctrl.ImageManager.EnableImageEditor = true;
            ctrl.ImageManager.DeletePaths = imgsPath;
            ctrl.ImageManager.ViewPaths = imgsPath;
            ctrl.ImageManager.UploadPaths = imgsPath;
            ctrl.ImageManager.MaxUploadFileSize = 6144000;
            ctrl.ImageManager.SearchPatterns = imgPatterns;
            ctrl.DocumentManager.DeletePaths = docsPath;
            ctrl.DocumentManager.ViewPaths = docsPath;
            ctrl.DocumentManager.UploadPaths = docsPath;
            ctrl.DocumentManager.MaxUploadFileSize = 6144000;
            ctrl.DocumentManager.SearchPatterns = docPatterns;
            ctrl.ContentAreaCssFile = "/blogfiles/assets/css/editorstyle.css";
            ctrl.CssFiles.Add("/blogfiles/assets/css/editorclasses.css");
            ctrl.DialogHandlerUrl = "/Telerik.Web.UI.DialogHandler.axd";
            ctrl.SpellCheckSettings.AjaxUrl = "/Telerik.Web.UI.SpellCheckHandler.axd";
        }

        public void SetTelerikEditorOptions_Min(RadEditor ctrl)
        {
            ctrl.ToolsFile = "~/blogfiles/ToolsFile_Min.xml";
            ctrl.DisableFilter(EditorFilters.FixEnclosingP);
            ctrl.ContentAreaCssFile = "/blogfiles/assets/css/editorstyle.css";
            ctrl.CssFiles.Add("/blogfiles/assets/css/editorclasses.css");
        }

        public string FormatDate(string inputdate)
        {
            string returndate = DateTime.Parse(inputdate).Month.ToString();

            switch (returndate)
            {
                case "1":
                    returndate = "JAN";
                    break;
                case "2":
                    returndate = "FEB";
                    break;
                case "3":
                    returndate = "MAR";
                    break;
                case "4":
                    returndate = "APR";
                    break;
                case "5":
                    returndate = "MAY";
                    break;
                case "6":
                    returndate = "JUN";
                    break;
                case "7":
                    returndate = "JUL";
                    break;
                case "8":
                    returndate = "AUG";
                    break;
                case "9":
                    returndate = "SEP";
                    break;
                case "10":
                    returndate = "OCT";
                    break;
                case "11":
                    returndate = "NOV";
                    break;
                case "12":
                    returndate = "DEC";
                    break;
            }

            return returndate;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter);
            base.Render(htmlWriter);

            string html = stringWriter.ToString();
            int startPoint = html.IndexOf("<input type=\"hidden\" name=\"__VIEWSTATE\"");
            if (startPoint >= 0)
            {
                int endPoint = html.IndexOf("/>", startPoint) + 2;
                string viewstateInput = html.Substring(startPoint, endPoint - startPoint);
                html = html.Remove(startPoint, endPoint - startPoint);
                int formEndStart = html.IndexOf("</form>");
                if (formEndStart >= 0)
                    html = html.Insert(formEndStart, viewstateInput);
            }

            writer.Write(html);
        }
    }
}