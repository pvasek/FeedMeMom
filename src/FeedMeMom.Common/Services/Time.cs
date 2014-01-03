using System;

namespace FeedMeMom.Common
{
	public static class Time
	{
		private static DateTime? _overwritedNow;

		public static void NowIs(DateTime? value) 
		{
			_overwritedNow = value;
		}

		public static DateTime Now {
			get {
				return _overwritedNow != null ? _overwritedNow.Value : DateTime.Now;
			}
		}

		public static string AsMinutesText(this TimeSpan? time) 
		{
			if (time == null)
				return "";		

			return Math.Round(time.Value.TotalMinutes).ToString();
		}
	}
}

