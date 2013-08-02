using System;
using MonoTouch.UIKit;

namespace FeedMeMom
{
	public static class Fonts
	{
		static Fonts()
		{
			SideMenuFont = UIFont.FromName("Helvetica Neue", 16);
		}

		public static UIFont SideMenuFont { get; set; }
	}
}

