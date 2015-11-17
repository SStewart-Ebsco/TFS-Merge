using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ER_BestPickReports_Dev.App_Code
{
	public class TopCategoriesDispenser
	{
		#region [Constants]

		private const int TopCategoriesMaxItemsNumber = 7;

		#endregion

		#region [Fields]

		private readonly int _areaId;
		private readonly DataAccessVault _dataAccessVault;
		private static readonly HashSet<string> UsedPages = new HashSet<string> {"area.aspx"};

		#endregion

		#region [Properties]

		public DataTable TopCategories { get; private set; }

		public bool HasCategories
		{
			get { return TopCategories != null && TopCategories.Rows.Count > 0; }
		}

		#endregion

		#region [Constructors]

		public TopCategoriesDispenser(int areaId, bool withIcons = false)
		{
			_areaId = areaId;
			_dataAccessVault = new DataAccessVault();

            if (withIcons)
            {
                GatherTopCategoriesForAreaWithIcons();
            } else {
                GatherTopCategoriesForArea();
            }
		}

		#endregion

		#region [Helpers]

		private void GatherTopCategoriesForArea()
		{
			string sqlQuery = String.Format(@"SELECT TOP {0} CI.CategoryID, CI.DisplayName AS CategoryName, CI.UrlName
							FROM CategoryArea CA
								JOIN Area A ON (CA.AreaID = A.AreaID)
								JOIN CategoryInfo CI ON (CA.CategoryID = CI.CategoryID)
							WHERE A.AreaID = @areaId AND isTopCategory = 1
							ORDER BY CI.DisplayName", TopCategoriesMaxItemsNumber);
			SqlParameter areaIdParameter = new SqlParameter("@areaId", SqlDbType.Int)
			{
				Value = _areaId
			};

			TopCategories = _dataAccessVault.DataAccessProvider.ExecuteDatasetWithOneTable(sqlQuery, areaIdParameter);
		}

        private void GatherTopCategoriesForAreaWithIcons()
        {
            var query = String.Format(@"SELECT TOP {0} CI.CategoryID, CI.DisplayName AS CategoryName, CI.UrlName, I.Website
							FROM CategoryArea CA
								JOIN Area A ON (CA.AreaID = A.AreaID)
								JOIN CategoryInfo CI ON (CA.CategoryID = CI.CategoryID)
								JOIN Info I ON (I.ReferenceID = CI.CategoryID)
							WHERE A.AreaID = @areaId AND isTopCategory = 1  AND I.InfoType = 1
							ORDER BY CI.DisplayName", TopCategoriesMaxItemsNumber);
            SqlParameter areaIdParameter = new SqlParameter("@areaId", SqlDbType.Int)
            {
                Value = _areaId
            };

            TopCategories = _dataAccessVault.DataAccessProvider.ExecuteDatasetWithOneTable(query, areaIdParameter);
        }

		#endregion

		#region [Methods]

		/// <summary>
		/// Checks if Top Categories section can be shown at the current page
		/// </summary>
		/// <param name="pageVirtualPath">Page virtual path</param>
		/// <returns></returns>
		public static bool DoesObeyRulesToBeShown(string pageVirtualPath)
		{
			return UsedPages.All(page => pageVirtualPath.Contains(page));
		}

		#endregion
	}
}