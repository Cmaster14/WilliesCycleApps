using System;
using CoreGraphics;
using Foundation;
using UIKit;
using FlyoutNavigation;
namespace App.iOS
{
	public class LicensureViewController : UIViewController
	{
		FlyoutNavigationController flyout;
		UIButton hamburgerMenu;
		LicensureView licensureView;

		public LicensureViewController (FlyoutNavigationController flyoutViewController)
		{
			flyout = flyoutViewController;	
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			hamburgerMenu = new UIButton {
				Frame = new CGRect (10, 10, 25, 25)
			};
			hamburgerMenu.SetImage (UIImage.FromFile ("HamburgerMenu.png"), UIControlState.Normal);
			hamburgerMenu.TouchUpInside += (sender, e) => {
				flyout.ToggleMenu ();
			};

			licensureView = new LicensureView (View.Frame) {
				Frame = new CGRect (0, 0, View.Bounds.Width, View.Bounds.Height)
			};
					
			Add (licensureView);
			Add (hamburgerMenu);
		}

		public override bool PrefersStatusBarHidden ()
		{
			return true;
		}
	}
}

