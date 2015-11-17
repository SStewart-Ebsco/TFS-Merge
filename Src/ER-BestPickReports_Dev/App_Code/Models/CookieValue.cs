using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ER_BestPickReports_Dev.App_Code.Models
{
    public class CookieValue<T>
    {
        private T _value;

        public CookieValue(T value)
        {
            _value = value;
        }

        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        public override String ToString()
        {
            return _value.ToString();
        }
    }
}