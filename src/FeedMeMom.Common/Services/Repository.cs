using System;
using SQLite;
using System.IO;
using FeedMeMom.Common.Entities;
using System.Collections.Generic;


namespace FeedMeMom.Common
{
	public class Repository: SQLiteConnection
	{
		private Repository (string path) : base (path)
		{

			//CreateCommand("DROP TABLE " + Table<FeedingEntry>().Table.TableName).ExecuteNonQuery();
			CreateTable<FeedingEntry> ();
		}

		public void GenerateFeedings()
		{
			var feedings = Table<FeedingEntry>();
			foreach (var item in feedings)
			{
				Delete(item);
			}
			const int numberOfDays = 360;
			var random = new Random();
			var date = DateTime.Now.AddDays(-1*numberOfDays);
			var startDate = date;
			var list = new List<FeedingEntry>();
			for (var i = 0; i < numberOfDays; i++) {
				date = startDate.AddDays(i);
				for (var j = 0; j < 6 + random.Next(0, 3); j++)
				{
					date = date.AddMinutes(random.Next(61, 300)); 
					list.Add(new FeedingEntry
				       { 
							Date = date,
							LeftBreastLengthSeconds = random.Next(0, 35) * 60,
							RightBreastLengthSeconds = random.Next(0, 35)*60,					
						});
				}
			}
			foreach (var item in list)
			{
				Insert(item);
			}
		}

		public static Repository CreatePersonalDb(string dbName) 
		{				
			var documents = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			var dbPath = Path.Combine (documents, dbName);
			return new Repository(dbPath);
		}
	}
}

