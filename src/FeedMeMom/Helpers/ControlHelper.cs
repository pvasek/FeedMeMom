using System;
using MonoTouch.UIKit;
using System.Drawing;

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

		public static RectangleF Add(this RectangleF rect, float x = 0, float y = 0, float width = 0, float height = 0)
		{
			return new RectangleF(rect.X + x, rect.Y + y, rect.Width + width, rect.Height + height);
		}

		public static RectangleF Set(this RectangleF rect, float? x = null, float? y = null, float? width = null, float? height = null)
		{
			return new RectangleF(x ?? rect.X, y ?? rect.Y, width ?? rect.Width, height ?? rect.Height);
		}
}
}

