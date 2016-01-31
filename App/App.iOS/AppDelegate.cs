using CoreGraphics;
using Foundation;
using UIKit;
using PayPalMobileXamarinBindings;

namespace App.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate 
	{
		UIViewController slideOutViewController;
		UIWindow window;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			ConfigureAppearanceOptions ();

			var keys = new NSObject [] {
				new NSString ("PayPalEnvironmentProduction"),
				// new NSString ("PayPalEnvironmentSandbox")
			};

			var values = new NSObject[] {
				// new NSString (@"AekAtpGCqK5rRyQWjkkGLeB_xUBMMMlVy6LZAOksXyL_JTy_j12zU0cWH_u2AO-xxGpdTeekj-Z9GfD6"),
				new NSString (@"AekAtpGCqK5rRyQWjkkGLeB_xUBMMMlVy6LZAOksXyL_JTy_j12zU0cWH_u2AO-xxGpdTeekj-Z9GfD6")
			};

			var dictionary = NSDictionary.FromObjectsAndKeys (values, keys);
			PayPalMobile.InitializeWithClientIdsForEnvironments (dictionary);

			slideOutViewController = new SlideoutViewController ();
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			window.RootViewController = slideOutViewController;
			window.MakeKeyAndVisible ();
		
			return true;
		}

		private void ConfigureAppearanceOptions ()
		{
			var textAttributes = new UITextAttributes {
				TextColor = UIColor.White,
				TextShadowColor = UIColor.Clear,
				Font = UIFont.FromName ("SegoeUI-Light", 17f)
			};
			UIBarButtonItem.Appearance.SetTitleTextAttributes (textAttributes, UIControlState.Normal);
		}
	}
}