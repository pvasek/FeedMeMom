using System;

namespace FeedMeMom
{
	public static class Configuration
	{
		static Configuration() 
		{
			//AppStoreUrl = System.Configuration.ConfigurationManager
			AppStoreUrl = "http://itunes.apple.com/us/app/feedmemom/id692782735?mt=8";
			FeedbackEmail = "support@feedmemom.com";
			AppId = 692782735;
		}

		public static string AppStoreUrl { get; set; }
		public static string FeedbackEmail { get; set; }
		public static int AppId { get; set; }
		
	}
}

