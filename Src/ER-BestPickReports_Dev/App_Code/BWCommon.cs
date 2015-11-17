using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ER_BestPickReports_Dev.App_Code
{
    public class BWCommon
    {

        public static int GetRequestInteger(int nullValue, HttpRequest request, string variableName)
        {
            int tempInt = nullValue;
            if (request[variableName] != null)
            {
                if (!Int32.TryParse(request[variableName].ToString(), out tempInt))
                    tempInt = nullValue;
            }
            return tempInt;
        }
        
    }
}