using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.IO;
using System.Data.SqlClient;
using ER_BestPickReports_Dev.Helpers;

namespace ER_BestPickReports_Dev
{
    public class Global : System.Web.HttpApplication
    {
        /// <summary>
        /// Save image to file system - Checks for existing file name and rename if necessary
        /// </summary>
        static public string GetSaveFileName(string savePath, string fileName)
        {
            string tempfileName = "";
            string ext = "";
            string pathToCheck = savePath + "/" + fileName;

            // Check to see if a file already exists with the same name as the file to upload.        
            if (System.IO.File.Exists(pathToCheck))
            {
                int counter = 1;
                while (System.IO.File.Exists(pathToCheck))
                {
                    // if a file with this name already exists, add _1 to the filename.
                    ext = System.IO.Path.GetExtension(fileName);
                    tempfileName = System.IO.Path.GetFileNameWithoutExtension(fileName) + "_" + counter.ToString() + ext;
                    pathToCheck = savePath + "/" + tempfileName;
                    counter++;
                }

                return tempfileName;
            }
            else
            {
                return fileName;
            }
        }

        public static void SendEmailNotification(string EmailTo, string EmailSubject, string EmailFrom, string EmailBody, bool IsHTML)
        {
            if (ConfigurationManager.AppSettings["SendEmailToTest"].ToLower() == "true")
            {
                EmailBody = "TEST ENVIRONMENT EMAIL<br/>" + "To: " + EmailTo +
                            "<br/>Email Resumes Here -------------------------<br/><br/>" + EmailBody;
                EmailTo = ConfigurationManager.AppSettings["TestEmailAddress"];
            }
            //else
            //{
            //    if (!BWConfig.IsProduction())
            //    {
            //        EmailBody = "TEST ENVIRONMENT EMAIL<br/>" + "To: " + EmailTo +
            //                    "<br/>Email Resumes Here -------------------------<br/><br/>" + EmailBody;
            //        EmailTo = BWConfig.testEnvironmentEmail;
            //    }
            //    else if (BWConfig.IsPreStaging())
            //    {
            //        EmailBody = "PRODUCTION ENVIRONMENT EMAIL<br/>" + "To: " + EmailTo +
            //                    "<br/>Email Resumes Here -------------------------<br/><br/>" + EmailBody;
            //        EmailTo = BWConfig.testEnvironmentEmail;
            //    }
            //}

            MailMessage msg = new MailMessage(EmailFrom, EmailTo);

            // Add all of the email information
            msg.Subject = EmailSubject;
            msg.Body = EmailBody;
            msg.IsBodyHtml = IsHTML;

            //Send email
            SmtpClient smtp = new SmtpClient();
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(msg);
        }

        public static void SaveFormData(string firstname, string mi, string lastname, string email, string address, string city, string state, string zip, bool updates, bool future, int type, int areaid, int cityid)
        {
            var dataSetHepler = new DataAccessHelper();
            string sql = "INSERT INTO FormData (DataType, AreaID, CityID, FirstName, MI, LastName, Email, Address, City, State, Zip, Updates, FutureEditions) VALUES (@TYPE, @AREAID, @CITYID, @FNAME, @MI, @LNAME, @EMAIL, @ADDRESS, @CITY, @STATE, @ZIP, @UPDATES, @FUTURE)";
            dataSetHepler.Data.ExecuteNonQuery(sql,
                new SqlParameter("@TYPE", type),
                new SqlParameter("@CITYID", cityid),
                new SqlParameter("@AREAID", areaid),
                new SqlParameter("@FNAME", firstname),
                new SqlParameter("@LNAME", lastname),
                new SqlParameter("@MI", mi),
                new SqlParameter("@EMAIL", email),
                new SqlParameter("@ADDRESS", address),
                new SqlParameter("@CITY", city),
                new SqlParameter("@STATE", state),
                new SqlParameter("@ZIP", zip),
                new SqlParameter("@FUTURE", future),
                new SqlParameter("@UPDATES", updates));

            //Send email
            string msg = "<span style=\"font-family:Arial; font-size:12px;\">";
            string subject = "";

            msg += "<p>This form submission was made on <strong>" + DateTime.Now.ToString() + "</strong></p>";

            if (type == 1)
            {
                subject = "Newsletter Sign Up Submission";
                msg += "<p>A user has signed up for monthly home care tips and articles via <a href=\"www.bestpickreports.com\">www.bestpickreports.com</a></p><p>Details are below:</p>";
            }
            else if (type == 2)
            {
                subject = "Request a Guide Submission";
                msg += "<p>A user has requested a guide via <a href=\"www.bestpickreports.com\">www.bestpickreports.com</a></p><p>Details are below:</p>";
            }

            msg += "<strong>First Name: </strong>" + firstname + "<br />";
            msg += "<strong>Middle Initial: </strong>" + mi + "<br />";
            msg += "<strong>Last Name: </strong>" + lastname + "<br />";
            msg += "<strong>Email Address: </strong>" + email + "<br />";

            if (type == 2)
            {
                msg += "<strong>Address: </strong>" + address + "<br />";
                msg += "<strong>City: </strong>" + city + "<br />";
                msg += "<strong>State: </strong>" + state + "<br />";
                msg += "<strong>Zip Code: </strong>" + zip + "<br />";
                msg += "<strong>Receive Updates: </strong>" + updates + "<br />";
            }

            msg += "</span>";

            try
            {
                SendEmailNotification("marketing@ebscoresearch.com", subject, ConfigurationManager.AppSettings["EmailRequest"], msg, true);
            }
            catch (Exception)
            {
            }
        }

        public static bool IsEmail(string inputEmail)
        {
            inputEmail = inputEmail.Trim();
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }

        public static bool IsPhoneNumber(string inputPhone)
        {
            int numberCount = 0;
            char[] chars = inputPhone.ToCharArray();
            // Count the number of digits in the string
            foreach (char c in chars)
            {
                if (Char.IsDigit(c))
                    numberCount++;
            }

            // US phone number is 10 digits
            if (numberCount != 10)
                return false;

            return true;
        }

        public static bool IsZipCode(string inputZip)
        {
            int numberCount = 0;
            char[] chars = inputZip.ToCharArray();
            // Count the number of digits in the string
            foreach (char c in chars)
            {
                if (Char.IsDigit(c))
                    numberCount++;
            }

            // US phone number is 10 digits
            if (numberCount != 5)
                return false;

            return true;
        }

        void Application_Start(object sender, EventArgs e)
        {
            //For URL rewrites
            RegisterRoutes(RouteTable.Routes);

            RegisterBundles(BundleTable.Bundles);
        }

        private void RegisterBundles(BundleCollection bundles)
        {
            /***** JAVASCRIPT ******/

            // global
            bundles.Add(new ScriptBundle("~/Scripts/autoHeight").Include(
                        "~/js/autoHeight.js"));

            bundles.Add(new ScriptBundle("~/Scripts/jquery").Include(
                        "~/js/jquery-1.9.0.min.js",
                        "~/js/jquery.cookie.js",
                        "~/js/jquery.validate.min.js"));

            // site desktop
            bundles.Add(new ScriptBundle("~/Scripts/site-desktop/main").Include(
                        "~/js/main.js"));

            // site mobile
            bundles.Add(new ScriptBundle("~/Scripts/site-mobile/global").Include(
                        "~/mobile/scripts/global.js"));
            bundles.Add(new ScriptBundle("~/Scripts/site-mobile/home").Include(
                        "~/mobile/scripts/home.js"));

            // blog desktop
            bundles.Add(new ScriptBundle("~/Scripts/blog-desktop/main").Include(
                        "~/blogfiles/js/main.js"));

            // blog mobile
            bundles.Add(new ScriptBundle("~/Scripts/blog-mobile/global").Include(
                        "~/blogfiles/mobile/js/global.js"));
            bundles.Add(new ScriptBundle("~/Scripts/blog-mobile/post").Include(
                        "~/blogfiles/mobile/js/post.js"));

            /***** CSS ******/

            // site desktop
            bundles.Add(new StyleBundle("~/bundles/site-desktop/styles").Include(
                        "~/css/style.css",
                        "~/css/jquery.fancybox.css",
                        "~/css/social_networks_styles.css"));
            bundles.Add(new StyleBundle("~/bundles/site-desktop/style_inline").Include(
                        "~/css/style_inline.css"));
            bundles.Add(new StyleBundle("~/bundles/site-desktop/TestimonialStyle").Include(
                        "~/css/TestimonialStyle.css"));

            // site mobile
            bundles.Add(new StyleBundle("~/bundles/site-mobile/master").Include(
                        "~/mobile/css/emailform.css",
                        "~/mobile/css/font-awesome.min.css",
                        "~/mobile/css/footer.css",
                        "~/mobile/css/modal.css",
                        "~/css/social_networks_styles.css"));
            bundles.Add(new StyleBundle("~/bundles/site-mobile/sprite").Include(
                        "~/mobile/css/sprite.css"));
            bundles.Add(new StyleBundle("~/bundles/site-mobile/global").Include(
                        "~/mobile/css/global.css"));
            bundles.Add(new StyleBundle("~/bundles/site-mobile/default").Include(
                        "~/mobile/css/home.css",
                        "~/mobile/css/post.css"));
            bundles.Add(new StyleBundle("~/bundles/site-mobile/pagecontent").Include(
                        "~/mobile/css/pagecontent.css"));
            bundles.Add(new StyleBundle("~/bundles/site-mobile/search").Include(
                        "~/mobile/css/search.css"));
            bundles.Add(new StyleBundle("~/bundles/site-mobile/testimonials").Include(
                        "~/mobile/css/testimonials.css"));
            bundles.Add(new StyleBundle("~/bundles/site-mobile/contractor").Include(
                        "~/mobile/css/contractor.css",
                        "~/mobile/css/contractor-testimonials.css"));
            bundles.Add(new StyleBundle("~/bundles/site-mobile/ad").Include(
                        "~/mobile/css/ad.css"));
            bundles.Add(new StyleBundle("~/bundles/site-mobile/categoriesList").Include(
                        "~/mobile/css/categoriesList.css"));
            bundles.Add(new StyleBundle("~/bundles/site-mobile/category").Include(
                        "~/mobile/css/category.css"));
            bundles.Add(new StyleBundle("~/bundles/site-mobile/style_inline").Include(
                        "~/mobile/css/style_inline.css"));

            // blog desktop
            bundles.Add(new StyleBundle("~/bundles/blog-desktop/master").Include(
                        "~/blogfiles/css/new_style.css",
                        "~/blogfiles/css/sidebar.css",
                        "~/blogfiles/css/post.css",
                        "~/blogfiles/css/email-form.css",
                        "~/css/social_networks_styles.css"));

            // blog mobile
            bundles.Add(new StyleBundle("~/bundles/blog-mobile/master").Include(
                        "~/blogfiles/mobile/css/bootstrap.min.css",
                        "~/blogfiles/mobile/css/global.css",
                        "~/blogfiles/mobile/css/emailform.css",
                        "~/blogfiles/mobile/css/modal.css",
                        "~/blogfiles/mobile/css/footer.css",
                        "~/blogfiles/mobile/css/post.css",
                        "~/mobile/css/font-awesome.min.css",
                        "~/css/social_networks_styles.css"));
        }

        private void RegisterRoutes(RouteCollection routes)
        {
            routes.Add("error", new Route("error", new ErrorRouteHandler()));

            routes.Add(new Route("images/{*pathInfo}", new StopRoutingHandler()));
            routes.Add(new Route("css/{*pathInfo}", new StopRoutingHandler()));
            routes.Add(new Route("js/{*pathInfo}", new StopRoutingHandler()));
            routes.Add(new Route("fonts/{*pathInfo}", new StopRoutingHandler()));
            routes.Add(new Route("{resource}.axd/{*pathInfo}", new StopRoutingHandler()));
            routes.Add(new Route("inlinecontent.aspx/{*pathInfo}", new StopRoutingHandler()));
            routes.Add(new Route("assets/{*pathInfo}", new StopRoutingHandler()));
            routes.Add(new Route("blogfiles/assets/{*pathInfo}", new StopRoutingHandler()));

            routes.Add("oldcat", new Route("Category/{id}/{city}/{area}/{category}", new CategoryRedirectRouteHandler()));
            routes.Add("oldarea", new Route("Area/{areaid}/{catid}/{city}/{area}/{category}", new AreaRedirectRouteHandler()));
            routes.Add("oldcon", new Route("Contractor/{catid}/{conid}/{city}/{area}/{category}/{contractor}", new ContractorRedirectRouteHandler()));
            routes.Add("oldpage", new Route("Content/{id}/{pagename}", new PageRedirectRouteHandler()));
            routes.Add("beacon", new Route("da/{page}", new BeaconRedirectRouteHandler()));

            routes.Add("categorycityareappc", new Route("ppc/{category}/{city}/{area}", new CategoryCityAreaRouteHandler_PPC()));
            routes.Add("categorycityareacontractorppc", new Route("ppc/{category}/{city}/{area}/{contractor}", new CategoryCityAreaContractorRouteHandler_PPC()));
            routes.Add("ppclanding", new Route("ppclanding/{category}/{city}/{ppcname}", new PPCLandingRouteHandler()));

            routes.Add("blog", new Route("blog", new BlogRouteHandler()));
            routes.Add("blogsearch", new Route("blog/search", new BlogRouteHandler_Search()));
            routes.Add("blogrss", new Route("blog/rss/{metro}", new BlogRSSRouteHandler_Search()));
            routes.Add("blogpost", new Route("blog/post/{title}", new BlogRouteHandler_Post()));
            routes.Add("blogcategorypost", new Route("blog/post/{category}/{title}", new BlogRouteHandlerCategoryPost()));
            //routes.Add("blogtkmmy", new Route("blog/tag/{keyword}/{metro}/{month}/{year}", new BlogRouteHandler()));
            //routes.Add("blogtkmy", new Route("blog/tag/{keyword}/{month}/{year}", new BlogRouteHandler()));
            //routes.Add("blogtkm", new Route("blog/tag/{keyword}/{metro}", new BlogRouteHandler()));
            //routes.Add("blogtk", new Route("blog/tag/{keyword}", new BlogRouteHandler()));
            routes.Add("blogcmmy", new Route("blog/{category}/{metro}/{month}/{year}", new BlogRouteHandler_CatMetro()));
            routes.Add("blogfmy", new Route("blog/{filter}/{month}/{year}", new BlogRouteHandler())); // blog/category/month/year OR blog/metro/month/year
            routes.Add("blogmy", new Route("blog/{param1}/{param2}", new BlogRouteHandler_Param())); // blog/category/metro OR blog/month/year
            routes.Add("blogf", new Route("blog/{filter}", new BlogRouteHandler())); // blog/category OR blog/metro
            routes.Add("email", new Route("email", new EmailRouteHandler()));
            routes.Add("bloglisting", new Route("articleListing", new BlogArticleListngRouteHandler()));

            routes.Add("test", new Route("testimonials", new TestimonialRouteHandler()));
            routes.Add("nominate", new Route("nominate-a-company", new NominateRouteHandler()));
            routes.Add("book", new Route("requestabook", new BookRouteHandler()));
            routes.Add("guide", new Route("requestaguide", new GuideRouteHandler()));
            routes.Add("newsletter", new Route("newsletter-sign-up", new NewsletterRouteHandler()));
            routes.Add("feedback", new Route("share-your-feedback", new FeedbackRouteHandler()));
            routes.Add("page", new Route("content/{pagename}", new PageRouteHandler()));
            routes.Add("tips", new Route("{category}/tips/{articlename}", new TipRouteHandler()));
            routes.Add("cityarea", new Route("{city}/{area}", new CityAreaRouteHandler()));
            routes.Add("categorycityarea", new Route("{category}/{city}/{area}", new CategoryCityAreaRouteHandler()));
            routes.Add("categorycityareacontractor", new Route("{category}/{city}/{area}/{contractor}", new CategoryCityAreaContractorRouteHandler()));
            routes.Add("categories", new Route("categories", new CategoriesRouteHandler()));
            routes.Add("search", new Route("search", new SearchRouteHandler()));
            routes.Add("city", new Route("{city}", new CityRouteHandler()));
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            const int eventId = 234;  
            const string logSource = "Best Pick Reports";

            var errorMessageBuiler = new StringBuilder();
            try
            {
                Exception ex = Server.GetLastError().GetBaseException();

                string strIP = Request.UserHostAddress;
                string strUrlReferrer = (Request.UrlReferrer != null) ? Request.UrlReferrer.ToString() : "";

                errorMessageBuiler.AppendLine("The following error occurred");
                errorMessageBuiler.AppendLine("Message: " + ex.Message);
                errorMessageBuiler.AppendLine("Request Url: " + Request.Url);
                errorMessageBuiler.AppendLine("Url IP: " + strIP);
                errorMessageBuiler.AppendLine("Url Referrer: " + strUrlReferrer);
                errorMessageBuiler.AppendLine("Stack Trace:\n" + ex.StackTrace);
                errorMessageBuiler.AppendLine("Target Site: " + ex.TargetSite);
                errorMessageBuiler.AppendLine("Source: " + ex.Source);
                errorMessageBuiler.AppendLine("\nRequest.ServerVariable:");

                if (Request != null)
                {
                    foreach (string key in Request.ServerVariables.AllKeys)
                    {
                        errorMessageBuiler.AppendLine(key.ToString() + " = " + Request.ServerVariables[key]);
                    }
                }
                EventLog.WriteEntry(logSource, errorMessageBuiler.ToString(), EventLogEntryType.Error, eventId);
            }
            catch
            {
            }

            Response.Redirect("/error.aspx");
        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started

        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }

    }
}
