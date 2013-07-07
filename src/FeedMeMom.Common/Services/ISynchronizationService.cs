using System;

namespace FeedMeMom.Common
{
	public interface ISynchronizationService
	{
		void Synchronize (Action action);
	}
}

