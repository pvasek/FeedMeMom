using System;
using MonoTouch.UIKit;

namespace FeedMeMom
{
	public static class Backgrounds
	{
		static Backgrounds()
		{
			Clear = UIImage.FromBundle("clear_color").CreateResizableImage(new UIEdgeInsets(2, 25, 2, 2));
		}

		public static UIImage Clear { get; private set; }
	}
}

