using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ER_BestPickReports_Dev.App_Code
{
    public class CookiesProvider
    {
        private static CookiesProvider _instance;
        
        private CookiesProvider(){}

        public static CookiesProvider Instance
        {
            get { return _instance ?? (_instance = new CookiesProvider()); }
        }

        public T ReadCookieValue<T>(HttpRequest request, string cookieName)
        {
            T value = default(T);

            if (request[cookieName] != null)
            {
                TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(T));
                object propValue = typeConverter.ConvertFromString(request[cookieName]);
                value = (T) propValue;
            }

            return value;
        }

        //public bool SetCookieValue<T>(HttpResponse response, string cookieName, T cookieValue)
        //{
        //    bool result = false;

        //    response.Cookies[cookieName] = cookieValue.ToString();

        //    return result;
        //}
    }
}