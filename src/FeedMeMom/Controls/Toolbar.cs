using System;
using MonoTouch.UIKit;
using FeedMeMom.Helpers;

namespace FeedMeMom
{
	public class Toolbar: UIView
	{
		private UILabel _lblTitle;
		private UIButton _btnLeft;
		private UIButton _btnRight;

		public Toolbar()
		{
			_lblTitle = new UILabel();
			_btnLeft = new UIButton();
			_btnRight = new UIButton();
			AddSubview(_lblTitle);
			AddSubview(_btnLeft);
			AddSubview(_btnRight);
			_lblTitle.BackgroundColor = Colors.Active.Toolbar;
			_lblTitle.TextColor = Colors.Active.ToolbarText;
		}	

		protected override void Dispose(bool disposing)
		{
			_lblTitle = _lblTitle.SafeDispose();
			_btnLeft = _btnLeft.SafeDispose();
			_btnRight = _btnRight.SafeDispose();
			base.Dispose(disposing);
		}
	}
}

