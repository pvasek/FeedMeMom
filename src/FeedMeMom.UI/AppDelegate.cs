using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using FeedMeMom.Common;
using MTiRate;
using System.Runtime.InteropServices;
using System;

namespace FeedMeMom
{
	public partial class AppDelegateBase : UIApplicationDelegate
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
			app.SetStatusBarHidden(false, true);

			var navigationController = new MyUINavigationController (viewController);

			UpdateAppearance();
			Skin.SkinChanged += (sender, e) => {
				UpdateAppearance();
			};

			window.RootViewController = navigationController;
			window.MakeKeyAndVisible ();
			ServiceLocator.Register<EmailSender>(new EmailSender(Configuration.FeedbackEmail, navigationController));
			RegisterIRater();

			//CrashHelper.EnableCrashReporting();

			return true;
		}

		private static void RegisterIRater()
		{
			//iRate.SharedInstance.PreviewMode = true;
			iRate.SharedInstance.AppStoreID = (uint)Configuration.AppId;
			iRate.SharedInstance.DaysUntilPrompt = 3;
			iRate.SharedInstance.UsesUntilPrompt = 10;
			iRate.SharedInstance.OnlyPromptIfLatestVersion = true;
			iRate.SharedInstance.PromptAtLaunch = false;
			iRate.SharedInstance.EventsUntilPrompt = 15;
			iRate.SharedInstance.MessageTitle = Resources.RateThisApp;
			iRate.SharedInstance.Message = Resources.RateMessage;
			iRate.SharedInstance.CancelButtonLabel = Resources.RateNoButton;
			iRate.SharedInstance.RemindButtonLabel = Resources.RateLaterButton;
			iRate.SharedInstance.RateButtonLabel = Resources.RateYesButton;
		}

		public override void OnActivated (UIApplication application)
		{
			if (viewController != null) {
				viewController.ReloadData ();
			}
		}

		private void UpdateAppearance()
		{
			var normalColor = Backgrounds.Clear;
			UIBarButtonItem.Appearance.SetBackButtonBackgroundImage(normalColor, UIControlState.Normal, UIBarMetrics.Default);
			UIBarButtonItem.Appearance.SetBackButtonBackgroundImage(normalColor, UIControlState.Highlighted, UIBarMetrics.Default);
			UIBarButtonItem.Appearance.SetBackgroundImage(normalColor, UIControlState.Normal, UIBarMetrics.Default);
			UIBarButtonItem.Appearance.SetBackgroundImage(normalColor, UIControlState.Highlighted, UIBarMetrics.Default);

			UINavigationBar.Appearance.ShadowImage = new UIImage();
			UINavigationBar.Appearance.SetBackgroundImage(new UIImage(), UIBarMetrics.Default);
			UINavigationBar.Appearance.BackgroundColor = Skin.Active.Toolbar;
			UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes {
				TextColor = Skin.Active.ToolbarText,
				TextShadowColor = UIColor.Clear,
				Font = Fonts.ToolbarTitle,
			});
			window.BackgroundColor = Skin.Active.Toolbar;
		}

		protected override void Dispose(bool disposing)
		{
			ServiceLocator.Dispose();
			base.Dispose(disposing);
		}
	}

	public class MyUINavigationController : UINavigationController 
	{
		public MyUINavigationController(UIViewController controller) : base(controller) 
		{
		}

		public override UIStatusBarStyle PreferredStatusBarStyle()
		{
			return UIStatusBarStyle.LightContent;
		}
	}


	public class CrashHelper
	{
		[DllImport ("libc")]
		private static extern int sigaction (Signal sig, IntPtr act, IntPtr oact);

		enum Signal {
			SIGBUS = 10,
			SIGSEGV = 11
		}

		public static void EnableCrashReporting ()
		{
			IntPtr sigbus = Marshal.AllocHGlobal (512);
			IntPtr sigsegv = Marshal.AllocHGlobal (512);

			// Store Mono SIGSEGV and SIGBUS handlers
			sigaction (Signal.SIGBUS, IntPtr.Zero, sigbus);
			sigaction (Signal.SIGSEGV, IntPtr.Zero, sigsegv);

			// Enable crash reporting libraries
			EnableCrashReportingUnsafe ();

			// Restore Mono SIGSEGV and SIGBUS handlers            
			sigaction (Signal.SIGBUS, sigbus, IntPtr.Zero);
			sigaction (Signal.SIGSEGV, sigsegv, IntPtr.Zero);

			Marshal.FreeHGlobal (sigbus);
			Marshal.FreeHGlobal (sigsegv);
		}

		static void EnableCrashReportingUnsafe ()
		{
			// Run your crash reporting library initialization code here--
			// this example uses HockeyApp but it should work well
			// with TestFlight or other libraries.

			// Verify in documentation that your library of choice
			// installs its sigaction hooks before leaving this method.

			Crashlytics.Crashlytics.StartWithAPIKey("b782c25f54e466d8428dd52d5e261bc304d0756e");
		}
	}
}

