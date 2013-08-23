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

	public class Skin
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
		public UIColor ToolbarTextActive { get; set; }
		public UIColor AgoText { get; set; }
		public UIColor TimeText { get; set; }
		public UIColor ButtonText { get; set; }
		public UIColor ButtonTextSelected { get; set; }
		public UIColor AgoInfoText { get; set; }
		public UIColor TimeInfoText { get; set; }
		public UIColor ButtonInfoText { get; set; }

		public UIColor SideMenuRow { get; set; }
		public UIColor SideMenuRowSelected { get; set; }
		public UIColor SideMenuRowSelectedText { get; set; }
		public UIColor SideMenuRowText { get; set; }
		public UIColor SideMenuRowBorder { get; set; }

		public UIColor TableRow { get; set; }
		public UIColor TableRowSelected { get; set; }
		public UIColor TableRowText { get; set; }
		public UIColor TableRowSelectedText { get; set; }
		public UIColor TableRowBorder { get; set; }
		public UIColor TableHeader { get; set; }

		public UIColor IndicatorText { get; set; }
		public UIColor IndicatorBackground { get; set; }
		public UIColor IndicatorForeground { get; set; }
		public UIColor IndicatorActiveText { get; set; }
		public UIColor IndicatorActiveBackground { get; set; }
		public UIColor IndicatorActiveForeground { get; set; }

		public UIColor PageText { get; set; }
		public UIColor TitleText { get; set; }

		public UIImage ImageHamburger { get; set; }
		public UIImage ImageArrow { get; set; }

		public string Name { get { return GetType().Name; }}

		private static Skin _dayMode = new LightSkin();
		private static Skin _nightMode = new DarkColors();
		private static Skin _active;

		static Skin() 
		{
			Active = _dayMode;
		}


		public static Skin Active 
		{ 
			get { return _active; }
			set 
			{
				var changed = value != _active;
				_active = value;
				if (changed)
				{
					if (SkinChanged != null)
					{
						SkinChanged(null, EventArgs.Empty);
					}
				}
			}
		}

		public static bool IsDark { get { return Active == _nightMode; } }

		public static void ToggleDayNightMode()
		{
			if (Active == _dayMode)
			{
				Active = _nightMode;
			} 
			else
			{
				Active = _dayMode;
			}
		}

		public static event EventHandler SkinChanged;
	}

	public class LightSkin: Skin
	{
		public LightSkin() 
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
			ButtonTextSelected = ColorUtil.FromHex("#F7A144");
			ToolbarTextActive = ColorUtil.FromHex("#F7A144");
			AgoInfoText = ColorUtil.FromHex("#F7A144");
			TimeInfoText = ColorUtil.FromHex("#B92157");
			ButtonInfoText = ColorUtil.FromHex("#919191");

			SideMenuRow = ColorUtil.FromHex("#560928"); //ColorUtil.FromHex("#560928");
			SideMenuRowText = ColorUtil.FromHex("#e7b8c4"); //ColorUtil.FromHex("#fc9bbe"); //ColorUtil.FromHex("#c26f8e");//UIColor.White;
			SideMenuRowBorder = ColorUtil.FromHex("#42071f");//ColorUtil.FromHex("#ec8fb1"); //ColorUtil.FromHex("#c26f8e");// ColorUtil.FromHex("#770c37");
			SideMenuRowSelected = ColorUtil.FromHex("#330416"); //ColorUtil.FromHex("#330416");
			SideMenuRowSelectedText = ColorUtil.FromHex("#ffffff"); //ColorUtil.FromHex("#330416");

			TableRow = ColorUtil.FromHex("#ffffff");
			TableRowSelected = ColorUtil.FromHex("#b02157");
			TableRowText = ColorUtil.FromHex("#b02157");
			TableRowSelectedText = ColorUtil.FromHex("#ffffff");
			TableRowBorder = ColorUtil.FromHex("#cccccc");
			TableHeader = ColorUtil.FromHex("#b02157");

			PageText = ColorUtil.FromHex("#ae2458");
			TitleText = ColorUtil.FromHex("#830537");

			IndicatorText = ColorUtil.FromHex("#999999");
			IndicatorBackground = ColorUtil.FromHex("#bfbfbf");
			IndicatorForeground = ColorUtil.FromHex("#d6d6d6");
			IndicatorActiveText = AgoText;
			IndicatorActiveBackground = Ago;
			IndicatorActiveForeground = ButtonInactive;

			ImageHamburger = Backgrounds.DayHamburger;
			ImageArrow = Backgrounds.DayArrow;

		}
	}

	public class DarkColors: Skin 
	{
		public DarkColors() {
			ButtonActive = ColorUtil.FromHex("#4B4B4B");//FromHex ("50AEFF");
			ButtonInactive = ColorUtil.FromHex ("#2F2F2F");
			Toolbar = ColorUtil.FromHex("#232323");
			RunningTimeText = ColorUtil.FromHex("#A8A8A8");
			PausedTimeText = ColorUtil.FromHex("#595959");
			Ago = ColorUtil.FromHex("#2F2F2F");
			Time = ColorUtil.FromHex("#2A2A2A");
			Background = ColorUtil.FromHex("#232323");

			ToolbarText = ColorUtil.FromHex("#555555");
			ToolbarTextActive = ColorUtil.FromHex("#525252");
			AgoText = ColorUtil.FromHex("#A8A8A8");
			AgoInfoText = ColorUtil.FromHex("#727272");
			TimeText = ColorUtil.FromHex("#595959");
			TimeInfoText = ColorUtil.FromHex("#595959");
			ButtonText = ColorUtil.FromHex("#777777");
			ButtonTextSelected = ColorUtil.FromHex("#A8A8A8");
			ButtonInfoText = ColorUtil.FromHex("#3F3E3E");

			SideMenuRow = ColorUtil.FromHex("#555555");
			SideMenuRowText = ColorUtil.FromHex("#cccccc");
			SideMenuRowBorder = ColorUtil.FromHex("#444444");
			SideMenuRowSelected = SideMenuRowBorder;
			SideMenuRowSelectedText = ColorUtil.FromHex("#eeeeee");

			TableRow = ColorUtil.FromHex("#333333");
			TableRowSelected = ColorUtil.FromHex("#444444");
			TableRowText = ColorUtil.FromHex("#aaaaaa");
			TableRowSelectedText = ColorUtil.FromHex("#cccccc");
			TableRowBorder = ColorUtil.FromHex("#222222");
			TableHeader = ColorUtil.FromHex("#555555");

			PageText = ColorUtil.FromHex("#595959");
			TitleText = ColorUtil.FromHex("#4B4B4B");

			IndicatorText = ColorUtil.FromHex("#aaaaaa");
			IndicatorBackground = ColorUtil.FromHex("#555555");
			IndicatorForeground = ColorUtil.FromHex("#777777");
			IndicatorActiveText = UIColor.White;
			IndicatorActiveBackground = ColorUtil.FromHex("#999999");
			IndicatorActiveForeground = ColorUtil.FromHex("#bbbbbb");


			ImageHamburger = Backgrounds.NightHamburger;
			ImageArrow = Backgrounds.NightArrow;

		}
	}
}

