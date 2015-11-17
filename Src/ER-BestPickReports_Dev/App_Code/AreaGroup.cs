using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;

namespace ER_BestPickReports_Dev.App_Code
{
	public class AreaGroup
	{
		#region [Constants]

        private const string GeneralPromoClipUri = @"http://youtube.com/embed/RnkoYGxHhyg?autoplay=1";

		public const string DefaultCoverImageFileName = @"generalCover.jpg";

		#endregion

		#region [Fields]

		private string _promoClipUri;
		private string _coverImageFileName;
		private string _skylineImageFileName = String.Empty;

		#endregion

		#region [Properties]

		#region [Static Properties]

		public static string DefaultPromoClipUri
		{
			get { return AddAutoPlayQueryParameter(GeneralPromoClipUri); }
		}

		#endregion

		public AreaGroupName GroupName { get; set; }

		public string PromoClipUri
		{
			get
			{
				string purePromoClipUri = _promoClipUri ?? GeneralPromoClipUri;
				return AddAutoPlayQueryParameter(purePromoClipUri);
			}
			set { _promoClipUri = value; }
		}

		public string CoverImageFileName
		{
			get { return _coverImageFileName ?? DefaultCoverImageFileName; }
			set { _coverImageFileName = value; }
		}

		public string SkylineImageFileName
		{
			get { return _skylineImageFileName; }
			set { _skylineImageFileName = value; }
		}

		#endregion

		#region [Methods]

		private static string AddAutoPlayQueryParameter(string uriText)
		{
			const string autoPlayParameterKey = "autoplay";
			Uri promoClipUri = new Uri(uriText);

			NameValueCollection queryParameters = HttpUtility.ParseQueryString(promoClipUri.Query);
			queryParameters.Add(autoPlayParameterKey, "1");

			return String.Format("{0}://{1}{2}?{3}", promoClipUri.Scheme, promoClipUri.Host, promoClipUri.AbsolutePath,
								 queryParameters);
		}

		#endregion
	}
}