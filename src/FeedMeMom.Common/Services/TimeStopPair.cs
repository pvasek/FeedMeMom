using System;
using FeedMeMom.Common.Entities;

namespace FeedMeMom.Common
{

	public class TimeStopPair 
	{
		public TimeStopPair(FeedingEntry entry)
		{
			Left = new TimeStop ();
			Right = new TimeStop ();

			_entry = entry;
			Left.StartTime = entry.LeftStartTime;
			Left.Length = entry.LeftBreastLength ?? TimeSpan.Zero;
			Right.StartTime = entry.RightStartTime;
			Right.Length = entry.RightBreastLength ?? TimeSpan.Zero;
		}

		private FeedingEntry _entry;

		public TimeStop Left { get; set; }
		public TimeStop Right { get; set; }

		public TimeSpan GetTotalLength()
		{
			return Left.GetTotalLength () + Right.GetTotalLength ();
		}

		public void Start(bool left) 
		{
			if (left) {
				Right.Stop ();
				Left.Start ();
			} else {
				Left.Stop ();
				Right.Start ();
			}
			UpdateEntry ();
		}

		public void Stop()
		{
			Left.Stop ();
			Right.Stop ();
			UpdateEntry ();
		}

		private void UpdateEntry()
		{
			_entry.LeftStartTime = Left.StartTime;
			_entry.LeftBreastLength = Left.Length;
			_entry.RightStartTime = Right.StartTime;
			_entry.RightBreastLength = Right.Length;
		}
	}
	
}
