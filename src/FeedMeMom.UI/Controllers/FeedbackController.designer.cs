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
	[Register ("FeedbackController")]
	partial class FeedbackController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton btnBug { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnNewFeature { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnStayInTouch { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblHelp { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView pnlNavigationBarPlaceholder { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (pnlNavigationBarPlaceholder != null) {
				pnlNavigationBarPlaceholder.Dispose ();
				pnlNavigationBarPlaceholder = null;
			}

			if (btnBug != null) {
				btnBug.Dispose ();
				btnBug = null;
			}

			if (btnNewFeature != null) {
				btnNewFeature.Dispose ();
				btnNewFeature = null;
			}

			if (btnStayInTouch != null) {
				btnStayInTouch.Dispose ();
				btnStayInTouch = null;
			}

			if (lblHelp != null) {
				lblHelp.Dispose ();
				lblHelp = null;
			}
		}
	}
}
