using System;

namespace FeedMeMom.Common
{


	public class TimeStop
	{
		public TimeStop()
		{
		}

		public TimeStop(int lengthSeconds, DateTime? startTime, bool isPaused)
		{
			Length = TimeSpan.FromSeconds (lengthSeconds);
			if (!isPaused)
			{
				StartTime = startTime;
			}
		}

		public TimeSpan Length { get; set; }

		public TimeSpan GetTotalLength()
		{
			if (StartTime == null) {
				return Length;
			}
			return Length + ((Time.Now - StartTime) ?? TimeSpan.Zero);
		}

		public DateTime? StartTime { get; set; }

		public bool IsRunning { get { return StartTime != null; } }

		public void Start()
		{
			StartTime = Time.Now;
		}

		public void Stop()
		{
			if (StartTime == null) 
			{
				return;
			}

			Length = GetTotalLength();
			StartTime = null;
		}
	}
}

