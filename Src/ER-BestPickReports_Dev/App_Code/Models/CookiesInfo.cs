using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ER_BestPickReports_Dev.App_Code.Models
{
    public class CookiesInfo
    {
        private readonly string _cityId;
        private readonly string _cityUrl;
        private readonly string _cityDisplayName;
        private readonly string _areaId;
        private readonly string _areaUrl;
        private readonly string _areaDisplayName;


        public string CityId { get { return _cityId; } }
        public string CityUrl { get { return _cityUrl; } }
        public string CityDisplayName { get { return _cityDisplayName; } }
        public string AreaId { get { return _areaId; } }
        public string AreaUrl { get { return _areaUrl; } }
        public string AreaDisplayName { get { return _areaDisplayName; } }

        public CookiesInfo(){}

        public CookiesInfo(string cityId, string cityUrl, string cityDisplayName, string areaId, string areaUrl, string areaDisplayName)
        {
            _cityId = cityId;
            _cityUrl = cityUrl;
            _cityDisplayName = cityDisplayName;
            _areaId = areaId;
            _areaUrl = areaUrl;
            _areaDisplayName = areaDisplayName;
        }
    }
}