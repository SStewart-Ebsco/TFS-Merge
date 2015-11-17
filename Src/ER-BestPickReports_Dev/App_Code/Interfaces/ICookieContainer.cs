using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ER_BestPickReports_Dev.App_Code.Interfaces
{
    public interface ICookieContainer
    {
        bool Exists(string key);

        string GetValue(string key);
        T GetValue<T>(string key);
        string GetValue(string key, string subKey);
        T GetValue<T>(string key, string subKey);

        void SetValue(string key, object value);
        void SetValue(string key, object value, DateTime expiration);
        void SetValue(string key, string subKey, object value);

        void Delete(string key);
        void Delete(string key, string subKey);

        void SetExpiration(string key, DateTime expiration);
    }
}