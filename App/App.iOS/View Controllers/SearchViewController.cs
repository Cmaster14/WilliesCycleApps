using System;
using System.Threading;
using CoreGraphics;
using Foundation;
using UIKit;
using BigTed;
using FlyoutNavigation;
using MonoTouch.Dialog;

namespace App.iOS
{
	[Register ("SearchViewController")]
	public class SearchViewController : UIViewController
	{
		UIView mainView;

		FlyoutNavigationController flyout;

		UIButton hamburgerMenu;
		MakeView makeView;
		YearView yearView;
		PartNameView partNameView;

		public SearchViewController (FlyoutNavigationController flyoutViewController)
		{
			flyout = flyoutViewController;
		}

		public override void ViewDidLoad ()
		{
			SetupUserInterface ();
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			NavigationController.NavigationBarHidden = true;
		}

		private void SetupUserInterface ()
		{
			NavigationController.NavigationBarHidden = true;

			mainView = new UIView {
				Frame = new CGRect (0, 0, 320, View.Frame.Height),
				UserInteractionEnabled = true
			};

			hamburgerMenu = new UIButton {
				Frame = new CGRect (10, 10, 25, 25)
			};
			hamburgerMenu.SetImage (UIImage.FromFile ("HamburgerMenu.png"), UIControlState.Normal);
			hamburgerMenu.TouchUpInside += (sender, e) => {
				flyout.ToggleMenu ();
			};
				
			// Step 1: Choose a make.
			makeView = new MakeView (View.Frame, this) {
				Frame = new CGRect (0, 0, View.Frame.Width, View.Frame.Height/3)
			};

			// Step 2: Choose a year.
			yearView = new YearView (View.Frame, this) {
				Frame = new CGRect (0, View.Bounds.Height/3, View.Bounds.Width, View.Bounds.Height/3)
			};

			// Step 3: Choose a part name.
			partNameView = new PartNameView (View.Frame, this) {
				Frame = new CGRect (0, View.Bounds.Height * 2/3, View.Bounds.Width, View.Bounds.Height/3)
			};

			mainView.Add (makeView);
			mainView.Add (hamburgerMenu);
			mainView.Add (yearView);
			mainView.Add (partNameView);

			View.Add (mainView);
		}
	}
}