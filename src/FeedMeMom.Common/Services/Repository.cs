using System;
using SQLite;
using System.IO;
using FeedMeMom.Common.Entities;


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
		}
		
		public static Repository CreatePersonalDb(string dbName) 
		{				
			var documents = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			var dbPath = Path.Combine (documents, dbName);
			return new Repository(dbPath);
		}
	}
}

