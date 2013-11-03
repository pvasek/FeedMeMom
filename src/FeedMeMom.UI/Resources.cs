
	


// #####################################################################
//
// THIS FILE IS GENERATED
//
// #####################################################################

using System;
using MonoTouch.Foundation;


namespace FeedMeMom {
			
	public static class Resources
	{
		private static string GetLocalized(string key)
		{
			var result = NSBundle.MainBundle.LocalizedString(key, key);
			if (result != null)
			{
				result = result.Replace("|", Environment.NewLine);
			}
			return result;
		}
		
		static Resources()
		{
			TapToPause = GetLocalized("TapToPause");
			TapToContinue = GetLocalized("TapToContinue");
			SwitchSides = GetLocalized("SwitchSides");
			NewFeeding = GetLocalized("NewFeeding");
			StartNewFeeding = GetLocalized("StartNewFeeding");
			Minutes = GetLocalized("Minutes");
			Now = GetLocalized("Now");
			LastFeeding = GetLocalized("LastFeeding");
			Feedback = GetLocalized("Feedback");
			IlikeThisApp = GetLocalized("IlikeThisApp");
			SwitchDayNightMode = GetLocalized("SwitchDayNightMode");
			History = GetLocalized("History");
			Back = GetLocalized("Back");
			Cancel = GetLocalized("Cancel");
			Save = GetLocalized("Save");
			BuyPhrase = GetLocalized("BuyPhrase");
			HistoryBuyHeadline = GetLocalized("HistoryBuyHeadline");
			Buy = GetLocalized("Buy");
			LeftLetter = GetLocalized("LeftLetter");
			RightLetter = GetLocalized("RightLetter");
			GetFullVersion = GetLocalized("GetFullVersion");
			ReportBug = GetLocalized("ReportBug");
			RequestNewFeature = GetLocalized("RequestNewFeature");
			JustStayInTouch = GetLocalized("JustStayInTouch");
			Appstore = GetLocalized("Appstore");
			Share = GetLocalized("Share");
			ShareByEmail = GetLocalized("ShareByEmail");
			RateReviewThisApp = GetLocalized("RateReviewThisApp");
			ShareEmailSubject = GetLocalized("ShareEmailSubject");
			ShareEmailBody = GetLocalized("ShareEmailBody");
			FeedbackHelpText = GetLocalized("FeedbackHelpText");
			FirstStartAgoText = GetLocalized("FirstStartAgoText");
			FirstStartTimeText = GetLocalized("FirstStartTimeText");
			NightModeBuyHeadline = GetLocalized("NightModeBuyHeadline");
			HoursAgo = GetLocalized("HoursAgo");
			MinutesAgo = GetLocalized("MinutesAgo");
			DayAgo = GetLocalized("DayAgo");
			DaysAgo = GetLocalized("DaysAgo");
			AgoPrefix = GetLocalized("AgoPrefix");
			RateThisApp = GetLocalized("RateThisApp");
			RateMessage = GetLocalized("RateMessage");
			RateYesButton = GetLocalized("RateYesButton");
			RateNoButton = GetLocalized("RateNoButton");
			RateLaterButton = GetLocalized("RateLaterButton");
			Statistics = GetLocalized("Statistics");
			StatisticsNumberOfFeedings = GetLocalized("StatisticsNumberOfFeedings");
			StatisticsLengthOfFeeding = GetLocalized("StatisticsLengthOfFeeding");
			StatisticsFeedingsADay = GetLocalized("StatisticsFeedingsADay");
			StatisticsSides = GetLocalized("StatisticsSides");
			StatisticsNumberDescription = GetLocalized("StatisticsNumberDescription");
			StatisticsLengthDescription = GetLocalized("StatisticsLengthDescription");
			StatisticsCountDescription = GetLocalized("StatisticsCountDescription");
			StatisticsUsageDescriptions = GetLocalized("StatisticsUsageDescriptions");
			StatisticsLeft = GetLocalized("StatisticsLeft");
			StatisticsRight = GetLocalized("StatisticsRight");
			StatisticsDay = GetLocalized("StatisticsDay");
			StatisticsWeek = GetLocalized("StatisticsWeek");
			StatisticsMonth = GetLocalized("StatisticsMonth");
			StatisticsAll = GetLocalized("StatisticsAll");
			StatisticsBuyHeadline = GetLocalized("StatisticsBuyHeadline");
			DoYouReallyWantToThisCancelFeeding = GetLocalized("DoYouReallyWantToThisCancelFeeding");
			Yes = GetLocalized("Yes");
			No = GetLocalized("No");
			StatisticsForTheLastDay = GetLocalized("StatisticsForTheLastDay");
			StatisticsForTheLastWeek = GetLocalized("StatisticsForTheLastWeek");
			StatisticsForTheLastMonth = GetLocalized("StatisticsForTheLastMonth");
	
		}
		
		public static string TapToPause { get; private set; }
		public static string TapToContinue { get; private set; }
		public static string SwitchSides { get; private set; }
		public static string NewFeeding { get; private set; }
		public static string StartNewFeeding { get; private set; }
		public static string Minutes { get; private set; }
		public static string Now { get; private set; }
		public static string LastFeeding { get; private set; }
		public static string Feedback { get; private set; }
		public static string IlikeThisApp { get; private set; }
		public static string SwitchDayNightMode { get; private set; }
		public static string History { get; private set; }
		public static string Back { get; private set; }
		public static string Cancel { get; private set; }
		public static string Save { get; private set; }
		public static string BuyPhrase { get; private set; }
		public static string HistoryBuyHeadline { get; private set; }
		public static string Buy { get; private set; }
		public static string LeftLetter { get; private set; }
		public static string RightLetter { get; private set; }
		public static string GetFullVersion { get; private set; }
		public static string ReportBug { get; private set; }
		public static string RequestNewFeature { get; private set; }
		public static string JustStayInTouch { get; private set; }
		public static string Appstore { get; private set; }
		public static string Share { get; private set; }
		public static string ShareByEmail { get; private set; }
		public static string RateReviewThisApp { get; private set; }
		public static string ShareEmailSubject { get; private set; }
		public static string ShareEmailBody { get; private set; }
		public static string FeedbackHelpText { get; private set; }
		public static string FirstStartAgoText { get; private set; }
		public static string FirstStartTimeText { get; private set; }
		public static string NightModeBuyHeadline { get; private set; }
		public static string HoursAgo { get; private set; }
		public static string MinutesAgo { get; private set; }
		public static string DayAgo { get; private set; }
		public static string DaysAgo { get; private set; }
		public static string AgoPrefix { get; private set; }
		public static string RateThisApp { get; private set; }
		public static string RateMessage { get; private set; }
		public static string RateYesButton { get; private set; }
		public static string RateNoButton { get; private set; }
		public static string RateLaterButton { get; private set; }
		public static string Statistics { get; private set; }
		public static string StatisticsNumberOfFeedings { get; private set; }
		public static string StatisticsLengthOfFeeding { get; private set; }
		public static string StatisticsFeedingsADay { get; private set; }
		public static string StatisticsSides { get; private set; }
		public static string StatisticsNumberDescription { get; private set; }
		public static string StatisticsLengthDescription { get; private set; }
		public static string StatisticsCountDescription { get; private set; }
		public static string StatisticsUsageDescriptions { get; private set; }
		public static string StatisticsLeft { get; private set; }
		public static string StatisticsRight { get; private set; }
		public static string StatisticsDay { get; private set; }
		public static string StatisticsWeek { get; private set; }
		public static string StatisticsMonth { get; private set; }
		public static string StatisticsAll { get; private set; }
		public static string StatisticsBuyHeadline { get; private set; }
		public static string DoYouReallyWantToThisCancelFeeding { get; private set; }
		public static string Yes { get; private set; }
		public static string No { get; private set; }
		public static string StatisticsForTheLastDay { get; private set; }
		public static string StatisticsForTheLastWeek { get; private set; }
		public static string StatisticsForTheLastMonth { get; private set; }
	

	}
	
}