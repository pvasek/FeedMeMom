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
		MonoTouch.UIKit.UIButton btnLeft { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnRight { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnStartLeft { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnStartRight { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView indicatorSecondLeft { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView indicatorSecondRight { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblButtonsHeader { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblMainTime { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblMainTimeInfo { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblSecondTime { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblSecondTimeInfo { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblTitle { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView pgbContainerLeft { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView pgbContainerRight { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel pgbTextLeft { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel pgbTextRight { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView pgbValueLeft { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView pgbValueRight { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView pnlAgo { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView pnlInfoSmall { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView pnlTime { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView viewHeader { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (pgbValueLeft != null) {
				pgbValueLeft.Dispose ();
				pgbValueLeft = null;
			}

			if (pgbValueRight != null) {
				pgbValueRight.Dispose ();
				pgbValueRight = null;
			}

			if (btnLeft != null) {
				btnLeft.Dispose ();
				btnLeft = null;
			}

			if (btnRight != null) {
				btnRight.Dispose ();
				btnRight = null;
			}

			if (btnStartLeft != null) {
				btnStartLeft.Dispose ();
				btnStartLeft = null;
			}

			if (btnStartRight != null) {
				btnStartRight.Dispose ();
				btnStartRight = null;
			}

			if (indicatorSecondLeft != null) {
				indicatorSecondLeft.Dispose ();
				indicatorSecondLeft = null;
			}

			if (indicatorSecondRight != null) {
				indicatorSecondRight.Dispose ();
				indicatorSecondRight = null;
			}

			if (lblButtonsHeader != null) {
				lblButtonsHeader.Dispose ();
				lblButtonsHeader = null;
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

			if (lblTitle != null) {
				lblTitle.Dispose ();
				lblTitle = null;
			}

			if (pgbContainerLeft != null) {
				pgbContainerLeft.Dispose ();
				pgbContainerLeft = null;
			}

			if (pgbContainerRight != null) {
				pgbContainerRight.Dispose ();
				pgbContainerRight = null;
			}

			if (pgbTextLeft != null) {
				pgbTextLeft.Dispose ();
				pgbTextLeft = null;
			}

			if (pgbTextRight != null) {
				pgbTextRight.Dispose ();
				pgbTextRight = null;
			}

			if (pnlAgo != null) {
				pnlAgo.Dispose ();
				pnlAgo = null;
			}

			if (pnlInfoSmall != null) {
				pnlInfoSmall.Dispose ();
				pnlInfoSmall = null;
			}

			if (pnlTime != null) {
				pnlTime.Dispose ();
				pnlTime = null;
			}

			if (viewHeader != null) {
				viewHeader.Dispose ();
				viewHeader = null;
			}
		}
	}
}
