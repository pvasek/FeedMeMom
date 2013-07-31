using System;
using MonoTouch.UIKit;

namespace FeedMeMom
{
	public static class UIColorExtensions
	{
		public static UIColor FromHex(string hexColor)
		{
			if (hexColor == null)
				throw new ArgumentNullException ();

			if (hexColor.Length != 7 || hexColor[0] != '#')
				throw new ArgumentException ("Hexa string needs to have 7 #rrggbb characters");

			hexColor = hexColor.Substring(1, 6);

			return UIColor.FromRGB (
				Convert.ToSByte (""+hexColor[0]+hexColor[1], 16),
				Convert.ToSByte (""+hexColor[2]+hexColor[3], 16),
				Convert.ToSByte (""+hexColor[4]+hexColor[5], 16));
		}

		static UIColorExtensions() 
		{
			ButtonActive = FromHex("#b02157");//FromHex ("50AEFF");
			ButtonInactive = FromHex ("#fc9bbe");
			Toolbar = FromHex("#830537");
		}

		public static UIColor ButtonActive { get; set; }
		public static UIColor ButtonInactive { get; set; }
		public static UIColor Toolbar { get; set; }

	}
}

