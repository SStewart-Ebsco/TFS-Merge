using System.Configuration;
using ER_BestPickReports_Dev.App_Code.Interfaces;
using PinnexLib.Data;

namespace ER_BestPickReports_Dev.App_Code
{
	public class DataAccessVault: IDataAccessProvider
	{
		#region [Constants]

		private const string ConnectionStringConfigName = "ConnString";

		#endregion

		#region [Fields]

		private DataAccess _dataAccessProvider;

		#endregion

		#region [Properties]

		public string ConnectionString
		{
			get { return ConfigurationManager.ConnectionStrings[ConnectionStringConfigName].ToString(); }
		}

		public DataAccess DataAccessProvider
		{
			get { return _dataAccessProvider ?? (_dataAccessProvider = new DataAccess(ConnectionString)); }
		}

		#endregion
	}
}