using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.StoreKit;

namespace FeedMeMom
{
	public partial class ReviewController : UIViewController
	{
		public ReviewController() : base ("ReviewController", null)
		{
			Title = Resources.IlikeThisApp;
			NavigationItem.HidesBackButton = true;
			NavigationItem.LeftBarButtonItem = new UIBarButtonItem(Resources.Back, UIBarButtonItemStyle.Plain, Close);
		}

		public void Close(object sender, EventArgs e)
		{
			NavigationController.PopViewControllerAnimated(true);
		}

		public override void DidReceiveMemoryWarning()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning();
			
			// Release any cached data, images, etc that aren't in use.
		}

		private void ApplyColors(object sender = null, EventArgs e = null)
		{
			Skin.Active.SkinButton(btnReview);
			Skin.Active.SkinButton(btnShareByEmail);
			Skin.Active.SkinButton(btnShareByTwitter);
			Skin.Active.SkinButton(btnShareByFacebook);		
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			
			ApplyColors();

			Skin.SkinChanged += ApplyColors;

			btnReview.TouchUpInside += (sender, e) => {
				var skController = new SKStoreProductViewController();
				skController.Finished += (sender2, e2) => {
					NavigationController.DismissViewController(true, () => {
						skController.Dispose();
						skController = null;
					});
				};
				skController.LoadProduct(new StoreProductParameters{ITunesItemIdentifier = 496963922}, (ok, error) => {
					if (ok) 
					{ 
						NavigationController.PresentViewController(skController, true, null);
					}
				});
			};
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			Skin.SkinChanged -= ApplyColors;
		}
	}
}

