// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace FeedMeMom.Controllers
{
	[Register ("ReviewController")]
	partial class ReviewController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton btnReview { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnShareByEmail { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnShareByFacebook { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnShareByTwitter { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblAppstore { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblShare { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView pnlNavigationBarPlaceholder { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (pnlNavigationBarPlaceholder != null) {
				pnlNavigationBarPlaceholder.Dispose ();
				pnlNavigationBarPlaceholder = null;
			}

			if (btnReview != null) {
				btnReview.Dispose ();
				btnReview = null;
			}

			if (btnShareByEmail != null) {
				btnShareByEmail.Dispose ();
				btnShareByEmail = null;
			}

			if (btnShareByFacebook != null) {
				btnShareByFacebook.Dispose ();
				btnShareByFacebook = null;
			}

			if (btnShareByTwitter != null) {
				btnShareByTwitter.Dispose ();
				btnShareByTwitter = null;
			}

			if (lblAppstore != null) {
				lblAppstore.Dispose ();
				lblAppstore = null;
			}

			if (lblShare != null) {
				lblShare.Dispose ();
				lblShare = null;
			}
		}
	}
}
