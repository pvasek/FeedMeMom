using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.MessageUI;
using System.Text;
using System.Globalization;
using FeedMeMom.Common;
using System.Linq;

namespace FeedMeMom
{
	public partial class FeedbackController : UIViewController
	{
		public FeedbackController() : base ("FeedbackController", null)
		{
			Title = Resources.Feedback;
			NavigationItem.HidesBackButton = true;
			NavigationItem.LeftBarButtonItem = new UIBarButtonItem(Resources.Back, UIBarButtonItemStyle.Plain, Close);
		}

		public void Close(object sender, EventArgs e)
		{
			NavigationController.PopViewControllerAnimated(true);
		}

		
		private void ApplyColors(object sender = null, EventArgs e = null)
		{
			Skin.Active.SkinButton(btnNewFeature);
			Skin.Active.SkinButton(btnBug);
			Skin.Active.SkinButton(btnStayInTouch);
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			btnBug.TouchUpInside += (sender, e) => SendEmail("Bug Report", "Description:");
			btnNewFeature.TouchUpInside += (sender, e) => SendEmail("Feature request", "What about this feature?");
			btnStayInTouch.TouchUpInside += (sender, e) => SendEmail("Feedback", "");

			ApplyColors();

			Skin.SkinChanged += ApplyColors;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			Skin.SkinChanged -= ApplyColors;
		}

		private void SendEmail(string subject, string startOfTheBody)
		{
			var msg = new StringBuilder();
			msg.AppendLine(startOfTheBody);
			msg.AppendLine("");
			msg.AppendLine("");
			msg.AppendLine("");
			msg.AppendLine("");
			msg.AppendLine(UIDevice.CurrentDevice.Name.Split(new []{"’s"}, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault());
			msg.AppendLine("");
			msg.AppendLine("");
			msg.AppendLine("============================");
			msg.AppendLine("Device information:");
			msg.AppendLine(String.Format("IOS Version: {0}", UIDevice.CurrentDevice.SystemVersion));
			msg.AppendLine(String.Format("Device: {0}", UIDevice.CurrentDevice.Model));
			msg.AppendLine(String.Format("Localization: {0}", CultureInfo.CurrentCulture.Name));
			msg.AppendLine(String.Format("System: {0}", UIDevice.CurrentDevice.SystemName));

			ServiceLocator.Get<EmailSender>().SendEmail(subject, msg.ToString(), () => { 
				//Close(null, null);
			});

		}


	}
}

