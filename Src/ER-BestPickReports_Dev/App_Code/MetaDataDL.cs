using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ER_BestPickReports_Dev.App_Code
{
    public class MetaDataDL
    {
        private string _key;
        private string _description;
        private string _title;

        public string key
        { get { return _key; } }

        public string description
        { get { return _description; } }

        public string title
        { get { return _title; } }

        public MetaDataDL(string mKey, string mDesc, string mTitle)
        {
            _key = mKey;
            _description = mDesc;
            _title = mTitle;
        }
    }
}