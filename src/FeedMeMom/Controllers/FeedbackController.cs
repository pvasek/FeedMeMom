using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace FeedMeMom
{
	public partial class FeedbackController : UIViewController
	{
		public FeedbackController() : base ("FeedbackController", null)
		{
			Title = Resources.ShareTheLove;
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
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			ApplyColors();

			Skin.SkinChanged += ApplyColors;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			Skin.SkinChanged -= ApplyColors;
		}
	}
}

