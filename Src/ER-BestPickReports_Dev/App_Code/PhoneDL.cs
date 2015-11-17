using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ER_BestPickReports_Dev.App_Code
{
    public class PhoneDL
    {
        private string _number;
        private PhoneType _type;

        public string number
        { get { return _number; } }

        public PhoneType type
        { get { return _type; } }

        public PhoneDL(string pNumber, PhoneType pType)
        {
            _number = pNumber;
            _type = pType;
        }
    }

    public enum PhoneType
    {
        regular,
        organic,
        ppc,
        facebook
    }
}