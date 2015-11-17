using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ER_BestPickReports_Dev.App_Code.Interfaces;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;

namespace ER_BestPickReports_Dev.App_Code.Models
{
    public class CookieContainer : ICookieContainer
    {
        #region[Fields]

        private readonly HttpRequestBase _request;
        private readonly HttpResponseBase _response;

        #endregion

        #region[Constructors]

        public CookieContainer()
        {
            var httpContext = new HttpContextWrapper(HttpContext.Current);
            _request = httpContext.Request;
            _response = httpContext.Response;
        }

        public CookieContainer(HttpRequestBase request, HttpResponseBase response)
        {
            _request = request;
            _response = response;
        }

        #endregion

        #region[Methods]

        public bool Exists(string key)
        {
            return !String.IsNullOrWhiteSpace(key) && _request.Cookies[key] != null;
        }

        public string GetValue(string key)
        {
            HttpCookie cookie = GetCookie(key);
            return cookie != null ? cookie.Value : null;
        }

        private HttpCookie GetCookie(string key)
        {
            return Exists(key) ? _request.Cookies[key] : null;
        }

        public T GetValue<T>(string key)
        {
            string value = GetValue(key);
            T adjustedValue = GetAdjustedValue<T>(value);
            return adjustedValue;
        }

        private static T GetAdjustedValue<T>(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return default(T);
            }

            Type type = ConvertTypeToCookieAcceptable(typeof(T));
            T result;
            try
            {
                result = (T)Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                result = default(T);
            }
            return result;
        }

        public string GetValue(string key, string subKey)
        {
            HttpCookie cookie = GetCookie(key);
            if (cookie != null)
            {
                //return cookie.Values[subKey];

                HttpCookie responseCookie = _response.Cookies.AllKeys.Contains(key) ?
                    _response.Cookies[key] :
                    null;

                // Try to get from response first
                if (responseCookie != null)
                {
                    return responseCookie.Values.AllKeys.Contains(subKey) ?
                        responseCookie.Values[subKey] :
                        cookie.Values[subKey];
                }
                else
                {
                    return cookie.Values[subKey];
                }
            }

            throw new ArgumentException(String.Format("There is no cookie with key '{0}'", key));
        }

        public T GetValue<T>(string key, string subKey)
        {
            string value = GetValue(key, subKey);
            T adjustedValue = GetAdjustedValue<T>(value);
            return adjustedValue;
        }

        public void SetValue(string key, object value)
        {
            SetValue(key, value, null);
        }

        public void SetValue(string key, object value, DateTime expiration)
        {
            SetValue(key, value, new DateTime?(expiration));
        }

        private void SetValue(string key, object value, DateTime? expiration)
        {
            CheckMissingKey(key);

            string adjustedValue = ConvertValueToCookieAcceptable(value);
            HttpCookie cookie = new HttpCookie(key, adjustedValue);

            if (expiration.HasValue)
            {
                cookie.Expires = expiration.Value;
            }

            _response.Cookies.Set(cookie);
        }

        private static void CheckMissingKey(string key)
        {
            if (String.IsNullOrWhiteSpace(key))
            {
                throw new NotSupportedException("Empty and null keys are not supported to set cookies");
            }
        }

        private static string ConvertValueToCookieAcceptable(object value)
        {
            if (value == null)
            {
                return null;
            }

            ConvertTypeToCookieAcceptable(value.GetType());

            return (string)Convert.ChangeType(value, typeof(string), CultureInfo.InvariantCulture);
        }

        private static Type ConvertTypeToCookieAcceptable(Type type)
        {
            bool isNullable = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
            if (isNullable)
            {
                NullableConverter converter = new NullableConverter(type);
                Type underlyingType = converter.UnderlyingType;
                if (underlyingType.IsValueType)
                {
                    return underlyingType;
                }
            }

            if (type.IsValueType || type == typeof(String))
            {
                return type;
            }

            throw new NotSupportedException(String.Format("Type '{0}' is not supported to be saved in cookies. Only value types and Nullable<ValueType> are supported.",
                type.FullName));
        }

        public void SetValue(string key, string subKey, object value)
        {
            CheckMissingKey(key);
            CheckMissingKey(subKey);

            string adjustedValue = ConvertValueToCookieAcceptable(value);
            HttpCookie cookie = (_response.Cookies.Get(key) ?? GetCookie(key)) ?? new HttpCookie(key);

            cookie.Values[subKey] = adjustedValue;
            _response.Cookies.Set(cookie);
        }

        public void Delete(string key)
        {
            HttpCookie cookie = GetCookie(key);
            if (cookie != null)
            {
                HttpCookie expiredCookie = new HttpCookie(cookie.Name);
                expiredCookie.Expires = DateTime.Now.AddDays(-1);
                _response.Cookies.Set(expiredCookie);
            }
        }

        public void Delete(string key, string subKey)
        {
            HttpCookie requestCookie = GetCookie(key);
            if (requestCookie != null)
            {
                requestCookie.Values.Remove(subKey);
            }

            HttpCookie responseCookie = _response.Cookies.Get(key);
            if (responseCookie != null)
            {
                responseCookie.Values.Remove(subKey);
            }
        }

        public void SetExpiration(string key, DateTime expiration)
        {
            CheckMissingKey(key);

            HttpCookie cookie = _response.Cookies.Get(key) ?? GetCookie(key);

            if (cookie == null)
            {
                throw new ArgumentException("There is no cookie with specified key");
            }

            cookie.Expires = expiration;
            //_response.Cookies.Set(cookie);
        }

        #endregion



    }
}