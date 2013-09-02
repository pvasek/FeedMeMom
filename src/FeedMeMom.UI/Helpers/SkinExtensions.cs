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

		public static void SkinNavigationBar(this Skin skin, UINavigationBar nb) 
		{
			nb.SetBackgroundImage(new UIImage(), UIBarMetrics.Default);
			nb.SetBackgroundImage(new UIImage(), UIBarMetrics.LandscapePhone);
			nb.ShadowImage = new UIImage();
			nb.BackgroundColor = Skin.Active.Toolbar;
			nb.TintColor = Skin.Active.Toolbar;
			nb.SetTitleTextAttributes(new UITextAttributes {
				TextColor = Skin.Active.ToolbarText,
				TextShadowColor = UIColor.Clear,
				Font = Fonts.ToolbarTitle,
			});

		}
	}
}

