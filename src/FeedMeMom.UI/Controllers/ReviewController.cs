using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.StoreKit;
using FeedMeMom.Common;
using FeedMeMom.Helpers;
using System.Text;

namespace FeedMeMom.Controllers
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

			btnReview.SetTitle(Resources.RateReviewThisApp);
			btnShareByEmail.SetTitle(Resources.ShareByEmail);
			lblShare.Text = Resources.Share;
			lblAppstore.Text = Resources.Appstore;

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
			NavigationController.NavigateToAppStore(Configuration.AppId, Configuration.AppStoreUrl);
		}

		private void ShareByEmailClick(object sender, EventArgs e)
		{
			var emailSender = ServiceLocator.Get<EmailSender>();
			var body = new StringBuilder();
			body.AppendLine(Resources.ShareEmailBody);
			body.AppendLine();
			body.AppendLine(Configuration.AppStoreUrl);
			body.AppendLine();
			//body.AppendLine(DeviceHelper.GetUserName());
			emailSender.SendEmail(Resources.ShareEmailSubject, body.ToString());
		}
	}
}

