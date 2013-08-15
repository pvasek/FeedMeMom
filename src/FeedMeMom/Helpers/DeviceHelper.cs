using System;
using MonoTouch.UIKit;
using System.Linq;

namespace FeedMeMom
{
	public static class DeviceHelper
	{

		public static string GetUserName()
		{
			return UIDevice.CurrentDevice.Name.Split(new [] { "’s" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
		}
	}
}

