using System;
using System.Linq;

namespace ER_BestPickReports_Dev
{
   
    /// <summary>
    /// Stored in the Info.Level field so we can easily determine what level of information
    /// </summary>
    public enum InfoLevel
    {
        None = 0,
        Global = 1,
        City = 2,
        Area = 3
    }

	public enum AreaGroupName
	{
		Atlanta = 1,
		Birmingham = 2,
		Boston = 3,
		Chicago = 4,
		Dallas = 5,
		Houston = 6,
		Maryland = 7,
		NorthVirginia = 8,
		Philadelphia = 9,
        SouthAtlanta = 10
	}
}