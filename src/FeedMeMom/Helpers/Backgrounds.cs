using System;
using MonoTouch.UIKit;

namespace FeedMeMom
{
	public static class Backgrounds
	{
		static Backgrounds()
		{
			Clear = UIImage.FromBundle("clear_color").CreateResizableImage(new UIEdgeInsets(2, 25, 2, 2));
			DayHamburger = UIImage.FromBundle("hamburger_icon");
			NightHamburger = UIImage.FromBundle("night_hamburger_icon");
			DayArrow = UIImage.FromBundle("arrow");
			NightArrow = UIImage.FromBundle("night_arrow");
		}

		public static UIImage Clear { get; private set; }
		public static UIImage DayHamburger { get; private set; }
		public static UIImage NightHamburger { get; private set; }
		public static UIImage DayArrow { get; private set; }
		public static UIImage NightArrow { get; private set; }

	}
}

