using System;
using MonoTouch.UIKit;

namespace FeedMeMom.Helpers
{
	public static class ControlHelper
	{
		public static T SafeDispose<T>(this T view) where T: class, IDisposable
		{
			if (view != null)
			{
				view.Dispose();
			}
			return null;
		}

		public static void SetAllBackgrounds(this UIBarButtonItem button, UIImage image)
		{
			button.SetBackgroundImage(image, UIControlState.Normal, UIBarMetrics.Default);
			button.SetBackgroundImage(image, UIControlState.Highlighted, UIBarMetrics.Default);
			button.SetBackgroundImage(image, UIControlState.Selected, UIBarMetrics.Default);
		}
	}
}

