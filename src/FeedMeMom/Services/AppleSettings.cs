using FeedMeMom.Common;
using MonoTouch.Foundation;

namespace FeedMeMom.UI.Services
{
	public class AppleSettings: Settings
	{
		private AppleSettings() {}

		public static AppleSettings Create() 
		{
			var result = new AppleSettings();
			result.Load ();
			return result;
		}

		private const string LengthUnitKey = "LengthUnit";
		private const string WeightUnitKey = "WeightUnit";
		private const string TemperatureUnitKey = "TemperatureUnit";
		private const string VolumeUnitKey = "VolumeUnit";
		private const string LastActionKey = "LastAction";
		private const string LastStateKey = "LastState";

		public override void Load()
		{
			var settings = NSUserDefaults.StandardUserDefaults;
			var dict = settings.AsDictionary();
			LengthUnit = (LengthUnitType)settings.IntForKey(LengthUnitKey);
			WeightUnit = (WeightUnitType)settings.IntForKey(WeightUnitKey);
			TemperatureUnit = (TemperatureUnitType)settings.IntForKey(TemperatureUnitKey);
			VolumeUnit = (VolumeUnitType)settings.IntForKey(VolumeUnitKey);
			LastAction = (string)settings.StringForKey(LastActionKey);
			LastState = (string)settings.StringForKey(LastStateKey);
		}

		public override void Save ()
		{
			var settings = NSUserDefaults.StandardUserDefaults;
			settings.SetInt((int)LengthUnit, LengthUnitKey);
			settings.SetInt((int)WeightUnit, WeightUnitKey);
			settings.SetInt((int)TemperatureUnit, TemperatureUnitKey);
			settings.SetInt((int)VolumeUnit, VolumeUnitKey);
			settings.SetString(LastAction ?? "", LastActionKey);
			settings.SetString(LastState ?? "", LastStateKey);
			settings.Synchronize();
		}
	}

}

