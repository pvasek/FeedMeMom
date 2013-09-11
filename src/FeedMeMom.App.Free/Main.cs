using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace FeedMeMom
{
	public class Application
	{
		// This is the main entry point of the application.
		static void Main (string[] args)
		{
			Configuration.IsFreeApp = true;
			Configuration.FeedbackEmail = "support@feedmemom.com";
			Configuration.AppId = 699987636;

			UIApplication.Main (args, null, "AppDelegate");
		}
	}
}
