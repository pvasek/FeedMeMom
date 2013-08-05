using System;
using System.Collections.Generic;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Linq;

namespace FeedMeMom
{
	
	public partial class SideMenu : UIViewController
	{
		public SideMenu() : base ("SideMenu", null)
		{
		}

		public override void DidReceiveMemoryWarning()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning();
			
			// Release any cached data, images, etc that aren't in use.
		}

		private SideMenuDataSource _source;
		public List<ActionItem> Items { get { return _source.Data; } }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			tblList.RowHeight = 50;
			tblList.BackgroundColor = Colors.Active.SideMenuRow;
			tblList.SeparatorColor = Colors.Active.SideMenuRowBorder;
			_source = new SideMenuDataSource();
			tblList.Source = _source;
		}
	}

	public class SideMenuDataSource: UITableViewSource
	{
		public SideMenuDataSource()
		{
			Data = new List<ActionItem>();
		}

		private const string NormalCellId = "normalCell";
		private UIView _footerView;

		public List<ActionItem> Data { get; private set; }

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

		public override int NumberOfSections(UITableView tableView)
		{
			return 1;
		}

		public override int RowsInSection(UITableView tableView, int section)
		{
			return Data.Count;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
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
			cell.TextLabel.Text = Data[indexPath.Row].Name;
			return cell;
		}	

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			Data[indexPath.Row].Action();
			tableView.DeselectRow(indexPath, true);
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (_footerView != null)
			{
				_footerView.Dispose();
				_footerView = null;
			}
		}

	}

	public class ActionItem
	{
		public ActionItem(string name, Action action = null)
		{
			Name = name;
			if (action == null)
			{
				action = () => {};
			}
			Action = action;
		}

		public string Name { get; set; }
		public Action Action { get; set; }
	}
}

