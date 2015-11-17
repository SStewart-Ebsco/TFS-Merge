using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PinnexLib.Data;

namespace ER_BestPickReports_Dev.App_Code.Interfaces
{
	public interface IDataAccessProvider
	{
		DataAccess DataAccessProvider { get; }
	}
}