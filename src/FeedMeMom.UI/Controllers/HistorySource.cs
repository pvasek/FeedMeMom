using System;
using MonoTouch.UIKit;
using System.Collections.Generic;
using FeedMeMom.Common.Entities;
using System.Drawing;
using FeedMeMom.Helpers;

namespace FeedMeMom.UI
{
	public class HistorySource: UITableViewSource
	{
		public List<Tuple<DateTime?, List<FeedingEntry>>> Data { get; set; }
		private const string NormalCellId = "_historySourceCell";
		private UIView _footerView;

		public override int NumberOfSections(UITableView tableView)
		{
			return Data.Count;
		}

		public override int RowsInSection(UITableView tableview, int section)
		{
			return Data[section].Item2.Count;
		}

		public override UITableViewCell GetCell(UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell(NormalCellId + Skin.Active.Name);
			if (cell == null)
			{
				cell = new UITableViewCell(UITableViewCellStyle.Default, NormalCellId);
				cell.TextLabel.Font = Fonts.SideMenuFont;
				cell.ContentView.BackgroundColor = Skin.Active.TableRow;
				cell.TextLabel.BackgroundColor = Skin.Active.TableRow;
				cell.TextLabel.TextColor = Skin.Active.TableRowText;
				cell.SelectedBackgroundView = new UIView();
				cell.SelectedBackgroundView.BackgroundColor = Skin.Active.TableRowSelected;
				cell.TextLabel.HighlightedTextColor = Skin.Active.TableRowSelectedText;
				var pgbLeft = new ProgressBar { Width = 30 };
				var pgbRight = new ProgressBar { Width = 30 };
				pgbLeft.Center = new PointF(230, 20);
				pgbRight.Center = new PointF(270, 20);
				cell.ContentView.AddSubview(pgbLeft);
				cell.ContentView.AddSubview(pgbRight);
			}
			var item = Data[indexPath.Section].Item2[indexPath.Row];
			cell.TextLabel.Text = String.Format("{0:t} - {1:0} " + Resources.Minutes, item.Date, item.TotalBreastLength == null ? 0 : item.TotalBreastLength.Value.TotalMinutes);
			((ProgressBar)cell.ContentView.Subviews[1]).UpdateValue(item.TotalBreastLengthSeconds, item.LeftBreastLengthSeconds, true);
			((ProgressBar)cell.ContentView.Subviews[2]).UpdateValue(item.TotalBreastLengthSeconds, item.RightBreastLengthSeconds, true);
			return cell;
		}

		public override float GetHeightForHeader(UITableView tableView, int section)
		{
			// this hides row separator between empty rows
			return 25;
		}

		public override UIView GetViewForHeader(UITableView tableView, int section)
		{
			var view = tableView.DequeueReusableHeaderFooterView(new MonoTouch.Foundation.NSString("tableHeader" + Skin.Active.Name));
			if (view == null)
			{
				view = new UITableViewHeaderFooterView();
				view.BackgroundView = new UIView();
				view.BackgroundView.BackgroundColor = Skin.Active.TableHeader;
				var lbl = new UILabel{ Frame = new RectangleF(10, 0, 300, 25)};
				view.ContentView.AddSubview(lbl);					
				lbl.BackgroundColor = Skin.Active.TableHeader;
				lbl.Font = Fonts.TableHeader;
				lbl.TextColor = Skin.Active.TableRowSelectedText;
				lbl.Text = String.Format("{0:d}", Data[section].Item1);
			}
			return view;
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

