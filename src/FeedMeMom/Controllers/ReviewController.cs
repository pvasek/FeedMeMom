using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.StoreKit;
using FeedMeMom.Common;
using System.Text;

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
			base.DidReceiveMemoryWarning();
		}

		private void ApplyColors(object sender = null, EventArgs e = null)
		{
			View.BackgroundColor = Skin.Active.Background;
			Skin.Active.SkinButton(btnReview);
			Skin.Active.SkinButton(btnShareByEmail);
			Skin.Active.SkinButton(btnShareByTwitter);
			Skin.Active.SkinButton(btnShareByFacebook);		
			lblAppstore.TextColor = Skin.Active.TitleText;
			lblShare.TextColor = Skin.Active.TitleText;
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			
			ApplyColors();

			Skin.SkinChanged += ApplyColors;

			btnShareByTwitter.Hidden = true;
			btnShareByFacebook.Hidden = true;

			btnReview.TouchUpInside += ReviewClick;
			btnShareByEmail.TouchUpInside += ShareByEmailClick;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			Skin.SkinChanged -= ApplyColors;
		}

		private void ReviewClick(object sender, EventArgs e)
		{
			if (UIDevice.CurrentDevice.CheckSystemVersion (6,0)) 
			{
				var skController = new SKStoreProductViewController();
				skController.Finished += (sender2, e2) => {
					NavigationController.DismissViewController(true, () => {
						skController.Dispose();
						skController = null;
					});
				};
				skController.LoadProduct(new StoreProductParameters{ITunesItemIdentifier = Configuration.AppId}, (ok, error) => {
					if (ok) 
					{ 
						NavigationController.PresentViewController(skController, true, null);
					}
				});
			} 
			else 
			{
				var nsurl = new NSUrl(Configuration.AppStoreUrl);
				UIApplication.SharedApplication.OpenUrl (nsurl);
			}		
		}

		private void ShareByEmailClick(object sender, EventArgs e)
		{
			var emailSender = ServiceLocator.Get<EmailSender>();
			var body = new StringBuilder();
			body.AppendLine("I found a really handy app.");
			body.AppendLine("You can give it a try.");
			body.AppendLine();
			body.AppendLine(Configuration.AppStoreUrl);
			body.AppendLine();
			//body.AppendLine(DeviceHelper.GetUserName());
			emailSender.SendEmail("Good app for you", body.ToString());
		}
	}
}

