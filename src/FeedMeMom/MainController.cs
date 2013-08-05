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

		private PointF _defaultContainerRightCenter;
		private PointF _defaultContainerLeftCenter;
		private RectangleF _defaultTimeFrame;
		private const int _defaultRadius = 4;
		private SideMenuHub _sideMenuHub;
		private SideMenu _sideMenu;

		public void ApplyColors()
		{
			var colors = Colors.Active;
			pnlToolbar.BackgroundColor = colors.Toolbar;
			pnlAgo.BackgroundColor = colors.Ago;
			pnlTime.BackgroundColor = colors.Time;
			pnlRunningTime.BackgroundColor = colors.Time;	
			View.BackgroundColor = colors.Background;
			lblTitle.TextColor = colors.ToolbarText;
			lblMainTime.TextColor = colors.AgoText;
			lblMainTimeInfo.TextColor = colors.AgoInfoText;
			lblSecondTime.TextColor = colors.TimeText;
			lblSecondTimeInfo.TextColor = colors.TimeInfoText;
			lblButtonsHeader.TextColor = colors.ButtonInfoText;
			lblRunningTime.TextColor = colors.TimeText;
			lblRunningInfo.TextColor = colors.TimeInfoText;

			pnlFSAgo.BackgroundColor = colors.Ago;		
			pnlFSMainTime.BackgroundColor = colors.Time;
			//pnlFSMainAction.BackgroundColor = colors.Toolbar;
			//pnlFSMainAction.TextColor = colors.AgoInfoText;
			lblFSAgoInfo.TextColor = colors.AgoInfoText;
			pnlFirstStart.BackgroundColor = colors.Background;
			lblFSTimeInfo.TextColor = colors.TimeInfoText;

			btnLeft.SetTitleColor(colors.ToolbarText, UIControlState.Normal);
			btnRight.SetTitleColor(colors.ToolbarText, UIControlState.Normal);
			btnStartLeft.SetTitleColor(colors.ButtonText, UIControlState.Normal);
			btnStartRight.SetTitleColor(colors.ButtonText, UIControlState.Normal);


			var opacity = Colors.IsDark ? 0.5f : 1f;
			btnSideMenu.Layer.Opacity = opacity;
			pgbContainerLeft.Layer.Opacity = opacity;
			pgbContainerRight.Layer.Opacity = opacity;
			pgbValueLeft.Layer.Opacity = opacity;
			pgbValueRight.Layer.Opacity = opacity;
			pgbTextLeft.TextColor = colors.IndicatorText;
			pgbTextRight.TextColor = colors.IndicatorText;
		}

		private void StyleSideMenuButton()
		{
			var hamburgerImage = UIImage.FromBundle("hamburger_icon");
			btnSideMenu.SetImage(hamburgerImage, UIControlState.Normal);
			btnSideMenu.SetImage(hamburgerImage, UIControlState.Highlighted);
			btnSideMenu.SetImage(hamburgerImage, UIControlState.Selected);
			btnSideMenu.SetTitle("", UIControlState.Normal);
			btnSideMenu.SetTitle("", UIControlState.Selected);
			btnSideMenu.SetTitle("", UIControlState.Highlighted);

		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();		
			var repo = ServiceLocator.Get<Repository>();

			ApplyColors();
			Colors.ColorsChanged += (sender, e) => {
				ApplyColors();
				ReloadData();
			};

			View.AddSubview(pnlFirstStart);
			pnlFirstStart.Frame = new RectangleF(pnlAgo.Frame.X, pnlAgo.Frame.Y, pnlFirstStart.Frame.Width, pnlFirstStart.Frame.Height);		

			pnlFirstStart.Hidden = false;
			NavigationController.NavigationBarHidden = true;
			btnStartLeft.Layer.CornerRadius = _defaultRadius;
			btnStartRight.Layer.CornerRadius = _defaultRadius;
			pnlStartNewFeeding.Layer.CornerRadius = _defaultRadius;
			pgbContainerLeft.Layer.CornerRadius = 20;
			pgbContainerLeft.ClipsToBounds = true;
			pgbContainerRight.Layer.CornerRadius = 20;
			pgbContainerRight.ClipsToBounds = true;
			pnlRunningTime.Hidden = true;
			btnLeft.Hidden = true;
			btnRight.Hidden = true;
			imgFirstStartArrow.Image = UIImage.FromBundle("arrow");

			StyleSideMenuButton();

			_sideMenu = new SideMenu();
			_sideMenuHub = SideMenuHub.CreateAndHookup(View, _sideMenu.View);

			_sideMenu.Items.Add(new ActionItem(Resources.History));
			_sideMenu.Items.Add(new ActionItem(Resources.SwitchDayNightMode, Colors.ToggleDayNightMode));
			_sideMenu.Items.Add(new ActionItem(Resources.Feedback));
			_sideMenu.Items.Add(new ActionItem("Delete Data", () => {
				var feedings = repo.Table<FeedingEntry>();
				foreach (var item in feedings) {
					repo.Delete(item);
					ReloadData();
				}
			}));




			lblSecondTimeInfo.TextColor = UIColor.Gray;
			_defaultTimeFrame = pnlTime.Frame;
			_defaultContainerRightCenter = pgbContainerRight.Center;
			_defaultContainerLeftCenter = pgbContainerLeft.Center;

			var touchRecondizerSecondaryTime = new UITapGestureRecognizer((e) => {
				if (_active != null) {
					if (_stopPair.Toggle())
					{
						lblRunningTime.TextColor = Colors.Active.RunningTimeText;
						lblRunningInfo.TextColor = Colors.Active.RunningTimeText;
						lblRunningInfo.Text = Resources.TapToPause;
					} 
					else 
					{
						lblRunningTime.TextColor = Colors.Active.PausedTimeText;
						lblRunningInfo.TextColor = Colors.Active.PausedTimeText;
						lblRunningInfo.Text = Resources.TapToContinue;
					}
				}
			});
			lblRunningTime.UserInteractionEnabled = true;
			lblRunningTime.AddGestureRecognizer(touchRecondizerSecondaryTime);


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

			btnStartLeft.TouchUpInside += (sender, e) => {

				if (_active == null) {
					StartFeeding(repo, true);
				} else if (_active.LeftStartTime == null) {
					_stopPair.Start (true);
					SelectRightLeftButton(true);
				}

			};

			btnStartRight.TouchUpInside += (sender, e) => {

				if (_active == null) {
					StartFeeding(repo, false);
				} else if (_active.RightStartTime == null) {
					_stopPair.Start (false);
					SelectRightLeftButton(false);
				}
			};


			btnSideMenu.TouchUpInside += (sender, e) => {
				_sideMenuHub.Toggle();
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

		private void UpdateHieghtInPercent(int? total, int? current, UIView parent, UIView child, UILabel label, bool showPercent)
		{
			string sufix = showPercent ? "%" : "";
			if (total == 0)
			{
				current = 0;
				total = 1;
			}
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

			if (percent < 0.001)
			{
				sufix = "";
			}
			label.Text = String.Format("{0:#}{1}", showPercent ? (int)(100 * percent) : (current / 60), sufix);
		}

		private void UpdateView(FeedingEntry entry)
		{
			btnLeft.Hidden = true;
			btnRight.Hidden = true;
			lblTitle.Text = Resources.LastFeeding;

			lblMainTime.Hidden = false;
			lblMainTimeInfo.Hidden = false;
			lblSecondTimeInfo.Hidden = false;
			pnlAgo.Hidden = false;
			pnlFirstStart.Hidden = true;

			// main info
			if (entry.TotalBreastLength != null) {
				if ((DateTime.Now - entry.Date) < TimeSpan.FromMinutes (5)) {
					lblMainTime.Text = Resources.Now;
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
			lblSecondTimeInfo.Text = Resources.Minutes;

			UpdateHieghtInPercent(entry.TotalBreastLengthSeconds, entry.LeftBreastLengthSeconds, pgbContainerLeft, pgbValueLeft, pgbTextLeft, true);
			UpdateHieghtInPercent(entry.TotalBreastLengthSeconds, entry.RightBreastLengthSeconds, pgbContainerRight, pgbValueRight, pgbTextRight, true);

			SelectRightLeftButton ((entry.RightBreastLengthSeconds ?? 0) - (entry.LeftBreastLengthSeconds ?? 0)); // if there was more from right, the next should be form left
			// secondary info
			if (entry.TotalBreastLength == null) {
				//TODO: support for more then 60 m inutes
			}
			lblSecondTime.Text = entry.TotalBreastLength == null ? "" : String.Format("{0:0}", entry.TotalBreastLength.Value.TotalMinutes); // (@"mm\:ss");
		}

		private void EmptyView()
		{
			lblMainTime.Text = "";
			lblSecondTimeInfo.Text = "";
			pnlAgo.Hidden = false;
			pnlTime.Hidden = false;
			lblMainTime.Hidden = true;
			lblMainTimeInfo.Hidden = false;
			pnlRunningTime.Hidden = true;
			pnlStartNewFeeding.Hidden = false;
			pnlFirstStart.Hidden = false;
			btnSideMenu.Hidden = false;
			btnStartLeft.BackgroundColor = Colors.Active.ButtonActive;
			btnStartRight.BackgroundColor = Colors.Active.ButtonActive;
		}

		private void SwitchToInfoMode(Action done = null)
		{
			pnlFirstStart.Hidden = true;
			lblMainTime.Hidden = false;
			lblMainTimeInfo.Hidden = false;
			lblSecondTimeInfo.Hidden = false;
			pnlAgo.Hidden = false;
			lblMainTime.Text = "";
			lblMainTimeInfo.Text = "";
			lblButtonsHeader.Text = Resources.StartNewFeeding;
			pnlRunningTime.Hidden = true;


			Action animFinished = () => {
				btnLeft.Hidden = true;
				btnRight.Hidden = true;
				pnlAgo.Hidden = false;
				btnSideMenu.Hidden = false;
				pnlTime.Layer.CornerRadius = 0;
			};
			Action move = () => {
				btnLeft.Alpha = 0;
				btnRight.Alpha = 0;
				pnlTime.Frame = _defaultTimeFrame;
				pgbContainerRight.Center = _defaultContainerRightCenter;		
				pgbContainerLeft.Center = _defaultContainerLeftCenter;
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
			btnSideMenu.Hidden = true;
			pnlFirstStart.Hidden = true;
			lblMainTime.Hidden = true;
			lblMainTimeInfo.Hidden = true;
			lblSecondTimeInfo.Hidden = true;
			lblSecondTime.Text = "";
			lblTitle.Text = Resources.NewFeeding;
			lblRunningInfo.Text = Resources.TapToPause;
			lblButtonsHeader.Text = Resources.SwitchSides;

			btnLeft.Alpha = 0;
			btnRight.Alpha = 0;
			btnLeft.Hidden = false;
			btnRight.Hidden = false;
			pnlAgo.Hidden = true;
			pnlTime.Layer.CornerRadius = _defaultRadius;

			Action startFeeding = () => {
				pnlAgo.Hidden = true;
				_active = entry;
				SelectRightLeftButton(left);
				_stopPair = new TimeStopPair (_active);
			};

			Action move = () => {
				pnlTime.Center = pnlTime.Center;
				pnlTime.Frame = new RectangleF(30, pnlAgo.Frame.Y + 30, 260, 160);
				pgbContainerRight.Center = new PointF(155, 130);
				pgbContainerLeft.Center = new PointF(105, 130);
				btnLeft.Alpha = 1;
				btnRight.Alpha = 1;
				pnlRunningTime.Center = new PointF(pnlTime.Frame.Width / 2, pnlTime.Frame.Height / 2 - 20);
			};

			Action animFinished = () => {
				pnlRunningTime.Hidden = false;
			};

			if (done == null)
			{
				move();
				animFinished();
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
						animFinished();
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
					lblRunningTime.Text = String.Format("{0}:{1:00}", (int)now.TotalMinutes, (int)now.Seconds);
					const int min30 = 30*60;
					var rightLength = (int)_stopPair.Right.GetTotalLength().TotalSeconds;
					var leftLength = (int)_stopPair.Left.GetTotalLength().TotalSeconds;
					var percentRight100 = rightLength < min30 ? min30 : rightLength;
					var percentLeft100 = leftLength < min30 ? min30 : leftLength;
					UpdateHieghtInPercent (percentRight100, rightLength, pgbContainerRight, pgbValueRight, pgbTextRight, false);
					UpdateHieghtInPercent (percentLeft100, leftLength, pgbContainerLeft, pgbValueLeft, pgbTextLeft, false);
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
				btnStartLeft.BackgroundColor = Colors.Active.ButtonActive;
				btnStartRight.BackgroundColor = Colors.Active.ButtonActive;
			}
			else if (left > 0) {
				btnStartLeft.BackgroundColor = Colors.Active.ButtonActive;
				btnStartRight.BackgroundColor = Colors.Active.ButtonInactive;
			} else {
				btnStartLeft.BackgroundColor = Colors.Active.ButtonInactive;
				btnStartRight.BackgroundColor = Colors.Active.ButtonActive;
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

