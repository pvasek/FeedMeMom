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
//			Insert(new FeedingEntry
//				{ 
//					Date = DateTime.Now.AddHours(-2.43),
//					LeftBreastLengthSeconds = 23*60,
//					RightBreastLengthSeconds = 6*60,					
//				});
			//GenerateFeedings();
		}

		private void GenerateFeedings()
		{
			var feedings = Table<FeedingEntry>();
			foreach (var item in feedings)
			{
				Delete(item);
			}
			var random = new Random();
			var date = DateTime.Now.AddDays(-5);
			var startDate = date;
			var list = new List<FeedingEntry>();
			for (var i = 0; i < 5; i++) {
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

