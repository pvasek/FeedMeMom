using System;
using System.Collections.Generic;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

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

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			tblList.BackgroundColor = Colors.Active.SideMenuRow;
			tblList.SeparatorColor = Colors.Active.SideMenuRowBorder;
			var dataSource = new SideMenuDataSource();
			dataSource.Data.Add("History");
			dataSource.Data.Add("Switch Night/Day Mode");
			dataSource.Data.Add("Feedback");
			tblList.DataSource = dataSource;
		}
	}

	public class SideMenuDataSource: UITableViewDataSource
	{
		public SideMenuDataSource()
		{
			Data = new List<string>();
		}

		private const string NormalCellId = "normalCell";

		public List<string> Data { get; private set; }

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
			}
			cell.TextLabel.Text = Data[indexPath.Row];
			return cell;
		}
	}
}

