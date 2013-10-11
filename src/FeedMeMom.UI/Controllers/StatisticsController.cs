using System;
using System.Linq;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using FeedMeMom.Helpers;
using System.Collections.Generic;
using FeedMeMom.Common;
using FeedMeMom.Common.Entities;

namespace FeedMeMom.UI
{
	public partial class StatisticsController : UIViewController
	{
		public StatisticsController() : base ("StatisticsController", null)
		{
			Title = Resources.Statistics;
			NavigationItem.HidesBackButton = true;
			NavigationItem.LeftBarButtonItem = new UIBarButtonItem(Resources.Back, UIBarButtonItemStyle.Plain, Close);
			NavigationItem.LeftBarButtonItem.SetToolbarStyle();
		}

		public void Close(object sender, EventArgs e)
		{
			NavigationController.PopViewControllerAnimated(true);
		}

		private static readonly List<int> _days = new List<int> { 1, 7, 30, 5000};

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			ApplyColors();
			LoadResources();
			Skin.SkinChanged += HandleColorsChanged;

			periodSelector.ValueChanged += (sender, e) => {
				UpdateCounts();
			};
			UpdateCounts();
		}

		private void LoadResources()
		{
			lblNumberTitle.Text = Resources.StatisticsNumberOfFeedings;
			lblNumberDescription.Text = Resources.StatisticsNumberDescription;
			lblLengthTitle.Text = Resources.StatisticsLengthOfFeeding;
			lblLengthDescription.Text = Resources.StatisticsLengthDescription;
			lblCountTitle.Text = Resources.StatisticsFeedingsADay;
			lblCountDescription.Text = Resources.StatisticsCountDescription;
			lblUsageTitle.Text = Resources.StatisticsSides;
			lblUsageDescription.Text = Resources.StatisticsUsageDescriptions;
			lblUsageLeftHint.Text = Resources.StatisticsLeft;
			lblUsageRightHint.Text = Resources.StatisticsRight;

			periodSelector.RemoveAllSegments();
			periodSelector.InsertSegment(Resources.StatisticsDay, 0, false);
			periodSelector.InsertSegment(Resources.StatisticsWeek, 1, false);
			periodSelector.InsertSegment(Resources.StatisticsMonth, 2, false);
			periodSelector.InsertSegment(Resources.StatisticsAll, 3, false);
			periodSelector.SelectedSegment = 0;
		}

		private void UpdateCounts()
		{
			var days = _days[periodSelector.SelectedSegment];
			var repo = ServiceLocator.Get<Repository>();
			var fromDate = DateTime.Now.AddDays(days*-1);
			var entries = repo
				.Table<FeedingEntry>()
					.ToList()
					.Where(i => i.Date >= fromDate)
					.ToList();

			var leftTime = TimeSpan.FromSeconds(entries.Sum(i => i.LeftBreastLengthSeconds) ?? 0);
			var rightTime = TimeSpan.FromSeconds(entries.Sum(i => i.RightBreastLengthSeconds) ?? 0);
			var totalTime = leftTime + rightTime;
			var left = (int)Math.Round(100*leftTime.TotalMinutes/totalTime.TotalMinutes);
			var right = 100-left;

			var feedingDays = entries
				.Select(i => i.Date.Date)
					.Distinct()
					.Count()-1;

			if (feedingDays < 1)
			{
				feedingDays = 1;
			}

			pnlData.Hidden = true;
			lblLengthValue.Text = String.Format("{0:0}", totalTime.TotalMinutes / entries.Count);
			lblNumberValue.Text = entries.Count.ToString();
			lblCountValue.Text = ((int)(entries.Count / feedingDays)).ToString();
			lblUsageValue.Text = String.Format("{0:###}%  {1:###}%", left, right);

			pnlData.Layer.Opacity = 0;
			pnlData.Hidden = false;
			UIView.Animate(0.6, () => {
				pnlData.Layer.Opacity = 1;
			});
		}

		private void HandleColorsChanged (object sender, EventArgs e)
		{
			ApplyColors();
		}

		private void ApplyColors() 
		{
			View.BackgroundColor = Skin.Active.Background;
			pnlToolbarPlaceholder.BackgroundColor = Skin.Active.Toolbar;
			periodSelector.TintColor = Skin.Active.SegmentTint;
		}
	}
}

