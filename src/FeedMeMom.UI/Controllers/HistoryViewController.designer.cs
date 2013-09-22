// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace FeedMeMom.UI
{
	[Register ("HistoryViewController")]
	partial class HistoryViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIView pnlNavigationBarPlaceholder { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITableView tblView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (pnlNavigationBarPlaceholder != null) {
				pnlNavigationBarPlaceholder.Dispose ();
				pnlNavigationBarPlaceholder = null;
			}

			if (tblView != null) {
				tblView.Dispose ();
				tblView = null;
			}
		}
	}
}
