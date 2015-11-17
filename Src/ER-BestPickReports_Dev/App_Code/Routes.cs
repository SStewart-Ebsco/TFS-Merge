using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Compilation;
using System.Web.SessionState;
using System.Web.Routing;
using ER_BestPickReports_Dev.App_Code;
using ER_BestPickReports_Dev.Helpers;

namespace ER_BestPickReports_Dev
{
    public class BlogRouteHandler : IRouteHandler, IRequiresSessionState
    {
        public IHttpHandler GetHttpHandler(RequestContext context)
        {
            string month = context.RouteData.Values["month"] as string;
            string year = context.RouteData.Values["year"] as string;
            string filter = context.RouteData.Values["filter"] as string;

            //Check values
            int m = 0;
            int y = 0;

            int cityid = 0;
            int catid = 0;
            int areaid = 0;
            string strcat = String.Empty;
            string strcity = String.Empty;
            string strarea = String.Empty;

            if (month != null && year != null)
            {
                if (!int.TryParse(month, out m) || !int.TryParse(year, out y))
                {
                    strcat = filter;
                    strcity = month;
                    strarea = year;

                    cityid = CityDL.GetIDByUrl(strcity);
                    catid = CategoriesDL.GetCategoryIdByUrl(strcat);
                    areaid = AreaDL.GetAreaIdByUrl(strarea);
                    if (cityid == 0 || catid == 0 || areaid == 0)
                    {
                        return
                            BuildManager.CreateInstanceFromVirtualPath("/error.aspx", typeof(System.Web.UI.Page)) as
                            System.Web.UI.Page;
                    }
                }
                else
                {
                    if (m < 0 || m > 12)
                        return
                            BuildManager.CreateInstanceFromVirtualPath("/error.aspx", typeof(System.Web.UI.Page)) as
                            System.Web.UI.Page;
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(filter))
                {
                    catid = CategoriesDL.GetCategoryIdByUrl(filter);
                    strcat = filter;
                }
            }

            //Check for filter match - only for dates
            if (filter != null)
            {
                //Check for city filter first
                cityid = CityDL.GetIDByUrl(filter);
                strcity = (cityid != 0) ? filter : String.Empty;
                if (cityid == 0)
                {
                    catid = CategoriesDL.GetCategoryIdByUrl(filter);
                    strcat = filter;
                }

                if (cityid == 0 && catid == 0)
                    return
                        BuildManager.CreateInstanceFromVirtualPath("/error.aspx", typeof(System.Web.UI.Page)) as
                        System.Web.UI.Page;
            }

            HttpContext.Current.Items["areaid"] = areaid;
            HttpContext.Current.Items["area"] = strarea;
            HttpContext.Current.Items["cityid"] = cityid;
            HttpContext.Current.Items["catid"] = catid;
            HttpContext.Current.Items["city"] = strcity;
            HttpContext.Current.Items["cat"] = strcat;
            HttpContext.Current.Items["month"] = m;
            HttpContext.Current.Items["year"] = y;

            //context.HttpContext.Request.Cookies.Get('useMobileVersion');
            if (MobileBlogHelper.isMobileVersionRequested(context))
            {
                return BuildManager.CreateInstanceFromVirtualPath("/blogfiles/mobile/default.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
            }
            return BuildManager.CreateInstanceFromVirtualPath("/blogfiles/default.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
        }
    }

    public class BlogRouteHandler_Param : IRouteHandler, IRequiresSessionState
    {
        public IHttpHandler GetHttpHandler(RequestContext context)
        {
            var dataAccessHelper = new DataAccessHelper();

            string param1 = context.RouteData.Values["param1"] as string;
            string param2 = context.RouteData.Values["param2"] as string;

            int m = 0;
            int y = 0;
            int cityid = 0;
            int catid = 0;
            int areaid = 0;
            string strcat = String.Empty;
            string strcity = String.Empty;
            string strarea = String.Empty;

            //if (String.CompareOrdinal(param1, param2) != 0)
            //{
            string sql = "";

            //Check values

            if (!int.TryParse(param1, out m) || !int.TryParse(param2, out y))
            {
                //Check for city filter first
                cityid = CityDL.GetIDByUrl(param2);
                strcity = param2;

                //Check for category filter
                catid = CategoriesDL.GetCategoryIdByUrl(param1);
                strcat = param1;

                if (cityid == 0 || catid == 0)
                {
                    if (param1 != null && param2 != null)
                    {
                        //Get combination areaid/cityid
                        sql =
                            "SELECT Area.AreaID, Area.CityID FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON Area.AreaID = AreaInfo.AreaID " +
                            "WHERE (AreaInfo.UrlName = @AREA) AND (CityInfo.UrlName = @CITY)";

                        using (SqlConnection conn = new SqlConnection(dataAccessHelper.ConnString))
                        {
                            conn.Open();

                            SqlDataReader rdr = dataAccessHelper.Data.ExecuteDatareader(conn, sql,
                                                                                new SqlParameter("@AREA", param2),
                                                                                new SqlParameter("@CITY", param1));

                            if (rdr.Read())
                            {
                                cityid = int.Parse(rdr["cityid"].ToString());
                                areaid = int.Parse(rdr["areaid"].ToString());
                            }
                            rdr.Close();

                            conn.Close();
                        }
                        strcity = (cityid != 0) ? param1 : String.Empty;
                        strarea = (areaid != 0) ? param2 : String.Empty;
                    }
                    else
                    {
                        return
                            BuildManager.CreateInstanceFromVirtualPath("/error.aspx", typeof(System.Web.UI.Page))
                            as
                            System.Web.UI.Page;
                    }
                }
            }
            else
            {
                if (m < 0 || m > 12)
                    return
                        BuildManager.CreateInstanceFromVirtualPath("/error.aspx", typeof(System.Web.UI.Page)) as
                        System.Web.UI.Page;
            }
            //}

            HttpContext.Current.Items["cityid"] = cityid;
            HttpContext.Current.Items["catid"] = catid;
            HttpContext.Current.Items["city"] = strcity;
            HttpContext.Current.Items["cat"] = strcat;
            HttpContext.Current.Items["areaid"] = areaid;
            HttpContext.Current.Items["area"] = strarea;
            HttpContext.Current.Items["month"] = m;
            HttpContext.Current.Items["year"] = y;

            if (MobileBlogHelper.isMobileVersionRequested(context))
            {
                return BuildManager.CreateInstanceFromVirtualPath("/blogfiles/mobile/default.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
            }
            return BuildManager.CreateInstanceFromVirtualPath("/blogfiles/default.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
        }
    }

    public class BlogRouteHandler_CatMetro : IRouteHandler, IRequiresSessionState
    {
        public IHttpHandler GetHttpHandler(RequestContext context)
        {
            var dataAccessHelper = new DataAccessHelper();

            string month = context.RouteData.Values["month"] as string;
            string year = context.RouteData.Values["year"] as string;
            string category = context.RouteData.Values["category"] as string;
            string metro = context.RouteData.Values["metro"] as string;
            string area;

            //Check values
            int m = 0;
            int y = 0;
            if (month != null && year != null)
            {
                if (!int.TryParse(month, out m) || !int.TryParse(year, out y))
                {
                    return BuildManager.CreateInstanceFromVirtualPath("/error.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
                }
                else
                {
                    if (m < 0 || m > 12)
                        return BuildManager.CreateInstanceFromVirtualPath("/error.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
                }
            }

            //Check for filter match
            int cityid = 0;
            int catid = 0;
            int areaid = 0; //TODO get area info for city

            //Check for city filter first
            string sql = "SELECT CityID FROM CityInfo WHERE (URLName = @METRO)";
            object o = dataAccessHelper.Data.ExecuteScalar(sql,
                new SqlParameter("@METRO", SqlDbType.VarChar, 250, ParameterDirection.Input, false, 0, 0, "URLName", DataRowVersion.Default, metro));

            if (o != null)
                cityid = int.Parse(o.ToString());

            //Check for category filter
            sql = "SELECT BlogCatID FROM BlogCategories WHERE (BlogCatUrlName = @CAT)";
            object j = dataAccessHelper.Data.ExecuteScalar(sql,
                new SqlParameter("@CAT", SqlDbType.VarChar, 250, ParameterDirection.Input, false, 0, 0, "BlogCatUrlName", DataRowVersion.Default, category));

            if (j != null)
                catid = int.Parse(j.ToString());

            if (cityid == 0 || catid == 0 || m == 0 || y == 0 || areaid == 0)
                return BuildManager.CreateInstanceFromVirtualPath("/error.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;

            //Check for area filter
            areaid = AreaDL.GetAreaIdByCityId(cityid.ToString());
            area = AreaDL.GetAreaUrlByID(areaid);

            HttpContext.Current.Items["cityid"] = cityid;
            HttpContext.Current.Items["catid"] = catid;
            HttpContext.Current.Items["areaid"] = areaid;
            HttpContext.Current.Items["city"] = metro;
            HttpContext.Current.Items["cat"] = category;
            HttpContext.Current.Items["area"] = area;
            HttpContext.Current.Items["month"] = m;
            HttpContext.Current.Items["year"] = y;

            if (MobileBlogHelper.isMobileVersionRequested(context))
            {
                return BuildManager.CreateInstanceFromVirtualPath("/blogfiles/mobile/default.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
            }
            return BuildManager.CreateInstanceFromVirtualPath("/blogfiles/default.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
        }
    }

    public class BlogRouteHandler_Post : IRouteHandler, IRequiresSessionState
    {
        public IHttpHandler GetHttpHandler(RequestContext context)
        {
            string title = context.RouteData.Values["title"] as string;

            //Check for filter match
            int postid = BlogPostDL.GetPostIdByUrlTitle(title);

            if (postid <= 0)
            {
                return
                    BuildManager.CreateInstanceFromVirtualPath("/error.aspx", typeof(System.Web.UI.Page)) as
                    System.Web.UI.Page;
            }

            HttpContext.Current.Items["title"] = title;
            HttpContext.Current.Items["postid"] = postid;

            if (MobileBlogHelper.isMobileVersionRequested(context))
            {
                return BuildManager.CreateInstanceFromVirtualPath("/blogfiles/mobile/post.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
            }
            return BuildManager.CreateInstanceFromVirtualPath("/blogfiles/post.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
        }
    }

    public class BlogRouteHandlerCategoryPost : IRouteHandler, IRequiresSessionState
    {
        public IHttpHandler GetHttpHandler(RequestContext context)
        {
            string title = context.RouteData.Values["title"] as string;
            string category = context.RouteData.Values["category"] as string;

            int postid = BlogPostDL.GetPostIdByUrlTitle(title);
            int catid = CategoriesDL.GetCategoryIdByUrl(category);

            if (postid <= 0)
            {
                return
                    BuildManager.CreateInstanceFromVirtualPath("/error.aspx", typeof(System.Web.UI.Page)) as
                    System.Web.UI.Page;
            }

            HttpContext.Current.Items["title"] = title;
            HttpContext.Current.Items["postid"] = postid;
            HttpContext.Current.Items["category"] = category;
            HttpContext.Current.Items["catid"] = catid;

            if (MobileBlogHelper.isMobileVersionRequested(context))
            {
                return
                    BuildManager.CreateInstanceFromVirtualPath("/blogfiles/mobile/post.aspx",
                                                               typeof(System.Web.UI.Page)) as System.Web.UI.Page;
            }
            return
                BuildManager.CreateInstanceFromVirtualPath("/blogfiles/post.aspx", typeof(System.Web.UI.Page)) as
                System.Web.UI.Page;
        }
    }

    public class BlogRouteHandler_Search : IRouteHandler, IRequiresSessionState
    {
        public IHttpHandler GetHttpHandler(RequestContext context)
        {
            if (MobileBlogHelper.isMobileVersionRequested(context))
            {
                return BuildManager.CreateInstanceFromVirtualPath("/blogfiles/mobile/search.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
            }
            return BuildManager.CreateInstanceFromVirtualPath("/blogfiles/search.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
        }
    }

    public class BlogRSSRouteHandler_Search : IRouteHandler, IRequiresSessionState
    {
        public IHttpHandler GetHttpHandler(RequestContext context)
        {
            var dataAccessHelper = new DataAccessHelper();

            string metro = context.RouteData.Values["metro"] as string;
            string sql = "";

            //Check for filter match
            int cityid = 0;

            //Check for city
            sql = "SELECT CityID FROM CityInfo WHERE (UrlName = @CITY)";
            object o = dataAccessHelper.Data.ExecuteScalar(sql,
                new SqlParameter("@CITY", SqlDbType.VarChar, 500, ParameterDirection.Input, false, 0, 0, "UrlName", DataRowVersion.Default, metro));

            if (o != null)
                cityid = int.Parse(o.ToString());
            else
                return BuildManager.CreateInstanceFromVirtualPath("/error.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;

            HttpContext.Current.Items["cityid"] = cityid;
            HttpContext.Current.Items["city"] = metro;

            return BuildManager.CreateInstanceFromVirtualPath("/blogfiles/rss.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
        }
    }

    public class EmailRouteHandler : IRouteHandler, IRequiresSessionState
    {
        public IHttpHandler GetHttpHandler(RequestContext context)
        {
            return BuildManager.CreateInstanceFromVirtualPath("/email.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
        }
    }

    public class BlogArticleListngRouteHandler : IRouteHandler, IRequiresSessionState
    {
        public IHttpHandler GetHttpHandler(RequestContext context)
        {
            return BuildManager.CreateInstanceFromVirtualPath("/blogfiles/articleListing.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
        }
    }

    public class PPCLandingRouteHandler : IRouteHandler, IRequiresSessionState
    {
        public IHttpHandler GetHttpHandler(RequestContext context)
        {
            var dataAccessHelper = new DataAccessHelper();

            string category = context.RouteData.Values["category"] as string;
            string city = context.RouteData.Values["city"] as string;
            string ppcname = context.RouteData.Values["ppcname"] as string;

            int ppcid = 0;
            int cityid = 0;
            int catid = 0;

            if (city != null && ppcname != null && category != null)
            {
                string sql = "SELECT PPCCategories.PPCCategoryID, PPCCategories.CityID, PPCCategories.CategoryID FROM CategoryCityRel INNER JOIN CategoryInfo ON CategoryCityRel.CategoryID = CategoryInfo.CategoryID INNER JOIN " +
                      "PPCCategories ON CategoryCityRel.CategoryID = PPCCategories.CategoryID AND CategoryCityRel.CityID = PPCCategories.CityID WHERE " +
                      "(CategoryInfo.UrlName = @CAT) AND (CategoryCityRel.UrlName = @CITY) AND (PPCCategories.PPCUrlTitle = @URL)";

                using (SqlConnection conn = new SqlConnection(dataAccessHelper.ConnString))
                {
                    conn.Open();

                    SqlDataReader rdr = dataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@URL", ppcname),
                        new SqlParameter("@CITY", city),
                        new SqlParameter("@CAT", category));

                    if (rdr.Read())
                    {
                        cityid = int.Parse(rdr["cityid"].ToString());
                        ppcid = int.Parse(rdr["ppccategoryid"].ToString());
                        catid = int.Parse(rdr["categoryid"].ToString());
                    }
                    rdr.Close();

                    conn.Close();
                }
            }

            if (cityid == 0 || ppcid == 0 || catid == 0)
            {
                //We have an error if a city or area was not found
                return BuildManager.CreateInstanceFromVirtualPath("/error.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
            }
            else
            {
                HttpContext.Current.Items["categoryid"] = catid;
                HttpContext.Current.Items["cityid"] = cityid;
                HttpContext.Current.Items["ppcid"] = ppcid;
                HttpContext.Current.Items["city"] = city;
                HttpContext.Current.Items["ppcname"] = ppcname;
                HttpContext.Current.Items["category"] = category;
                return BuildManager.CreateInstanceFromVirtualPath("/ppclanding.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
            }
        }
    }

    public class PageRedirectRouteHandler : IRouteHandler, IRequiresSessionState
    {
        public IHttpHandler GetHttpHandler(RequestContext context)
        {
            string page = context.RouteData.Values["pagename"] as string;
            page = page.ToLower();

            string basedomain = "";
            if (HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToString() != "localhost")
                basedomain = "http://" + HttpContext.Current.Request.ServerVariables["SERVER_NAME"];

            string url = basedomain + "/content/" + page;
            var response = HttpContext.Current.Response;
            response.Clear();
            response.Status = "301 Moved Permanently";
            response.AddHeader("Location", url);
            response.End();

            return BuildManager.CreateInstanceFromVirtualPath("/error.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
        }
    }

    public class BeaconRedirectRouteHandler : IRouteHandler, IRequiresSessionState
    {
        public IHttpHandler GetHttpHandler(RequestContext context)
        {
            string page = context.RouteData.Values["page"] as string;
            page = page.ToLower();
            string url = "";

            string basedomain = "";
            if (HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToString() != "localhost")
                basedomain = "http://" + HttpContext.Current.Request.ServerVariables["SERVER_NAME"];

            switch (page)
            {
                case "main.htm":
                    url = basedomain + "/dallas/dallas";
                    break;
                case "aboutus.htm":
                    url = basedomain + "/content/about-us";
                    break;
                case "faqs.htm":
                    url = basedomain + "/content/frequently-asked-questions";
                    break;
                case "feedbackform.htm":
                    url = basedomain + "/content/share-your-feedback";
                    break;
                case "consumertips.htm":
                    url = basedomain + "/content/consumer-tips";
                    break;
                case "aircond.htm":
                    url = basedomain + "/air-conditioning-and-heating/dallas/dallas";
                    break;
                case "carpetcleaners.htm":
                    url = basedomain + "/carpet-and-upholstery-cleaning/dallas/dallas";
                    break;
                case "chimney.htm":
                    url = basedomain + "/chimney-and-fireplace-work/dallas/dallas";
                    break;
                case "concrete.htm":
                    url = basedomain + "/dallas/dallas";
                    break;
                case "decks.htm":
                    url = basedomain + "/deck-building-and-maintenance/dallas/dallas";
                    break;
                case "drainage.htm":
                    url = basedomain + "/drainage-systems/dallas/dallas";
                    break;
                case "ductcleaning.htm":
                    url = basedomain + "/duct-cleaning/dallas/dallas";
                    break;
                case "electricians.htm":
                    url = basedomain + "/electricians/dallas/dallas";
                    break;
                case "fencing.htm":
                    url = basedomain + "/fences/dallas/dallas";
                    break;
                case "flooringhardwood.htm":
                    url = basedomain + "/flooring-hardwood/dallas/dallas";
                    break;
                case "foundations.htm":
                    url = basedomain + "/foundation-repair/dallas/dallas";
                    break;
                case "gutters.htm":
                    url = basedomain + "/gutter-installation/dallas/dallas";
                    break;
                case "lawnmaint.htm":
                    url = basedomain + "/dallas/dallas";
                    break;
                case "lawntreatment.htm":
                    url = basedomain + "/lawn-treatment/dallas/dallas";
                    break;
                case "masons.htm":
                    url = basedomain + "/dallas/dallas";
                    break;
                case "painters.htm":
                    url = basedomain + "/painters/dallas/dallas";
                    break;
                case "pestcontrol.htm":
                    url = basedomain + "/pest-and-terminte-control/dallas/dallas";
                    break;
                case "plumbers.htm":
                    url = basedomain + "/plumbers/dallas/dallas";
                    break;
                case "pools.htm":
                    url = basedomain + "/dallas/dallas";
                    break;
                case "windowcleaners.htm":
                    url = basedomain + "/window-cleaning-and-pressure-washing/dallas/dallas";
                    break;
                case "beaconreportsresearch.htm":
                    url = basedomain + "/content/methodology";
                    break;
                case "roofers.htm":
                    url = basedomain + "/roofers/dallas/dallas";
                    break;
                case "siding.htm":
                    url = basedomain + "/siding/dallas/dallas";
                    break;
                case "sprinklers.htm":
                    url = basedomain + "/sprinkler-systems/dallas/dallas";
                    break;
                case "tile.htm":
                    url = basedomain + "/tile-installation/dallas/dallas";
                    break;
                case "trees.htm":
                    url = basedomain + "/tree-services/dallas/dallas";
                    break;
                case "windows.htm":
                    url = basedomain + "/window-and-door-replacement/dallas/dallas";
                    break;
                case "totalair.htm":
                    url = basedomain + "/air-conditioning-and-heating/dallas/dallas/total-air-and-heat-co";
                    break;
                case "dallasplumbingac.htm":
                    url = basedomain + "/air-conditioning-and-heating/dallas/dallas/dallas-plumbing-company";
                    break;
                case "genie.htm":
                    url = basedomain + "/carpet-and-upholstery-cleaning/dallas/dallas/genie-carpet-cleaning-services";
                    break;
                case "joy.htm":
                    url = basedomain + "/carpet-and-upholstery-cleaning/dallas/dallas/joy-carpet-dry-cleaning";
                    break;
                case "chimneysweep.htm":
                    url = basedomain + "/drainage-systems/dallas/dallas/andys-sprinkler-drainage-and-lighting";
                    break;
                case "parkcities.htm":
                    url = basedomain + "/electricians/dallas/dallas/park-cities-electrical-co-inc";
                    break;
                case "youngblood.htm":
                    url = basedomain + "/flooring-hardwood/dallas/dallas/youngbloods-hardwood-flooring";
                    break;
                case "dallasgutter.htm":
                    url = basedomain + "/gutter-installation/dallas/dallas/dallas-gutter-and-repair";
                    break;
                case "all-safe.htm":
                    url = basedomain + "/pest-and-termite-control/dallas/dallas/all-safe-pest-and-termite";
                    break;
                case "barrett.htm":
                    url = basedomain + "/pest-and-termite-control/dallas/dallas/berrett-pest-control";
                    break;
                case "dallasplumbing.htm":
                    url = basedomain + "/plumbers/dallas/dallas/dallas-plumbing-company";
                    break;
                case "andyssprinkler.htm":
                    url = basedomain + "/sprinkler-systems/dallas/dallas/andy%E2%80%99s-sprinkler-drainage-and-lighting";
                    break;
                case "cgreensprinkler.htm":
                    url = basedomain + "/sprinkler-systems/dallas/dallas/cgreen-landscape-irrigation";
                    break;
                case "cherokee.htm":
                    url = basedomain + "/tile-installation/dallas/dallas/cherokee-tile";
                    break;
                default:
                    url = basedomain + "/";
                    break;
            }

            var response = HttpContext.Current.Response;
            response.Clear();
            response.Status = "301 Moved Permanently";
            response.AddHeader("Location", url);
            response.End();

            return BuildManager.CreateInstanceFromVirtualPath("/error.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
        }
    }

    public class ContractorRedirectRouteHandler : IRouteHandler, IRequiresSessionState
    {
        public IHttpHandler GetHttpHandler(RequestContext context)
        {
            string city = context.RouteData.Values["city"] as string;
            string area = context.RouteData.Values["area"] as string;
            string category = context.RouteData.Values["category"] as string;
            string contractor = context.RouteData.Values["contractor"] as string;

            BasePage basepage = new BasePage();

            string basedomain = "";
            if (HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToString() != "localhost")
                basedomain = "http://" + HttpContext.Current.Request.ServerVariables["SERVER_NAME"];

            string newcat = basepage.renameCat(category);

            contractor = contractor.Replace("'", "").Replace(".", "").Replace(",", "");

            string url = basedomain + "/" + newcat.ToLower() + "/" + city.ToLower() + "/" + area.ToLower() + "/" + contractor.ToLower();
            var response = HttpContext.Current.Response;
            response.Clear();
            response.Status = "301 Moved Permanently";
            response.AddHeader("Location", url);
            response.End();

            return BuildManager.CreateInstanceFromVirtualPath("/error.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
        }
    }

    public class CategoryRedirectRouteHandler : IRouteHandler, IRequiresSessionState
    {
        public IHttpHandler GetHttpHandler(RequestContext context)
        {
            string city = context.RouteData.Values["city"] as string;
            string area = context.RouteData.Values["area"] as string;
            string category = context.RouteData.Values["category"] as string;

            BasePage basepage = new BasePage();

            string newcat = basepage.renameCat(category);
            string url = "";

            string basedomain = "";
            if (HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToString() != "localhost")
                basedomain = "http://" + HttpContext.Current.Request.ServerVariables["SERVER_NAME"];

            if (city == "NA" && area == "NA")
                url = basedomain + "/" + newcat.ToLower() + "/atlanta/north-atlanta";
            else
                url = basedomain + "/" + newcat.ToLower() + "/" + city.ToLower() + "/" + area.ToLower();


            var response = HttpContext.Current.Response;
            response.Clear();
            response.Status = "301 Moved Permanently";
            response.AddHeader("Location", url);
            response.End();

            return BuildManager.CreateInstanceFromVirtualPath("/error.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
        }
    }

    public class AreaRedirectRouteHandler : IRouteHandler, IRequiresSessionState
    {
        public IHttpHandler GetHttpHandler(RequestContext context)
        {
            string city = context.RouteData.Values["city"] as string;
            string area = context.RouteData.Values["area"] as string;
            string category = context.RouteData.Values["category"] as string;

            string basedomain = "";
            if (HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToString() != "localhost")
                basedomain = "http://" + HttpContext.Current.Request.ServerVariables["SERVER_NAME"];

            string url = basedomain + "/" + category.ToLower() + "/" + city.ToLower() + "/" + area.ToLower();
            var response = HttpContext.Current.Response;
            response.Clear();
            response.Status = "301 Moved Permanently";
            response.AddHeader("Location", url);
            response.End();

            return BuildManager.CreateInstanceFromVirtualPath("/error.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
        }
    }

    public class CityRouteHandler : IRouteHandler, IRequiresSessionState
    {
        public IHttpHandler GetHttpHandler(RequestContext context)
        {
            var dataAccessHelper = new DataAccessHelper();

            string city = context.RouteData.Values["city"] as string;

            int cityid = 0;

            if (city != null)
            {
                //Get cityid
                string sql = "SELECT CityID FROM CityInfo WHERE (CityInfo.UrlName = @CITY)";

                using (SqlConnection conn = new SqlConnection(dataAccessHelper.ConnString))
                {
                    conn.Open();

                    SqlDataReader rdr = dataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@CITY", city));

                    if (rdr.Read())
                    {
                        cityid = int.Parse(rdr["cityid"].ToString());
                    }
                    rdr.Close();

                    conn.Close();
                }
            }

            if (cityid == 0)
            {
                //We have an error if a city or area was not found
                return BuildManager.CreateInstanceFromVirtualPath("/error.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
            }
            else
            {
                HttpContext.Current.Items["cityid"] = cityid;
                HttpContext.Current.Items["city"] = city;
                return BuildManager.CreateInstanceFromVirtualPath("/city.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
            }
        }
    }

    public class TestimonialRouteHandler : IRouteHandler, IRequiresSessionState
    {
        public IHttpHandler GetHttpHandler(RequestContext context)
        {
            if (MobileBlogHelper.isMobileVersionRequested(context))
            {
                if (MobileBlogHelper.isAdPageRequired(context))
                {
                    return BuildManager.CreateInstanceFromVirtualPath("/mobile/ad.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
                }
                return BuildManager.CreateInstanceFromVirtualPath("/mobile/testimonials.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
            }
            return BuildManager.CreateInstanceFromVirtualPath("/homeownertestimonials.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
        }
    }

    public class FeedbackRouteHandler : IRouteHandler, IRequiresSessionState
    {
        public IHttpHandler GetHttpHandler(RequestContext context)
        {
            return BuildManager.CreateInstanceFromVirtualPath("/feedback.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
        }
    }

    public class NominateRouteHandler : IRouteHandler, IRequiresSessionState
    {
        public IHttpHandler GetHttpHandler(RequestContext context)
        {
            if (MobileBlogHelper.isMobileVersionRequested(context))
            {
                if (MobileBlogHelper.isAdPageRequired(context))
                {
                    return BuildManager.CreateInstanceFromVirtualPath("/mobile/ad.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
                }
                return BuildManager.CreateInstanceFromVirtualPath("/mobile/nominationform.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
            }
            return BuildManager.CreateInstanceFromVirtualPath("/nominationform.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
        }
    }

    public class BookRouteHandler : IRouteHandler, IRequiresSessionState
    {
        public IHttpHandler GetHttpHandler(RequestContext context)
        {
            HttpContext.Current.Response.Redirect("/requestaguide");

            HttpContext.Current.Items["type"] = "2";
            return BuildManager.CreateInstanceFromVirtualPath("/dataforms.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
        }
    }

    public class GuideRouteHandler : IRouteHandler, IRequiresSessionState
    {
        public IHttpHandler GetHttpHandler(RequestContext context)
        {
            HttpContext.Current.Items["type"] = "2";
            return BuildManager.CreateInstanceFromVirtualPath("/dataforms.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
        }
    }

    public class NewsletterRouteHandler : IRouteHandler, IRequiresSessionState
    {
        public IHttpHandler GetHttpHandler(RequestContext context)
        {
            HttpContext.Current.Items["type"] = "1";
            return BuildManager.CreateInstanceFromVirtualPath("/dataforms.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
        }
    }

    public class TipRouteHandler : IRouteHandler, IRequiresSessionState
    {
        public IHttpHandler GetHttpHandler(RequestContext context)
        {
            var dataAccessHelper = new DataAccessHelper();

            string articlename = context.RouteData.Values["articlename"] as string;
            string category = context.RouteData.Values["category"] as string;

            // *** query database for page
            int articleid = 0;
            int catid = 0;

            using (SqlConnection conn = new SqlConnection(dataAccessHelper.ConnString))
            {
                conn.Open();

                string sql = "SELECT TipArticle.ArticleID, CategoryInfo.CategoryID FROM TipArticle INNER JOIN CategoryInfo ON TipArticle.CategoryID = CategoryInfo.CategoryID " +
                    "WHERE (TipArticle.UrlTitle = @TITLE) AND (CategoryInfo.UrlName = @CAT)";

                SqlDataReader rdr = dataAccessHelper.Data.ExecuteDatareader(conn, sql,
                    new SqlParameter("@TITLE", SqlDbType.VarChar, 150, ParameterDirection.Input, false, 0, 0, "UrlTitle", DataRowVersion.Default, articlename),
                    new SqlParameter("@CAT", SqlDbType.VarChar, 150, ParameterDirection.Input, false, 0, 0, "UrlName", DataRowVersion.Default, category));

                if (rdr.Read())
                {
                    articleid = int.Parse(rdr["ArticleID"].ToString());
                    catid = int.Parse(rdr["CategoryID"].ToString());
                }

                rdr.Close();
                conn.Close();
            }

            if (articleid == 0)
            {
                //We have an error if a page was not found
                return BuildManager.CreateInstanceFromVirtualPath("/error.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
            }
            else
            {
                HttpContext.Current.Items["articleid"] = articleid;
                HttpContext.Current.Items["categoryid"] = catid;
                if (MobileBlogHelper.isMobileVersionRequested(context))
                {
                    if (MobileBlogHelper.isAdPageRequired(context))
                    {
                        return BuildManager.CreateInstanceFromVirtualPath("/mobile/ad.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
                    }
                    return BuildManager.CreateInstanceFromVirtualPath("/mobile/article.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
                }
                return BuildManager.CreateInstanceFromVirtualPath("/article.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
            }
        }
    }

    public class PageRouteHandler : IRouteHandler, IRequiresSessionState
    {
        public IHttpHandler GetHttpHandler(RequestContext context)
        {
            string pagename = context.RouteData.Values["pagename"] as string;

            // *** query database for page
            int pageid = PageContentDL.GetPageIdByUrl(pagename);

            if (pageid == 0)
            {
                //We have an error if a page was not found
                return BuildManager.CreateInstanceFromVirtualPath("/error.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
            }
            else
            {
                HttpContext.Current.Items["pageid"] = pageid;

                if (MobileBlogHelper.isMobileVersionRequested(context))
                {
                    if (MobileBlogHelper.isAdPageRequired(context))
                    {
                        return BuildManager.CreateInstanceFromVirtualPath("/mobile/ad.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
                    }
                    return BuildManager.CreateInstanceFromVirtualPath("/mobile/pagecontent.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
                }
                return BuildManager.CreateInstanceFromVirtualPath("/pagecontent.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
            }
        }
    }

    public class CityAreaRouteHandler : IRouteHandler, IRequiresSessionState
    {
        public IHttpHandler GetHttpHandler(RequestContext context)
        {
            var dataAccessHelper = new DataAccessHelper();

            string city = context.RouteData.Values["city"] as string;
            string area = context.RouteData.Values["area"] as string;

            int cityid = 0;
            int areaid = 0;

            if (city != null && area != null)
            {
                //Get combination areaid/cityid
                string sql = "SELECT Area.AreaID, Area.CityID FROM Area INNER JOIN CityInfo ON Area.CityID = CityInfo.CityID INNER JOIN AreaInfo ON Area.AreaID = AreaInfo.AreaID " +
                    "WHERE (AreaInfo.UrlName = @AREA) AND (CityInfo.UrlName = @CITY)";

                using (SqlConnection conn = new SqlConnection(dataAccessHelper.ConnString))
                {
                    conn.Open();

                    SqlDataReader rdr = dataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@AREA", area),
                        new SqlParameter("@CITY", city));

                    if (rdr.Read())
                    {
                        cityid = int.Parse(rdr["cityid"].ToString());
                        areaid = int.Parse(rdr["areaid"].ToString());
                    }
                    rdr.Close();

                    conn.Close();
                }
            }

            if (cityid == 0 || areaid == 0)
            {
                //We have an error if a city or area was not found
                return BuildManager.CreateInstanceFromVirtualPath("/error.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
            }
            else
            {
                HttpContext.Current.Items["cityid"] = cityid;
                HttpContext.Current.Items["areaid"] = areaid;
                HttpContext.Current.Items["city"] = city;
                HttpContext.Current.Items["area"] = area;
                if (MobileBlogHelper.isMobileVersionRequested(context))
                {
                    if (MobileBlogHelper.isAdPageRequired(context))
                    {
                        return BuildManager.CreateInstanceFromVirtualPath("/mobile/ad.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
                    }
                    return BuildManager.CreateInstanceFromVirtualPath("/mobile/default.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
                }
                return BuildManager.CreateInstanceFromVirtualPath("/area.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
            }
        }
    }

    public class CategoryCityAreaRouteHandler : IRouteHandler, IRequiresSessionState
    {
        public IHttpHandler GetHttpHandler(RequestContext context)
        {
            var dataAccessHelper = new DataAccessHelper();

            string category = context.RouteData.Values["category"] as string;
            string city = context.RouteData.Values["city"] as string;
            string area = context.RouteData.Values["area"] as string;

            int cityid = 0;
            int areaid = 0;
            int catid = 0;
            int catareaid = 0;

            if (city != null && area != null && category != null)
            {
                string sql = "SELECT CategoryInfo.CategoryID, CategoryArea.CategoryAreaID, CityInfo.CityID, AreaInfo.AreaID " +
                        "FROM CategoryArea INNER JOIN CategoryInfo ON CategoryArea.CategoryID = CategoryInfo.CategoryID INNER JOIN " +
                        "AreaInfo ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CityInfo ON AreaInfo.CityID = CityInfo.CityID WHERE (AreaInfo.UrlName = @AREA) AND " +
                        "(CityInfo.UrlName = @CITY) AND (CategoryInfo.UrlName = @CAT)";

                using (SqlConnection conn = new SqlConnection(dataAccessHelper.ConnString))
                {
                    conn.Open();

                    SqlDataReader rdr = dataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@AREA", area),
                        new SqlParameter("@CITY", city),
                        new SqlParameter("@CAT", category));

                    if (rdr.Read())
                    {
                        cityid = int.Parse(rdr["cityid"].ToString());
                        areaid = int.Parse(rdr["areaid"].ToString());
                        catid = int.Parse(rdr["categoryid"].ToString());
                        catareaid = int.Parse(rdr["categoryareaid"].ToString());
                    }
                    rdr.Close();

                    conn.Close();
                }
            }

            if (cityid == 0 || areaid == 0 || catareaid == 0)
            {
                //We have an error if a city or area was not found
                return BuildManager.CreateInstanceFromVirtualPath("/error.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
            }
            else
            {
                HttpContext.Current.Items["categoryareaid"] = catareaid;
                HttpContext.Current.Items["categoryid"] = catid;
                HttpContext.Current.Items["cityid"] = cityid;
                HttpContext.Current.Items["areaid"] = areaid;
                HttpContext.Current.Items["city"] = city;
                HttpContext.Current.Items["area"] = area;
                HttpContext.Current.Items["category"] = category;
                HttpContext.Current.Items["isppc"] = "false";
                if (MobileBlogHelper.isMobileVersionRequested(context))
                {
                    if (MobileBlogHelper.isAdPageRequired(context))
                    {
                        return BuildManager.CreateInstanceFromVirtualPath("/mobile/ad.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
                    }
                    return BuildManager.CreateInstanceFromVirtualPath("/mobile/Category.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
                }
                return BuildManager.CreateInstanceFromVirtualPath("/category.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
            }
        }
    }

    public class CategoryCityAreaContractorRouteHandler : IRouteHandler, IRequiresSessionState
    {
        public IHttpHandler GetHttpHandler(RequestContext context)
        {
            var dataAccessHelper = new DataAccessHelper();

            string category = context.RouteData.Values["category"] as string;
            string city = context.RouteData.Values["city"] as string;
            string area = context.RouteData.Values["area"] as string;
            string contractor = context.RouteData.Values["contractor"] as string;

            int concatareaid = 0;
            int concatid = 0;
            int cityid = 0;
            int areaid = 0;
            int catid = 0;
            int cid = 0;

            if (city != null && area != null && category != null && contractor != null)
            {
                //Get combination areaid/cityid/categoryid/contractorid
                string sql = "SELECT CityInfo.CityID, AreaInfo.AreaID, ContractorInfo.ContractorID, CategoryInfo.CategoryID, ContractorCategoryAreaRel.ContractorCategoryID, ContractorCategoryAreaRel.ContractorCategoryAreaID FROM ContractorCategory INNER JOIN " +
                    "CategoryInfo ON ContractorCategory.CategoryID = CategoryInfo.CategoryID INNER JOIN ContractorInfo ON ContractorCategory.ContractorID = ContractorInfo.ContractorID " +
                    "INNER JOIN CityInfo INNER JOIN ContractorCategoryAreaRel INNER JOIN AreaInfo ON ContractorCategoryAreaRel.AreaID = AreaInfo.AreaID ON CityInfo.CityID = AreaInfo.CityID ON " +
                    "ContractorCategory.ContractorCategoryID = ContractorCategoryAreaRel.ContractorCategoryID WHERE (CityInfo.UrlName = @CITY) AND (AreaInfo.UrlName = @AREA) AND " +
                    "(CategoryInfo.UrlName = @CAT) AND (ContractorInfo.UrlName = @CONT)";

                using (SqlConnection conn = new SqlConnection(dataAccessHelper.ConnString))
                {
                    conn.Open();

                    SqlDataReader rdr = dataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@AREA", area),
                        new SqlParameter("@CITY", city),
                        new SqlParameter("@CAT", category),
                        new SqlParameter("@CONT", contractor));

                    if (rdr.Read())
                    {
                        concatareaid = int.Parse(rdr["contractorcategoryareaid"].ToString());
                        concatid = int.Parse(rdr["contractorcategoryid"].ToString());
                        cityid = int.Parse(rdr["cityid"].ToString());
                        areaid = int.Parse(rdr["areaid"].ToString());
                        catid = int.Parse(rdr["categoryid"].ToString());
                        cid = int.Parse(rdr["contractorid"].ToString());
                    }
                    rdr.Close();

                    conn.Close();
                }
            }

            if (cityid == 0 || areaid == 0 || catid == 0 || cid == 0)
            {
                //Redirect to category page instead of error page
                HttpContext.Current.Response.Redirect("/" + category + "/" + city + "/" + area);

                //We have an error if a city or area was not found
                return BuildManager.CreateInstanceFromVirtualPath("/error.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
            }
            else
            {
                HttpContext.Current.Items["contractorcategoryareaid"] = concatareaid;
                HttpContext.Current.Items["contractorcategoryid"] = concatid;
                HttpContext.Current.Items["categoryid"] = catid;
                HttpContext.Current.Items["cityid"] = cityid;
                HttpContext.Current.Items["areaid"] = areaid;
                HttpContext.Current.Items["city"] = city;
                HttpContext.Current.Items["area"] = area;
                HttpContext.Current.Items["contractorid"] = cid;
                HttpContext.Current.Items["isppc"] = "false";
                if (MobileBlogHelper.isMobileVersionRequested(context))
                {
                    if (MobileBlogHelper.isAdPageRequired(context))
                    {
                        return BuildManager.CreateInstanceFromVirtualPath("/mobile/ad.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
                    }
                    return BuildManager.CreateInstanceFromVirtualPath("/mobile/contractor.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
                }
                return BuildManager.CreateInstanceFromVirtualPath("/contractor.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
            }
        }
    }

    public class CategoryCityAreaRouteHandler_PPC : IRouteHandler, IRequiresSessionState
    {
        public IHttpHandler GetHttpHandler(RequestContext context)
        {
            var dataAccessHelper = new DataAccessHelper();

            string category = context.RouteData.Values["category"] as string;
            string city = context.RouteData.Values["city"] as string;
            string area = context.RouteData.Values["area"] as string;

            int cityid = 0;
            int areaid = 0;
            int catid = 0;
            int catareaid = 0;

            if (city != null && area != null && category != null)
            {
                string sql = "SELECT CategoryInfo.CategoryID, CategoryArea.CategoryAreaID, CityInfo.CityID, AreaInfo.AreaID " +
                        "FROM CategoryArea INNER JOIN CategoryInfo ON CategoryArea.CategoryID = CategoryInfo.CategoryID INNER JOIN " +
                        "AreaInfo ON CategoryArea.AreaID = AreaInfo.AreaID INNER JOIN CityInfo ON AreaInfo.CityID = CityInfo.CityID WHERE (AreaInfo.UrlName = @AREA) AND " +
                        "(CityInfo.UrlName = @CITY) AND (CategoryInfo.UrlName = @CAT)";

                using (SqlConnection conn = new SqlConnection(dataAccessHelper.ConnString))
                {
                    conn.Open();

                    SqlDataReader rdr = dataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@AREA", area),
                        new SqlParameter("@CITY", city),
                        new SqlParameter("@CAT", category));

                    if (rdr.Read())
                    {
                        cityid = int.Parse(rdr["cityid"].ToString());
                        areaid = int.Parse(rdr["areaid"].ToString());
                        catid = int.Parse(rdr["categoryid"].ToString());
                        catareaid = int.Parse(rdr["categoryareaid"].ToString());
                    }
                    rdr.Close();

                    conn.Close();
                }
            }

            if (cityid == 0 || areaid == 0 || catareaid == 0)
            {
                //We have an error if a city or area was not found
                return BuildManager.CreateInstanceFromVirtualPath("/error.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
            }
            else
            {
                HttpContext.Current.Items["categoryareaid"] = catareaid;
                HttpContext.Current.Items["categoryid"] = catid;
                HttpContext.Current.Items["cityid"] = cityid;
                HttpContext.Current.Items["areaid"] = areaid;
                HttpContext.Current.Items["city"] = city;
                HttpContext.Current.Items["area"] = area;
                HttpContext.Current.Items["category"] = category;
                HttpContext.Current.Items["isppc"] = "true";
                if (MobileBlogHelper.isMobileVersionRequested(context))
                {
                    if (MobileBlogHelper.isAdPageRequired(context))
                    {
                        return BuildManager.CreateInstanceFromVirtualPath("/mobile/ad.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
                    }
                    return BuildManager.CreateInstanceFromVirtualPath("/mobile/Category.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
                }
                return BuildManager.CreateInstanceFromVirtualPath("/category.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
            }
        }
    }

    public class CategoryCityAreaContractorRouteHandler_PPC : IRouteHandler, IRequiresSessionState
    {
        public IHttpHandler GetHttpHandler(RequestContext context)
        {
            var dataAccessHelper = new DataAccessHelper();

            string category = context.RouteData.Values["category"] as string;
            string city = context.RouteData.Values["city"] as string;
            string area = context.RouteData.Values["area"] as string;
            string contractor = context.RouteData.Values["contractor"] as string;

            int concatareaid = 0;
            int concatid = 0;
            int cityid = 0;
            int areaid = 0;
            int catid = 0;
            int cid = 0;

            if (city != null && area != null && category != null && contractor != null)
            {
                //Get combination areaid/cityid/categoryid/contractorid
                string sql = "SELECT CityInfo.CityID, AreaInfo.AreaID, ContractorInfo.ContractorID, CategoryInfo.CategoryID, ContractorCategoryAreaRel.ContractorCategoryID, ContractorCategoryAreaRel.ContractorCategoryAreaID FROM ContractorCategory INNER JOIN " +
                    "CategoryInfo ON ContractorCategory.CategoryID = CategoryInfo.CategoryID INNER JOIN ContractorInfo ON ContractorCategory.ContractorID = ContractorInfo.ContractorID " +
                    "INNER JOIN CityInfo INNER JOIN ContractorCategoryAreaRel INNER JOIN AreaInfo ON ContractorCategoryAreaRel.AreaID = AreaInfo.AreaID ON CityInfo.CityID = AreaInfo.CityID ON " +
                    "ContractorCategory.ContractorCategoryID = ContractorCategoryAreaRel.ContractorCategoryID WHERE (CityInfo.UrlName = @CITY) AND (AreaInfo.UrlName = @AREA) AND " +
                    "(CategoryInfo.UrlName = @CAT) AND (ContractorInfo.UrlName = @CONT)";

                using (SqlConnection conn = new SqlConnection(dataAccessHelper.ConnString))
                {
                    conn.Open();

                    SqlDataReader rdr = dataAccessHelper.Data.ExecuteDatareader(conn, sql,
                        new SqlParameter("@AREA", area),
                        new SqlParameter("@CITY", city),
                        new SqlParameter("@CAT", category),
                        new SqlParameter("@CONT", contractor));

                    if (rdr.Read())
                    {
                        concatareaid = int.Parse(rdr["contractorcategoryareaid"].ToString());
                        concatid = int.Parse(rdr["contractorcategoryid"].ToString());
                        cityid = int.Parse(rdr["cityid"].ToString());
                        areaid = int.Parse(rdr["areaid"].ToString());
                        catid = int.Parse(rdr["categoryid"].ToString());
                        cid = int.Parse(rdr["contractorid"].ToString());
                    }
                    rdr.Close();

                    conn.Close();
                }
            }

            if (cityid == 0 || areaid == 0 || catid == 0 || cid == 0)
            {
                //We have an error if a city or area was not found
                return BuildManager.CreateInstanceFromVirtualPath("/error.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
            }
            else
            {
                HttpContext.Current.Items["contractorcategoryareaid"] = concatareaid;
                HttpContext.Current.Items["contractorcategoryid"] = concatid;
                HttpContext.Current.Items["categoryid"] = catid;
                HttpContext.Current.Items["cityid"] = cityid;
                HttpContext.Current.Items["areaid"] = areaid;
                HttpContext.Current.Items["city"] = city;
                HttpContext.Current.Items["area"] = area;
                HttpContext.Current.Items["contractorid"] = cid;
                HttpContext.Current.Items["isppc"] = "true";

                if (MobileBlogHelper.isMobileVersionRequested(context))
                {
                    if (MobileBlogHelper.isAdPageRequired(context))
                    {
                        return BuildManager.CreateInstanceFromVirtualPath("/mobile/ad.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
                    }
                    return BuildManager.CreateInstanceFromVirtualPath("/mobile/contractor.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
                }
                return BuildManager.CreateInstanceFromVirtualPath("/contractor.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
            }
        }
    }

    public class CategoriesRouteHandler : IRouteHandler, IRequiresSessionState
    {
        public IHttpHandler GetHttpHandler(RequestContext context)
        {
            if (MobileBlogHelper.isMobileVersionRequested(context))
            {
                if (MobileBlogHelper.isAdPageRequired(context))
                {
                    return BuildManager.CreateInstanceFromVirtualPath("/mobile/ad.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
                }
                return BuildManager.CreateInstanceFromVirtualPath("/mobile/CategoriesList.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
            }
            return BuildManager.CreateInstanceFromVirtualPath("/default.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
        }
    }

    public class SearchRouteHandler : IRouteHandler, IRequiresSessionState
    {
        public IHttpHandler GetHttpHandler(RequestContext context)
        {
            if (MobileBlogHelper.isMobileVersionRequested(context))
            {
                if (MobileBlogHelper.isAdPageRequired(context))
                {
                    return BuildManager.CreateInstanceFromVirtualPath("/mobile/ad.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
                }
                return BuildManager.CreateInstanceFromVirtualPath("/mobile/search.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
            }
            return BuildManager.CreateInstanceFromVirtualPath("/search.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
        }
    }

    public class ErrorRouteHandler : IRouteHandler, IRequiresSessionState
    {
        public IHttpHandler GetHttpHandler(RequestContext context)
        {
            if (MobileBlogHelper.isMobileVersionRequested(context))
            {
                return BuildManager.CreateInstanceFromVirtualPath("/mobile/error.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
            }
            return BuildManager.CreateInstanceFromVirtualPath("/error.aspx", typeof(System.Web.UI.Page)) as System.Web.UI.Page;
        }
    }
}
