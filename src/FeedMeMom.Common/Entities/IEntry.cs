using System;

namespace FeedMeMom.Common.Entities
{
	public interface IEntry
	{
		int Id { get; set; }
		DateTime Date { get; set; }	
		string FormatAsString ();
		Type GetType();
	}
}

