using System;
using MonoTouch.UIKit;

namespace FeedMeMom
{
	public static class SkinExtensions
	{
		public static void SkinButton(this Skin skin, UIButton button) 
		{
			button.Layer.CornerRadius = 4;
			button.BackgroundColor = skin.ButtonActive;
			button.Font = Fonts.DefaultButton;
			button.SetTitleColor(skin.ButtonText, UIControlState.Normal);
			button.SetTitleColor(skin.ButtonTextSelected, UIControlState.Selected);
			button.SetTitleColor(skin.ButtonTextSelected, UIControlState.Highlighted);
		}
	}
}

