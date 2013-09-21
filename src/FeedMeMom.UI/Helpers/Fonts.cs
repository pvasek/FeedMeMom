using System;
using MonoTouch.UIKit;

namespace FeedMeMom
{
	public static class Fonts
	{
		static Fonts()
		{
			SideMenuFont = UIFont.FromName("HelveticaNeue", 16);
			ToolbarButton = UIFont.FromName("HelveticaNeue", 15);
			ToolbarTitle = UIFont.FromName("HelveticaNeue-Medium", 15);
			TableHeader = UIFont.FromName("HelveticaNeue", 14);
			Progress = UIFont.FromName("HelveticaNeue", 10);
			DefaultButton = SideMenuFont = UIFont.FromName("HelveticaNeue", 15);
		}

		public static UIFont SideMenuFont { get; set; }
		public static UIFont ToolbarButton { get; set; }
		public static UIFont ToolbarTitle { get; set; }
		public static UIFont TableHeader { get; set; }
		public static UIFont Progress { get; set; }
		public static UIFont DefaultButton { get; set; }
	}
}

