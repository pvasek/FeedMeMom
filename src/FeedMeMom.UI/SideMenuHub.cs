using System;
using MonoTouch.UIKit;
using System.Drawing;
using System.Linq;
using FeedMeMom.Helpers;

namespace FeedMeMom
{
	public class SideMenuHub
	{
		private UIView _mainView;
		private UIView _sideView;
		private UIView _touchCover;
		private bool _visible;

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
			//_sideView.Hidden = true;

			var topWindow = app.Windows.First();
			topWindow.InsertSubview(_sideView, 0);

			_touchCover = new UIView();
			_touchCover.Hidden = true;
			_touchCover.Frame = _mainView.Frame;
			_touchCover.UserInteractionEnabled = true;
			_touchCover.AddGestureRecognizer(new UITapGestureRecognizer((e) => {
				Toggle();
			}));
			//topWindow.AddSubview(_touchCover);


			var recognizer = new UIPanGestureRecognizer(Pan); 
			recognizer.MinimumNumberOfTouches = 1;
			recognizer.MaximumNumberOfTouches = 1;
			_mainView.AddGestureRecognizer(recognizer);
			_mainView.UserInteractionEnabled = true;

		}

		private float _panStartX;
		private bool _panStarted;

		private void Pan(UIPanGestureRecognizer recognizer)
		{
			var topView = GetTopView(_mainView);
			if (recognizer.State == UIGestureRecognizerState.Began)
			{
				var x = recognizer.LocationOfTouch(0, topView).X;
				_panStartX = topView.Frame.X - x;
				_panStarted = true;
//				if (x < 80)
//				{
//					_panStarted = true;
//				}
			}

			if (!_panStarted)
			{
				return;
			}

			if (recognizer.State == UIGestureRecognizerState.Changed)
			{
				var x = recognizer.TranslationInView(topView).X;
				var newX = x + _panStartX;
				newX = newX < 0 ? 0 : newX;
				newX = newX > _sideView.Frame.Width ? _sideView.Frame.Width : newX;
				topView.Frame = topView.Frame.Set(x: newX);
			} 

			if (recognizer.State == UIGestureRecognizerState.Ended)
			{
				_panStartX = 0;
				_panStarted = false;

				var x = recognizer.TranslationInView(topView).X;
				var newX = x + _panStartX;
				var half = _sideView.Frame.Width / 2;
				if (newX < 0)
				{
					if (newX*-1 > half)
					{
						Hide();
					} else
					{
						Show();
					}
				} 
				else
				{
					if (newX > half)
					{
						Show();
					} else
					{
						Hide();
					}
				}
			}

			//recognizer.X
		}

		private static UIView GetTopView(UIView view)
		{
			if (view.Superview == null || view.Superview is UIWindow)
				return view;

			return GetTopView(view.Superview);
		}

		public void Hide(UIView topView = null)
		{
			_visible = false;

			topView = GetTopView(_mainView);

			UIView.Animate(0.15, 0, UIViewAnimationOptions.CurveEaseInOut, () => {
				var frame = topView.Frame;
				topView.Frame = new RectangleF(0, 0, frame.Width, frame.Height);
			}, () => {
				//_sideView.Hidden = true;
				_touchCover.Hidden = true;
			});

		}

		public void Show(UIView topView = null)
		{
			_visible = true;

			topView = GetTopView(_mainView);
			//_sideView.Hidden = false;
			UIView.Animate(0.15, 0, UIViewAnimationOptions.CurveEaseInOut, () => {
				var frame = topView.Frame;
				topView.Frame = new RectangleF(frame.Width - 80, 0, frame.Width, frame.Height);
			}, () => {
				_touchCover.Frame = topView.Frame;
				_touchCover.Hidden = false;
			});
		}

		public void Toggle()
		{
			var app = UIApplication.SharedApplication;
			//var top = app.StatusBarHidden ? 0 : app.StatusBarFrame.Height;

			var topView = GetTopView(_mainView);

			topView.Layer.ShadowColor = UIColor.Black.CGColor;
			topView.Layer.ShadowOpacity = 0.7f;
			topView.Layer.ShadowRadius = 9;

			if (_visible) 
			{
				Hide(topView);
			}
			else
			{
				Show(topView);
			}
		}	

	}
}

