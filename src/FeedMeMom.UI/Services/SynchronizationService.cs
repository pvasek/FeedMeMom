using System;
using FeedMeMom.Common;
using MonoTouch.Foundation;

namespace FeedMeMom.UI
{
	public class SynchronizationService: ISynchronizationService
	{
		public SynchronizationService (NSObject rootView)
		{
			_rootView = rootView;
		}

		private NSObject _rootView;

		#region ISynchronizationService implementation
		public void Synchronize (Action action)
		{
			_rootView.InvokeOnMainThread(() => action());
		}
		#endregion
	}
}

