using System;
using System.Text;
using FeedMeMom.Common.Entities;

namespace FeedMeMom.Common
{
	public static class Utils
	{
		public static string FormatAsDateTime (this DateTime dateTime)
		{
			return FormatAsDateTime((DateTime?)dateTime);
		}

		public static string FormatAsDateTime (this DateTime? dateTime)
		{
			if (dateTime == null) 
			{
				return null; 
			}
			var value = dateTime.Value;
			var result = new StringBuilder ();
			var now = DateTime.Now;
			if (now.Date == value.Date) {
				result.Append ("Today ");
			} else if (now.AddDays (-1).Date == value.Date) {
				result.Append ("Yesterday ");
			} else {
				result.Append(value.ToString("M"));
				result.Append(" ");
			}

			result.Append(value.ToShortTimeString());
			return result.ToString();
		}
	

		public static TimeSpan? SecondsAsTimeSpan(this int? value, int? defaultValue = null)
		{				
			value = value ?? defaultValue;
			if (value == null)
					return null;
			return TimeSpan.FromSeconds (value.Value);
		}

		public static string FormatAsCompleteText (this IEntry entry)
		{
			if (entry == null)
				return "";
			var title = entry.FormatAsString ();
			if (String.IsNullOrWhiteSpace (title)) {
				return entry.Date.FormatAsDateTime ();
			} else {
				return entry.Date.FormatAsDateTime () + " - " + title;
			}
		}

		private static readonly DateTime DayOnly = new DateTime(2000,1,1);

		public static string ToShortTimeString(this TimeSpan time)
		{
			return DayOnly.Add (time).ToShortTimeString();
		}

		public static Tuple<string, string> AsAgoText(this DateTime time) 
		{
			var diff = DateTime.Now - time;
			return diff.AsAgoText ();
		}

		public static Tuple<string, string> AsAgoText(this TimeSpan time) 
		{
			if (((int)time.TotalMinutes) == 1) {
				return new Tuple<string, string> ("", "");
			} else if (time.TotalMinutes < 60) {
				return new Tuple<string, string>(((int)time.TotalMinutes).ToString (), "Minutes Ago");
			} else if (time.TotalHours < 24) {
				return new Tuple<string, string>(time.ToString(@"hh\:mm"), "Hours Ago");
			}
			if (((int)time.TotalDays) == 1) {
				return new Tuple<string, string> ("1", "Day Ago");
			} else {
				return new Tuple<string, string> (((int)time.TotalDays).ToString(), "Days Ago");
			}
		}
	}
}
























