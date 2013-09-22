using System;
using MonoTouch.UIKit;
using System.Text;
using System.Globalization;
using FeedMeMom.Common;
using FeedMeMom.Helpers;

namespace FeedMeMom.Controllers
{
	public partial class FeedbackController : UIViewController
	{
		public FeedbackController() : base ("FeedbackController", null)
		{
			Title = Resources.Feedback;
			NavigationItem.HidesBackButton = true;
			NavigationItem.LeftBarButtonItem = new UIBarButtonItem(Resources.Back, UIBarButtonItemStyle.Plain, Close);
			NavigationItem.LeftBarButtonItem.SetToolbarStyle();
		}

		public void Close(object sender, EventArgs e)
		{
			NavigationController.PopViewControllerAnimated(true);
		}

		
		private void ApplyColors(object sender = null, EventArgs e = null)
		{
			View.BackgroundColor = Skin.Active.Background;
			Skin.Active.SkinButton(btnNewFeature);
			Skin.Active.SkinButton(btnBug);
			Skin.Active.SkinButton(btnStayInTouch);
			lblHelp.TextColor = Skin.Active.PageText;
			pnlNavigationBarPlaceholder.BackgroundColor = Skin.Active.Toolbar;
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			pnlNavigationBarPlaceholder.SetNavbarPlaceholder();
			lblHelp.Text = Resources.FeedbackHelpText;

			btnBug.TouchUpInside += (sender, e) => SendEmail("Bug Report", "Description:");
			btnNewFeature.TouchUpInside += (sender, e) => SendEmail("Feature request", "What about this feature?");
			btnStayInTouch.TouchUpInside += (sender, e) => SendEmail("Feedback", "");

			ApplyColors();

			btnBug.SetTitle(Resources.ReportBug);
			btnNewFeature.SetTitle(Resources.RequestNewFeature);
			btnStayInTouch.SetTitle(Resources.JustStayInTouch);

			Skin.SkinChanged += ApplyColors;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			Skin.SkinChanged -= ApplyColors;
		}

		private static void SendEmail(string subject, string startOfTheBody)
		{
			var msg = new StringBuilder();
			msg.AppendLine(startOfTheBody);
			msg.AppendLine("");
			msg.AppendLine("");
			msg.AppendLine("");
			msg.AppendLine("");
			msg.AppendLine("");
			msg.AppendLine("Device information:");
			msg.AppendLine(String.Format("Device: {0}", UIDevice.CurrentDevice.Model));
			msg.AppendLine(String.Format("Version: {0}", UIDevice.CurrentDevice.SystemVersion));
			msg.AppendLine(String.Format("Localization: {0}", CultureInfo.CurrentCulture.Name));
			msg.AppendLine(String.Format("System: {0}", UIDevice.CurrentDevice.SystemName));
			ServiceLocator.Get<EmailSender>().SendEmail(subject, msg.ToString());
		}


	}
}

