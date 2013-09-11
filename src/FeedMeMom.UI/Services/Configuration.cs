using System;

namespace FeedMeMom
{
	public static class Configuration
	{
		static Configuration() 
		{
			//AppStoreUrl = System.Configuration.ConfigurationManager
			IsFreeApp = true;
			PaidAppId = 692782735;

			//PaidAppStoreUrl = "http://itunes.apple.com/us/app/feedmemom/id420447115?mt=8";
			//PaidAppId = 420447115;

#if (DEBUG)
			IsTest = true;
#endif
		}

		private const string AppStoreUrlTemplate = "https://itunes.apple.com/app/id{0}?mt=8";

		public static int AppId { get; set; }
		public static string AppStoreUrl { get { return String.Format(AppStoreUrlTemplate, AppId); } }
		public static int PaidAppId { get; set; }
		public static string PaidAppStoreUrl { get { return String.Format(AppStoreUrlTemplate, PaidAppId); } }
		public static string FeedbackEmail { get; set; }
		public static bool IsFreeApp { get; set; }
		public static bool IsTest { get; set; }
		
	}
}

