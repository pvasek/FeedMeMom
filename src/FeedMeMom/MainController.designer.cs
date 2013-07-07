// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace FeedMeMom
{
	[Register ("MainController")]
	partial class MainController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton btnStartLeft { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnStartRight { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView indicatorLeft { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView indicatorRight { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblMainTime { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblMainTimeInfo { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblSecondTime { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblSecondTimeInfo { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView indicatorSecondLeft { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView indicatorSecondRight { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView pnlInfoSmall { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (btnStartLeft != null) {
				btnStartLeft.Dispose ();
				btnStartLeft = null;
			}

			if (btnStartRight != null) {
				btnStartRight.Dispose ();
				btnStartRight = null;
			}

			if (indicatorLeft != null) {
				indicatorLeft.Dispose ();
				indicatorLeft = null;
			}

			if (indicatorRight != null) {
				indicatorRight.Dispose ();
				indicatorRight = null;
			}

			if (lblMainTime != null) {
				lblMainTime.Dispose ();
				lblMainTime = null;
			}

			if (lblMainTimeInfo != null) {
				lblMainTimeInfo.Dispose ();
				lblMainTimeInfo = null;
			}

			if (lblSecondTime != null) {
				lblSecondTime.Dispose ();
				lblSecondTime = null;
			}

			if (lblSecondTimeInfo != null) {
				lblSecondTimeInfo.Dispose ();
				lblSecondTimeInfo = null;
			}

			if (indicatorSecondLeft != null) {
				indicatorSecondLeft.Dispose ();
				indicatorSecondLeft = null;
			}

			if (indicatorSecondRight != null) {
				indicatorSecondRight.Dispose ();
				indicatorSecondRight = null;
			}

			if (pnlInfoSmall != null) {
				pnlInfoSmall.Dispose ();
				pnlInfoSmall = null;
			}
		}
	}
}
