using System;

namespace FeedMeMom
{
	public static class Configuration
	{
		static Configuration() 
		{
			//AppStoreUrl = System.Configuration.ConfigurationManager
			IsFreeApp = true;
			FeedbackEmail = "freesupport@feedmemom.com";

			AppStoreUrl = "http://itunes.apple.com/us/app/feedmemom/id692782735?mt=8";
			AppId = 692782735;

			PaidAppStoreUrl = "http://itunes.apple.com/us/app/feedmemom/id692782735?mt=8";
			PaidAppId = 692782735;

			//PaidAppStoreUrl = "http://itunes.apple.com/us/app/feedmemom/id420447115?mt=8";
			//PaidAppId = 420447115;

#if (DEBUG)
			IsTest = true;
#endif
		}

		public static int AppId { get; set; }
		public static string AppStoreUrl { get; set; }
		public static int PaidAppId { get; set; }
		public static string PaidAppStoreUrl { get; set; }
		public static string FeedbackEmail { get; set; }
		public static bool IsFreeApp { get; set; }
		public static bool IsTest { get; set; }
		
	}
}

