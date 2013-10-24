using System;
using System.Drawing;
using MonoTouch.UIKit;
using FeedMeMom.Common;
using FeedMeMom.Common.Entities;
using System.Linq;
using System.Threading;
using FeedMeMom.Helpers;
using FeedMeMom.Controllers;
using FeedMeMom.UI;
using MTiRate;

namespace FeedMeMom
{
	public partial class MainController : UIViewController
	{
		public MainController () : base ("MainController", null)
		{
		}

		private PointF _defaultContainerRightCenter;
		private PointF _defaultContainerLeftCenter;
		private RectangleF _defaultTimeFrame;
		private const int _defaultRadius = 4;
		private SideMenuHub _sideMenuHub;
		private SideMenu _sideMenu;

		private UIBarButtonItem _btnSideMenu;
		private UIBarButtonItem _btnLeft;
		private UIBarButtonItem _btnRight;
		private ProgressBar _pgbLeft;
		private ProgressBar _pgbRight;

		private HistoryViewController _historyController;
		private BuyController _buyController;
		private StatisticsController _statisticsController;

		private RectangleF _leftFrame;
		private RectangleF _rightFrame;
		private RectangleF _buttonHeaderFrame;

		public void ApplyColors()
		{
			var skin = Skin.Active;
			pnlNavigationBarPlaceHolder.BackgroundColor = skin.Toolbar;
			pnlAgo.BackgroundColor = skin.Ago;
			pnlTime.BackgroundColor = skin.Time;
			pnlRunningTime.BackgroundColor = skin.Time;	
			View.BackgroundColor = skin.Background;
			lblMainTime.TextColor = skin.AgoText;
			lblMainTimeInfo.TextColor = skin.AgoInfoText;
			lblMainTimeInfoFirstHalf.TextColor = skin.AgoInfoText;
			lblSecondTime.TextColor = skin.TimeText;
			lblSecondTimeInfo.TextColor = skin.TimeInfoText;
			lblButtonsHeader.TextColor = skin.ButtonInfoText;
			lblRunningTime.TextColor = skin.TimeText;
			lblRunningInfo.TextColor = skin.TimeInfoText;

			pnlFSAgo.BackgroundColor = skin.Ago;		
			pnlFSMainTime.BackgroundColor = skin.Time;
			pnlFSMainAction.BackgroundColor = skin.Toolbar;
			pnlFSMainAction.TextColor = skin.AgoText;
			lblFSAgoInfo.TextColor = skin.FirstFeedingInfoText;
			pnlFirstStart.BackgroundColor = skin.Background;
			lblFSTimeInfo.TextColor = skin.TimeInfoText;

			btnStartLeft.SetTitleColor(skin.ButtonText, UIControlState.Normal);
			btnStartRight.SetTitleColor(skin.ButtonText, UIControlState.Normal);


			var opacity = Skin.IsDark ? 0.5f : 1f;
			_pgbLeft.Layer.Opacity = opacity;
			_pgbRight.Layer.Opacity = opacity;
			Action<ProgressBar> updateIndicator = (i) => {
				i.TextColor = skin.IndicatorText;
				i.BackColor = skin.IndicatorBackground;
				i.ForeColor = skin.IndicatorForeground;
				i.ActiveTextColor = skin.IndicatorActiveText;
				i.ActiveBackColor = skin.IndicatorActiveBackground;
				i.ActiveForeColor = skin.IndicatorActiveForeground;
				i.ApplyColors();
			};
			updateIndicator(_pgbLeft);
			updateIndicator(_pgbRight);

			imgFirstStartArrow.Image = skin.ImageArrow;
		}

		public void ApplyNavigationBarAppearance()
		{
			var nb = NavigationController.NavigationBar;
			Skin.Active.SkinNavigationBar(nb);
			_btnLeft.TintColor = Skin.Active.ToolbarButtonText;
			_btnRight.TintColor = Skin.Active.ToolbarButtonText;
			_btnSideMenu.TintColor = Skin.Active.ToolbarButtonText;
			_btnSideMenu.Image = Skin.Active.ImageHamburger;

			_btnLeft.SetToolbarStyle();
			_btnRight.SetToolbarStyle();
			_btnSideMenu.SetToolbarStyle();

			// force title redraw
			var title = Title;
			Title = "";
			Title = title;		
		}

		private void CancelClick(object sender, EventArgs e)
		{
			_pgbLeft.Active = false;
			_pgbRight.Active = false;

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
			_pgbLeft.Active = false;
			_pgbRight.Active = false;

			var repo = ServiceLocator.Get<Repository>();
			if (_active != null) {
				_active.Date = _active.PausedAt ?? DateTime.Now;
				_stopPair.Stop();
				_active.PausedAt = null;
				repo.Update(_active);
				_active = null;
				SwitchToInfoMode(() => {
					ReloadData();
					iRate.SharedInstance.LogEvent(false);
				});
			}
		}

		private void ShowSideMenuClick(object sender, EventArgs e)
		{
			_sideMenuHub.Toggle();
		}

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

		private void TimePanelSwitchToRunning(bool? left = null)
		{
			lblRunningTime.TextColor = Skin.Active.RunningTimeText;
			lblRunningInfo.TextColor = Skin.Active.RunningTimeText;
			lblRunningInfo.Text = Resources.TapToPause;
			if (left != null)
			{
				_pgbLeft.Active = left.Value;
				_pgbRight.Active = !left.Value;
			}
		}

		private void TimePanelSwitchToPaused()
		{
			lblRunningTime.TextColor = Skin.Active.PausedTimeText;
			lblRunningInfo.TextColor = Skin.Active.PausedTimeText;
			lblRunningInfo.Text = Resources.TapToContinue;
		}

		private void CreateLayout()
		{
			pnlNavigationBarPlaceHolder.SetNavbarPlaceholder();
			Title = Resources.LastFeeding;
			_btnSideMenu = new UIBarButtonItem(Skin.Active.ImageHamburger, UIBarButtonItemStyle.Plain, ShowSideMenuClick);
			_btnLeft = new UIBarButtonItem(Resources.Cancel, UIBarButtonItemStyle.Plain, CancelClick);
			_btnRight = new UIBarButtonItem(Resources.Save, UIBarButtonItemStyle.Plain, SaveClick);
			_pgbLeft = new ProgressBar { Center = new PointF(34, 44) };
			_pgbRight = new ProgressBar { Center = new PointF(285, 44) };
			pnlTime.AddSubview(_pgbLeft);
			pnlTime.AddSubview(_pgbRight);
			View.AddSubview(pnlFirstStart);
			pnlFirstStart.Frame = new RectangleF(pnlAgo.Frame.X, pnlAgo.Frame.Y, pnlFirstStart.Frame.Width, pnlFirstStart.Frame.Height);		
			pnlFirstStart.Hidden = false;
			btnStartLeft.Layer.CornerRadius = _defaultRadius;
			btnStartRight.Layer.CornerRadius = _defaultRadius;
			pnlFSMainAction.Layer.CornerRadius = _defaultRadius;
			pnlRunningTime.Hidden = true;

			var touchRecondizerSecondaryTime = new UITapGestureRecognizer(PauseRunToggle);
			lblRunningTime.UserInteractionEnabled = true;
			lblRunningTime.AddGestureRecognizer(touchRecondizerSecondaryTime);		

			btnStartLeft.SetTitle(Resources.LeftLetter);
			btnStartRight.SetTitle(Resources.RightLetter);
			lblButtonsHeader.Text = Resources.StartNewFeeding;

			pnlFSMainAction.Text = Resources.StartNewFeeding;
			lblFSAgoInfo.Text = Resources.FirstStartAgoText;
			lblFSTimeInfo.Text = Resources.FirstStartTimeText;

			CreateSideMenu();

		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();		

			if (ControlHelper.IsIPhone5)
			{
				lblButtonsHeader.Frame = lblButtonsHeader.Frame.Add(y: 60);
				btnStartLeft.Frame = btnStartLeft.Frame.Add(y: 60);
				btnStartRight.Frame = btnStartRight.Frame.Add(y: 60);
			}

			CreateLayout();

			var repo = ServiceLocator.Get<Repository>();

			ApplyColors();
			ApplyNavigationBarAppearance();
			Skin.SkinChanged += (sender, e) => {
				ApplyColors();
				ApplyNavigationBarAppearance();
				ReloadData();
			};

			SetFeedingVisible(false);

			_defaultTimeFrame = pnlTime.Frame;
			_defaultContainerRightCenter = _pgbRight.Center;
			_defaultContainerLeftCenter = _pgbLeft.Center;

			_timer = new Timer (TimerElapsed, null, 200, 200);

			_leftFrame = btnStartLeft.Frame;
			_rightFrame = btnStartRight.Frame;
			_buttonHeaderFrame = lblButtonsHeader.Frame;
			btnStartLeft.TouchUpInside += (sender, e) => {

				if (_active == null) {
					StartFeeding(repo, true);
				} else if (_active.LeftStartTime == null) {
					_stopPair.Start (true);
					SelectRightLeftButton(false);
					TimePanelSwitchToRunning(true);
					_active.PausedAt = null;
					repo.Update(_active);
				}
			};

			btnStartRight.TouchUpInside += (sender, e) => {

				if (_active == null) {
					StartFeeding(repo, false);
				} else if (_active.RightStartTime == null) {
					_stopPair.Start (false);
					SelectRightLeftButton(true);
					TimePanelSwitchToRunning(false);
					_active.PausedAt = null;
					repo.Update(_active);
				}
			};
		}	

		public void PauseRunToggle(UITapGestureRecognizer e)
		{
			if (_active != null) {
				if (_stopPair.Toggle())
				{
					TimePanelSwitchToRunning();
					_active.PausedAt = null;
				} 
				else 
				{
					TimePanelSwitchToPaused();
					_active.PausedAt = Time.Now;
				}
				var repo = ServiceLocator.Get<Repository>();
				repo.Update(_active);
			}
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			ReloadData();
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);
		}

		protected override void Dispose (bool disposing)
		{
			_timer = _timer.SafeDispose();
			_historyController = _historyController.SafeDispose();
			_buyController = _buyController.SafeDispose();
			_statisticsController = _statisticsController.SafeDispose();
			_sideMenu = _sideMenu.SafeDispose();

			base.Dispose (disposing);
		}

		private void CreateSideMenu()
		{
			_sideMenu = new SideMenu();
			_sideMenuHub = SideMenuHub.CreateAndHookup(View, _sideMenu.View);


			if (Configuration.IsFreeApp)
			{
				_sideMenu.Items.Add(new ActionItem(Resources.History, () => {
					if (_buyController == null) {
						_buyController = new BuyController();
					}
					_sideMenuHub.Hide();
					_buyController.BuyTitle = Resources.History;
					_buyController.BuyDescription = Resources.HistoryBuyHeadline;
					_buyController.ScreenImageName = "buy_history";
					NavigationController.PushViewController(_buyController, false);
				}, UIImage.FromBundle("list")));

				_sideMenu.Items.Add(new ActionItem(Resources.Statistics, () => {
					if (_buyController == null) {
						_buyController = new BuyController();
					}
					_sideMenuHub.Hide();
					_buyController.BuyTitle = Resources.Statistics;
					_buyController.BuyDescription = Resources.StatisticsBuyHeadline;
					_buyController.ScreenImageName = "buy_statistics";
					NavigationController.PushViewController(_buyController, false);
				}, UIImage.FromBundle("chart")));

				_sideMenu.Items.Add(new ActionItem(Resources.SwitchDayNightMode, () => {
					if (_buyController == null) {
						_buyController = new BuyController();
					}
					_sideMenuHub.Hide();
					_buyController.BuyTitle = Resources.SwitchDayNightMode;
					_buyController.BuyDescription = Resources.NightModeBuyHeadline;
					_buyController.ScreenImageName = "buy_nightmode";
					NavigationController.PushViewController(_buyController, false);
				}, UIImage.FromBundle("moon")));
			} 
			else
			{
			
				_sideMenu.Items.Add(new ActionItem(Resources.History, () => {
					if (_historyController == null)
					{
						_historyController = new HistoryViewController();
					}
					//_historyController.ReloadData();
					_sideMenuHub.Hide();
					NavigationController.PushViewController(_historyController, false);

				}, UIImage.FromBundle("list")));

				_sideMenu.Items.Add(new ActionItem(Resources.Statistics, () => {
					if (_statisticsController == null)
					{
						_statisticsController = new StatisticsController();
					}
					_sideMenuHub.Hide();
					NavigationController.PushViewController(_statisticsController, false);
				}, UIImage.FromBundle("chart")));

				_sideMenu.Items.Add(new ActionItem(Resources.SwitchDayNightMode, () => {
					Skin.ToggleDayNightMode();
					_sideMenuHub.Hide();
				}, UIImage.FromBundle("moon")));
			}
			_sideMenu.Items.Add(new ActionItem(Resources.IlikeThisApp, () => {
				_sideMenuHub.Hide();
				NavigationController.PushViewController(new ReviewController(), false);
			}, UIImage.FromBundle("star")));

			_sideMenu.Items.Add(new ActionItem(Resources.Feedback, () => {
				_sideMenuHub.Hide();
				NavigationController.PushViewController(new FeedbackController(), false);
			}, UIImage.FromBundle("envelope")));

			if (Configuration.IsTest)
			{
				var repo = ServiceLocator.Get<Repository> ();
				_sideMenu.Items.Add(new ActionItem("Generate Data", () => {
					repo.GenerateFeedings();
					ReloadData();
				}));		
				_sideMenu.Items.Add(new ActionItem("Delete Data", () => {
					var feedings = repo.Table<FeedingEntry>();
					foreach (var item in feedings) {
						repo.Delete(item);
						ReloadData();
					}
				}));		
				_sideMenu.Items.Add(new ActionItem("Generate Running L", () => {
					var entry = new FeedingEntry();
					entry.Date = DateTime.Now.AddMinutes(-18);
					entry.LeftStartTime = entry.Date;
					repo.Insert(entry);
					ReloadData();
				}));		
				_sideMenu.Items.Add(new ActionItem("Generate Running L + R", () => {
					var entry = new FeedingEntry();
					entry.Date = DateTime.Now.AddMinutes(-32);
					entry.LeftBreastLengthSeconds = (int)TimeSpan.FromMinutes(18).TotalSeconds;
					entry.RightStartTime = DateTime.Now.AddMinutes(-14);
					repo.Insert(entry);
					ReloadData();
				}));		
				_sideMenu.Items.Add(new ActionItem("Crash", () => {
					if (Crashlytics.Crashlytics.SharedInstance != null) {
						Crashlytics.Crashlytics.SharedInstance.Crash();
					} else {
						UIAlertView alert = new UIAlertView ();
						alert.Title = "Error";
						alert.AddButton ("OK");
						alert.AddButton ("Cancel");
						alert.Message = "SharedInstance is null";
						//alert.AlertViewStyle = UIAlertViewStyle.SecureTextInput;
						alert.Show ();
					}
				}));		
			}
		}

		private void UpdateView(FeedingEntry entry)
		{
			SetFeedingVisible(false);
			Title = Resources.LastFeeding;

			lblMainTime.Hidden = false;
			lblMainTimeInfoFirstHalf.Hidden = false;
			lblMainTimeInfo.Hidden = false;
			lblSecondTimeInfo.Hidden = false;
			pnlAgo.Hidden = false;
			pnlFirstStart.Hidden = true;

			// main info
			if (entry.TotalBreastLength != null) {
				if ((DateTime.Now - entry.Date) < TimeSpan.FromMinutes (5)) {
					lblMainTime.Text = Resources.Now;
					lblMainTimeInfoFirstHalf.Text = "";
					lblMainTimeInfo.Text = "";
				} else {
					var touple = entry.Date.AsAgoTextTuple();
					lblMainTime.Text = touple.Item1;
					lblMainTimeInfoFirstHalf.Text = Resources.AgoPrefix;
					if (touple.Item2 == AgoInterval.Minutes)
					{
						lblMainTimeInfo.Text = Resources.MinutesAgo;
					} 
					else if (touple.Item2 == AgoInterval.Hours)
					{
						lblMainTimeInfo.Text = Resources.HoursAgo;
					}
					else if (touple.Item2 == AgoInterval.Day)
					{
						lblMainTimeInfo.Text = Resources.DayAgo;
					}
					else if (touple.Item2 == AgoInterval.Days)
					{
						lblMainTimeInfo.Text = Resources.DaysAgo;
					}
				}
			} else {
				lblMainTime.Text = "";
				lblMainTimeInfoFirstHalf.Text = "";
				lblMainTimeInfo.Text = "";
			}
			lblSecondTimeInfo.Text = Resources.Minutes;

			_pgbLeft.UpdateValue(entry.TotalBreastLengthSeconds, entry.LeftBreastLengthSeconds, true);
			_pgbRight.UpdateValue(entry.TotalBreastLengthSeconds, entry.RightBreastLengthSeconds, true);

			SelectRightLeftButton ((entry.RightBreastLengthSeconds ?? 0) - (entry.LeftBreastLengthSeconds ?? 0)); // if there was more from right, the next should be form left
			lblSecondTime.Text = entry.TotalBreastLength == null ? "" : String.Format("{0:0}", entry.TotalBreastLength.Value.TotalMinutes); // (@"mm\:ss");
		}

		private void EmptyView()
		{
			lblMainTime.Text = "";
			lblSecondTimeInfo.Text = "";
			pnlAgo.Hidden = false;
			pnlTime.Hidden = false;
			lblMainTime.Hidden = true;
			lblMainTimeInfoFirstHalf.Hidden = false;
			lblMainTimeInfo.Hidden = false;
			pnlRunningTime.Hidden = true;
			pnlFSMainAction.Hidden = false;
			pnlFirstStart.Hidden = false;
			SetFeedingVisible(false);
			btnStartLeft.BackgroundColor = Skin.Active.ButtonActive;
			btnStartRight.BackgroundColor = Skin.Active.ButtonActive;
		}

		private void SwitchToInfoMode(Action done = null)
		{
			pnlFirstStart.Hidden = true;
			lblMainTime.Hidden = false;
			lblMainTimeInfoFirstHalf.Hidden = false;
			lblMainTimeInfo.Hidden = false;
			lblSecondTimeInfo.Hidden = false;
			pnlAgo.Hidden = false;
			lblMainTime.Text = "";
			lblMainTimeInfoFirstHalf.Text = "";
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

		private void SwitchToFeedingMode(bool left, FeedingEntry entry, Action done = null, bool updateButtonsColors = true)
		{
			pnlFirstStart.Hidden = true;
			lblMainTime.Hidden = true;
			lblMainTimeInfoFirstHalf.Hidden = true;
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
				if (updateButtonsColors) {
					SelectRightLeftButton(!left);
				}
				_stopPair = new TimeStopPair (_active);
			};

			Action move = () => {
				pnlTime.Center = pnlTime.Center;
				pnlTime.Frame = new RectangleF(30, pnlAgo.Frame.Y + 30, 260, 160);
				_pgbRight.Center = new PointF(155, 130);
				_pgbLeft.Center = new PointF(105, 130);
				SetFeedingVisible(true);				
				pnlRunningTime.Center = new PointF(pnlTime.Frame.Width / 2, pnlTime.Frame.Height / 2 - 20);
			};

			Action animFinished = () => {
				pnlRunningTime.Hidden = false;
				if (entry.IsPaused == true) {
					TimePanelSwitchToPaused();
				} 
				else 
				{
					TimePanelSwitchToRunning(left);
				}
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

		public override UIStatusBarStyle PreferredStatusBarStyle()
		{
			return UIStatusBarStyle.LightContent;
		}

		private FeedingEntry _last;

		public void ReloadData() 
		{
			var repo = ServiceLocator.Get<Repository> ();
			_last = repo.Query<FeedingEntry> ("SELECT * FROM FeedingEntry ORDER BY Id DESC LIMIT 1").FirstOrDefault();
			if (_last == null) 
			{
				EmptyView();
			} 
			else 
			{
				if (_last.IsRunning || _last.IsPaused)
				{
					SwitchToFeedingMode(_last.IsLeft == true, _last);
				} 
				else 
				{
					UpdateView(_last);
				}
			}
		}

		private Timer _timer;
		private FeedingEntry _active;
		private DateTime? _lastUpdate;

		private void TimerElapsed (object state)
		{
			if (_active == null && _lastUpdate > DateTime.Now.AddSeconds(-10)) {
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
				} else if (_last != null){
					UpdateView(_last);
					_lastUpdate = DateTime.Now;
				}
			});
		}


		private void SelectRightLeftButton(bool left)
		{
			SelectRightLeftButton (left ? 1 : -1);
		}

		private void SelectRightLeftButton(int left)
		{
			const int buttonOffset = 8;
			const int headerOffset = 50;

			lblButtonsHeader.TextAlignment = UITextAlignment.Center;
			if (_active != null)
			{
				left *= -1;
			} 
			else
			{
				if (left > 0)
				{
					lblButtonsHeader.TextAlignment = UITextAlignment.Left;
				}
				else if (left < 0)
				{
					lblButtonsHeader.TextAlignment = UITextAlignment.Right;
				}
			}

			if (left == 0) {
				btnStartLeft.BackgroundColor = Skin.Active.ButtonActive;
				btnStartRight.BackgroundColor = Skin.Active.ButtonActive;
				btnStartLeft.Frame = _leftFrame;
				btnStartRight.Frame = _rightFrame;
			}
			else if (left > 0) {
				btnStartLeft.BackgroundColor = Skin.Active.ButtonActive;
				btnStartRight.BackgroundColor = Skin.Active.ButtonInactive;

				btnStartLeft.Frame = _leftFrame;
				btnStartRight.Frame = _rightFrame.Add(buttonOffset, buttonOffset, -2*buttonOffset, -2*buttonOffset);
			} else {
				btnStartLeft.BackgroundColor = Skin.Active.ButtonInactive;
				btnStartRight.BackgroundColor = Skin.Active.ButtonActive;

				btnStartLeft.Frame = _leftFrame.Add(buttonOffset, buttonOffset, -2*buttonOffset, -2*buttonOffset);
				btnStartRight.Frame = _rightFrame;
			}
		}

		private TimeStopPair _stopPair;

		private void StartFeeding(Repository repo, bool left)
		{
			SwitchToFeedingMode(left, new FeedingEntry{ Date = Time.Now }, () => {
				_stopPair.Start (left);
				repo.Insert(_active);
				_pgbLeft.UpdateValue(0,0, false);
				_pgbRight.UpdateValue(0,0, false);
				TimePanelSwitchToRunning(left);

				UIView.Animate(2, () => {
					SelectRightLeftButton(!left);
				});
			}, false);
		}

	}
}

