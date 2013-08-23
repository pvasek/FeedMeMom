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

		private void ApplyColors() 
		{
			tblList.BackgroundColor = Skin.Active.SideMenuRow;
			tblList.SeparatorColor = Skin.Active.SideMenuRowBorder;
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			tblList.RowHeight = 50;

			ApplyColors();
			_source = new SideMenuDataSource();
			tblList.Source = _source;

			Skin.SkinChanged += (sender, e) => {
				ApplyColors();
				tblList.ReloadData();
			};
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
			UITableViewCell cell = tableView.DequeueReusableCell (NormalCellId + Skin.Active.Name);
			if (cell == null)
			{
				cell = new UITableViewCell(UITableViewCellStyle.Default, NormalCellId);
				cell.TextLabel.Font = Fonts.SideMenuFont;
				cell.ContentView.BackgroundColor = Skin.Active.SideMenuRow;
				cell.TextLabel.BackgroundColor = Skin.Active.SideMenuRow;
				cell.TextLabel.TextColor = Skin.Active.SideMenuRowText;
				cell.TextLabel.HighlightedTextColor = Skin.Active.SideMenuRowSelectedText;
				cell.SelectedBackgroundView = new UIView();
				cell.SelectedBackgroundView.BackgroundColor = Skin.Active.SideMenuRowSelected;
				cell.ImageView.Layer.Opacity = 0.6f;
			}
			var item = Data[indexPath.Row];
			cell.TextLabel.Text = item.Name;
			cell.ImageView.Image = item.Image;
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
		public ActionItem(string name, Action action = null, UIImage image = null)
		{
			Name = name;
			if (action == null)
			{
				action = () => {};
			}
			Action = action;
			Image = image;
		}

		public string Name { get; set; }
		public Action Action { get; set; }
		public UIImage Image { get; set; }
	}
}

