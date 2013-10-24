using System;
using SQLite;
using System.Text;


namespace FeedMeMom.Common.Entities
{
	public class FeedingEntry: IEntry
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public FeedingType Type { get; set; }
		public string Name { get; set; }
		public DateTime Date { get; set; }	
		public int? Value { get; set; }
		public int? LeftBreastLengthSeconds { get; set; }
		public int? RightBreastLengthSeconds { get; set; }
		
		public int? TotalBreastLengthSeconds { 
			get {
				return (LeftBreastLengthSeconds ?? 0) + (RightBreastLengthSeconds ?? 0); 
			}
		}

		public TimeSpan? TotalBreastLength {
			get { return TotalBreastLengthSeconds.SecondsAsTimeSpan(0); }
		}

		public DateTime? LeftStartTime { get; set; }
		public DateTime? RightStartTime { get; set; }

		public DateTime? PausedAt { get; set; }
		public bool? IsLeft { get; set; }

		[Ignore]
		public TimeSpan? LeftBreastLength {
			get { return LeftBreastLengthSeconds.SecondsAsTimeSpan(0); }
			set 
			{
				if (value == null) {
					LeftBreastLengthSeconds = null;
				} else {
					LeftBreastLengthSeconds = (int)value.Value.TotalSeconds;
				}
			}
		}

		[Ignore]
		public TimeSpan? RightBreastLength {
			get { return RightBreastLengthSeconds.SecondsAsTimeSpan(0); }
			set { 
				if (value == null) {
					RightBreastLengthSeconds = null;
				} else {
					RightBreastLengthSeconds = (int)value.Value.TotalSeconds;
				}
			}
		}


		public bool IsLeftBreastRunning { get { return LeftStartTime != null; } }
		public bool IsRightBreastRunning { get { return RightStartTime != null; } }

		public bool IsRunning 
		{ 
			get { return IsLeftBreastRunning || IsRightBreastRunning; } 
		}

		public bool IsPaused 
		{
			get { return PausedAt != null; }
		}
			
		private string FormatBreastFeeding ()
		{
			var result = new StringBuilder();
			result.AppendFormat ("BREAST {0} min ", (int)TotalBreastLength.Value.TotalMinutes);
			var r = RightBreastLength;
			var l = LeftBreastLength;
			var hasLeft = l != null && l != TimeSpan.Zero;
			var hasRight = r != null && r != TimeSpan.Zero;
			if (hasLeft && hasRight) {
				result.AppendFormat ("(left {0}, right {1})", (int)l.Value.TotalMinutes, (int)r.Value.TotalMinutes);
			}
			else
				if (hasLeft) {
					result.Append ("left");
				}
				else
					if (hasRight) {
						result.Append ("right");
					}
			return result.ToString();
		}

		public string FormatAsString ()
		{
			if (Type == FeedingType.Breast) {
				return FormatBreastFeeding ();
			}
			if (Type == FeedingType.Bottle) {
				return String.Format ("BOTTLE {0} ml", Value);
			}
			if (Type == FeedingType.Solid) {
				return String.Format ("SOLID {0} ml", Value);
			}
			return "";
		}

	}
}

