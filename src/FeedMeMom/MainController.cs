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

		//private UITapGestureRecognizer _touchRecondizerSecondaryTime;

		private PointF _defaultTimePanelCenter;
		private PointF _defaultContainerRightCenter;
		private RectangleF _defaultTimeFrame;

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();		
			NavigationController.NavigationBarHidden = true;
			viewHeader.BackgroundColor = UIColorExtensions.Toolbar;
			btnStartLeft.Layer.CornerRadius = 6;
			btnStartRight.Layer.CornerRadius = 6;
			pgbContainerLeft.Layer.CornerRadius = 20;
			pgbContainerLeft.ClipsToBounds = true;
			pgbContainerRight.Layer.CornerRadius = 20;
			pgbContainerRight.ClipsToBounds = true;

			var repo = ServiceLocator.Get<Repository>();		

			lblSecondTimeInfo.TextColor = UIColor.Gray;
			_defaultTimePanelCenter = pnlTime.Center;
			_defaultTimeFrame = pnlTime.Frame;
			_defaultContainerRightCenter = pgbContainerRight.Center;

			var touchRecondizerSecondaryTime = new UITapGestureRecognizer((e) => {
				if (_active != null) {
					if (_stopPair.Toggle())
					{
						lblSecondTime.TextColor = UIColor.Black;
						lblSecondTimeInfo.Text = "Tap to Pause";
					} 
					else 
					{
						lblSecondTime.TextColor = UIColor.Gray;
						lblSecondTimeInfo.Text = "Tap to Continue";
					}
				}
			});
			lblSecondTime.UserInteractionEnabled = true;
			lblSecondTime.AddGestureRecognizer(touchRecondizerSecondaryTime);


			btnRight.TouchUpInside += (sender, e) => {
				if (_active != null) {
					repo.Delete(_active);
					_active = null;
					SwitchToInfoMode(() => {
						ReloadData();
					});
				}
			};

			btnLeft.TouchUpInside += (sender, e) => {
				if (_active != null) {
					_active.Date = DateTime.Now;
					_stopPair.Stop();
					repo.Update(_active);
					_active = null;
					SwitchToInfoMode(() => {
						ReloadData();
					});
				}
			};

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

		private void UpdateHieghtInPercent(int? total, int? current, UIView parent, UIView child, UILabel label, string sufix = "")
		{
			var percent =  (float)(current ?? 0) / (total ?? 1);
			var rect = child.Frame;
			var height = parent.Frame.Height * percent;
			if (height > parent.Frame.Height) {
				height = parent.Frame.Height;
			}
			// keep it on the bottom
			//child.Frame = new RectangleF(rect.Left, parent.Frame.Bottom - height, rect.Size.Width, height);
			// keep it on the top
			child.Frame = new RectangleF(rect.Left, 0, rect.Size.Width, height);

			label.Text = String.Format("{0:#}{1}", 100 * percent, sufix);
		}

		private void UpdateView(FeedingEntry entry)
		{
			btnLeft.Hidden = true;
			btnRight.Hidden = true;
			lblTitle.Text = "Last Feeding";

			lblMainTime.Hidden = false;
			lblSecondTimeInfo.Hidden = false;
			pnlInfoSmall.Hidden = false;

			// main info
			if (entry.TotalBreastLength != null) {
				if ((DateTime.Now - entry.Date) < TimeSpan.FromMinutes (5)) {
					lblMainTime.Text = "now";
					lblMainTimeInfo.Text = "";
				} else {
					var touple = entry.Date.AsAgoTextTuple();
					lblMainTime.Text = touple.Item1;
					lblMainTimeInfo.Text = touple.Item2;
				}
			} else {
				lblMainTime.Text = "";
				lblMainTimeInfo.Text = "";
			}
			lblSecondTimeInfo.Text = "minutes";

			UpdateHieghtInPercent(entry.TotalBreastLengthSeconds, entry.LeftBreastLengthSeconds, pgbContainerLeft, pgbValueLeft, pgbTextLeft, "%");
			UpdateHieghtInPercent(entry.TotalBreastLengthSeconds, entry.RightBreastLengthSeconds, pgbContainerRight, pgbValueRight, pgbTextRight, "%");

			SelectRightLeftButton ((entry.RightBreastLengthSeconds ?? 0) - (entry.LeftBreastLengthSeconds ?? 0)); // if there was more from right, the next should be form left
			// secondary info
			if (entry.TotalBreastLength == null) {
				//TODO: support for more then 60 m inutes
			}
			lblSecondTime.Text = entry.TotalBreastLength == null ? "" : String.Format("{0:#}", entry.TotalBreastLength.Value.TotalMinutes); // (@"mm\:ss");
		}

		private void EmptyView()
		{
			lblMainTime.Text = "";
			lblSecondTimeInfo.Text = "";
			lblSecondTime.Text = "";
			pnlInfoSmall.Hidden = true;
			lblMainTimeInfo.Text = "Start on the top";
			pnlAgo.Hidden = true;
		}

		private void SwitchToInfoMode(Action done = null)
		{
			lblMainTime.Hidden = false;
			lblMainTimeInfo.Hidden = false;
			pnlAgo.Hidden = false;
			lblMainTime.Text = "";
			lblMainTimeInfo.Text = "";
			lblButtonsHeader.Text = "start a new feeding";


			Action animFinished = () => {
				btnLeft.Hidden = true;
				btnRight.Hidden = true;
				pnlAgo.Hidden = false;
				pnlTime.Layer.CornerRadius = 0;
			};
			Action move = () => {
				btnLeft.Alpha = 0;
				btnRight.Alpha = 0;
				pnlTime.Frame = _defaultTimeFrame;
				pgbContainerRight.Center = _defaultContainerRightCenter;			
			};

			if (done == null)
			{
				move();
				animFinished();
			}
			else 
			{
				UIView.Animate(0.3, 0, UIViewAnimationOptions.CurveEaseInOut,
               () => {
					move();
				},
				() => {
					animFinished();
					done();
				});
			}
		}

		private void SwitchToFeedingMode(bool left, FeedingEntry entry, Action done = null)
		{
			lblMainTime.Hidden = true;
			lblMainTimeInfo.Hidden = true;
			lblSecondTime.Text = "";
			lblTitle.Text = "New Feeding";
			lblSecondTimeInfo.Text = "Tap to Pause";
			lblButtonsHeader.Text = "switch sides";

			btnLeft.Alpha = 0;
			btnRight.Alpha = 0;
			btnLeft.Hidden = false;
			btnRight.Hidden = false;
			pnlAgo.Hidden = true;
			pnlTime.Layer.CornerRadius = 6;

			Action startFeeding = () => {
				pnlAgo.Hidden = true;
				lblSecondTimeInfo.Hidden = false;
				pnlInfoSmall.Hidden = false;
				_active = entry;
				SelectRightLeftButton(left);
				_stopPair = new TimeStopPair (_active);
			};

			Action move = () => {
				pnlTime.Center = pnlTime.Center;
				pnlTime.Frame = new RectangleF(pnlAgo.Frame.X + 30, pnlAgo.Frame.Y + 30, pnlAgo.Frame.Width - 60, pnlAgo.Frame.Height - 60);
				pgbContainerRight.Center = new PointF(pgbContainerRight.Center.X - 60, pgbContainerRight.Center.Y);
				btnLeft.Alpha = 1;
				btnRight.Alpha = 1;
			};

			if (done == null)
			{
				move();
				startFeeding();
			}
			else 
			{
				UIView.Animate(0.3, 0, UIViewAnimationOptions.CurveEaseInOut,
		            () => {
						move();
					},
					() => {
						startFeeding();
						done();
					});
			}
		}

		public void ReloadData() 
		{
			var repo = ServiceLocator.Get<Repository> ();
			var last = repo.Query<FeedingEntry> ("SELECT * FROM FeedingEntry ORDER BY Id DESC LIMIT 1").FirstOrDefault();
			if (last == null) 
			{
				EmptyView();
			} 
			else 
			{
				if (last.IsRunning)
				{
					SwitchToFeedingMode(last.IsLeftBreastRunning, last);
				} 
				else 
				{
					UpdateView(last);
				}
			}
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
					UpdateHieghtInPercent (min30, rightLength > min30 ? min30 : rightLength, pgbContainerLeft, pgbValueLeft, pgbTextLeft);
					UpdateHieghtInPercent (min30, leftLength > min30 ? min30 : leftLength, pgbContainerRight, pgbValueRight, pgbTextRight);
				}
			});
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
			SwitchToFeedingMode(left, new FeedingEntry{ Date = Time.Now }, () => {
				_stopPair.Start (left);
				repo.Insert(_active);
			});
		}

	}
}

