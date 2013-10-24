using System;
using FeedMeMom.Common.Entities;

namespace FeedMeMom.Common
{

	public class TimeStopPair 
	{
		public TimeStopPair(FeedingEntry entry)
		{
			Left =  new TimeStop(entry.LeftBreastLengthSeconds ?? 0, entry.LeftStartTime, entry.IsPaused);
			Right = new TimeStop(entry.RightBreastLengthSeconds ?? 0, entry.RightStartTime, entry.IsPaused);
			_lastLeft = entry.IsLeft == true;

			_entry = entry;
		}

		private readonly FeedingEntry _entry;
		private bool _lastLeft;

		public TimeStop Left { get; set; }
		public TimeStop Right { get; set; }

		public TimeSpan GetTotalLength()
		{
			return Left.GetTotalLength() + Right.GetTotalLength();
		}

		public void Start(bool left) 
		{
			_lastLeft = left;
			if (left) {
				Right.Stop();
				Left.Start();
			} else {
				Left.Stop();
				Right.Start();
			}		
			SaveToEntry();
		}

		public bool Toggle()
		{
			if (Left.IsRunning || Right.IsRunning)
			{
				Stop();
				return false;
			} else
			{
				Start(_lastLeft);
				return true;
			}
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
			_entry.IsLeft = _lastLeft;
			_entry.LeftStartTime = Left.StartTime;
			_entry.LeftBreastLength = Left.Length;
			_entry.RightStartTime = Right.StartTime;
			_entry.RightBreastLength = Right.Length;

		}
	}
	
}
