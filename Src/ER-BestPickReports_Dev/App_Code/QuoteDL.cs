using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ER_BestPickReports_Dev.App_Code
{
    public class QuoteDL
    {
        private string _name;
        private string _title;
        private string _text;

        public string name
        { get { return _name; } }

        public string title
        { get { return _title; } }

        public string text
        { get { return _text; } }

        public QuoteDL(string qName, string qTitle, string qText)
        {
            _name = qName;
            _title = qTitle;
            _text = qText;
        }
    }
}