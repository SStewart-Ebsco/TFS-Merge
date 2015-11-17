using System;
using System.Data;
using System.Data.SqlClient;
using ER_BestPickReports_Dev.App_Code.Interfaces;

namespace ER_BestPickReports_Dev.App_Code
{
	public class AreaGroupDistributor
	{
		#region [Fields]

		private readonly IDataAccessProvider _dataAccessProvider;

		#endregion

		#region [Properties]

		#endregion

		#region [Constructors]

		public AreaGroupDistributor() : this(new DataAccessVault()){}

		public AreaGroupDistributor(IDataAccessProvider dataAccessProvider)
		{
			_dataAccessProvider = dataAccessProvider;
		}

		#endregion

		#region [Methods]

		public AreaGroup GetAreaGroupByAreaId(int areaId)
		{
			string sqlQuery = String.Format(@"SELECT a.AreaID, ag.GroupName, ag.PromoClipUrl, ag.CoverImageFileName, ag.SkylineImageFileName
							FROM Area a
								LEFT JOIN AreaGroup ag ON (a.AreaGroupID = ag.AreaGroupID)
							WHERE a.AreaID = @areaId");
			SqlParameter areaIdParameter = new SqlParameter("@areaId", SqlDbType.Int)
			{
				Value = areaId
			};

			DataTable areaGroups = _dataAccessProvider.DataAccessProvider.ExecuteDatasetWithOneTable(sqlQuery, areaIdParameter);
			if (areaGroups.Rows.Count > 0)
			{
				DataRow areaGroup = areaGroups.Rows[0];
				return new AreaGroup
					       {
							   GroupName = (AreaGroupName) Enum.Parse(typeof(AreaGroupName), areaGroup["GroupName"].ToString().Replace(" ", "")),
						       PromoClipUri = (String) ConvertRawValue<string>(areaGroup["PromoClipUrl"]),
							   CoverImageFileName = (String) ConvertRawValue<string>(areaGroup["CoverImageFileName"]),
						       SkylineImageFileName = (String) ConvertRawValue<string>(areaGroup["SkylineImageFileName"]),
					       };
			}

			return null;
		}

		private static object ConvertRawValue<T> (object value)
		{
			if (value != DBNull.Value)
			{
				try
				{
					return (T)Convert.ChangeType(value, typeof(T));
				}
				catch (InvalidCastException)
				{
					return null;
				}
			}

			return null;

		}

		#endregion
	}
}