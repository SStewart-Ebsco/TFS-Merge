using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ER_BestPickReports_Dev.App_Code.Interfaces;

namespace ER_BestPickReports_Dev.App_Code.Models
{
    public class AppCookies : IAppCookies
    {
        #region [Constants]

        private const string CookieName = "bprpreferences";

        public const string CITY_ID = "cityid";
        public const string CITY_NAME = "cityname";
        public const string CITY_URL_NAME = "cityurlname";
        public const string AREA_ID = "areaid";
        public const string AREA_NAME = "areaname";
        public const string AREA_URL_NAME = "areaurlname";
        public const string OUT_OF_MARKET_ZIP = "outOfMarketZip";
        public const string CATEGORY_ID = "categoryid";
        public const string CATEGORY_NAME = "categoryname";


        #endregion

        #region[Fields]

        private readonly ICookieContainer _cookieContainer;

        #endregion

        #region[Properties]

        public int CityId
        {
            get
            {
                Ensure(CookieName);
                return _cookieContainer.GetValue<int>(CookieName, CITY_ID);
            }
            set
            {
                Ensure(CookieName);
                _cookieContainer.SetValue(CookieName, CITY_ID, value);
            }
        }

        public int AreaId
        {
            get
            {
                Ensure(CookieName);
                return _cookieContainer.GetValue<int>(CookieName, AREA_ID);
            }
            set
            {
                Ensure(CookieName);
                _cookieContainer.SetValue(CookieName, AREA_ID, value);
            }
        }

        public string CityName
        {
            get
            {
                Ensure(CookieName);
                return _cookieContainer.GetValue<string>(CookieName, CITY_NAME);
            }
            set
            {
                Ensure(CookieName);
                _cookieContainer.SetValue(CookieName, CITY_NAME, value);
            }
        }

        public string CityUrlName
        {
            get
            {
                Ensure(CookieName);
                return _cookieContainer.GetValue<string>(CookieName, CITY_URL_NAME);
            }
            set
            {
                Ensure(CookieName);
                _cookieContainer.SetValue(CookieName, CITY_URL_NAME, value);
            }
        }

        public string AreaName
        {
            get
            {
                Ensure(CookieName);
                return _cookieContainer.GetValue<string>(CookieName, AREA_NAME);
            }
            set
            {
                Ensure(CookieName);
                _cookieContainer.SetValue(CookieName, AREA_NAME, value);
            }
        }


        public string AreaUrlName
        {
            get
            {
                Ensure(CookieName);
                return _cookieContainer.GetValue<string>(CookieName, AREA_URL_NAME);
            }
            set
            {
                Ensure(CookieName);
                _cookieContainer.SetValue(CookieName, AREA_URL_NAME, value);
            }
        }

        public string OutOfMarketZip
        {
            get
            {
                Ensure(CookieName);
                return _cookieContainer.GetValue<string>(CookieName, OUT_OF_MARKET_ZIP);
            }
            set
            {
                Ensure(CookieName);
                _cookieContainer.SetValue(CookieName, OUT_OF_MARKET_ZIP, value);
            }
        }

        public int CategoryId
        {
            get
            {
                Ensure(CookieName);
                return _cookieContainer.GetValue<int>(CookieName, CATEGORY_ID);
            }
            set
            {
                Ensure(CookieName);
                _cookieContainer.SetValue(CookieName, CATEGORY_ID, value);
            }
        }

        public string CategoryName
        {
            get
            {
                Ensure(CookieName);
                return _cookieContainer.GetValue<string>(CookieName, CATEGORY_NAME);
            }
            set
            {
                Ensure(CookieName);
                _cookieContainer.SetValue(CookieName, CATEGORY_NAME, value);
            }
        }

        #endregion

        #region[Constructors]

        public AppCookies(ICookieContainer cookieContainer)
        {
            _cookieContainer = cookieContainer;
        }

        #endregion

        #region [Methods]

        public void SetExpiration(DateTime expiration)
        {
            Ensure(CookieName);
            _cookieContainer.SetExpiration(CookieName, expiration);
        }

        public bool Exists()
        {
            return _cookieContainer.Exists(CookieName);
        }

        public void Remove(string subKey)
        {
            _cookieContainer.Delete(CookieName, subKey);
        }

        public void RemoveAll()
        {
            _cookieContainer.Delete(CookieName);
        }

        public void Ensure(string key)
        {
            if (!_cookieContainer.Exists(key))
            {
                _cookieContainer.SetValue(key, "");
            }
        }

        public void Ensure()
        {
            Ensure(CookieName);
        }

        #endregion

        #region [Static Methods]

        static public AppCookies CreateInstance()
        {
            return new AppCookies(new CookieContainer());
        }

        #endregion
    }
}