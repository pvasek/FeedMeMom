using System;

namespace FeedMeMom
{
	public static class Configuration
	{
		static Configuration() 
		{
			//AppStoreUrl = System.Configuration.ConfigurationManager
			AppStoreUrl = "http://itunes.apple.com/us/app/angry-birds/id343200656?mt=8";
			FeedbackEmail = "pvasek@gmail.com";
			AppId = 496963922;
		}

		public static string AppStoreUrl { get; set; }
		public static string FeedbackEmail { get; set; }
		public static int AppId { get; set; }
		
	}
}

