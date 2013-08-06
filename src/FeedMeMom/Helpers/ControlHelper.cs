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
	}
}

