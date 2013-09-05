using System;

namespace FeedMeMom
{
	public static class Configuration
	{
		static Configuration() 
		{
			//AppStoreUrl = System.Configuration.ConfigurationManager
			AppStoreUrl = "http://itunes.apple.com/us/app/feedmemom/id692782735?mt=8";
			FeedbackEmail = "freesupport@feedmemom.com";
			AppId = 0;
			IsFreeApp = true;
#if (DEBUG)
			IsTest = true;
#endif
		}

		public static string AppStoreUrl { get; set; }
		public static string FeedbackEmail { get; set; }
		public static int AppId { get; set; }
		public static bool IsFreeApp { get; set; }
		public static bool IsTest { get; set; }
		
	}
}

