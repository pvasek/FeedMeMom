using System;
using MonoTouch.UIKit;

namespace FeedMeMom
{
	public static class ColorUtil
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
	}

	public class Colors
	{
		public UIColor ButtonActive { get; set; }
		public UIColor ButtonInactive { get; set; }
		public UIColor Toolbar { get; set; }
		public UIColor RunningTimeText { get; set; }
		public UIColor PausedTimeText { get; set; }
		public UIColor Ago { get; set; }
		public UIColor Time { get; set; }
		public UIColor Background { get; set; }
		public UIColor ToolbarText { get; set; }
		public UIColor AgoText { get; set; }
		public UIColor TimeText { get; set; }
		public UIColor ButtonText { get; set; }
		public UIColor AgoInfoText { get; set; }
		public UIColor TimeInfoText { get; set; }
		public UIColor ButtonInfoText { get; set; }
		public UIColor SideMenuRow { get; set; }
		public UIColor SideMenuRowText { get; set; }
		public UIColor SideMenuRowBorder { get; set; }

		public bool IsDark { get; set; }

		public static Colors Active { get; set; }
	}

	public class LightColors: Colors
	{
		public LightColors() 
		{
			ButtonActive = ColorUtil.FromHex("#b02157");//FromHex ("50AEFF");
			ButtonInactive = ColorUtil.FromHex ("#fc9bbe");
			Toolbar = ColorUtil.FromHex("#830537");
			RunningTimeText = ButtonActive;
			PausedTimeText = ColorUtil.FromHex("#bbbbbb");
			Ago = ColorUtil.FromHex("#b02157");
			Time = ColorUtil.FromHex("#eaeaea");
			Background = UIColor.White;
			ToolbarText = UIColor.White;
			AgoText = UIColor.White;
			TimeText = ButtonActive;
			ButtonText = UIColor.White;
			AgoInfoText = ColorUtil.FromHex("#F7A144");
			TimeInfoText = ColorUtil.FromHex("#B92157");
			ButtonInfoText = ColorUtil.FromHex("#919191");
			SideMenuRow = ColorUtil.FromHex("#560928");
			SideMenuRowText = UIColor.White;
			SideMenuRowBorder = ColorUtil.FromHex("#770c37");
		}
	}

	public class DarkColors: Colors 
	{
		public DarkColors() {
			IsDark = true;
			ButtonActive = ColorUtil.FromHex("#4B4B4B");//FromHex ("50AEFF");
			ButtonInactive = ColorUtil.FromHex ("#2F2F2F");
			Toolbar = ColorUtil.FromHex("#232323");
			RunningTimeText = ColorUtil.FromHex("#A8A8A8");
			PausedTimeText = ColorUtil.FromHex("#595959");
			Ago = ColorUtil.FromHex("#2F2F2F");
			Time = ColorUtil.FromHex("#2A2A2A");
			Background = ColorUtil.FromHex("#232323");

			ToolbarText = ColorUtil.FromHex("#555555");
			AgoText = ColorUtil.FromHex("#A8A8A8");
			AgoInfoText = ColorUtil.FromHex("#525252");
			TimeText = ColorUtil.FromHex("#595959");
			TimeInfoText = ColorUtil.FromHex("#595959");
			ButtonText = ColorUtil.FromHex("#595959");
			ButtonInfoText = ColorUtil.FromHex("#3F3E3E");
			SideMenuRow = ColorUtil.FromHex("#A8A8A8");
			SideMenuRowText = ColorUtil.FromHex("#444444");
			SideMenuRowBorder = ColorUtil.FromHex("#999999");
		}
	}
}

