using System;
using MonoTouch.UIKit;
using FeedMeMom.Helpers;
using FeedMeMom.Common;
using FeedMeMom.Common.Entities;
using System.Linq;
using System.Collections.Generic;

namespace FeedMeMom.UI
{
	public partial class HistoryViewController : UIViewController
	{
		public HistoryViewController() : base ("HistoryViewController", null)
		{
			_historySource = new HistorySource();
			Title = Resources.History;
			NavigationItem.HidesBackButton = true;
			NavigationItem.LeftBarButtonItem = new UIBarButtonItem(Resources.Back, UIBarButtonItemStyle.Plain, Close);
			NavigationItem.LeftBarButtonItem.SetToolbarStyle();
		}

		public void Close(object sender, EventArgs e)
		{
			NavigationController.PopViewControllerAnimated(true);
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			tblView.Source = _historySource;
			tblView.SectionHeaderHeight = 20;
			
			ApplyColors();
			Skin.SkinChanged += HandleColorsChanged;
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			ReloadData();
		}

		private void HandleColorsChanged (object sender, EventArgs e)
		{
			ApplyColors();
		}

		private void ApplyColors() 
		{
			pnlNavigationBarPlaceholder.BackgroundColor = Skin.Active.Toolbar;
			tblView.BackgroundColor = Skin.Active.TableRow;
			tblView.SeparatorColor = Skin.Active.TableRowBorder;
			ReloadData();
		}

		private HistorySource _historySource;

		public void ReloadData()
		{
			var repo = ServiceLocator.Get<Repository>();
			_historySource.Data = repo
				.Query<FeedingEntry>("select * from FeedingEntry order by date desc")
					.GroupBy(i => i.Date.Date)
					.Select(i => new Tuple<DateTime?, List<FeedingEntry>>(i.Key, i.ToList()))
					.ToList();				
			tblView.ReloadData();
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			Skin.SkinChanged -= HandleColorsChanged;
		}
	}
}

