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
			if (hexColor.Length != 6)
				throw new ArgumentException ("Hexa string needs to have 6 characters");

			return UIColor.FromRGB (
				Convert.ToSByte (""+hexColor[0]+hexColor[1], 16),
				Convert.ToSByte (""+hexColor[2]+hexColor[3], 16),
				Convert.ToSByte (""+hexColor[4]+hexColor[5], 16));
		}

		static UIColorExtensions() 
		{
			ButtonActive = FromHex ("50AEFF");
			ButtonInactive = FromHex ("C3C3C3");
		}

		public static UIColor ButtonActive { get; set; }
		public static UIColor ButtonInactive { get; set; }

	}
}

