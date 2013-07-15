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
			//CreateTable<SleepEntry> ();
			//CreateTable<PooEntry> ();
			//CreateTable<HeightEntry>();
			//CreateTable<TemperatureEntry>();
			//CreateTable<WeightEntry>();
			//CreateTable<MedicineEntry>();

			//CreateTable<MedicineSetting>();



			//foreach (var i in Query<FeedingEntry>("select * from FeedingEntry")) {
			//	Delete (i);	
			//}

//			var date = DateTime.Now.AddYears(-1);
//			for (var i = 0; i < 3650; i++) {
//				if (i % 10 == 0)
//				{
//					date = date.AddDays(1).AddHours(-1 * date.Hour);
//				}	
//				else
//					date = date.AddHours(1);
//				Insert (new SleepEntry { Date = date, Value = i});
//			}
			//Insert (new FeedingEntry { Name = "Bottle feeding 1", Date = DateTime.Now.AddDays(-2)});
			//Insert (new FeedingEntry { Name = "Bottle feeding 2", Date = DateTime.Now.AddDays(-1)});

			//Insert (new SleepEntry { Date = DateTime.Now.AddDays(-1), Value = 150});
			//Insert (new PooEntry { Date = DateTime.Now.AddDays(-2)});
			//Insert (new PooEntry { Date = DateTime.Now.AddDays(-1)});
		}
		
		public static Repository CreatePersonalDb(string dbName) 
		{				
			var documents = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			var dbPath = Path.Combine (documents, dbName);
			return new Repository(dbPath);
		}
	}
}

