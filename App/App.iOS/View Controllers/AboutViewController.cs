using System;
using CoreGraphics;
using Foundation;
using UIKit;
using FlyoutNavigation;

namespace App.iOS
{
	public class AboutViewController : UIViewController
	{
		FlyoutNavigationController flyout;
		UIScrollView scrollView;

		UIButton hamburgerMenu;
		AboutView aboutView;

		public AboutViewController (FlyoutNavigationController flyoutViewController)
		{
			flyout = flyoutViewController;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			scrollView = new UIScrollView {
				BackgroundColor = UIColor.Black,
				Frame = new CGRect (0, 0, 320, View.Bounds.Height * 3)
			};

			hamburgerMenu = new UIButton {
				Frame = new CGRect (10, 10, 25, 25)
			};
			hamburgerMenu.SetImage (UIImage.FromFile ("HamburgerMenu.png"), UIControlState.Normal);
			hamburgerMenu.TouchUpInside += (sender, e) => {
				flyout.ToggleMenu ();
			};
				
			aboutView = new AboutView (View.Bounds) {
				Frame = new CGRect (0, 0, View.Bounds.Width, View.Bounds.Height)
			};

			scrollView.Add (aboutView);
			scrollView.Add (hamburgerMenu);

			View.Add (scrollView);
		}

		public override bool PrefersStatusBarHidden ()
		{
			return true;
		}
	}
}