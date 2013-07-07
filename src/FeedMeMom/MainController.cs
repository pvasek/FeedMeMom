using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using FeedMeMom.Common;
using FeedMeMom.Common.Entities;
using System.Linq;
using System.Threading;

namespace FeedMeMom
{
	public partial class MainController : UIViewController
	{
		public MainController () : base ("MainController", null)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		private void UpdateHieghtInPercent(int? total, int? current, UIView parent, UIView child)
		{
			var percent =  (float)(current ?? 0) / (total ?? 1);
			var rect = child.Frame;
			var height = parent.Frame.Height * percent;
			if (height > parent.Frame.Height) {
				height = parent.Frame.Height;
			}
			child.Frame = new RectangleF(rect.Left, parent.Frame.Height - height, rect.Size.Width, height);
		}

		private void UpdateView(FeedingEntry entry)
		{
			indicatorRight.Hidden = true;
			indicatorLeft.Hidden = true;

			lblMainTime.Hidden = false;
			lblSecondTimeInfo.Hidden = false;
			pnlInfoSmall.Hidden = false;

			// main info
			if (entry.TotalBreastLength != null) {
				if ((DateTime.Now - entry.Date) < TimeSpan.FromMinutes (5)) {
					lblMainTime.Text = "Now";
					lblMainTimeInfo.Text = "";
				} else {
					var ago = entry.Date.AsAgoText ();
					lblMainTime.Text = ago.Item1;
					lblMainTimeInfo.Text = ago.Item2;
				}
			} else {
				lblMainTime.Text = "";
				lblMainTimeInfo.Text = "";
			}
			lblSecondTimeInfo.Text = "minutes";

			UpdateHieghtInPercent (entry.TotalBreastLengthSeconds, entry.LeftBreastLengthSeconds, pnlInfoSmall, indicatorSecondLeft);
			UpdateHieghtInPercent (entry.TotalBreastLengthSeconds, entry.RightBreastLengthSeconds, pnlInfoSmall, indicatorSecondRight);

			SelectRightLeftButton ((entry.RightBreastLengthSeconds ?? 0) - (entry.LeftBreastLengthSeconds ?? 0)); // if there was more from right, the next should be form left
			// secondary info
			if (entry.TotalBreastLength == null) {
				//TODO: support for more then 60 m inutes
			}
			lblSecondTime.Text = entry.TotalBreastLength == null ? "" : entry.TotalBreastLength.Value.ToString (@"mm\:ss");
		}

		private void EmptyView()
		{
			indicatorRight.Hidden = true;
			indicatorLeft.Hidden = true;

			//lblMainTime.Hidden = true;
			//lblSecondTimeInfo.Hidden = true;
			lblMainTime.Text = "";
			lblSecondTimeInfo.Text = "";
			lblSecondTime.Text = "";
			pnlInfoSmall.Hidden = true;
			lblMainTimeInfo.Text = "Start on the top";
		}


		public void ReloadData() 
		{
			var repo = ServiceLocator.Get<Repository> ();
			var last = repo.Query<FeedingEntry> ("SELECT * FROM FeedingEntry ORDER BY Date DESC LIMIT 1").FirstOrDefault();
			if (last == null) 
			{
				EmptyView ();
			} 
			else 
			{
				UpdateView (last);
			}
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			ReloadData ();
		}

		protected override void Dispose (bool disposing)
		{
			if (_timer != null) {
				_timer.Dispose ();
				_timer = null;
			}
			base.Dispose (disposing);
		}

		private Timer _timer;
		private FeedingEntry _active;

		private void TimerElapsed (object state)
		{
			if (_active == null) {
				return;
			}
			InvokeOnMainThread(() => {
				if (_active != null) {
					var now = _stopPair.GetTotalLength();
					lblSecondTime.Text = now.ToString (@"mm\:ss");
					const int min30 = 30*60;
					var rightLength = (int)_stopPair.Right.GetTotalLength().TotalSeconds;
					var leftLength = (int)_stopPair.Left.GetTotalLength().TotalSeconds;
					UpdateHieghtInPercent (min30, rightLength > min30 ? min30 : rightLength, pnlInfoSmall, indicatorSecondRight);
					UpdateHieghtInPercent (min30, leftLength > min30 ? min30 : leftLength, pnlInfoSmall, indicatorSecondLeft);
				}
			});
		}



		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();		

			var repo = ServiceLocator.Get<Repository>();

			NavigationItem.RightBarButtonItem = new UIBarButtonItem("Cancel", UIBarButtonItemStyle.Plain, (s, ea) => {
				if (_active != null) {
					SwitchToInfoMode();
					repo.Delete(_active);
					_active = null;
					ReloadData();
				}
				NavigationController.NavigationBarHidden = true;
			});

			NavigationItem.LeftBarButtonItem = new UIBarButtonItem("Save", UIBarButtonItemStyle.Plain, (s, ea) => {
				if (_active != null) {
					SwitchToInfoMode();
					_active.Date = DateTime.Now;
					_stopPair.Stop();
					repo.Update(_active);
					_active = null;
					ReloadData();
				}
				NavigationController.NavigationBarHidden = true;
			});

			NavigationController.NavigationBarHidden = true;

			_timer = new Timer (TimerElapsed, null, 200, 200);

			btnStartLeft.TouchUpInside += (object sender, EventArgs e) => {

				if (_active == null) {
					StartFeeding(repo, true);
				} else if (_active.LeftStartTime == null) {
					_stopPair.Start (true);
					SelectRightLeftButton(true);
				}

			};

			btnStartRight.TouchUpInside += (object sender, EventArgs e) => {

				if (_active == null) {
					StartFeeding(repo, false);
				} else {
					_stopPair.Start (false);
					SelectRightLeftButton(false);
				}
			};
		}

		private void SwitchToInfoMode()
		{
			lblMainTime.Hidden = false;
			lblMainTimeInfo.Hidden = false;
		}


		private void SelectRightLeftButton(bool left)
		{
			SelectRightLeftButton (left ? 1 : -1);
		}

		private void SelectRightLeftButton(int left)
		{
			if (left == 0) {
				btnStartLeft.BackgroundColor = UIColorExtensions.ButtonActive;
				btnStartRight.BackgroundColor = UIColorExtensions.ButtonActive;
			}
			else if (left > 0) {
				btnStartLeft.BackgroundColor = UIColorExtensions.ButtonActive;
				btnStartRight.BackgroundColor = UIColorExtensions.ButtonInactive;
			} else {
				btnStartLeft.BackgroundColor = UIColorExtensions.ButtonInactive;
				btnStartRight.BackgroundColor = UIColorExtensions.ButtonActive;
			}
		}

		private TimeStopPair _stopPair;

		private void StartFeeding(Repository repo, bool left)
		{
			lblMainTime.Hidden = true;
			lblMainTimeInfo.Hidden = true;
			SelectRightLeftButton(left);
			NavigationController.NavigationBarHidden = false;
			lblSecondTimeInfo.Text = "Tap to Pause";
			_active = new FeedingEntry {
				IsRunning = true
			};
			_stopPair = new TimeStopPair (_active);
			_stopPair.Start (left);
			repo.Insert(_active);
		}

	}
}

