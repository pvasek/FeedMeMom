using MonoTouch.UIKit;
using System;
using MonoTouch.CoreGraphics;
using System.Linq;
using System.Threading.Tasks;

namespace FeedMeMom
{
	public partial class BuyController : UIViewController
	{
		public BuyController() : base ("BuyController", null)
		{
		}

		public void Close(object sender, EventArgs e)
		{
			NavigationController.PopViewControllerAnimated(true);
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			Title = Resources.Feedback;
			NavigationItem.HidesBackButton = true;
			NavigationItem.LeftBarButtonItem = new UIBarButtonItem(Resources.Back, UIBarButtonItemStyle.Plain, Close);

			lblBuyTitle.Text = Resources.BuyPhrase;
			ApplyColors();

			Skin.SkinChanged += (sender, e) => ApplyColors();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			NavigationItem.Title = BuyTitle;
			lblDescription.Text = BuyDescription;
			lblDescription.SizeToFit();
			if ((568.0f - UIScreen.MainScreen.Bounds.Height) < 0.1)
			{
				imgScreen.Image = UIImage.FromBundle("buy_history_iphone5");
			} 
			else
			{
				imgScreen.Image = UIImage.FromBundle("buy_history");
			}
			imgScreen.ContentMode = UIViewContentMode.ScaleAspectFit;
			imgScreen.Transform = CGAffineTransform.MakeIdentity();
			var window = UIApplication.SharedApplication.KeyWindow;
			var topLeft = window.ConvertPointToView(new System.Drawing.PointF(0, 0), pnlPreview);

			imgScreen.Frame = new System.Drawing.RectangleF(0, topLeft.Y+UIApplication.SharedApplication.StatusBarFrame.Height /*pnlPreview.Frame.Top * -1f*/, window.Frame.Width, window.Frame.Height - UIApplication.SharedApplication.StatusBarFrame.Height);
			imgScreen.ContentMode = UIViewContentMode.ScaleAspectFit;
			imgScreen.Layer.ShadowColor = UIColor.White.CGColor;
			imgScreen.Layer.ShadowRadius = 9;
			imgScreen.Layer.ShadowOpacity = 0.85f;	
			imgScreen.Layer.ShadowOffset = new System.Drawing.SizeF(0, 4);

			pnlPreview.ClipsToBounds = false;

			UIView.Animate(2, 1, UIViewAnimationOptions.CurveEaseInOut, () => {
				imgScreen.Frame = new System.Drawing.RectangleF(90, -10, 200, 290);

			}, () => {
				UIView.Animate(0.7f, 0, UIViewAnimationOptions.CurveEaseIn, () => {
					imgScreen.Transform = CGAffineTransform.MakeRotation(-0.25f);
				}, () => {
					Task.Delay(1300).ContinueWith((task) => {
						InvokeOnMainThread(() => {
							pnlPreview.ClipsToBounds = true;
						});
					});
				});
			});
		}
		private void ApplyColors()
		{
			Skin.Active.SkinButton(btnBuy);
			lblBuyTitle.TextColor = Skin.Active.PageText;
			lblDescription.TextColor = Skin.Active.PageText;
			pnlPreview.BackgroundColor = Skin.Active.Toolbar;
		}

		public string BuyTitle { get; set; }
		public string BuyDescription { get; set; }

	}
}

