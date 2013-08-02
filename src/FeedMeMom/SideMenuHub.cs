using System;
using MonoTouch.UIKit;
using System.Drawing;
using System.Linq;

namespace FeedMeMom
{
	public class SideMenuHub
	{
		private UIView _mainView;
		private UIView _sideView;
		private UIView _touchCover;

		private SideMenuHub(UIView mainView, UIView sideView)
		{
			_mainView = mainView;
			_sideView = sideView;
		}

		public static SideMenuHub CreateAndHookup(UIView mainView, UIView sideView)
		{
			var result = new SideMenuHub(mainView, sideView);
			result.HookupToWindow();
			return result;
		}

		public void HookupToWindow()
		{
			var app = UIApplication.SharedApplication;
			var top = app.StatusBarHidden ? 0 : app.StatusBarFrame.Height;
			_sideView.Frame = new RectangleF(0, top, _sideView.Frame.Width - 80, _sideView.Frame.Height);
			_sideView.Hidden = true;

			var topWindow = app.Windows.First();
			topWindow.InsertSubview(_sideView, 0);

			_touchCover = new UIView();
			_touchCover.Hidden = true;
			_touchCover.Frame = _mainView.Frame;
			_touchCover.UserInteractionEnabled = true;
			_touchCover.AddGestureRecognizer(new UITapGestureRecognizer((e) => {
				Toggle();
			}));
			topWindow.AddSubview(_touchCover);
		}

		private static UIView GetTopView(UIView view)
		{
			if (view.Superview == null || view.Superview is UIWindow)
				return view;

			return GetTopView(view.Superview);
		}

		public void Toggle()
		{
			var app = UIApplication.SharedApplication;
			//var top = app.StatusBarHidden ? 0 : app.StatusBarFrame.Height;

			var topView = GetTopView(_mainView);

			topView.Layer.ShadowColor = UIColor.Black.CGColor;
			topView.Layer.ShadowOpacity = 0.7f;
			topView.Layer.ShadowRadius = 9;

			if (_sideView.Hidden) 
			{
				_sideView.Hidden = false;

				UIView.Animate(0.3, 0, UIViewAnimationOptions.CurveEaseInOut, () => {
					var frame = topView.Frame;
					topView.Frame = new RectangleF(frame.Width - 80, 0, frame.Width, frame.Height);
				}, () => {
					_touchCover.Frame = topView.Frame;
					_touchCover.Hidden = false;
				});

			}
			else
			{
				UIView.Animate(0.3, 0, UIViewAnimationOptions.CurveEaseInOut, () => {
					var frame = topView.Frame;
					topView.Frame = new RectangleF(0, 0, frame.Width, frame.Height);
				}, () => {
					_sideView.Hidden = true;
					_touchCover.Hidden = true;
				});
			}
		}	

	}
}

