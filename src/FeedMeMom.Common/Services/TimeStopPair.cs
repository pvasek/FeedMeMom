using System;
using FeedMeMom.Common.Entities;

namespace FeedMeMom.Common
{

	public class TimeStopPair 
	{
		public TimeStopPair(FeedingEntry entry)
		{
			Left =  new TimeStop(entry.LeftBreastLengthSeconds ?? 0, entry.LeftStartTime);
			Right = new TimeStop(entry.RightBreastLengthSeconds ?? 0, entry.RightStartTime);

			_entry = entry;
		}

		private FeedingEntry _entry;

		public TimeStop Left { get; set; }
		public TimeStop Right { get; set; }

		public TimeSpan GetTotalLength()
		{
			return Left.GetTotalLength() + Right.GetTotalLength();
		}

		public void Start(bool left) 
		{
			if (left) {
				Right.Stop();
				Left.Start();
			} else {
				Left.Stop();
				Right.Start();
			}
			SaveToEntry();
		}

		public void Stop()
		{
			Left.Stop();
			Right.Stop();
			SaveToEntry();
		}

//		private void LoadFromEntry()
//		{
//			Left.StartTime = _entry.LeftStartTime;
//			Left.Length = _entry.LeftBreastLength ?? TimeSpan.Zero;
//			Right.StartTime = _entry.RightStartTime;
//			Right.Length = _entry.RightBreastLength ?? TimeSpan.Zero;
//		}

		private void SaveToEntry()
		{
			_entry.LeftStartTime = Left.StartTime;
			_entry.LeftBreastLength = Left.Length;
			_entry.RightStartTime = Right.StartTime;
			_entry.RightBreastLength = Right.Length;
		}
	}
	
}
