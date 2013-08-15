using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using FeedMeMom.Common;

namespace FeedMeMom
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UIWindow window;
		MainController viewController;
		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			app.IdleTimerDisabled = true;
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			viewController = new MainController ();

			var navigationController = new UINavigationController (viewController);

			UpdateAppearance();
			Skin.SkinChanged += (sender, e) => {
				UpdateAppearance();
			};

			window.RootViewController = navigationController;
			window.MakeKeyAndVisible ();
			ServiceLocator.Register<EmailSender>(new EmailSender(Configuration.FeedbackEmail, navigationController));
			return true;
		}

		public override void OnActivated (UIApplication application)
		{
			if (viewController != null) {
				viewController.ReloadData ();
			}
		}

		static void UpdateAppearance()
		{
			var normalColor = Backgrounds.Clear;
			UIBarButtonItem.Appearance.SetBackButtonBackgroundImage(normalColor, UIControlState.Normal, UIBarMetrics.Default);
			UIBarButtonItem.Appearance.SetBackButtonBackgroundImage(normalColor, UIControlState.Highlighted, UIBarMetrics.Default);
			UIBarButtonItem.Appearance.SetBackButtonBackgroundImage(normalColor, UIControlState.Selected, UIBarMetrics.Default);
			UIBarButtonItem.Appearance.SetBackgroundImage(normalColor, UIControlState.Normal, UIBarMetrics.Default);
			UIBarButtonItem.Appearance.SetBackgroundImage(normalColor, UIControlState.Highlighted, UIBarMetrics.Default);
			UIBarButtonItem.Appearance.SetBackgroundImage(normalColor, UIControlState.Selected, UIBarMetrics.Default);

			UINavigationBar.Appearance.ShadowImage = new UIImage();
			UINavigationBar.Appearance.SetBackgroundImage(new UIImage(), UIBarMetrics.Default);
			UINavigationBar.Appearance.BackgroundColor = Skin.Active.Toolbar;
			UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes {
				TextColor = Skin.Active.ToolbarText,
				TextShadowColor = UIColor.Clear,
				Font = Fonts.ToolbarTitle,
			});
		}

		protected override void Dispose(bool disposing)
		{
			ServiceLocator.Dispose();
			base.Dispose(disposing);
		}
	}
}

