using System;
using System.Collections.Generic;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Linq;

namespace FeedMeMom
{
	public class SideMenuHub
	{

	}

	public partial class SideMenu : UIViewController
	{
		private SideMenu(UIView mainView) : base ("SideMenu", null)
		{
			_mainView = mainView;
		}

		private UIView _mainView;

		public void HookupToWindow()
		{
			var app = UIApplication.SharedApplication;
			var top = app.StatusBarHidden ? 0 : app.StatusBarFrame.Height;
			View.Frame = new RectangleF(0, top, View.Frame.Width - 80, View.Frame.Height);
			View.Hidden = true;

			var topWindow = app.Windows.First();
			topWindow.InsertSubview(View, 0);
		}

		private static UIView GetTopView(UIView view)
		{
			if (view.Superview == null || view.Superview is UIWindow)
				return view;

			return GetTopView(view.Superview);
		}

		public void Toggle()
		{
			var app = UIApplication.SharedApplication;
			//var top = app.StatusBarHidden ? 0 : app.StatusBarFrame.Height;

			var topView = GetTopView(_mainView);

			topView.Layer.ShadowColor = UIColor.Black.CGColor;
			topView.Layer.ShadowOpacity = 0.7f;
			topView.Layer.ShadowRadius = 9;

			if (View.Hidden) 
			{
				View.Hidden = false;

				UIView.Animate(0.3, 0, UIViewAnimationOptions.CurveEaseInOut, () => {
					var frame = topView.Frame;
					topView.Frame = new RectangleF(frame.Width - 80, 0, frame.Width, frame.Height);
				}, () => {
				});

			}
			else
			{
				UIView.Animate(0.3, 0, UIViewAnimationOptions.CurveEaseInOut, () => {
					var frame = topView.Frame;
					topView.Frame = new RectangleF(0, 0, frame.Width, frame.Height);
				}, () => {
					View.Hidden = true;
				});
			}
		}	

		public static SideMenu CreateAndHookup(UIView mainView)
		{
			var result = new SideMenu(mainView);
			result.HookupToWindow();
			return result;
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
			tblList.RowHeight = 50;
			tblList.BackgroundColor = Colors.Active.SideMenuRow;
			tblList.SeparatorColor = Colors.Active.SideMenuRowBorder;
			var source = new SideMenuDataSource();
			source.Data.Add(new ActionItem(Resources.History));
			source.Data.Add(new ActionItem(Resources.SwitchDayNightMode, Colors.ToggleDayNightMode));
			source.Data.Add(new ActionItem(Resources.Feedback));
			tblList.Source = source;
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

