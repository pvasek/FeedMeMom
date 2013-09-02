// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace FeedMeMom
{
	[Register ("SideMenu")]
	partial class SideMenu
	{
		[Outlet]
		MonoTouch.UIKit.UITableView tblList { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (tblList != null) {
				tblList.Dispose ();
				tblList = null;
			}
		}
	}
}
