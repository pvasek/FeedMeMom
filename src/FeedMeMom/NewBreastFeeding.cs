using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace FeedMeMom
{
	public partial class NewBreastFeeding : UIViewController
	{
		public NewBreastFeeding () : base ("NewBreastFeeding", null)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			NavigationItem.RightBarButtonItem = new UIBarButtonItem () {Title = "Cancel"};
			NavigationItem.LeftBarButtonItem = new UIBarButtonItem () {Title = "Save"};
			NavigationItem.LeftBarButtonItem.SetBackgroundImage (UIImage.FromBundle("btn_primary.png"), UIControlState.Normal, UIBarMetrics.Default);

			NavigationItem.RightBarButtonItem.Clicked += (object sender, EventArgs e) => {
				//NavigationController.DismissViewController(true, null);
				NavigationController.PopToRootViewController(true);
			};
			NavigationItem.LeftBarButtonItem.Clicked += (object sender, EventArgs e) => {
				//NavigationController.DismissViewController(true, null);
				NavigationController.PopToRootViewController(true);
			};
		}
	}
}

