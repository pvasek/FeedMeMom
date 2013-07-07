using System;

namespace FeedMeMom.Common
{

	public enum TemperatureUnitType
	{
		Celsius,
		Fahrenheit
	}

	public enum WeightUnitType 
	{
		Kilogram,
		Pound
	}

	public enum LengthUnitType 
	{
		Centimeter,
		Inch
	}

	public enum VolumeUnitType 
	{
		Mililiter,
		OZ
	}

	public abstract class Settings
	{
		public TemperatureUnitType TemperatureUnit { get; set; }
		public WeightUnitType WeightUnit { get; set; }
		public LengthUnitType LengthUnit { get; set; }
		public VolumeUnitType VolumeUnit { get; set; }
		public string LastAction { get; set; }
		public string LastState { get; set; }

		public abstract void Load();
		public abstract void Save();

		public void ResetLastState()
		{
			LastState = null;
			LastAction = null;
			Save ();
		}
	}
}

