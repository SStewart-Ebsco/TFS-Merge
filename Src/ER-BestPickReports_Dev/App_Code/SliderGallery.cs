using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using ER_BestPickReports_Dev.App_Code.Interfaces;

namespace ER_BestPickReports_Dev.App_Code
{
	public class SliderGallery
	{
		#region [Constants]

		private const int UnknownCityId = 0;
		private const int UnknownAreaId = 0;

		#endregion

		#region [Fields]

		private readonly int _cityId;
		private readonly int _areaId;

		private AreaGroup _areaCluster;
		private string _promoClipUri;
		private string _coverImagePath;

		private readonly IDataAccessProvider _dataAccessVault;

		#endregion

		#region [Properties]

		public DataTable Slides { get; private set; }

		public bool HasSlides
		{
			get
			{
				return Slides != null && Slides.Rows.Count > 0;
			}
		}

		public bool HaveCityAndArea
		{
			get { return _cityId != UnknownCityId && _areaId != UnknownAreaId; }
		}

		private AreaGroup AreaCluster
		{
			get
			{
				if (HaveCityAndArea && _areaCluster == null)
				{
					AreaGroupDistributor areaGroupDistributor = new AreaGroupDistributor(_dataAccessVault);
					_areaCluster = areaGroupDistributor.GetAreaGroupByAreaId(_areaId);
				}

				return _areaCluster;
			}
		}

		public string PromoClipUri
		{
			get
			{
				if (_promoClipUri == null)
				{
					_promoClipUri = AreaCluster != null
										? AreaCluster.PromoClipUri
										: AreaGroup.DefaultPromoClipUri;
				}
				
				return _promoClipUri;
			}
		}

		public string CoverImageFileName
		{
			get
			{
				return _coverImagePath ?? (_coverImagePath = AreaCluster != null
					                                             ? AreaCluster.CoverImageFileName
					                                             : AreaGroup.DefaultCoverImageFileName);
			}
		}

		#endregion

		#region [Constructors]

		public SliderGallery() : this(UnknownCityId, UnknownAreaId){}

		public SliderGallery(int cityId, int areaId) : this (new DataAccessVault(), cityId, areaId){}

		public SliderGallery(IDataAccessProvider dataAccessProvider, int cityId, int areaId)
		{
			_cityId = cityId;
			_areaId = areaId;

			_dataAccessVault = dataAccessProvider;

			BuildSlidesDataTable();
			GatherSlidesForCity();
		}

		#endregion

		#region [Helpers]

		private void BuildSlidesDataTable()
		{
			Slides = new DataTable();
			Slides.Columns.Add("ImagePath", typeof(string));
			Slides.Columns.Add("Title", typeof(string));
			Slides.Columns.Add("Location", typeof(string));
		}

		private void GatherSlidesForCity()
		{
			const string sqlQuery = @"SELECT * FROM Slides WHERE cityID = @cityID ORDER BY NEWID()";
			SqlParameter cityIdParameter = new SqlParameter("@cityID", SqlDbType.Int)
			{
				//Currently we just always get the global images
				//If ebsco gets enough photos we can make this dynamic per market and uncomment the next line
				//Value = _cityId
				Value = UnknownCityId
			};

			Slides = _dataAccessVault.DataAccessProvider.ExecuteDatasetWithOneTable(sqlQuery, cityIdParameter);
		}

		#endregion
	}
}