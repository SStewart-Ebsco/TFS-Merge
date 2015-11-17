using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ER_BestPickReports_Dev.App_Code
{
    public class BWSession
    {
        public static bool emailStartedAndSent
        {
            get
            {
                if (HttpContext.Current.Session["emailStartedAndSent"] != null)
                {
                    bool tempBool;
                    Boolean.TryParse(HttpContext.Current.Session["emailStartedAndSent"].ToString(), out tempBool);
                    return tempBool;
                }
                else
                    return false;
            }
            set { HttpContext.Current.Session["emailStartedAndSent"] = value; }
        }

        public static int tempAreaID
        {
            get
            {
                if (HttpContext.Current.Items["areaid"] != null)
                {
                    int tempInt;
                    Int32.TryParse(HttpContext.Current.Items["areaid"].ToString(), out tempInt);
                    return tempInt;
                }
                else
                    return 0;
            }
            set { HttpContext.Current.Items["areaid"] = value; }
        }

        public static int tempCityID
        {
            get
            {
                if (HttpContext.Current.Items["cityid"] != null)
                {
                    int tempInt;
                    Int32.TryParse(HttpContext.Current.Items["cityid"].ToString(), out tempInt);
                    return tempInt;
                }
                else
                    return 0;
            }
            set { HttpContext.Current.Items["cityid"] = value; }
        }

        public static int tempCategoryID
        {
            get
            {
                if (HttpContext.Current.Items["categoryid"] != null)
                {
                    int tempInt;
                    Int32.TryParse(HttpContext.Current.Items["categoryid"].ToString(), out tempInt);
                    return tempInt;
                }
                else
                    return 0;
            }
            set { HttpContext.Current.Items["categoryid"] = value; }
        }

        public static int tempContractorID
        {
            get
            {
                if (HttpContext.Current.Items["contractorid"] != null)
                {
                    int tempInt;
                    Int32.TryParse(HttpContext.Current.Items["contractorid"].ToString(), out tempInt);
                    return tempInt;
                }
                else
                    return 0;
            }
            set { HttpContext.Current.Items["contractorid"] = value; }
        }

        public static string tempContractorCategoryID
        {
            get
            {
                if (HttpContext.Current.Items["contractorcategoryid"] != null)
                {
                    string value = HttpContext.Current.Items["contractorcategoryid"].ToString();
                    if (value.Equals("0"))
                        return "";
                    else
                        return value;
                }
                else
                    return "";
            }
            set { HttpContext.Current.Items["contractorcategoryid"] = value; }
        }

        public static string tempCityID_String
        {
            get
            {
                if (HttpContext.Current.Items["city"] != null)
                {
                    string value = HttpContext.Current.Items["city"].ToString();
                    if (value.Equals("0"))
                        return "";
                    else
                        return value;
                }
                else
                    return "";
            }
            set { HttpContext.Current.Items["city"] = value; }
        }

        public static string tempAreaID_String
        {
            get
            {
                if (HttpContext.Current.Items["area"] != null)
                {
                    string value = HttpContext.Current.Items["area"].ToString();
                    if (value.Equals("0"))
                        return "";
                    else
                        return value;
                }
                else
                    return "";
            }
            set { HttpContext.Current.Items["area"] = value; }
        }

        public static string tempIsPPC_String
        {
            get
            {
                if (HttpContext.Current.Items["isppc"] != null)
                {
                    string value = HttpContext.Current.Items["isppc"].ToString();
                    if (value.Equals(""))
                        return "false";
                    else
                        return value;
                }
                else
                    return "false";
            }
            set { HttpContext.Current.Items["isppc"] = value; }
        }
    }
}