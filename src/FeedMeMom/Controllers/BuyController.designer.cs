// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace FeedMeMom
{
	[Register ("BuyController")]
	partial class BuyController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton btnBuy { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView imgScreen { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblBuyTitle { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblDescription { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView pnlPreview { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (btnBuy != null) {
				btnBuy.Dispose ();
				btnBuy = null;
			}

			if (imgScreen != null) {
				imgScreen.Dispose ();
				imgScreen = null;
			}

			if (lblBuyTitle != null) {
				lblBuyTitle.Dispose ();
				lblBuyTitle = null;
			}

			if (lblDescription != null) {
				lblDescription.Dispose ();
				lblDescription = null;
			}

			if (pnlPreview != null) {
				pnlPreview.Dispose ();
				pnlPreview = null;
			}
		}
	}
}
