using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using FeedMeMom.Common;
using FeedMeMom.Common.Entities;
using System.Linq;
using System.Threading;
using MonoTouch.CoreGraphics;
using FeedMeMom.Helpers;

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

		private HistoryController _historyController;

		public void ApplyColors()
		{
			var skin = Skin.Active;
			pnlAgo.BackgroundColor = skin.Ago;
			pnlTime.BackgroundColor = skin.Time;
			pnlRunningTime.BackgroundColor = skin.Time;	
			View.BackgroundColor = skin.Background;
			lblMainTime.TextColor = skin.AgoText;
			lblMainTimeInfo.TextColor = skin.AgoInfoText;
			lblSecondTime.TextColor = skin.TimeText;
			lblSecondTimeInfo.TextColor = skin.TimeInfoText;
			lblButtonsHeader.TextColor = skin.ButtonInfoText;
			lblRunningTime.TextColor = skin.TimeText;
			lblRunningInfo.TextColor = skin.TimeInfoText;

			pnlFSAgo.BackgroundColor = skin.Ago;		
			pnlFSMainTime.BackgroundColor = skin.Time;
			pnlFSMainAction.BackgroundColor = skin.Toolbar;
			pnlFSMainAction.TextColor = skin.AgoInfoText;
			lblFSAgoInfo.TextColor = skin.AgoInfoText;
			pnlFirstStart.BackgroundColor = skin.Background;
			lblFSTimeInfo.TextColor = skin.TimeInfoText;

			btnStartLeft.SetTitleColor(skin.ButtonText, UIControlState.Normal);
			btnStartRight.SetTitleColor(skin.ButtonText, UIControlState.Normal);


			var opacity = Skin.IsDark ? 0.5f : 1f;
			_pgbLeft.Layer.Opacity = opacity;
			_pgbRight.Layer.Opacity = opacity;
			_pgbLeft.LabelView.TextColor = skin.IndicatorText;
			_pgbRight.LabelView.TextColor = skin.IndicatorText;

			imgFirstStartArrow.Image = skin.ImageArrow;
		}

		public void ApplyNavigationBarAppearance()
		{
			var nb = NavigationController.NavigationBar;
			nb.SetBackgroundImage(new UIImage(), UIBarMetrics.Default);
			nb.ShadowImage = new UIImage();
			nb.BackgroundColor = Skin.Active.Toolbar;
			nb.TintColor = Skin.Active.Toolbar;
			nb.SetTitleTextAttributes(new UITextAttributes {
				TextColor = Skin.Active.ToolbarText,
				TextShadowColor = UIColor.Clear,
				Font = Fonts.ToolbarTitle,
			});
			_btnLeft.TintColor = Skin.Active.Toolbar;
			_btnRight.TintColor = Skin.Active.Toolbar;
			_btnSideMenu.TintColor = Skin.Active.Toolbar;
			_btnSideMenu.Image = Skin.Active.ImageHamburger;

			var textAttrs = new UITextAttributes {
				TextColor = Skin.Active.ToolbarText,
				TextShadowColor = UIColor.Clear,
				Font = Fonts.ToolbarButton
			};
			_btnLeft.SetTitleTextAttributes(textAttrs, UIControlState.Normal);
			_btnRight.SetTitleTextAttributes(textAttrs, UIControlState.Normal);
			_btnSideMenu.SetTitleTextAttributes(textAttrs, UIControlState.Normal);

			// force title redraw
			var title = Title;
			Title = "";
			Title = title;		
		}

		private void CancelClick(object sender, EventArgs e)
		{
			var repo = ServiceLocator.Get<Repository>();
			if (_active != null) {
				repo.Delete(_active);
				_active = null;
				SwitchToInfoMode(() => {
					ReloadData();
				});
			}
		}

		private void SaveClick(object sender, EventArgs e)
		{
			var repo = ServiceLocator.Get<Repository>();
			if (_active != null) {
				_active.Date = DateTime.Now;
				_stopPair.Stop();
				repo.Update(_active);
				_active = null;
				SwitchToInfoMode(() => {
					ReloadData();
				});
			}
		}

		private void ShowSideMenuClick(object sender, EventArgs e)
		{
			_sideMenuHub.Show();
		}

		private UIBarButtonItem _btnSideMenu;
		private UIBarButtonItem _btnLeft;
		private UIBarButtonItem _btnRight;
		private ProgressBar _pgbLeft;
		private ProgressBar _pgbRight;

		private void SetFeedingVisible(bool visible)		
		{
			if (visible)
			{
				NavigationItem.LeftBarButtonItem = _btnLeft;
				NavigationItem.RightBarButtonItem = _btnRight;
			} 
			else
			{
				NavigationItem.LeftBarButtonItem = _btnSideMenu;
				NavigationItem.RightBarButtonItem = null;
			}

		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();		
			Title = Resources.LastFeeding;
			_btnSideMenu = new UIBarButtonItem(Skin.Active.ImageHamburger, UIBarButtonItemStyle.Plain, ShowSideMenuClick);
			_btnLeft = new UIBarButtonItem(Resources.Cancel, UIBarButtonItemStyle.Plain, CancelClick);
			_btnRight = new UIBarButtonItem(Resources.Save, UIBarButtonItemStyle.Plain, SaveClick);
			_pgbLeft = new ProgressBar { Center = new PointF(34, 44) };
			_pgbRight = new ProgressBar { Center = new PointF(285, 44) };
			pnlTime.AddSubview(_pgbLeft);
			pnlTime.AddSubview(_pgbRight);


			var repo = ServiceLocator.Get<Repository>();

			ApplyColors();
			ApplyNavigationBarAppearance();
			Skin.SkinChanged += (sender, e) => {
				ApplyColors();
				ApplyNavigationBarAppearance();
				ReloadData();
			};

			View.AddSubview(pnlFirstStart);
			pnlFirstStart.Frame = new RectangleF(pnlAgo.Frame.X, pnlAgo.Frame.Y, pnlFirstStart.Frame.Width, pnlFirstStart.Frame.Height);		

			SetFeedingVisible(false);
			pnlFirstStart.Hidden = false;
			btnStartLeft.Layer.CornerRadius = _defaultRadius;
			btnStartRight.Layer.CornerRadius = _defaultRadius;
			pnlStartNewFeeding.Layer.CornerRadius = _defaultRadius;
			pnlRunningTime.Hidden = true;

			CreateSideMenu();
			lblSecondTimeInfo.TextColor = UIColor.Gray;
			_defaultTimeFrame = pnlTime.Frame;
			_defaultContainerRightCenter = _pgbRight.Center;
			_defaultContainerLeftCenter = _pgbLeft.Center;

			var touchRecondizerSecondaryTime = new UITapGestureRecognizer((e) => {
				if (_active != null) {
					if (_stopPair.Toggle())
					{
						lblRunningTime.TextColor = Skin.Active.RunningTimeText;
						lblRunningInfo.TextColor = Skin.Active.RunningTimeText;
						lblRunningInfo.Text = Resources.TapToPause;
					} 
					else 
					{
						lblRunningTime.TextColor = Skin.Active.PausedTimeText;
						lblRunningInfo.TextColor = Skin.Active.PausedTimeText;
						lblRunningInfo.Text = Resources.TapToContinue;
					}
				}
			});
			lblRunningTime.UserInteractionEnabled = true;
			lblRunningTime.AddGestureRecognizer(touchRecondizerSecondaryTime);		

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
		}	

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			ReloadData ();
		}

		protected override void Dispose (bool disposing)
		{
			_timer = _timer.SafeDispose();
			_historyController = _historyController.SafeDispose();
			_sideMenu = _sideMenu.SafeDispose();

			base.Dispose (disposing);
		}

		private void CreateSideMenu()
		{
			var repo = ServiceLocator.Get<Repository> ();

			_sideMenu = new SideMenu();
			_sideMenuHub = SideMenuHub.CreateAndHookup(View, _sideMenu.View);


			_sideMenu.Items.Add(new ActionItem(Resources.History, () => {
				if (_historyController == null) 
				{
					_historyController = new HistoryController();
				}
				_historyController.ReloadData();
				_sideMenuHub.Hide();
				NavigationController.PushViewController(_historyController, false);

			}));

			_sideMenu.Items.Add(new ActionItem(Resources.SwitchDayNightMode, () => {
				Skin.ToggleDayNightMode();
				_sideMenuHub.Hide();
			}));

			_sideMenu.Items.Add(new ActionItem(Resources.Feedback, () => {
				_sideMenuHub.Hide();
				NavigationController.PushViewController(new FeedbackController(), false);
			}));

			_sideMenu.Items.Add(new ActionItem(Resources.ShareTheLove, () => {
				_sideMenuHub.Hide();
				NavigationController.PushViewController(new ReviewController(), false);
			}));

			_sideMenu.Items.Add(new ActionItem("Delete Data", () => {
				var feedings = repo.Table<FeedingEntry>();
				foreach (var item in feedings) {
					repo.Delete(item);
					ReloadData();
				}
			}));		
		}

		private void UpdateView(FeedingEntry entry)
		{
			SetFeedingVisible(false);
			Title = Resources.LastFeeding;

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

			_pgbLeft.UpdateValue(entry.TotalBreastLengthSeconds, entry.LeftBreastLengthSeconds, true);
			_pgbRight.UpdateValue(entry.TotalBreastLengthSeconds, entry.RightBreastLengthSeconds, true);

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
			SetFeedingVisible(false);
			btnStartLeft.BackgroundColor = Skin.Active.ButtonActive;
			btnStartRight.BackgroundColor = Skin.Active.ButtonActive;
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
				SetFeedingVisible(false);
				pnlAgo.Hidden = false;
				pnlTime.Layer.CornerRadius = 0;
			};
			Action move = () => {
				pnlTime.Frame = _defaultTimeFrame;
				_pgbRight.Center = _defaultContainerRightCenter;		
				_pgbLeft.Center = _defaultContainerLeftCenter;
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
			pnlFirstStart.Hidden = true;
			lblMainTime.Hidden = true;
			lblMainTimeInfo.Hidden = true;
			lblSecondTimeInfo.Hidden = true;
			lblSecondTime.Text = "";
			Title = Resources.NewFeeding;
			lblRunningInfo.Text = Resources.TapToPause;
			lblButtonsHeader.Text = Resources.SwitchSides;


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
				_pgbRight.Center = new PointF(155, 130);
				_pgbLeft.Center = new PointF(105, 130);
				SetFeedingVisible(true);				pnlRunningTime.Center = new PointF(pnlTime.Frame.Width / 2, pnlTime.Frame.Height / 2 - 20);
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
					_pgbRight.UpdateValue(percentRight100, rightLength, false);
					_pgbLeft.UpdateValue(percentLeft100, leftLength, false);
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
				btnStartLeft.BackgroundColor = Skin.Active.ButtonActive;
				btnStartRight.BackgroundColor = Skin.Active.ButtonActive;
			}
			else if (left > 0) {
				btnStartLeft.BackgroundColor = Skin.Active.ButtonActive;
				btnStartRight.BackgroundColor = Skin.Active.ButtonInactive;
			} else {
				btnStartLeft.BackgroundColor = Skin.Active.ButtonInactive;
				btnStartRight.BackgroundColor = Skin.Active.ButtonActive;
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

		private void ShowCurrentScreenBrightness()
		{
			using (var img = UIApplication.SharedApplication.Windows.First().Screen.Capture())
			{
				var cgimg = img.CGImage;
				var imgData = new byte[cgimg.Width*cgimg.Height*4];
				var totalPerc = 0f;
				using (var bc = new CGBitmapContext(imgData, cgimg.Width, cgimg.Height, 8, 4*cgimg.Width,
				                                    CGColorSpace.CreateDeviceRGB(),
				                                    CGBitmapFlags.PremultipliedLast | CGBitmapFlags.ByteOrder32Big))
				{
					bc.DrawImage(new RectangleF(0, 0, cgimg.Width, cgimg.Height), cgimg);
					var totalBrightnessPercentage = 0f;
					var count = 0;
					for(var i = 0; i < imgData.Length; i=i+4)
					{
						var r = imgData[i] / 255f;
						var g = imgData[i] / 255f;
						var b = imgData[i] / 255f;
						totalBrightnessPercentage += (r+g+b) / 3f;
						count++;
					}
					totalPerc = totalBrightnessPercentage*100 / count;
				}
				var alert = new UIAlertView();
				alert.Title = String.Format("Brightness: {0}%", totalPerc);
				alert.AddButton("Close");
				alert.CancelButtonIndex = 0;
				alert.Show();
			}
		}
	}
}

