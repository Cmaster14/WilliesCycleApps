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
		UIScrollView scrollView;

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

			scrollView = new UIScrollView {
				Frame = new CGRect (0, 0, 320, View.Frame.Height * 3),
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
				Frame = new CGRect (0, 0, View.Frame.Width, View.Frame.Height)
			};

			// Step 2: Choose a year.
			yearView = new YearView (View.Frame, this) {
				Frame = new CGRect (0, View.Bounds.Height, View.Bounds.Width, View.Bounds.Height)
			};

			// Step 3: Choose a part name.
			partNameView = new PartNameView (View.Frame, this) {
				Frame = new CGRect (0, View.Bounds.Height * 2, View.Bounds.Width, View.Bounds.Height)
			};

			scrollView.Add (makeView);
			scrollView.Add (hamburgerMenu);
			scrollView.Add (yearView);
			scrollView.Add (partNameView);

			View.Add (scrollView);
		}

		public void StepOneSwipeUp ()
		{
			var stepTwoOffset = new CGPoint (0, View.Bounds.Height);
			scrollView.SetContentOffset (stepTwoOffset, true);
		}

		public void StepTwoSwipeDown ()
		{
			var stepOneOffset = new CGPoint (0, 0);
			scrollView.SetContentOffset (stepOneOffset, true);
		}

		public void StepTwoSwipeUp ()
		{
			var stepTwoOffset = new CGPoint (0, View.Bounds.Height * 2);
			scrollView.SetContentOffset (stepTwoOffset, true);
		}

		public void StepThreeGoUp ()
		{
			var stepTwoOffset = new CGPoint (0, View.Bounds.Height);
			scrollView.SetContentOffset (stepTwoOffset, true);
		}
	}
}