using System;
using MonoTouch.UIKit;
using FeedMeMom.Common.Entities;
using System.Collections.Generic;
using FeedMeMom.Common;
using FeedMeMom.Helpers;

namespace FeedMeMom
{
	public class HistoryController: UITableViewController
	{
		public HistoryController()
		{
			_historySource = new HistorySource();
			((UITableView)View).Source = _historySource;
			Title = Resources.History;
			NavigationItem.HidesBackButton = true;
			NavigationItem.LeftBarButtonItem = new UIBarButtonItem(Resources.Back, UIBarButtonItemStyle.Plain, Close);
		}

		public void Close(object sender, EventArgs e)
		{
			NavigationController.PopViewControllerAnimated(true);
		}

		private HistorySource _historySource;

		public void ReloadData()
		{
			var repo = ServiceLocator.Get<Repository>();
			_historySource.Data = repo.Query<FeedingEntry>("select * from FeedingEntry order by date desc");
		}

		public override void ViewWillAppear (bool animated)
		{
			this.NavigationController.NavigationBarHidden = false;
			NavigationItem.BackBarButtonItem = new UIBarButtonItem { Title = Resources.Back };
		}

		public class HistorySource: UITableViewSource
		{
			public List<FeedingEntry> Data { get; set; }
			private const string NormalCellId = "_historySourceCell";
			private UIView _footerView;

			public override int NumberOfSections(UITableView tableView)
			{
				return 1;
			}

			public override int RowsInSection(UITableView tableview, int section)
			{
				return Data.Count;
			}

			public override UITableViewCell GetCell(UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
			{
				UITableViewCell cell = tableView.DequeueReusableCell (NormalCellId);
				if (cell == null)
				{
					cell = new UITableViewCell(UITableViewCellStyle.Default, NormalCellId);
					cell.TextLabel.Font = Fonts.SideMenuFont;
					cell.ContentView.BackgroundColor = Colors.Active.SideMenuRow;
					cell.TextLabel.BackgroundColor = Colors.Active.SideMenuRow;
					cell.TextLabel.TextColor = Colors.Active.SideMenuRowText;
					cell.SelectedBackgroundView = new UIView();
					cell.SelectedBackgroundView.BackgroundColor = Colors.Active.SideMenuRowSelected;
				}
				var item = Data[indexPath.Row];
				cell.TextLabel.Text = String.Format("{0} - {1:0}", item.Date, item.TotalBreastLength == null ? 0 : item.TotalBreastLength.Value.TotalMinutes);
				return cell;
			}

			public override float GetHeightForHeader(UITableView tableView, int section)
			{
				// this hides row separator between empty rows
				return 0.01f;
			}

			public override UIView GetViewForFooter(UITableView tableView, int section)
			{
				if (_footerView == null)
				{
					_footerView = new UIView();
				}

				return _footerView;
			}

			protected override void Dispose(bool disposing)
			{
				base.Dispose(disposing);
				_footerView = _footerView.SafeDispose();
			}
		}
	}
}

