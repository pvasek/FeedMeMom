using MonoTouch.UIKit;
using System;
using MonoTouch.CoreGraphics;
using System.Linq;
using System.Threading.Tasks;
using FeedMeMom.Helpers;
using System.Drawing;

namespace FeedMeMom.Controllers
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

		public string ScreenImageName { get; set; }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			NavigationItem.Title = Resources.Buy;
			NavigationItem.HidesBackButton = true;
			NavigationItem.LeftBarButtonItem = new UIBarButtonItem(Resources.Back, UIBarButtonItemStyle.Plain, Close);
			NavigationItem.LeftBarButtonItem.SetToolbarStyle();

			lblBuyTitle.Text = Resources.BuyPhrase;
			ApplyColors();

			Skin.SkinChanged += (sender, e) => ApplyColors();

			PrepareImage(imgScreen);
			imgScreen.Frame = new System.Drawing.RectangleF(90, -10, 200, 290);
			imgScreen.Transform = CGAffineTransform.MakeRotation(-0.25f);
			pnlPreview.ClipsToBounds = true;

			btnBuy.SetTitle(Resources.GetFullVersion);

			btnBuy.TouchUpInside += (sender, e) => {
				NavigationController.NavigateToAppStore(Configuration.PaidAppId, Configuration.PaidAppStoreUrl);
			};
			if (ControlHelper.IsIPhone5)
			{
				lblBuyTitle.Frame = lblBuyTitle.Frame.Set(y: 326);
				btnBuy.Frame = btnBuy.Frame.Set(y: 410);
			}
		}

		private UIImageView _topImage;

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);		

			var title = NavigationItem.Title;
			NavigationItem.Title = "";
			lblDescription.Text = BuyDescription;
			lblDescription.SizeToFit();

			if (ControlHelper.IsIPhone5)
			{
				imgScreen.Image = UIImage.FromBundle(ScreenImageName + "_iphone5");
			} 
			else
			{
				imgScreen.Image = UIImage.FromBundle(ScreenImageName);
			}

			if (_topImage == null)
			{
				_topImage = new UIImageView();
				PrepareImage(_topImage);
				NavigationController.View.AddSubview(_topImage);
			}

			_topImage.Image = imgScreen.Image;
			imgScreen.Hidden = true;
			_topImage.Hidden = false;
			var statusBarHeight = UIApplication.SharedApplication.StatusBarFrame.Height;
			_topImage.Transform = CGAffineTransform.MakeIdentity();
			_topImage.Frame = NavigationController.View.Frame.Add(y: statusBarHeight, height: -statusBarHeight);

			UIView.Animate(1.5f, 1, UIViewAnimationOptions.CurveEaseInOut, () => {
				_topImage.Frame = new RectangleF(new PointF(90, 143), new SizeF(200, 290)); 
				NavigationItem.Title = title;
			}, () => {
				UIView.Animate(0.2f, 0, UIViewAnimationOptions.CurveEaseIn, () => {
					_topImage.Transform = CGAffineTransform.MakeRotation(-0.25f);
				}, () => {
					Task.Delay(600).ContinueWith((task) => {
						InvokeOnMainThread(() => {
							imgScreen.Hidden = false;
							_topImage.Hidden = true;
						});
					});
				});
			});
		}

		private void PrepareImage(UIImageView img)
		{
			img.ContentMode = UIViewContentMode.ScaleAspectFit;
			img.Layer.ShadowColor = UIColor.White.CGColor;
			img.Layer.ShadowRadius = 9;
			img.Layer.ShadowOpacity = 0.85f;
			img.Layer.ShadowOffset = new System.Drawing.SizeF(0, 4);
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

