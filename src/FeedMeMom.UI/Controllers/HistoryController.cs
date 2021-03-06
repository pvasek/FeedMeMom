using System;
using MonoTouch.UIKit;
using FeedMeMom.Common.Entities;
using System.Collections.Generic;
using FeedMeMom.Common;
using FeedMeMom.Helpers;
using System.Linq;
using System.Drawing;
using FeedMeMom.UI;

namespace FeedMeMom.Controllers
{
	public class HistoryController2: UITableViewController
	{
		public HistoryController2()
		{
			_historySource = new HistorySource();
			((UITableView)View).Source = _historySource;
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
			var tblList = ((UITableView)View);
			tblList.SectionHeaderHeight = 20;
			ApplyColors();
			Skin.SkinChanged += HandleColorsChanged;
		}

		private void HandleColorsChanged (object sender, EventArgs e)
		{
			ApplyColors();
		}

		private void ApplyColors() 
		{
			View.BackgroundColor = Skin.Active.Toolbar;
			var tblList = ((UITableView)View);
			tblList.BackgroundColor = Skin.Active.TableRow;
			tblList.SeparatorColor = Skin.Active.TableRowBorder;
			tblList.ReloadData();
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
			((UITableView)View).ReloadData();
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			Skin.SkinChanged -= HandleColorsChanged;
		}

	}
}

