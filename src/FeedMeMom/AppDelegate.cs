using MonoTouch.Foundation;
using MonoTouch.UIKit;

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
			var backImage = UIImage
				.FromBundle ("btn_back.png")
				.CreateResizableImage (new UIEdgeInsets(2, 25, 2, 2));
			var backPressedImage = UIImage
				.FromBundle ("btn_back_selected.png")
					.CreateResizableImage (new UIEdgeInsets(2, 25, 2, 2));

			UINavigationBar.Appearance.SetBackgroundImage (UIImage.FromBundle("background.png"), UIBarMetrics.Default);
			UIBarButtonItem.Appearance.SetBackButtonBackgroundImage (backImage, UIControlState.Normal, UIBarMetrics.Default);
			UIBarButtonItem.Appearance.SetBackgroundImage (UIImage.FromBundle("btn.png"), UIControlState.Normal, UIBarMetrics.Default);

			UIBarButtonItem.Appearance.SetTitleTextAttributes (
				new UITextAttributes(){ 
					TextShadowColor = UIColor.Clear,
					TextShadowOffset = new UIOffset(0, 0),
					TextColor = UIColor.FromRGB(85, 85, 85),
					Font = UIFont.SystemFontOfSize(12)
				},		
				UIControlState.Normal);

			UIBarButtonItem.Appearance.SetTitleTextAttributes (
				new UITextAttributes(){ 
					TextShadowColor = UIColor.Clear,
					TextShadowOffset = new UIOffset(0, 0),
					TextColor = UIColor.FromRGB(95, 95, 95),
					Font = UIFont.SystemFontOfSize(12)
				}, 
				UIControlState.Highlighted);



			UIBarButtonItem.Appearance.SetBackButtonBackgroundImage (backPressedImage, UIControlState.Highlighted, UIBarMetrics.Default);
			UIBarButtonItem.Appearance.SetBackgroundImage (UIImage.FromBundle("btn_selected.png"), UIControlState.Highlighted, UIBarMetrics.Default);

			window.RootViewController = navigationController;
			//navigationController.NavigationBarHidden = true;
			window.MakeKeyAndVisible ();
			
			return true;
		}

		public override void OnActivated (UIApplication application)
		{
			if (viewController != null) {
				viewController.ReloadData ();
			}
		}
	}
}

