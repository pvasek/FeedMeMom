using System;
using MonoTouch.UIKit;
using System.Linq;
using MonoTouch.StoreKit;
using MonoTouch.Foundation;

namespace FeedMeMom
{
	public static class DeviceHelper
	{

		public static string GetUserName()
		{
			return UIDevice.CurrentDevice.Name.Split(new [] { "’s" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
		}

		private static SKStoreProductViewController _skController;

		public static void NavigateToAppStore(this UIViewController navCtrl, int? appId, string appUrl)
		{
			if (UIDevice.CurrentDevice.CheckSystemVersion (6,0)) 
			{
				if (_skController == null)
				{
					_skController = new SKStoreProductViewController();
					_skController.Finished += (sender2, e2) => {
						navCtrl.DismissViewController(true, () => {
							_skController.Dispose();
							_skController = null;
						});
					};
				}
				_skController.LoadProduct(new StoreProductParameters{ITunesItemIdentifier = appId}, (ok, error) => {
					if (ok) 
					{ 
						navCtrl.PresentViewController(_skController, true, null);
					}
				});
			} 
			else 
			{
				var nsurl = new NSUrl(appUrl);
				UIApplication.SharedApplication.OpenUrl (nsurl);
			}
		}
	}
}

