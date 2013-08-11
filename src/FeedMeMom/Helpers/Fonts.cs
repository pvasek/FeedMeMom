using System;
using MonoTouch.UIKit;

namespace FeedMeMom
{
	public static class Fonts
	{
		static Fonts()
		{
			SideMenuFont = UIFont.FromName("Helvetica Neue", 16);
			ToolbarButton = UIFont.FromName("Helvetica Neue", 15);
			ToolbarTitle = UIFont.FromName("Helvetica Neue", 21);
			TableHeader = UIFont.FromName("Helvetica Neue", 14);
			Progress = UIFont.FromName("Helvetica Neue", 10);
		}

		public static UIFont SideMenuFont { get; set; }
		public static UIFont ToolbarButton { get; set; }
		public static UIFont ToolbarTitle { get; set; }
		public static UIFont TableHeader { get; set; }
		public static UIFont Progress { get; set; }
	}
}

