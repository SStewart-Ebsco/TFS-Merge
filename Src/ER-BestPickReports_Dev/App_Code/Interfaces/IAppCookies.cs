using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ER_BestPickReports_Dev.App_Code.Interfaces
{
    public interface IAppCookies
    {
        int CityId { get; set; }
        string CityName { get; set; }
        string CityUrlName { get; set; }
        int AreaId { get; set; }
        string AreaName { get; set; }
        string AreaUrlName { get; set; }
        string OutOfMarketZip { get; set; }

        void SetExpiration(DateTime expiration);
    }
}